using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using HeatonResearchNeural.Feedforward;
using HeatonResearchNeural.Feedforward.Train;
using HeatonResearchNeural.Feedforward.Train.Backpropagation;
using System.Data;

namespace DemoDropOut.Apps.BussinessLogicLayer
{
    public delegate void NotifyErrorHandler(double dbError, uint iteration);
    public delegate void FinishHandler(object sender, uint iteration);

    public class DropOutForecast
    {
        #region Private fields
        /// <summary>
        /// Số tập mẫu
        /// </summary>
        private int _samples = 0;
        /// <summary>
        /// Số biến nhập (input count)
        /// </summary>
        private int _variables = 0;
        /// <summary>
        /// Số lớp xuất (số biến xuất)
        /// </summary>
        private int _classes = 0;
        /// <summary>
        /// Số nơ ron lớp ẩn
        /// </summary>
        private int _hiddenNeuroCount = 0;

        private double _learningRate = 0.1;
        private double _momentumValue = 2;
        private double _learningErrorLimit = 0.1;
        private uint _learningIterationLimit = 1000;
        private bool _useErrorLimit = true;

        private double[][] _input = null; // new double[samples][];  // training set
        private double[][] _output = null; // new double[samples][]; // ideal output

        private volatile bool signalStop = false;

        // Thread
        private Thread worker = null;
        
        // Network
        private FeedforwardNetwork network = null;

        #endregion

        #region Properties
        /// <summary>
        /// Set: tập mẫu học
        /// </summary>
        public DataTable IdealSamples
        {
            set
            {
                if (_classes <= 0)
                    throw new Exception("Output number can not equal or less than zero");
                _variables = value.Columns.Count - _classes;
                _samples = value.Rows.Count;

                //var input = new double[_samples][];  // training set
                //var output = new double[_samples][]; // ideal output
                _input = new double[_samples][];
                _output = new double[_samples][];

                // set sample dataset
                for (int i = 0; i < _samples; i++)
                {
                    _input[i] = new double[_variables];
                    _output[i] = new double[_classes];

                    // set input
                    for (int j = 0; j < _variables; j++)
                    {
                        var _value = value.Rows[i][j].ToString();
                        _input[i][j] = double.Parse(_value);
                    }
                    for (int j = 0; j < _classes; j++)
                    {
                        var _value = value.Rows[i][_variables + j].ToString();
                        _output[i][j] = double.Parse(_value);
                    }
                }
            }
        }
        /// <summary>
        /// Get: số mẫu học
        /// </summary>
        public int Samples
        {
            get { return _samples; }
        }
        /// <summary>
        /// Get: số biến nhập
        /// </summary>
        public int Variables
        {
            get { return _variables; }
        }
        /// <summary>
        /// Set: số lớp xuất
        /// </summary>
        public int Classes
        {
            get { return _classes; }
        }
        /// <summary>
        /// Get, set: số nơ ron lớp ẩn
        /// </summary>
        public int HiddenNeuros
        {
            get
            {
                if (_hiddenNeuroCount <= 0)
                    return (_variables >> 1) + 1;  // (variables << 1) / 3 + 1;
                return _hiddenNeuroCount;
            }
            set { _hiddenNeuroCount = value; }
        }

        public double LearningRate
        {
            get { return _learningRate; }
            set { _learningRate = value; }
        }

        public double Momentum
        {
            get { return _momentumValue; }
            set { _momentumValue = value; }
        }

        public double ErrorLimit
        {
            get { return _learningErrorLimit; }
            set
            {
                _learningErrorLimit = value;
                _useErrorLimit = true;
            }
        }

        public uint IterationLimit
        {
            get { return _learningIterationLimit; }
            set
            {
                _learningIterationLimit = value;
                _useErrorLimit = false;
            }
        }

        public bool IsCheckError
        {
            get { return _useErrorLimit; }
            set { _useErrorLimit = value; }
        }

        #endregion

        #region Events

        public event NotifyErrorHandler NotifyError;
        public event FinishHandler Finish;
        #endregion

        #region Protected
        protected virtual void OnNotifyError(double dbError, uint iteration)
        {
            if (NotifyError != null)
            {
                NotifyError(dbError, iteration);
            }
        }

        protected virtual void OnFinish(object sender, uint iteration)
        {
            if (Finish != null)
            {
                Finish(sender, iteration);
            }
        }
        /// <summary>
        /// Bắt đầu quá trình luyện mạng
        /// </summary>
        protected virtual void startTraining()
        {
            try
            {
                FeedforwardNetwork network = new FeedforwardNetwork();
                network.AddLayer(new FeedforwardLayer(_variables));
                network.AddLayer(new FeedforwardLayer(HiddenNeuros));
                network.AddLayer(new FeedforwardLayer(_classes));
                network.Reset(); // randomize Weights & Threshold
                //network.CalculateNeuronCount();
                Train teacher = new Backpropagation(network, _input, _output, _learningRate, _momentumValue); // 0.7, 0.9); //0.7 0.9

                uint epoch = 0;

                do
                {
                    teacher.Iteration();
                    epoch++;

                    // notify message
                    OnNotifyError(teacher.Error, epoch);

                    // stop ??
                    if (_useErrorLimit == true)
                    {
                        if (teacher.Error <= _learningErrorLimit)
                            break;
                    }
                    else if (epoch >= _learningIterationLimit)
                    {
                        break;
                    }
                } while (signalStop == false);
                // Hoàn tất việc học mẫu
                this.network = teacher.Network;
                OnFinish(this, epoch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Contructors
        /// <summary>
        /// Khởi tạo hệ thống: DropOutForecast
        /// </summary>
        /// <param name="ideal">Tập mẫu học</param>
        /// <param name="classes">Số lớp xuất</param>
        public DropOutForecast(DataTable ideal, int classes)
        {
            this._classes = classes;    // impliment first
            this.IdealSamples = ideal;  // set ideal data samples
        }
        /// <summary>
        /// Khởi tạo hệ thống: DropOutForecast
        /// </summary>
        /// <param name="ideal">Tập mẫu học</param>
        /// <param name="classes">Số lớp xuất</param>
        /// <param name="hiddenNeuroCount">Số nơ ron lớp ẩn</param>
        public DropOutForecast(DataTable ideal, int classes, int hiddenNeuroCount)
            : this(ideal, classes)
        {
            this._hiddenNeuroCount = hiddenNeuroCount;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Lấy thông tin mô hình mạng
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Model: {0}-{1}-{2}", _variables, HiddenNeuros, _classes);
        }

        public void Train()
        {
            signalStop = false;
            worker = new Thread(startTraining);
            worker.Start();
        }

        public void Stop()
        {
            if (worker != null)
            {
                signalStop = true;
                while (worker.Join(100) == false)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                worker = null;
            }
        }

        public double[] ComputeOutputs(double[] ip_ideal_input)
        {
            return this.network.ComputeOutputs(ip_ideal_input);
        }

        public double[][] ComputeOutputs(double[][] ip_ideal_inputs)
        {
            var lenHeight = ip_ideal_inputs.GetLength(0);
            var result = new double[lenHeight][];
            for (int i = 0; i < lenHeight; i++)
            {
                result[i] = ComputeOutputs(ip_ideal_inputs[i]);
            }
            return result;
        }

        public double[][] ComputeOutputs(DataTable ip_ideal_inputs)
        {
            throw new NotImplementedException("Chưa cài đặt chương trình");
        }
        #endregion
    }
}

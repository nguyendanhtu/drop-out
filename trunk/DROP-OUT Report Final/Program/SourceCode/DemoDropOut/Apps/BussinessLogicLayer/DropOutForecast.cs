using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data;
using DemoDropOut.Apps.Objects;
using Vux.Neuro.App.DataTransferObjects.Feedforward;
using Vux.Neuro.App.BussinessLogicLayer.Training.Backpropagation;
using Vux.Neuro.App.BussinessLogicLayer.Training;
using Vux.Neuro.App.DataTransferObjects.NeuralForecast;

namespace DemoDropOut.Apps.BussinessLogicLayer
{
    /// <summary>
    /// Báo cáo lỗi mạng, tại bước lặp iteration
    /// </summary>
    /// <param name="dbError">Lỗi trên tập mẫu luyện</param>
    /// <param name="vlError">Lỗi trên tập mẫu kiểm tra</param>
    /// <param name="iteration">Bước lặp</param>
    public delegate void NotifyErrorHandler(double dbError, double vlError, uint iteration);
    public delegate void FinishHandler(object sender, uint iteration);
    public delegate void StartTrainingHandler(object sender);

    public class DropOutForecast
    {
        #region Private fields

        #region Network Parameters

        private TrainingAlgorithmParameters m_trn_parameter;
        private NetworkParameters m_net_parameters;

        public NetworkParameters NetworkParameters
        {
            get { return m_net_parameters; }
            set { m_net_parameters = value; }
        }

        public TrainingAlgorithmParameters TrainingAlgorithmParameters
        {
            get { return m_trn_parameter; }
            set { m_trn_parameter = value; }
        }

        #endregion

        private double[][] _trainingSet = null; // new double[samples][];  // training set
        private double[][] _trainingIdeal = null; // new double[samples][]; // ideal output

        private double[][] _validationSet = null;
        private double[][] _validIdeal = null;

        public double[][] TrainingInputSet
        {
            get { return _trainingSet; }
            set
            {
                _trainingSet = value;
                m_bl_trained_data = false;
            }
        }

        public double[][] TrainingOutputSet
        {
            get { return _trainingIdeal; }
            set { _trainingIdeal = value; }
        }

        public double[][] ValidationSet
        {
            get { return _validationSet; }
            set { _validationSet = value; }
        }

        public double[][] ValidationOutputSet
        {
            get { return _validIdeal; }
            set { _validIdeal = value; }
        }

        private volatile bool signalStop = false;

        // Thread
        private Thread worker = null;

        #endregion

        #region Properties

        /// <summary>
        /// Get: số mẫu học
        /// </summary>
        public int Samples
        {
            get { return _trainingSet.GetLength(0); }
        }

        #endregion

        #region Controls Fields
        private bool m_bl_trained_data;
        /// <summary>
        /// Dữ liệu đã được luyện
        /// </summary>
        public bool IsTrainedData
        {
            get { return m_bl_trained_data; }
            set { m_bl_trained_data = value; }
        }


        #endregion

        #region Events

        public event NotifyErrorHandler NotifyError;
        public event StartTrainingHandler StartTraining;
        public event FinishHandler Finish;
        #endregion

        #region Protected
        /// <summary>
        /// Báo cáo lỗi mạng, tại bước lặp iteration
        /// </summary>
        /// <param name="dbError">Lỗi trên tập mẫu luyện</param>
        /// <param name="vlError">Lỗi trên tập mẫu kiểm tra</param>
        /// <param name="iteration">Bước lặp</param>
        protected virtual void OnNotifyError(double dbError, double vlError, uint iteration)
        {
            if (NotifyError != null)
            {
                NotifyError(dbError, vlError, iteration);
            }
        }

        protected virtual void OnStartTraining(object sender)
        {
            if (StartTraining != null)
            {
                StartTraining(sender);
            }
        }

        protected virtual void OnFinish(object sender, uint iteration)
        {
            m_bl_trained_data = true;
            if (Finish != null)
            {
                Finish(sender, iteration);
            }
        }

        private INeuralForecast m_final_forecast;
        private INeuralForecast m_best_forecast;
        private double m_db_min_error = double.MaxValue;
        private ushort m_us_best_epoch;

        public INeuralForecast BestForecast
        {
            get { return m_best_forecast; }
        }

        public double BestError
        {
            get { return m_db_min_error; }
        }

        public ushort BestEpoch
        {
            get { return m_us_best_epoch; }
        }

        /// <summary>
        /// Bắt đầu quá trình luyện mạng
        /// </summary>
        protected virtual void startTraining()
        {
            try
            {
                var back = new Vux.Neuro.App.BussinessLogicLayer.Training.Quickpropagation.Backpropagation((ushort)m_net_parameters.HiddenNeurons, _trainingSet, _trainingIdeal);
                var epoch = default(ushort);
                // Đặt lại
                m_us_best_epoch = 0;
                m_db_min_error = double.MaxValue;
                do
                {
                    back.Learn();

                    var trnError = back.Error;
                    var validError = back.CalculateRMSError(_validationSet, _validIdeal);

                    if (validError < m_db_min_error)
                    {
                        m_us_best_epoch = epoch;
                        m_db_min_error = validError;
                        m_best_forecast = back.GetBestNetwork();
                    }

                    epoch++;
                    OnNotifyError(trnError, validError, epoch);
                    // stop ??
                    if (m_trn_parameter.UseErrorLimit == true && m_trn_parameter.UseIterationsLimit == true)
                    {
                        if (back.RMSError <= m_trn_parameter.ErrorLimit || epoch >= m_trn_parameter.IterationsLimit)
                            break;
                    }
                    else if (m_trn_parameter.UseErrorLimit == true && back.RMSError <= m_trn_parameter.ErrorLimit)
                    {
                        break;
                    }
                    else if (m_trn_parameter.UseIterationsLimit == true && epoch >= m_trn_parameter.IterationsLimit)
                    {
                        break;
                    }
                } while (signalStop == false);
                OnFinish(this, epoch);
                this.m_final_forecast = back;
                return;

                #region Version 1
                //FeedforwardNetwork network = new FeedforwardNetwork();
                //network.AddLayer(new FeedforwardLayer(_variables));
                //network.AddLayer(new FeedforwardLayer(_hiddenNeurons));
                //network.AddLayer(new FeedforwardLayer(_classes));
                //network.Reset(); // randomize Weights & Threshold
                ////network.CalculateNeuronCount();
                //ITrain teacher = new Backpropagation(network, _trainingSet, _trainingIdeal, m_trn_parameter.LearningRate, m_trn_parameter.Momentum); // 0.7, 0.9); //0.7 0.9

                //uint epoch = 0;

                //do
                //{
                //    teacher.Iteration();
                //    epoch++;

                //    // notify message
                //    OnNotifyError(teacher.Error, 0, epoch);

                //    // stop ??
                //    if (m_trn_parameter.UseErrorLimit == true && m_trn_parameter.UseIterationsLimit == true)
                //    {
                //        if (teacher.Error <= m_trn_parameter.ErrorLimit || epoch >= m_trn_parameter.IterationsLimit)
                //            break;
                //    }
                //    else if (m_trn_parameter.UseErrorLimit == true && teacher.Error <= m_trn_parameter.ErrorLimit)
                //    {
                //        break;
                //    }
                //    else if (m_trn_parameter.UseIterationsLimit == true && epoch >= m_trn_parameter.IterationsLimit)
                //    {
                //        break;
                //    }
                //} while (signalStop == false);
                //// Hoàn tất việc học mẫu
                //this.forecast = (Backpropagation)teacher;
                //OnFinish(this, epoch);

                #endregion

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
        public DropOutForecast()
        {
            // không có gì: he he
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Lấy thông tin mô hình mạng
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Net: {0}-{1}-{2}", m_net_parameters.InputNeurons, m_net_parameters.HiddenNeurons, m_net_parameters.OutputNeurons); //Model: {0}-{1}-{2}
        }

        public void Train()
        {
            signalStop = false;
            worker = new Thread(startTraining);
            worker.Start();
            OnStartTraining(this);
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
            return this.m_final_forecast.ComputeOutput(ip_ideal_input);
        }

        public double[][] ComputeOutputs(double[][] ip_ideal_inputs)
        {
            var lenHeight = ip_ideal_inputs.GetLength(0);
            var result = new double[lenHeight][];
            for (int i = 0; i < lenHeight; i++)
            {
                #region Phiên bản 1: Lỗi tham chiếu mảng
                // Output của mạng là một mảng double[] toàn cục
                // Nếu gán trực tiếp result[i] cho ComputeOutputs()
                // Vậy tất cả các phần tử i của result đều cho tởi mảng toàn cục này
                // --> gặp lỗi tham chiếu logic
                // result[i] = ComputeOutputs(ip_ideal_inputs[i]);
                #endregion
                #region Phiên bản 2: Khắc phục lỗi tham chiếu đến cùng một phần tử
                var v_new_result = ComputeOutputs(ip_ideal_inputs[i]);
                var v_arr_len = v_new_result.Length;
                result[i] = new double[v_arr_len];
                Array.Copy(v_new_result, result[i], v_arr_len);
                #endregion
            }
            return result;
        }

        public double[][] ComputeOutputs(DataTable ip_ideal_inputs)
        {
            var v_input = Common.DataHelper.ToDoubles(ip_ideal_inputs);
            return ComputeOutputs(v_input);
        }
        #endregion
    }
}

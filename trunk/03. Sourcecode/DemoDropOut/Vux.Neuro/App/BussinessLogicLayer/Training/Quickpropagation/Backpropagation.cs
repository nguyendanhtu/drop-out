using System;
using System.Collections.Generic;

using System.Text;
using Vux.Neuro.App.DataTransferObjects.Matrix;
using Vux.Neuro.App.DataTransferObjects.NeuralForecast;

/// q = (∑Pk*Bk)y(y - 1);
/// pi = (z - t)z(1 - z);

namespace Vux.Neuro.App.BussinessLogicLayer.Training.Quickpropagation
{

    /// <summary>
    /// Lan truyền ngược sử dụng qui tắc học thích nghi
    /// </summary>
    [Serializable]
    public class Backpropagation : IBackpropagation, INeuralForecast
    {
        /// <summary>
        /// Số neuro lớp ẩn
        /// </summary>
        private ushort Hids;
        /// <summary>
        /// Số neuro lớp xuất
        /// </summary>
        private ushort Outs;
        /// <summary>
        /// Số neuro lớp nhập
        /// </summary>
        private ushort Ins;
        /// <summary>
        /// Số mẫu luyện
        /// </summary>
        private ushort Examples;

        private double[][] input;
        private double[][] ideal;

        private double rmsError;

        private Backpropagation()
        {
        }

        #region Properties

        public ushort HiddenCount
        {
            get { return Hids; }
            //set { Hids = value; }
        }

        public ushort InputCount
        {
            get { return Ins; }
        }

        public ushort OutputCount
        {
            get { return Outs; }
        }

        public ushort ExampleCount
        {
            get { return Examples; }
        }

        public double RMSError
        {
            get { return rmsError; }
        }

        public double[][] Input
        {
            get { return input; }
            protected set
            {
                input = value;
                Examples = (ushort)input.Length;
                if (input.Length > 0 && input[0] != null)
                {
                    // Lấy thông tin số nút nhập
                    Ins = (ushort)input[0].Length;
                }
                else
                {
                    throw new Exception("Mẫu luyện đưa vào không hợp lệ: NULL");
                }
            }
        }

        public double[][] Ideal
        {
            get { return ideal; }
            protected set
            {
                ideal = value;
                if (Examples != ideal.Length)
                {
                    var v_str_msg = string.Format("Số mẫu nhập vs xuất không hợp lệ: {0} # {1}", Examples, ideal.Length);
                    throw new Exception(v_str_msg);
                }
                if (ideal[0] != null)
                {
                    // Lấy thông tin số nút xuất
                    Outs = (ushort)ideal[0].Length;
                }
                else
                {
                    throw new Exception("Mẫu luyện đưa vào không hợp lệ: NULL");
                }
            }
        }

        #endregion

        #region Contructors
        public Backpropagation(ushort hids, double[][] input, double[][] ideal)
        {
            Hids = hids;
            Input = input;
            Ideal = ideal;
            initDefault();
            initComponents();
            initWeights();
        }
        /// <summary>
        /// Khởi tạo mạng với qui tắc học thích nghi
        /// </summary>
        public Backpropagation(double ip_db_phi, double ip_db_kappa, double ip_db_theta, double ip_db_momen, ushort hids, double[][] input, double[][] ideal)
            : this(hids, input, ideal)
        {
            phi = ip_db_phi;
            kappa = ip_db_kappa;
            theta = ip_db_theta;
            momen = ip_db_momen;
        }
        #endregion

        // training set
        private double[] x;
        private double[] t;

        // Kiến trúc mạng: 3 lớp
        private double phi;
        private double kappa;
        private double theta;
        private double momen;

        /// <summary>
        /// Trọng số lớp ẩn
        /// </summary>
        private double[,] a;    // a(0 TO ins, 1 to hids);
        /// <summary>
        /// Trọng số lớp xuất
        /// </summary>
        private double[,] b;    // b(0 TO hids, 1 TO outs);
        // Biến thiên trọng
        private double[,] cHid; // cHid(0 TO ins, 1 TO hids)
        private double[,] cOut; // cOut(0 TO hids, 1 TO outs)
        // Đạo hàm riêng
        private double[,] dHid; // dHid(0 TO ins, 1 TO hids)
        private double[,] dOut; // dOut(0 TO hids, 1 TO outs)
        // Hệ số học thích nghi
        private double[,] eHid; // eHid(0 TO ins, 1 TO hids)
        private double[,] eOut; // eOut(0 TO hids, 1 TO outs)
        // Trung bình đạo hàm
        private double[,] fHid;
        private double[,] fOut;

        //private double u;   // Tổng trọng hóa cho nút ẩn
        //private double v;   // Tổng trọng hóa cho nút xuất

        private double[] y; // kết xuất của nút ẩn: y(hids)
        private double[] z; // kết xuất của nút xuất: z(outs)
        /// <summary>
        /// p(outs)
        /// </summary>
        private double[] p; // ∂E/∂v


        /// <summary>
        /// Khởi tạo trọng
        /// </summary>
        private void initWeights()
        {
            // Khởi tạo trọng: P.tử cuối cùng của mảng lưu trọng ngưỡng
            a = new double[Ins + 1, Hids];
            b = new double[Hids + 1, Outs];
            var rnd = new Random();
            // Khởi tạo trọng lớp ẩn
            for (int j = 0; j < Hids; j++)
            {
                for (int i = 0; i <= Ins; i++)
                {
                    a[i, j] = 0.2 * (rnd.NextDouble() - 0.5);   // [-0.1, 0.1]
                    eHid[i, j] = kappa;
                }
            }
            // Khởi tạo trọng lớp xuất
            for (int k = 0; k < Outs; k++)
            {
                for (int j = 0; j < Hids; j++)
                {
                    if ((j & 1) == 0)   // Nếu j mod 2 == 0; chia hết hai
                    {
                        b[j, k] = 1;    // G.trị 1 hoặc -1 để cho kết xuất hoặc là ko
                    }
                    else
                    {
                        b[j, k] = -1;
                    }
                    eOut[j, k] = kappa;
                }
                // Khởi tạo trọng ngưỡng của nút xuất: nếu tổng số trọng của nút xuất lẻ thì khởi tạo bằng 0
                if ((Hids & 1) == 0)
                {
                    b[Hids, k] = 1;
                }
                else
                {
                    b[Hids, k] = 0;
                }
            }
        }
        /// <summary>
        /// Lan truyền tiến, tính kết xuất của lớp ẩn và lớp xuất
        /// </summary>
        private void Forward()
        {
            for (int j = 0; j < Hids; j++)
            {
                // Tổn trọng hóa nút ẩn
                var u = a[Ins, j];    // Trọng ngưỡng nút ẩn j
                for (int i = 0; i < Ins; i++)
                {
                    // Tính tổng trọng hóa các nút nhập đối với nút ẩn j
                    u = u + (a[i, j] * x[i]);
                }
                y[j] = logistic(u);
            }
            for (int k = 0; k < Outs; k++)
            {
                // Tổng trọng hóa nút xuất
                var v = b[Hids, k]; // Trọng ngưỡng nút xuất k
                for (int j = 0; j < Hids; j++)
                {
                    // Tính tổng trọng hóa các nút ẩn đối với nút xuất k
                    v = v + (b[j, k] * y[j]);
                }
                z[k] = logistic(v);
            }
        }
        /// <summary>
        /// Root Mean Square Error
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ideal"></param>
        /// <returns></returns>
        public double CalculateRMSError(double[][] input, double[][] ideal)
        {
            var totalError = 0.0;
            var setSize = 0;
            for (int i = 0; i < ideal.Length; i++)
            {
                var actual = ComputeOutput(input[i]);
                for (int j = 0; j < actual.Length; j++)
                {
                    var delta = ideal[i][j] - actual[j];
                    totalError += delta * delta;
                }
                setSize += ideal.Length;
            }
            return Math.Sqrt(totalError / setSize);
        }

        /// <summary>
        /// Tính đạo hàm hàm lỗi cho các trọng số
        /// </summary>
        private void Back()
        {
            // mỗi lần back đều khởi tạo lại q
            var q = new double[Hids]; // ∂E/∂u = 0;
            // tính cho lớp xuất
            for (int k = 0; k < Outs; k++)
            {
                p[k] = (z[k] - t[k]) * z[k] * (1.0 - z[k]);
                // đạo hàm đối với trọng ngưỡng nút xuất k
                dOut[Hids, k] += p[k];
                for (int j = 0; j < Hids; j++)  // Trọng lớp ẩn
                {
                    dOut[j, k] += p[k] * y[j];
                    q[j] += p[k] * b[j, k]; // sẽ dùng sau, tính cho lớp ẩn
                }
            }
            // tính cho lớp ẩn
            for (int j = 0; j < Hids; j++)
            {
                q[j] = q[j] * y[j] * (1.0 - y[j]);
                // đạo hàm đối với trọng ngưỡng nút ẩn j
                dHid[Ins, j] += q[j];
                for (int i = 0; i < Ins; i++)
                {
                    dHid[i, j] += q[j] * x[i];
                }
            }
        }
        /// <summary>
        /// Cập nhật trọng số theo đạo hàm riêng
        /// với mỗi trọng, chuyển 5 th.số cho chương trình con change
        /// </summary>
        private void changeWeights()
        {
            for (int j = 0; j < Hids; j++)
            {
                for (int i = 0; i <= Ins; i++)  // cập nhật cả trọng ngưỡng
                {
                    change(ref a[i, j], dHid[i, j], ref fHid[i, j], ref eHid[i, j], ref cHid[i, j]);
                }
            }

            for (int k = 0; k < Outs; k++)
            {
                for (int j = 0; j < Hids; j++)  // có cập nhật trọng ngưỡng không ?
                {
                    change(ref b[j, k], dOut[j, k], ref fOut[j, k], ref eOut[j, k], ref cOut[j, k]);
                }
            }
            //// khởi tạo lại đạo hàm riêng
            //dHid = new double[Ins + 1, Hids];
            //dOut = new double[Hids + 1, Outs];
        }
        /// <summary>
        /// Một lần học của mạng
        /// </summary>
        public void Learn()
        {
            // khởi tạo lại tổng trọng hóa, nút xuất
            y = new double[Hids];
            z = new double[Outs];
            // đạo hàm riêng hàm lỗi theo vk
            p = new double[Outs];

            // khởi tạo lại đạo hàm riêng
            dHid = new double[Ins + 1, Hids];
            dOut = new double[Hids + 1, Outs];

            for (int i = 0; i < Examples; i++)
            {
                x = input[i];
                t = ideal[i];
                Forward();
                Back();
            }
            // cập nhật trọng
            changeWeights();
            // tính rms error
            rmsError = CalculateRMSError(this.input, this.ideal);
        }

        public INeuralForecast GetBestNetwork()
        {
            // Khởi tạo mạng có dự đoán tốt nhất
            var bestNetwork = new Backpropagation();
            bestNetwork.Ins = Ins;
            bestNetwork.Hids = Hids;
            bestNetwork.Outs = Outs;
            // khởi tạo lại tổng trọng hóa, nút xuất
            // phục vụ cho việc tính g.trị xuất
            bestNetwork.y = new double[Hids];
            bestNetwork.z = new double[Outs];
            // khởi tạo bộ trọng tốt nhất
            bestNetwork.a = new double[Ins + 1, Hids];
            bestNetwork.b = new double[Hids + 1, Outs];
            // lưu lại bộ trọng tốt nhất của mạng
            Array.Copy(a, bestNetwork.a, a.Length);
            Array.Copy(b, bestNetwork.b, b.Length);

            return bestNetwork;
        }

        private void initComponents()
        {
            //dHid = new double[Ins + 1, Hids];
            //dOut = new double[Hids + 1, Outs];

            eHid = new double[Ins + 1, Hids];
            eOut = new double[Hids + 1, Outs];

            fHid = new double[Ins + 1, Hids];
            fOut = new double[Hids + 1, Outs];

            cHid = new double[Ins + 1, Hids];
            cOut = new double[Hids + 1, Outs];
        }

        /// <summary>
        /// Cập nhật trọng số dựa trên 5 tham số
        /// </summary>
        /// <param name="w">trọng cần cập nhật</param>
        /// <param name="d">đạo hàm hàm lỗi tương ứng</param>
        /// <param name="f">trung bình đạo hàm</param>
        /// <param name="e">hệ số học thích nghi bước trước</param>
        /// <param name="c">biến thiên trọng bước trước</param>
        private void change(ref double w, double d, ref double f, ref double e, ref double c)
        {
            if (d * f > 0)
            {
                e = e + kappa;
            }
            else
            {
                e = e * phi;
            }
            f = (1.0 - theta) * d + theta * f;
            c = (1 - momen) * (-e * d) + momen * c;
            w = w + c;
        }

        private double logistic(double uv)
        {
            if (uv < -100)
            {
                return 0;
            }
            else if (uv > 100)
            {
                return 1;
            }
            else
            {
                return 1.0 / (1 + Math.Exp(-uv));
            }
        }

        public void initDefault()
        {
            phi = 0.5;
            theta = 0.7;
            kappa = 0.1;
            momen = 0.9;
        }

        #region INeuralForecast Members


        public double Error
        {
            get { return rmsError; }
        }

        /// <summary>
        /// Tính kết quả của mạng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public double[] ComputeOutput(double[] input)
        {
            #region Version 1
            //var y = new double[Hids];
            //var z = new double[Outs];
            //for (int j = 0; j < Hids; j++)
            //{
            //    var u = a[Ins, j];    // Trọng ngưỡng nút ẩn j
            //    for (int i = 0; i < Ins; i++)
            //    {
            //        // Tính tổng trọng hóa các nút nhập đối với nút ẩn j
            //        u = u + (a[i, j] * input[i]);
            //    }
            //    y[j] = logistic(u);
            //}
            //for (int k = 0; k < Outs; k++)
            //{
            //    var v = b[Hids, k]; // Trọng ngưỡng nút xuất k
            //    for (int j = 0; j < Hids; j++)
            //    {
            //        // Tính tổng trọng hóa các nút ẩn đối với nút xuất k
            //        v = v + (b[j, k] * y[j]);
            //    }
            //    z[k] = logistic(v);
            //}
            #endregion

            #region Version 2
            x = input;
            Forward();
            #endregion
            return z;
        }

        public double[][] ComputeOutput(double[][] input)
        {
            var lenHeight = input.GetLength(0);
            var result = new double[lenHeight][];
            for (int i = 0; i < lenHeight; i++)
            {
                var v_new_result = ComputeOutput(input[i]);
                var v_arr_len = v_new_result.Length;
                result[i] = new double[v_arr_len];
                Array.Copy(v_new_result, result[i], v_arr_len);
            }
            return result;
        }
        #endregion

        #region IBackpropagation Members

        //private double[][] validInput;
        //private double[][] validIdeal;

        //public double[][] ValidInput
        //{
        //    get
        //    {
        //        return validInput;
        //    }
        //    set
        //    {
        //        validInput = value;
        //    }
        //}

        //public double[][] ValidIdeal
        //{
        //    get
        //    {
        //        return validIdeal;
        //    }
        //    set
        //    {
        //        validIdeal = value;
        //    }
        //}

        //public double ValidError
        //{
        //    get { return CalculateRMSError(validInput, validIdeal); }
        //}

        #endregion
    }
}

using System;
using System.Collections.Generic;

using System.Text;

namespace Vux.Neuro.App.BussinessLogicLayer.Training.Quickpropagation
{
    /// <summary>
    /// Lan truyền ngược sử dụng phương pháp Quick Propagation
    /// </summary>
    public class Quickpropagation : IBackpropagation
    {
        #region Private Members
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

        #endregion


        #region IBackpropagation Members


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

        public double[] ComputeOutput(double[] input)
        {
            x = input;
            Forward();
            return z;
        }

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
            //var total = ideal[0].Length * ideal.Length * 2;
            return Math.Sqrt(totalError / setSize);
        }
        /// <summary>
        /// Luyện mạng bắt đầu từ thế hệ thứ 2
        /// </summary>
        public void Learn()
        {
            // y, z, p đã được khởi tạo tại Initialize();
            // khởi tạo lại tổng trọng hóa, nút xuất
            //y = new double[Hids];
            //z = new double[Outs];
            //// đạo hàm riêng hàm lỗi theo vk
            //p = new double[Outs];

            // lưu lại các đạo hàm theo trọng thế hệ trước
            //Array.Copy(dHid, dHidBak, dHidBak.Length);
            //Array.Copy(dOut, dOutBak, dOutBak.Length);
            dHidBak = dHid;
            dOutBak = dOut;

            // Reset đạo hàm riêng theo trọng
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
            QuickUpdate();
            // tính rms error: 0.25385758618131615
            rmsError = CalculateRMSError(this.input, this.ideal);
        }
        /// <summary>
        /// Khởi tạo hệ thống + luyện mạng thế hệ 1
        /// Sử dụng qui tắc Delta
        /// </summary>
        public void Initialize()
        {
            //dHidBak = new double[Ins + 1, Hids];
            //dOutBak = new double[Hids + 1, Outs];
            // khởi tạo lại tổng trọng hóa, nút xuất
            y = new double[Hids];
            z = new double[Outs];
            // đạo hàm riêng hàm lỗi theo vk
            p = new double[Outs];
            initComponents();
            // Khởi tạo trọng được thực hiện sau bước initComponents();
            initWeights();

            shirnkFactor = maximumGrowthFactor_ / (1.0 + maximumGrowthFactor_);

            //for (int i = 0; i < Examples; i++)
            //{
            //    x = input[i];
            //    t = ideal[i];
            //    Forward();
            //    Back();
            //}
            //// Cập nhật trọng theo qui tắc Delta
            //DeltaUpdate();
            //// tính rms error
            //rmsError = CalculateRMSError(this.input, this.ideal);
        }

        #endregion

        #region Network Properties

        // training set
        private double[] x;
        private double[] t;

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

        private double u;   // Tổng trọng hóa cho nút ẩn
        private double v;   // Tổng trọng hóa cho nút xuất

        private double[] y; // kết xuất của nút ẩn: y(hids)
        private double[] z; // kết xuất của nút xuất: z(outs)
        /// <summary>
        /// p(outs)
        /// </summary>
        private double[] p; // ∂E/∂v

        #endregion

        #region Contructors

        public Quickpropagation(ushort hids, double[][] input, double[][] ideal)
        {
            Hids = hids;
            Input = input;
            Ideal = ideal;
            //resetComponents();
            //initWeights();
        }
        /// <summary>
        /// Khởi tạo mạng với qui tắc học thích nghi
        /// </summary>
        public Quickpropagation(double ip_db_learnRate, ushort hids, double[][] input, double[][] ideal)
            : this(hids, input, ideal)
        {
            learningRate_ = ip_db_learnRate;
        }

        #endregion

        #region Initialze Components

        private void initComponents()
        {
            dHid = new double[Ins + 1, Hids];
            dOut = new double[Hids + 1, Outs];

            cHid = new double[Ins + 1, Hids];
            cOut = new double[Hids + 1, Outs];
        }

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
        #endregion

        #region Backpropagation
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
        /// Cập nhật trong số theo qui tắc delta
        /// </summary>
        /// <param name="w">Trọng cần cập nhật</param>
        /// <param name="e">Hệ số học delta</param>
        /// <param name="d">Đạo hàm hiện tại</param>
        private double DeltaUpdate(ref double w, double e, double d)
        {
            var c = e * d;
            w = w - c;
            return c;
        }
        /// <summary>
        /// Cập nhật trọng số theo qui tắc Delta,
        /// được sử dụng ở thế hệ đầu tiên.
        /// </summary>
        private void DeltaUpdate()
        {
            for (int j = 0; j < Hids; j++)
            {
                for (int i = 0; i <= Ins; i++)
                {
                    cHid[i, j] = learningRate_ * dHid[i, j];
                    a[i, j] -= cHid[i, j];
                }
            }
            for (int k = 0; k < Outs; k++)
            {
                for (int j = 0; j <= Hids; j++)
                {
                    cOut[j, k] = learningRate_ * dOut[j, k];
                    b[j, k] -= cOut[j, k];
                }
            }
        }
        /// <summary>
        /// Lưu trữ đạo hàm thế hệ trước
        /// </summary>
        private double[,] dHidBak;
        private double[,] dOutBak;

        /// <summary>
        /// Cập nhật trọng số theo đạo hàm riêng.
        /// Đối với Quick Propagation, chú ý:
        /// 1. Chỉ có thể được áp dụng từ thế hệ thứ 2 trở đi.
        /// 2. Làm gì khi biến thiên trọng cập nhật là 0.
        /// 3. Làm gì nếu mẫu số bằng 0.
        /// 4. Giải quyết trường hợp 2 & 3 sử dụng qui tắc Delta
        /// </summary>
        private void QuickUpdate()
        {
            // Cập nhật trọng số lớp ẩn
            for (int j = 0; j < Hids; j++)
            {
                for (int i = 0; i <= Ins; i++)
                {
                    #region Version 1
                    //var ms = dHidBak[i, j] - dHid[i, j];
                    //if (ms == 0)
                    //{
                    //    // Rơi vào trường hợp 3: Mẫu số bằng 0
                    //    cHid[i, j] = DeltaUpdate(ref a[i, j], this.e, dHid[i, j]);
                    //    continue;
                    //}
                    //var e = dHid[i, j] / ms;
                    //var c = e * cHid[i, j];
                    //if (c == 0)
                    //{
                    //    // Rơi vào tr.hợp 2: Biến thiên trọng cập nhật là 0;
                    //    cHid[i, j] = DeltaUpdate(ref a[i, j], this.e, dHid[i, j]);
                    //}
                    //else
                    //{
                    //    // Cập nhật biến thiên trọng thế hệ h.tại
                    //    cHid[i, j] = c;
                    //    // Cập nhật trọng số.
                    //    a[i, j] -= c;
                    //}

                    #endregion
                    QuickUpdate(ref a[i, j], ref cHid[i, j], dHid[i, j], dHidBak[i, j]);
                }
            }
            // Cập nhật trọng số lớp xuất
            for (int k = 0; k < Outs; k++)
            {
                for (int j = 0; j <= Hids; j++)
                {
                    #region Version 1
                    //var ms = dOutBak[j, k] - dOut[j, k];
                    //if (ms == 0)
                    //{
                    //    // Rơi vào tr.hợp 3
                    //    cOut[j, k] = DeltaUpdate(ref b[j, k], this.e, dOut[j, k]);
                    //    continue;
                    //}
                    //var e = dOut[j, k] / (dOutBak[j, k] - dOut[j, k]);
                    //var c = e * cOut[j, k];
                    //if (c == 0)
                    //{
                    //    // Rơi vào tr.hợp 2
                    //    cOut[j, k] = DeltaUpdate(ref b[j, k], this.e, dOut[j, k]);
                    //}
                    //else
                    //{
                    //    b[j, k] -= cOut[j, k];
                    //}

                    #endregion
                    QuickUpdate(ref b[j, k], ref cOut[j, k], dOut[j, k], dOutBak[j, k]);

                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="net_w">Trọng cập nhật</param>
        /// <param name="prev_c">Biến thiên trọng thế hệ trước</param>
        /// <param name="net_e">Hệ số học</param>
        /// <param name="net_mu">Moment hướng giảm</param>
        /// <param name="slope">Đạo hàm riêng theo trọng thế hệ h.tại</param>
        /// <param name="prev_slope">Đạo hàm riêng theo trọng thế hệ trước</param>
        private void QuickUpdate(ref double net_w, ref double prev_c, double slope, double prev_slope)
        {
            var c = 0.0;    // Biến thiên trọng thế hệ tiếp theo
            slope += weightDecay_ * net_w;
            if (prev_c > modeSwitchThreshold_)
            {
                if (slope > 0)
                {
                    c += learningRate_ * slope;
                }

                if (slope > shirnkFactor * prev_slope)
                {
                    c += maximumGrowthFactor_ * prev_c;
                }
                else
                {
                    // dự đoán cực tiểu của đường cong lỗi (quick rate). giả sử nó là 1 parabola
                    c += (slope / (prev_slope - slope)) * prev_c;
                }
            }
            else if (prev_c < -modeSwitchThreshold_)
            {
                if (slope < 0)
                {
                    c += learningRate_ * slope;
                }

                if (slope < (shirnkFactor * prev_slope))
                {
                    c += maximumGrowthFactor_ * prev_c;
                }
                else
                {
                    // dự đoán cực tiểu của đường cong lỗi
                    c += (slope / (prev_slope - slope)) * prev_c;
                }
            }
            else
            {
                // sử dụng phương pháp hướng giảm với qui tắc moment
                c = learningRate_ * slope + momentum_ * prev_c;
            }

            // cập nhật trọng && biến thiên trọng
            prev_c = c;
            net_w += c;

            if (net_w > 1000)
                net_w = 1000;
            else if (net_w < -1000)
                net_w = -1000;
            //else
            //    net_w = net_w;
        }

        /// <summary>
        /// Hệ số học delta, sử dụng trong tr.hợp không tính được
        /// biến thiên trọng theo phương pháp Quick Propagation
        /// </summary>
        private double learningRate_ = 0.1;//0.0055;

        /** momentum */
        private double momentum_ = 0.7;

        /** maximum growth factor */
        private const double maximumGrowthFactor_ = 1.75;

        /** weight decay term */
        private const double weightDecay_ = -0.0001;

        ///** sigmoid prime offset */
        //private double sigmoidPrimeOffset_ = 0.4;

        /** mode switch threshold (between gradient descent and parabolic min) */
        private const double modeSwitchThreshold_ = 0.001;

        private double shirnkFactor = 0; // 0.63636363636363635
        #endregion
    }
}

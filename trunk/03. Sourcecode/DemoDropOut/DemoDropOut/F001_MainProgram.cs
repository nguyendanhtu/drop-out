using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.IO;
using System.Diagnostics;
using C1.Win.C1FlexGrid;
using System.Collections;
using DemoDropOut.Objects;
using System.Threading;
using AForge.Neuro;
using AForge.Neuro.Learning;
using HeatonResearchNeural.Feedforward;
using HeatonResearchNeural.Feedforward.Train;
using HeatonResearchNeural.Feedforward.Train.Backpropagation;

namespace DemoDropOut
{
    public partial class F001_MainProgram : Form
    {
        public F001_MainProgram()
        {
            InitializeComponent();
        }

        #region Private Members

        private DataTable m_dt_samples = null;
        private Hashtable m_ht_dimension = null;
        private List<string> m_list_dimension = null;
        #endregion

        #region Private Methods

        private void OpenRawDataFormFile(string ip_fileName)
        {

        }

        private DataTable OpenEncodedDataFormFile(string ip_fileName)
        {
            StreamReader v_reader = null;
            try
            {
                var v_dataTable = new DataTable("Encoded Data");
                var inputsCount = 0;
                var classesCount = 0;
                double v_db_value = 0;
                v_reader = File.OpenText(ip_fileName);
                var v_str_line = string.Empty;
                // Khởi tạo bảng thông tin
                if ((v_str_line = v_reader.ReadLine()) != null)
                {
                    var v_str_tokens = v_str_line.Split(',');
                    // đọc số trường thuộc tính
                    inputsCount = v_str_tokens.Length - 1;
                    // đọc các cột hiện có
                    for (int j = 0; j <= inputsCount; j++)
                    {
                        //var v_dataCol = new DataColumn(v_str_tokens[j]);
                        //v_dataTable.Columns.Add(v_dataCol);
                        var v_dataCol = new DataColumn(v_str_tokens[j]);
                        if (double.TryParse(v_str_tokens[j], out v_db_value) == true)
                        {
                            #region Đọc là mẫu 1 nếu không có header
                            // Nếu là kiểu dữ liệu số thì break ngay
                            for (j = 0; j <= inputsCount; j++)
                            {
                                var v_str_colName = "Column #" + (j + 1).ToString();
                                v_dataCol = new DataColumn(v_str_colName);
                                v_dataCol.Caption = v_str_colName;
                                //v_dataCol.ColumnName = "Column #" + j.ToString();
                                v_dataTable.Columns.Add(v_dataCol);
                            }
                            // đọc & đưa dữ liệu là 1 bản ghi (mẫu)
                            var v_dataRow = v_dataTable.NewRow();
                            for (j = 0; j < inputsCount; j++)
                            {
                                v_db_value = 0;
                                if (double.TryParse(v_str_tokens[j], out v_db_value) == false)
                                {
                                    // Không đọc được dữ liệu
                                    // Làm gì đó & Bỏ qua ô này

                                    continue;
                                }
                                v_dataRow[j] = double.Parse(v_str_tokens[j]);
                            }
                            // Đọc dữ liệu đích: targetType
                            var targetTypeValue = int.Parse(v_str_tokens[inputsCount]);
                            v_dataRow[inputsCount] = targetTypeValue;
                            if (targetTypeValue >= classesCount)
                                classesCount = targetTypeValue + 1;

                            // thêm được 1 mẫu mới: v_dataRow
                            v_dataTable.Rows.Add(v_dataRow);

                            #endregion
                            // Thoát khỏi vòng lặp đọc header
                            break;
                        }
                        v_dataCol.Caption = v_str_tokens[j];
                        v_dataTable.Columns.Add(v_dataCol);
                    }
                }
                while ((v_str_line = v_reader.ReadLine()) != null)
                {
                    // Chỉ đọc định dạng được ngăn cách bởi dấu ',' ở mỗi dòng
                    var v_str_tokens = v_str_line.Split(',');
                    var v_dataRow = v_dataTable.NewRow();
                    // Đọc dữ liệu của mẫu: inputsCount
                    for (int j = 0; j < inputsCount; j++)
                    {
                        //v_db_value = 0;
                        if (double.TryParse(v_str_tokens[j], out v_db_value) == false)
                        {
                            // Không đọc được dữ liệu
                            // Bỏ qua ô này
                            continue;
                        }
                        v_dataRow[j] = v_db_value;//double.Parse(v_str_tokens[j]);
                    }
                    // Đọc dữ liệu đích: targetType
                    var targetTypeValue = int.Parse(v_str_tokens[inputsCount]);
                    v_dataRow[inputsCount] = targetTypeValue;
                    if (targetTypeValue >= classesCount)
                        classesCount = targetTypeValue + 1;

                    // thêm được 1 mẫu mới: v_dataRow
                    v_dataTable.Rows.Add(v_dataRow);
                }
                // data table
                return v_dataTable;
            }
            catch (IOException ex)
            {
                MessageBox.Show("Failed reading the file\r\nMessage: " + ex.Message, "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show("File format is not correct" + ex.Message);
            }
            finally
            {
                if (v_reader != null)
                    v_reader.Close();
            }
            return null;
        }

        public void LoadDataTableToC1Grid(C1FlexGrid ip_c1Gird, DataTable ip_table)
        {
            Debug.Assert(ip_c1Gird != null, "Chưa khởi tạo C1 grid control: Null");
            Debug.Assert(ip_table != null, "Bảng không chứa dữ liệu: Null");
            // var v_current_row = ip_c1Gird.Row;
            // Xóa dữ liệu trong bảng
            ip_c1Gird.Rows.Count = ip_c1Gird.Rows.Fixed; // xóa dữ liệu bằng cách đặt lại số row = fixed
            ip_c1Gird.Cols.Count = ip_table.Columns.Count + ip_c1Gird.Cols.Fixed; //Số cột = số cột fixed + số cột dữ liệu
            // Đọc số cột & ghi tiêu đề
            m_ht_dimension = new Hashtable();
            m_list_dimension = new List<string>();
            for (int i = ip_c1Gird.Cols.Fixed, table_index = 0; i < ip_c1Gird.Cols.Count; i++, table_index++)
            {
                var v_str_caption = ip_table.Columns[table_index].Caption;
                ip_c1Gird[0, i] = v_str_caption;
                ip_c1Gird.Cols[i].Caption = v_str_caption;
                ip_c1Gird.Cols[i].Name = v_str_caption;//ip_table.Columns[table_index].ColumnName;

                #region Đọc thông tin các cột dữ liệu: kiểu dữ liệu categories ?? --> được mã (encoded) trong bao nhiêu cột
                if (v_str_caption.Contains(":") == true)
                {
                    var v_str_tokens = v_str_caption.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    // Tên thuộc tính
                    var v_str_fieldName = v_str_tokens[0];
                    // Giá trị thuộc tính
                    var v_str_value = v_str_tokens[1];
                    if (m_ht_dimension.Contains(v_str_fieldName) == true)
                    {
                        m_ht_dimension[v_str_fieldName] = (int)m_ht_dimension[v_str_fieldName] + 1;
                    }
                    else
                    {
                        m_list_dimension.Add(v_str_fieldName);
                        m_ht_dimension[v_str_fieldName] = 1;
                    }
                }
                else
                {
                    m_list_dimension.Add(v_str_caption);
                    m_ht_dimension.Add(v_str_caption, 1);
                }
                #endregion
            }
            for (int i = 0; i < ip_table.Rows.Count; i++)
            {
                // Đọc từng mẫu dữ liệu
                var v_dataRow = ip_table.Rows[i];
                ip_c1Gird.Rows.Add();
                for (int j = 0; j < ip_table.Columns.Count; j++)
                {
                    //ip_c1Gird.Rows[j].UserData = v_dataRow[j];
                    ip_c1Gird[ip_c1Gird.Rows.Count - 1, ip_c1Gird.Cols.Fixed + j] = v_dataRow[j];
                }
            }
        }
        #endregion

        #region Thông báo
        public delegate void SetTextHandler(Control control, string text);

        private void notifyMessage(Control control, string message)
        {
            if (control.InvokeRequired == true)
            {
                control.Invoke(new SetTextHandler(notifyMessage), control, message);
            }
            else
            {
                control.Text = message;
            }
        }

        #endregion

        #region Luyện mạng
        /// <summary>
        /// Số tập mẫu
        /// </summary>
        private int samples = 0;
        /// <summary>
        /// Số biến nhập
        /// </summary>
        private int variables = 0;
        /// <summary>
        /// Số biến xuất
        /// </summary>
        private int outputs = 0;
        /// <summary>
        /// Số lớp xuất
        /// </summary>
        private int classes = 0;

        private double learningRate = 0.1;
        private double sigmoidAlphaValue = 2;
        private double learningErrorLimit = 0.1;
        private double iterationLimit = 1000;
        private bool useErrorLimit = true;

        private volatile bool signalStop = false;

        // Thread
        private Thread worker = null;

        private void UpdateSettings()
        {
            learningRateBox.Text = learningRate.ToString();
            alphaBox.Text = sigmoidAlphaValue.ToString();
            errorLimitBox.Text = learningErrorLimit.ToString();
            iterationsBox.Text = iterationLimit.ToString();

            chkErrorLimit.Checked = useErrorLimit;
        }


        private void startTraining()
        {
            try
            {
                variables = m_dt_samples.Columns.Count - outputs;
                samples = m_dt_samples.Rows.Count;

                var input = new double[samples][];  // training set
                var output = new double[samples][]; // ideal output

                // set sample dataset
                for (int i = 0; i < samples; i++)
                {
                    input[i] = new double[variables];
                    output[i] = new double[outputs];

                    // set input
                    for (int j = 0; j < variables; j++)
                    {
                        var value = m_dt_samples.Rows[i][j].ToString();
                        input[i][j] = double.Parse(value);
                    }
                    for (int j = 0; j < outputs; j++)
                    {
                        var value = m_dt_samples.Rows[i][variables + j].ToString();
                        output[i][j] = double.Parse(value);
                    }
                }


                var neuronsCount = variables > 3 ? variables / 2 : 2;
                var v_str_current_net = string.Format("{0}-{1}-{2}", variables, neuronsCount, outputs);
                notifyMessage(currentNetBox, v_str_current_net);

                // Khởi tạo mạng 1 lớp ẩn: luật học perceptron
                SigmoidFunction function = new SigmoidFunction(sigmoidAlphaValue);
                ActivationNetwork network = new ActivationNetwork(function, variables, neuronsCount);
                var layer = network[0]; // Perceptron has one hidden layer

                // Học có thầy: qui tắc delta
                DeltaRuleLearning teacher = new DeltaRuleLearning(network);
                teacher.LearningRate = learningRate;

                // lặp
                int iteration = 1;
                // bảng lỗi
                //var errorsList = new ArrayList();
                var currentError = 0.0;
                // bảng trọng: new double[lớp ẩn][nơ ron][nút vào];
                // bảng trọng cho perceptron: new double[nơ ron][biến nhập]
                var weightsOfPerceptron = new double[neuronsCount][];

                while (signalStop == false)
                {
                    #region Lưu trạng thái của mạng học
                    //for (int i = 0; i < neuronsCount; i++)
                    //{
                    //    weightsOfPerceptron[i] = new double[variables];
                    //    for (int j = 0; j < variables; j++)
                    //    {
                    //        weightsOfPerceptron[i][j] = layer[i][j];
                    //    }
                    //}
                    #endregion

                    double error = teacher.RunEpoch(input, output) / samples;
                    //errorsList.Add(error);

                    // notify message
                    notifyMessage(currentIterationBox, iteration.ToString());
                    notifyMessage(currentErrorAvgBox, error.ToString());
                    iteration++;

                    // stop ??
                    if (useErrorLimit == true)
                    {
                        if (error <= learningErrorLimit)
                            break;
                    }
                    else if (iteration > iterationLimit)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Sử dụng mạng truyền thẳng kết hợp phương pháp lan truyền ngược
        /// </summary>
        private void runTraining()
        {
            variables = m_dt_samples.Columns.Count - outputs;
            samples = m_dt_samples.Rows.Count;

            var input = new double[samples][];  // training set
            var output = new double[samples][]; // ideal output

            // set sample dataset
            for (int i = 0; i < samples; i++)
            {
                input[i] = new double[variables];
                output[i] = new double[outputs];

                // set input
                for (int j = 0; j < variables; j++)
                {
                    var value = m_dt_samples.Rows[i][j].ToString();
                    input[i][j] = double.Parse(value);
                }
                for (int j = 0; j < outputs; j++)
                {
                    var value = m_dt_samples.Rows[i][variables + j].ToString();
                    output[i][j] = double.Parse(value);
                }
            }


            var neuronsCount = (variables >> 1) + 1;  // (variables << 1) / 3 + 1;
            var v_str_current_net = string.Format("{0}-{1}-{2}", variables, neuronsCount, outputs);
            notifyMessage(currentNetBox, v_str_current_net);

            FeedforwardNetwork network = new FeedforwardNetwork();
            network.AddLayer(new FeedforwardLayer(variables));
            network.AddLayer(new FeedforwardLayer(neuronsCount));
            network.AddLayer(new FeedforwardLayer(outputs));
            network.Reset(); // randomize Weights & Threshold

            Train teacher = new Backpropagation(network, input, output, 0.7, 0.9); //0.7 0.9
            int epoch = 0;

            do
            {
                teacher.Iteration();
                epoch++;

                // notify message
                notifyMessage(currentErrorAvgBox, teacher.Error.ToString("0.00000"));
                notifyMessage(currentIterationBox, epoch.ToString());

                // stop ??
                if (useErrorLimit == true)
                {
                    if (teacher.Error <= learningErrorLimit)
                        break;
                }
                else if (epoch > iterationLimit)
                {
                    break;
                }
            } while (signalStop == false);
        }
        #endregion

        private void btnOpenRawData_Click(object sender, EventArgs e)
        {

        }

        private void btnOpenProcessedData_Click(object sender, EventArgs e)
        {
            try
            {
                var v_openFileDialog = new OpenFileDialog();
                v_openFileDialog.Filter = "Processed Data Format (*.csv, *.txt)|*.csv;*.txt|All Files (*.*)|*.*";
                var v_dialogResult = v_openFileDialog.ShowDialog();
                if (v_dialogResult == DialogResult.OK)
                {
                    var v_table = OpenEncodedDataFormFile(v_openFileDialog.FileName);
                    LoadDataTableToC1Grid(c1RawDataFlexGrid, v_table);
                    // Gán tập mẫu: m_dt_samples
                    m_dt_samples = v_table;
                    // Load số cột vào combobox
                    toolStripTargetComboBox1.Items.Clear();
                    foreach (var targetItem in m_list_dimension)
                    {
                        // var v_cboText = targetItem;
                        toolStripTargetComboBox1.Items.Add(targetItem);
                    }
                    if (toolStripTargetComboBox1.Items.Count > 0)
                    {
                        // var v_selIndex = m_list_dimension.Count - 1;
                        toolStripTargetComboBox1.SelectedIndex = m_list_dimension.Count - 1; // v_selIndex;
                        //toolStripTargetLabel.Text = string.Format("Target [{0}]", m_ht_dimension[m_list_dimension[v_selIndex]]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripTargetComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (toolStripTargetComboBox1.Items.Count > 0 && m_ht_dimension != null)
                {
                    // Lấy thông tin số nút xuất của mạng
                    outputs = (int)m_ht_dimension[m_list_dimension[toolStripTargetComboBox1.SelectedIndex]];
                    toolStripTargetLabel.Text = string.Format("Target [{0}]", outputs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            learningRate = Math.Max(0.00001, Math.Min(1, double.Parse(learningRateBox.Text)));
            sigmoidAlphaValue = Math.Max(0.01, Math.Min(100, double.Parse(alphaBox.Text)));
            learningErrorLimit = Math.Max(0, double.Parse(errorLimitBox.Text));
            iterationLimit = Math.Max(0, int.Parse(iterationsBox.Text));

            useErrorLimit = chkErrorLimit.Checked;
            UpdateSettings();

            // start 
            signalStop = false;
            //worker = new Thread(startTraining);
            worker = new Thread(runTraining);
            worker.Start();
        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            signalStop = true;
            while (worker.Join(100) == false)
            {
                Application.DoEvents();
            }
            worker = null;
        }

    }
}

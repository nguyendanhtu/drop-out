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
using DemoDropOut.Apps;
using AForge;

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

        private void DrawErrorChart(List<double> ip_listError)
        {
            double[,] errorList = new double[ip_listError.Count, 1];
            for (int i = 0; i < ip_listError.Count; i++)
            {
                errorList[i, 0] = ip_listError[i];
            }
            chartErrorTraining.RangeX = new DoubleRange(0, ip_listError.Count);
            chartErrorTraining.UpdateDataSeries("Error Chart", errorList);
        }
        #endregion

        #region Luyện mạng
        /// <summary>
        /// Start Forecast
        /// </summary>
        private DropOutForecast m_dropOutForecast;
        /// <summary>
        /// Số biến xuất
        /// </summary>
        private int classes = 0;

        private List<double> listErrorChart;

        private void UpdateSettings()
        {
            learningRateBox.Text = m_dropOutForecast.LearningRate.ToString();
            alphaBox.Text = m_dropOutForecast.Momentum.ToString();
            errorLimitBox.Text = m_dropOutForecast.ErrorLimit.ToString();
            iterationsBox.Text = m_dropOutForecast.IterationLimit.ToString();

            chkErrorLimit.Checked = m_dropOutForecast.IsCheckError;
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
                    LoadDataTableToC1Grid(c1ProcessedDataFlexGrid, v_table);
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
                    classes = (int)m_ht_dimension[m_list_dimension[toolStripTargetComboBox1.SelectedIndex]];
                    toolStripTargetLabel.Text = string.Format("Target [{0}]", classes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                listErrorChart = new List<double>();
                m_dropOutForecast = new DropOutForecast(m_dt_samples, classes);

                m_dropOutForecast.LearningRate = Math.Max(0.00001, Math.Min(1, double.Parse(learningRateBox.Text)));
                m_dropOutForecast.Momentum = Math.Max(0.01, Math.Min(100, double.Parse(alphaBox.Text)));

                m_dropOutForecast.ErrorLimit = Math.Max(0, double.Parse(errorLimitBox.Text));
                m_dropOutForecast.IterationLimit = (uint)Math.Max(0, int.Parse(iterationsBox.Text));

                m_dropOutForecast.IsCheckError = chkErrorLimit.Checked;

                m_dropOutForecast.NotifyError += new NotifyErrorHandler(m_dropOutForecast_NotifyError);
                m_dropOutForecast.Finish += new FinishHandler(m_dropOutForecast_Finish);

                UpdateSettings();

                m_dropOutForecast.Train();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void m_dropOutForecast_Finish(object sender, uint iteration)
        {
            try
            {
                MessageBox.Show("Đã xong !!\r\nSố bước lặp hoàn tất: " + iteration.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void m_dropOutForecast_NotifyError(double dbError, uint iteration)
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new NotifyErrorHandler(m_dropOutForecast_NotifyError), dbError, iteration);
            }
            else
            {
                this.txtCurrentErrorBox.Text = dbError.ToString();
                this.txtCurrentIterationBox.Text = iteration.ToString();
                this.listErrorChart.Add(dbError);
                if (listErrorChart.Count % 3 == 0)
                {
                }
            }
        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                m_dropOutForecast.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}

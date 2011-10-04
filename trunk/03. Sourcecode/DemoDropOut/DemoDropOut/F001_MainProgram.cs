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
using DemoDropOut.Apps.Objects;
using System.Threading;
using AForge.Neuro;
using AForge.Neuro.Learning;
using HeatonResearchNeural.Feedforward;
using HeatonResearchNeural.Feedforward.Train;
using HeatonResearchNeural.Feedforward.Train.Backpropagation;
using AForge;
using DemoDropOut.Apps.BussinessLogicLayer;
using DemoDropOut.Apps.DataAccessLayer;
using DemoDropOut.Common;

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

        public void LoadProcessedDataTableToC1Grid(C1FlexGrid ip_c1Grid, DataTable ip_table)
        {
            Debug.Assert(ip_c1Grid != null, "Chưa khởi tạo C1 grid control: Null");
            Debug.Assert(ip_table != null, "Bảng không chứa dữ liệu: Null");
            // var v_current_row = ip_c1Gird.Row;
            // Xóa dữ liệu trong bảng
            ip_c1Grid.Rows.Count = ip_c1Grid.Rows.Fixed; // xóa dữ liệu bằng cách đặt lại số row = fixed
            ip_c1Grid.Cols.Count = ip_table.Columns.Count + ip_c1Grid.Cols.Fixed; //Số cột = số cột fixed + số cột dữ liệu
            // Đọc số cột & ghi tiêu đề
            m_ht_dimension = new Hashtable();
            m_list_dimension = new List<string>();
            for (int i = ip_c1Grid.Cols.Fixed, table_index = 0; i < ip_c1Grid.Cols.Count; i++, table_index++)
            {
                var v_str_caption = ip_table.Columns[table_index].Caption;
                ip_c1Grid[0, i] = v_str_caption;
                ip_c1Grid.Cols[i].Caption = v_str_caption;
                ip_c1Grid.Cols[i].Name = v_str_caption;//ip_table.Columns[table_index].ColumnName;

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
                ip_c1Grid.Rows.Add();
                for (int j = 0; j < ip_table.Columns.Count; j++)
                {
                    //ip_c1Gird.Rows[j].UserData = v_dataRow[j];
                    ip_c1Grid[ip_c1Grid.Rows.Count - 1, ip_c1Grid.Cols.Fixed + j] = v_dataRow[j];
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
                v_openFileDialog.Filter = "Processed Data Format (*.csv, *.txt)|*.csv;*.txt|Comma Separated Values(*.csv)|*.csv|All Files (*.*)|*.*";
                var v_dialogResult = v_openFileDialog.ShowDialog();
                if (v_dialogResult == DialogResult.OK)
                {
                    var v_table = CsvDataAccess.OpenCommaDelimitedFile(v_openFileDialog.FileName);
                    LoadProcessedDataTableToC1Grid(c1ProcessedDataFlexGrid, v_table);
                    // Gán tập mẫu: m_dt_samples
                    m_dt_samples = v_table;
                    // Load số cột vào combobox
                    tscboTarget.Items.Clear();
                    foreach (var targetItem in m_list_dimension)
                    {
                        // var v_cboText = targetItem;
                        tscboTarget.Items.Add(targetItem);
                    }
                    if (tscboTarget.Items.Count > 0)
                    {
                        // var v_selIndex = m_list_dimension.Count - 1;
                        tscboTarget.SelectedIndex = m_list_dimension.Count - 1; // v_selIndex;
                        //toolStripTargetLabel.Text = string.Format("Target [{0}]", m_ht_dimension[m_list_dimension[v_selIndex]]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tscboTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var tsCbo = (ToolStripComboBox)sender;
                if (tsCbo.Items.Count > 0 && m_ht_dimension != null)
                {
                    // Lấy thông tin số nút xuất của mạng
                    classes = (int)m_ht_dimension[m_list_dimension[tsCbo.SelectedIndex]];
                    tslblTarget.Text = string.Format("Target [{0}]", classes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTraining_Click(object sender, EventArgs e)
        {
            try
            {
                this.tabControl1.SelectedTab = this.tabTrainingPage;
                listErrorChart = new List<double>();
                m_dropOutForecast = new DropOutForecast();

                // Set tập mẫu luyện
                m_dropOutForecast.TrainingInputSet = m_dPreprocessing_obj.TrainingSetInputToDoubles();
                m_dropOutForecast.TrainingOutputSet = m_dPreprocessing_obj.TrainingSetOutputToDoubles();
                m_dropOutForecast.ValidationSet = m_dPreprocessing_obj.ValidationSetToDoubles();

                // Cấu hình mạng (default)
                m_dropOutForecast.Variables = m_dPreprocessing_obj.Variables;
                m_dropOutForecast.HiddenNeuros = m_dPreprocessing_obj.HiddenNeurons;
                m_dropOutForecast.Classes = m_dPreprocessing_obj.Classes;

                // Thông số mạng
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var tabControl = (TabControl)sender;
                var ip_c1Gird = this.c1ManualQueryFlexGrid;
                if (tabControl.SelectedTab.Name.Equals(tabQueryPage.Name) == true)
                {
                    // Load trường dữ liệu
                    ip_c1Gird.Rows.Count = ip_c1Gird.Rows.Fixed + 1; // xóa dữ liệu bằng cách đặt lại số row = fixed
                    ip_c1Gird.Cols.Count = this.m_dt_samples.Columns.Count + ip_c1Gird.Cols.Fixed - 1; //Số cột = số cột fixed + số cột dữ liệu
                    for (int i = ip_c1Gird.Cols.Fixed, table_index = 0; i < ip_c1Gird.Cols.Count; i++, table_index++)
                    {
                        var v_str_caption = this.m_dt_samples.Columns[table_index].Caption;
                        ip_c1Gird[0, i] = v_str_caption;
                        ip_c1Gird.Cols[i].Caption = v_str_caption;
                        ip_c1Gird.Cols[i].Name = v_str_caption;
                    }
                }
                // else if
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnManualQuery_Click(object sender, EventArgs e)
        {
            try
            {
                var input = new double[this.c1ManualQueryFlexGrid.Cols.Count - this.c1ManualQueryFlexGrid.Cols.Fixed];
                for (int i = 0; i < input.Length; i++)
                {
                    input[i] = double.Parse(this.c1ManualQueryFlexGrid[1, this.c1ManualQueryFlexGrid.Cols.Fixed + i].ToString());
                }
                var output = m_dropOutForecast.ComputeOutputs(input);
                for (int i = 0; i < output.Length; i++)
                {
                    this.c1ManualQueryResultFlexGrid[1, this.c1ManualQueryFlexGrid.Cols.Fixed + i] = output[i].ToString("0.######");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Thông số lớp nghiệp vụ
        /// <summary>
        /// Đối tượng xử lý nghiệp vụ phân tích, phân chia tập dữ liệu
        /// </summary>
        private DataAnalysisBlo m_dAnalysis_obj = null;
        /// <summary>
        /// Đối tượng xử lý nghiệp vụ tiền xử lý, biến đổi kiểu dữ liệu thô
        /// </summary>
        private DataPreprocessingBlo m_dPreprocessing_obj = null;

        #endregion

        #region Xử lý hiển thị dữ liệu

        private void PartitionDataset(double ip_training_percent, double ip_validation_percent, double ip_test_percent)
        {
            if (ip_training_percent + ip_validation_percent + ip_test_percent > 1)
            {
                throw new Exception("Tỉ lệ phân chia tập dữ liệu không hợp lệ");
            }
            // style
            var _flex = c1RawDataFlexGrid;
            // Tô màu blue cho tập dữ liệu luyện
            CellStyle cstyle = _flex.Styles.Add("TrainingSet");
            cstyle.ForeColor = Color.Blue;
            // Tô màu green cho tập dữ liệu khớp mạng
            cstyle = _flex.Styles.Add("ValidationSet");
            cstyle.ForeColor = Color.Green;
            // Tô màu red cho tập dữ liệu kiểm tra
            cstyle = _flex.Styles.Add("TestSet");
            cstyle.ForeColor = Color.Red;
            // Tô màu cho ô dữ liệu bị lỗi
            cstyle = _flex.Styles.Add("CellError");
            cstyle.BackColor = Color.Black;
            cstyle.ForeColor = Color.White;
            // Tô màu cho hàng dữ liệu bị lỗi
            cstyle = _flex.Styles.Add("RowError");
            cstyle.ForeColor = Color.Gray;

            //cstyle = _flex.Styles.Add("bold");
            //cstyle.Font = new Font("Tahoma", 8, FontStyle.Bold);

            // Thực hiện phân chia dữ liệu (test)
            m_dAnalysis_obj.Partition(ip_training_percent, ip_validation_percent, ip_test_percent);
            var v_row = 0;
            var v_col = 0;
            for (int i = 0; i < m_dAnalysis_obj.TrainingSetIndex.Count; i++)
            {
                v_row = _flex.Rows.Fixed + m_dAnalysis_obj.TrainingSetIndex[i];
                _flex.Rows[v_row].Style = _flex.Styles["TrainingSet"];
                _flex.SetCellStyle(v_row, 0, _flex.Styles["TrainingSet"]);
            }

            for (int i = 0; i < m_dAnalysis_obj.ValidationSetIndex.Count; i++)
            {
                v_row = _flex.Rows.Fixed + m_dAnalysis_obj.ValidationSetIndex[i];
                _flex.Rows[v_row].Style = _flex.Styles["ValidationSet"];
                _flex.SetCellStyle(v_row, 0, _flex.Styles["ValidationSet"]);
            }

            for (int i = 0; i < m_dAnalysis_obj.TestSetIndex.Count; i++)
            {
                v_row = _flex.Rows.Fixed + m_dAnalysis_obj.TestSetIndex[i];
                _flex.Rows[v_row].Style = _flex.Styles["TestSet"];
                _flex.SetCellStyle(v_row, 0, _flex.Styles["TestSet"]);
            }

            foreach (int row in m_dAnalysis_obj.InvalidPopulationRows.Keys)
            {
                var v_cells_error = (List<int>)m_dAnalysis_obj.InvalidPopulationRows[row];
                _flex.Rows[_flex.Rows.Fixed + row].Style = _flex.Styles["RowError"];
                for (v_col = 0; v_col < v_cells_error.Count; v_col++)
                {
                    _flex.SetCellStyle(_flex.Rows.Fixed + row, _flex.Cols.Fixed + v_cells_error[v_col], _flex.Styles["CellError"]);
                }
            }

            //CellRange rg = _flex.GetCellRange(2, 2, 4, 4);
            //rg.Style = _flex.Styles["bold"];
            //_flex.SetCellStyle(0, 0, _flex.Styles["green"]);
        }

        #endregion

        #region Xử lý các sự kiện phân tích dữ liệu

        private void tsbtnOpenRawData_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                var v_openFileDialog = new OpenFileDialog();
                v_openFileDialog.Filter = "Processed Data Format (*.csv, *.txt)|*.csv;*.txt|Comma Separated Values(*.csv)|*.csv|All Files (*.*)|*.*";
                var v_dialogResult = v_openFileDialog.ShowDialog();
                if (v_dialogResult == DialogResult.OK)
                {
                    m_dAnalysis_obj = new DataAnalysisBlo();
                    var v_table = m_dAnalysis_obj.Analyze(v_openFileDialog.FileName);
                    C1Helper.LoadDataTableToC1Grid(this.c1RawDataFlexGrid, v_table);
                    //c1RawDataFlexGrid.DataSource = v_table;

                    tscboTarget.Items.Clear();
                    foreach (var v_str_item in m_dAnalysis_obj.ColumnName)
                    {
                        this.tscboTarget.Items.Add(v_str_item);
                    }
                    if (this.tscboTarget.Items.Count > 0)
                        this.tscboTarget.SelectedIndex = this.tscboTarget.Items.Count - 1;
                    this.PartitionDataset(0.68, 0.16, 0.16);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsbtnPartition1_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (m_dAnalysis_obj != null)
                    PartitionDataset(0.68, 0.16, 0.16);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsbtnOpenRawData_DropDownOpening(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsbtnOpenRawData_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                var v_tsItem = (ToolStripItem)e.ClickedItem;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        #endregion

        #region Xử lý các sự kiện của form: F001_MainProgram

        private void F001_MainProgram_Load(object sender, EventArgs e)
        {
            try
            {
                loadComponents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void loadComponents()
        {
            this.c1RawDataFlexGrid.DrawMode = DrawModeEnum.OwnerDraw;
            this.c1RawDataFlexGrid.OwnerDrawCell += new OwnerDrawCellEventHandler(c1FlexGrid_OwnerDrawCell);
        }

        public void c1FlexGrid_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            var flex = (C1FlexGrid)sender;
            if ((e.Row >= flex.Rows.Fixed) & (e.Col == (flex.Cols.Fixed - 1)))
            {
                e.Text = ((e.Row - flex.Rows.Fixed) + 1).ToString();
            }
        }

        #endregion

        #region Xử lý các sự kiện: Tiền xử lý dữ liệu

        private void tsbtnPreprocess1_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                m_dPreprocessing_obj = new DataPreprocessingBlo();
                m_dPreprocessing_obj.CategoricalEncoding = CategoricalEncoding.Binary;
                m_dPreprocessing_obj.DateEncoding = DateEncoding.Weekly;
                m_dPreprocessing_obj.TrainingSet = m_dAnalysis_obj.TrainingSet;
                m_dPreprocessing_obj.ValidationSet = m_dAnalysis_obj.ValidationSet;
                m_dPreprocessing_obj.TestSet = m_dAnalysis_obj.TestSet;
                m_dPreprocessing_obj.Preprocessing();

                var v_table = m_dPreprocessing_obj.EncodedData;
                C1Helper.LoadDataTableToC1Grid(c1ProcessedDataFlexGrid, v_table);
                this.tabControl1.SelectedTab = this.tabPreprocessingPage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsbtnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectedTab = this.tabQueryPage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion


    }
}

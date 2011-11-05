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
using AForge;
using DemoDropOut.Apps.BussinessLogicLayer;
using DemoDropOut.Apps.DataAccessLayer;
using DemoDropOut.Common;
using DemoDropOut.Options;
using Vux.Neuro.App.DataTransferObjects.NeuralForecast;

namespace DemoDropOut
{
    public partial class F001_MainProgram : Form
    {
        public F001_MainProgram()
        {
            InitializeComponent();
        }

        #region Private Members
        private DataPartitionOptions m_traing_partition;
        #endregion

        #region Private Methods

        #region Backup Old
        //private DataTable m_dt_samples = null;
        //private Hashtable m_ht_dimension = null;
        //private List<string> m_list_dimension = null;
        //public void LoadProcessedDataTableToC1Grid(C1FlexGrid ip_c1Grid, DataTable ip_table)
        //{
        //    Debug.Assert(ip_c1Grid != null, "Chưa khởi tạo C1 grid control: Null");
        //    Debug.Assert(ip_table != null, "Bảng không chứa dữ liệu: Null");
        //    // var v_current_row = ip_c1Gird.Row;
        //    // Xóa dữ liệu trong bảng
        //    ip_c1Grid.Rows.Count = ip_c1Grid.Rows.Fixed; // xóa dữ liệu bằng cách đặt lại số row = fixed
        //    ip_c1Grid.Cols.Count = ip_table.Columns.Count + ip_c1Grid.Cols.Fixed; //Số cột = số cột fixed + số cột dữ liệu
        //    // Đọc số cột & ghi tiêu đề
        //    m_ht_dimension = new Hashtable();
        //    m_list_dimension = new List<string>();
        //    for (int i = ip_c1Grid.Cols.Fixed, table_index = 0; i < ip_c1Grid.Cols.Count; i++, table_index++)
        //    {
        //        var v_str_caption = ip_table.Columns[table_index].Caption;
        //        ip_c1Grid[0, i] = v_str_caption;
        //        ip_c1Grid.Cols[i].Caption = v_str_caption;
        //        ip_c1Grid.Cols[i].Name = v_str_caption;//ip_table.Columns[table_index].ColumnName;

        //        #region Đọc thông tin các cột dữ liệu: kiểu dữ liệu categories ?? --> được mã (encoded) trong bao nhiêu cột
        //        if (v_str_caption.Contains(":") == true)
        //        {
        //            var v_str_tokens = v_str_caption.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
        //            // Tên thuộc tính
        //            var v_str_fieldName = v_str_tokens[0];
        //            // Giá trị thuộc tính
        //            var v_str_value = v_str_tokens[1];
        //            if (m_ht_dimension.Contains(v_str_fieldName) == true)
        //            {
        //                m_ht_dimension[v_str_fieldName] = (int)m_ht_dimension[v_str_fieldName] + 1;
        //            }
        //            else
        //            {
        //                m_list_dimension.Add(v_str_fieldName);
        //                m_ht_dimension[v_str_fieldName] = 1;
        //            }
        //        }
        //        else
        //        {
        //            m_list_dimension.Add(v_str_caption);
        //            m_ht_dimension.Add(v_str_caption, 1);
        //        }
        //        #endregion
        //    }
        //    for (int i = 0; i < ip_table.Rows.Count; i++)
        //    {
        //        // Đọc từng mẫu dữ liệu
        //        var v_dataRow = ip_table.Rows[i];
        //        ip_c1Grid.Rows.Add();
        //        for (int j = 0; j < ip_table.Columns.Count; j++)
        //        {
        //            //ip_c1Gird.Rows[j].UserData = v_dataRow[j];
        //            ip_c1Grid[ip_c1Grid.Rows.Count - 1, ip_c1Grid.Cols.Fixed + j] = v_dataRow[j];
        //        }
        //    }
        //}
        //private void btnOpenProcessedData_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var v_openFileDialog = new OpenFileDialog();
        //        v_openFileDialog.Filter = "Processed Data Format (*.csv, *.txt)|*.csv;*.txt|Comma Separated Values(*.csv)|*.csv|All Files (*.*)|*.*";
        //        var v_dialogResult = v_openFileDialog.ShowDialog();
        //        if (v_dialogResult == DialogResult.OK)
        //        {
        //            var v_table = CsvDataAccess.OpenCommaDelimitedFile(v_openFileDialog.FileName);
        //            LoadProcessedDataTableToC1Grid(c1ProcessedDataFlexGrid, v_table);
        //            // Gán tập mẫu: m_dt_samples
        //            m_dt_samples = v_table;
        //            // Load số cột vào combobox
        //            tscboTarget.Items.Clear();
        //            foreach (var targetItem in m_list_dimension)
        //            {
        //                // var v_cboText = targetItem;
        //                tscboTarget.Items.Add(targetItem);
        //            }
        //            if (tscboTarget.Items.Count > 0)
        //            {
        //                // var v_selIndex = m_list_dimension.Count - 1;
        //                tscboTarget.SelectedIndex = m_list_dimension.Count - 1; // v_selIndex;
        //                //toolStripTargetLabel.Text = string.Format("Target [{0}]", m_ht_dimension[m_list_dimension[v_selIndex]]);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        #endregion

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
        private List<double> listTrnErrorChart;
        private List<double> listVldErrorChart;

        private bool m_bl_istraining = false;

        private void UpdateSettings()
        {
            learningRateBox.Text = m_dropOutForecast.TrainingAlgorithmParameters.LearningRate.ToString();
            alphaBox.Text = m_dropOutForecast.TrainingAlgorithmParameters.Momentum.ToString();
            errorLimitBox.Text = m_dropOutForecast.TrainingAlgorithmParameters.ErrorLimit.ToString();
            iterationsBox.Text = m_dropOutForecast.TrainingAlgorithmParameters.IterationsLimit.ToString();

            chkErrorLimit.Checked = m_dropOutForecast.TrainingAlgorithmParameters.UseErrorLimit;
        }

        #endregion

        #region Training Tab Handler

        private void tscboTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // gặp lỗi cho các thiết lập đầu ra, tạm thời return
                return;
                var v_ouput_index = ((ToolStripComboBox)sender).SelectedIndex;
                m_dAnalysis_obj.SetOutput(v_ouput_index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TrainDataset()
        {
            var v_fNetworkTrainingOptions = new F002_NetworkTrainingOptions();
            var v_dr_result = v_fNetworkTrainingOptions.ShowDialog(this);
            if (v_dr_result == DialogResult.OK)
            {
                // Khởi tạo hệ thống
                listTrnErrorChart = new List<double>();
                listVldErrorChart = new List<double>();
                m_dropOutForecast = new DropOutForecast();

                m_dropOutForecast.TrainingAlgorithmParameters = v_fNetworkTrainingOptions.TrainingParameters;
                // Set tập mẫu luyện
                m_dropOutForecast.TrainingInputSet = m_dPreprocessing_obj.TrainingSetInputToDoubles();
                m_dropOutForecast.TrainingOutputSet = m_dPreprocessing_obj.TrainingSetOutputToDoubles();
                m_dropOutForecast.ValidationSet = m_dPreprocessing_obj.ValidationSetInputToDoubles();
                m_dropOutForecast.ValidationOutputSet = m_dPreprocessing_obj.ValidationSetOutputToDoubles();

                // Cấu hình mạng (default)
                m_dropOutForecast.Variables = m_dPreprocessing_obj.Variables;
                m_dropOutForecast.HiddenNeuros = m_dPreprocessing_obj.HiddenNeurons;
                m_dropOutForecast.Classes = m_dPreprocessing_obj.Classes;

                // Tham số luyện mạng

                //m_dropOutForecast.LearningRate = Math.Max(0.00001, Math.Min(1, double.Parse(learningRateBox.Text)));
                //m_dropOutForecast.Momentum = Math.Max(0.01, Math.Min(100, double.Parse(alphaBox.Text)));

                //m_dropOutForecast.ErrorLimit = Math.Max(0, double.Parse(errorLimitBox.Text));
                //m_dropOutForecast.IterationLimit = (uint)Math.Max(0, int.Parse(iterationsBox.Text));

                //m_dropOutForecast.IsCheckError = chkErrorLimit.Checked;

                m_dropOutForecast.NotifyError += new NotifyErrorHandler(m_dropOutForecast_NotifyError);
                m_dropOutForecast.StartTraining += new StartTrainingHandler(m_dropOutForecast_StartTraining);
                m_dropOutForecast.Finish += new FinishHandler(m_dropOutForecast_Finish);

                UpdateSettings();

                // Luyện mạng
                m_dropOutForecast.Train();
            }
        }

        private void btnTraining_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dPreprocessing_obj != null)
                {
                    if (m_bl_istraining == false)
                    {
                        this.tabControl1.SelectedTab = this.tabTrainingPage;
                        TrainDataset();
                    }
                    else if (m_dropOutForecast != null)
                    {
                        m_dropOutForecast.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void m_dropOutForecast_StartTraining(object sender)
        {
            try
            {
                if (InvokeRequired == true && sender != null)
                {
                    Invoke(new StartTrainingHandler(m_dropOutForecast_StartTraining), sender);
                }
                else
                {
                    var v_dropout_forecast = sender as DropOutForecast;
                    this.lbNetArchitecture.Text = m_dropOutForecast.ToString();
                    this.listTrnErrorChart.Clear();
                    this.listVldErrorChart.Clear();
                    this.chartErrorTraining.UpdateDataSeries("TrnError", null);
                    this.chartErrorTraining.UpdateDataSeries("VldError", null);
                    //this.lbNetArchitecture.Text = string.Format("Net: {0} - {1} - {2}", v_dropout_forecast.NetworkParameters.InputNeurons, v_dropout_forecast.NetworkParameters.HiddenNeurons, v_dropout_forecast.NetworkParameters.OutputNeurons);
                    this.tsbtnTrain1.Image = DemoDropOut.Properties.Resources.train_stop_icon_1;
                    this.tsbtnTrain2.Image = DemoDropOut.Properties.Resources.train_stop_icon_1;
                    this.m_bl_istraining = true;
                    //this.tsbtnTrain1.Tag = true;
                    //this.tsbtnTrain2.Tag = true;
                }
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
                if (InvokeRequired == true)
                {
                    Invoke(new FinishHandler(m_dropOutForecast_Finish), sender, iteration);
                }
                else
                {
                    // Lấy mạng có khả năng dự đoán tốt nhất
                    m_best_forecast = m_dropOutForecast.BestForecast;

                    this.tsbtnTrain1.Image = DemoDropOut.Properties.Resources.train_start_icon_1;
                    this.tsbtnTrain2.Image = DemoDropOut.Properties.Resources.train_start_icon_1;
                    m_bl_istraining = false;
                    //this.tsbtnTrain1.Tag = false;
                    //this.tsbtnTrain2.Tag = false;
                    MessageBox.Show(this, "Đã xong !!\r\nSố bước lặp hoàn tất: " + iteration.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private ushort m_overfit_index = 0;
        private double m_overfit_value = double.MaxValue;
        private INeuralForecast m_best_forecast;

        private double[,] GetOverFitGraphData(ushort x)
        {
            var overfit = new double[listTrnErrorChart.Count, 2];
            for (int i = 0; i < listTrnErrorChart.Count; i++)
            {
                if (i < x)
                {
                    overfit[i, 0] = i;
                    overfit[i, 1] = chartErrorTraining.RangeY.Min;
                }
                else
                {
                    overfit[i, 0] = i;
                    overfit[i, 1] = chartErrorTraining.RangeY.Max;
                }
            }
            return overfit;
        }

        private void m_dropOutForecast_NotifyError(double dbError, double vlError, uint iteration)
        {
            if (InvokeRequired == true)
            {
                Invoke(new NotifyErrorHandler(m_dropOutForecast_NotifyError), dbError, vlError, iteration);
            }
            else
            {
                this.txtCurrentErrorBox.Text = dbError.ToString();
                this.txtCurrentIterationBox.Text = iteration.ToString();
                this.listTrnErrorChart.Add(dbError);
                this.listVldErrorChart.Add(vlError);
                if (listTrnErrorChart.Count % 3 == 0)
                {
                    // show error's dynamics
                    var trnError = new double[listTrnErrorChart.Count, 2];
                    var vldError = new double[listVldErrorChart.Count, 2];

                    for (ushort i = 0; i < listTrnErrorChart.Count; i++)
                    {
                        trnError[i, 0] = i;
                        trnError[i, 1] = listTrnErrorChart[i];

                        vldError[i, 0] = i;
                        vldError[i, 1] = listVldErrorChart[i];// +0.01;
                        //if (listVldErrorChart[i] < m_overfit_value)
                        //{
                        //    m_overfit_value = listVldErrorChart[i];
                        //    m_overfit_index = i;
                        //}
                    }
                    m_overfit_index = m_dropOutForecast.BestEpoch;
                    // draw
                    chartErrorTraining.RangeX = new DoubleRange(0, listTrnErrorChart.Count - 1);
                    chartErrorTraining.UpdateDataSeries("TrnError", trnError);
                    chartErrorTraining.UpdateDataSeries("VldError", vldError);
                    chartErrorTraining.UpdateDataSeries("Overfit", GetOverFitGraphData(m_overfit_index));
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dropOutForecast != null && m_bl_istraining == true)
                {
                    m_dropOutForecast.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
        /// <summary>
        /// Xử lý các sự kiện người dùng chuyển tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_dAnalysis_obj == null)
                    return; // Người dùng chưa mở dữ liệu, nên return luôn không cần nhắc nhở gì cả
                var tabControl = (TabControl)sender;
                if (tabControl.SelectedTab.Name.Equals(tabTestingPage.Name) == true)
                {
                    if (m_dPreprocessing_obj.IsProcessedData == false)
                    {
                        var v_dr_result = MessageBox.Show("Dataset has not preprocessed!\r\rWould you like automate them", "Preprocess Now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (v_dr_result == DialogResult.Yes)
                        {
                            tsbtnPreprocess1_ButtonClick(tsbtnPreprocess1, e);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (m_dropOutForecast.IsTrainedData == false)
                    {
                        var v_dr_result = MessageBox.Show("Dataset has not trained!\r\rWould you like automate them", "Train Now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (v_dr_result == DialogResult.Yes)
                        {
                            btnTraining_Click(tsbtnTestTrainingSet, e);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        btnTestTrainingSet_Click(tsbtnTestTestSet, e);
                    }
                }
                else if (tabControl.SelectedTab.Name.Equals(tabQueryPage.Name) == true)
                {
                    if (m_dPreprocessing_obj.IsProcessedData == false)
                    {
                        var v_dr_result = MessageBox.Show("Dataset has not preprocessed!\r\rWould you like automate them", "Preprocess Now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (v_dr_result == DialogResult.Yes)
                        {
                            tsbtnPreprocess1_ButtonClick(tsbtnPreprocess1, e);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (m_dropOutForecast.IsTrainedData == false)
                    {
                        var v_dr_result = MessageBox.Show("Dataset has not trained!\r\rWould you like automate them", "Train Now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (v_dr_result == DialogResult.Yes)
                        {
                            btnTraining_Click(tsbtnTestTrainingSet, e);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        var v_dt_samples = this.m_dAnalysis_obj.GetHeaderTable;
                        DataQueryBlo.FormatManualC1FlexGrid(this.c1ManualQueryFlexGrid, v_dt_samples);
                        // Format Output grid c1flex
                        if (this.c1ManualQueryResultFlexGrid.Tag == null)
                        {
                            this.c1ManualQueryResultFlexGrid.Cols.Count = 1;
                            this.c1ManualQueryResultFlexGrid.Cols.Fixed = 0;
                            this.c1ManualQueryResultFlexGrid.Rows.Count = 2;
                            this.c1ManualQueryResultFlexGrid.Rows.Fixed = 1;
                            // set row height
                            this.c1ManualQueryResultFlexGrid.Rows[1].Height = 60;
                            var v_cstyle = c1ManualQueryResultFlexGrid.Styles.Add("Bold");
                            v_cstyle.Font = new Font("tahoma", 25, FontStyle.Bold);
                            this.c1ManualQueryResultFlexGrid.SetCellStyle(1, 0, v_cstyle);
                            this.c1ManualQueryResultFlexGrid.FocusRect = FocusRectEnum.None;
                            this.c1ManualQueryResultFlexGrid.HighLight = HighLightEnum.Never;
                            this.c1ManualQueryResultFlexGrid.Tag = string.Empty;
                        }
                        var v_output_details = m_dAnalysis_obj.GetOuput();
                        this.c1ManualQueryResultFlexGrid.Cols[0].Name = v_output_details.ColumnName;
                        this.c1ManualQueryResultFlexGrid.Cols[0].Caption = v_output_details.ColumnName;
                        this.c1ManualQueryResultFlexGrid[1, 0] = string.Empty; // "A";// 
                        // Format result table
                        DataQueryBlo.FormatResultTable(this.c1TableQueryFlexGrid, v_dt_samples);
                    }
                }
                // else if
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Test Tab Handler

        private double TestDataset(DataTable ip_dt_dataset)
        {
            var v_dt_copy = ip_dt_dataset.Copy();
            var v_output_index = (int)v_dt_copy.ExtendedProperties["OutputIndex"];
            v_dt_copy.Columns.RemoveAt(v_output_index);

            var input = m_dPreprocessing_obj.Preprocessing(v_dt_copy);
            var ouput = chkUseBestNetwork.Checked == true ? m_best_forecast.ComputeOutput(input.ToDoubles()) : m_dropOutForecast.ComputeOutputs(input);

            var v_output_details = m_dAnalysis_obj.GetOuput();
            var v_dt_output = m_dPreprocessing_obj.DecodeCategoricalColumnByBinary(ouput, v_output_details);

            var v_dt_result = DataTestingBlo.CompareResultTable(v_dt_output, ip_dt_dataset);
            var v_db_ccr = (double)v_dt_result.ExtendedProperties["CCR"];
            DataTestingBlo.FormatTestingTable(this.c1ActualVsOuputFlexGrid, v_dt_result);
            return v_db_ccr;
        }

        private void btnTestTrainingSet_Click(object sender, EventArgs e)
        {
            try
            {
                this.tsbtnTestTestSet.Checked = false;
                this.tsbtnTestTrainingSet.Checked = true;
                this.tsbtnTestValidationSet.Checked = false;

                var v_dt_training_set = m_dAnalysis_obj.TrainingSet;
                tsLabelMeanCCR.Text = string.Format("Mean CCR: {0}", TestDataset(v_dt_training_set));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTestValidationSet_Click(object sender, EventArgs e)
        {
            try
            {
                this.tsbtnTestTestSet.Checked = false;
                this.tsbtnTestTrainingSet.Checked = false;
                this.tsbtnTestValidationSet.Checked = true;

                var v_dt_validation_set = m_dAnalysis_obj.ValidationSet;
                tsLabelMeanCCR.Text = string.Format("Mean CCR: {0}", TestDataset(v_dt_validation_set));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTestTestSet_Click(object sender, EventArgs e)
        {
            try
            {
                this.tsbtnTestTestSet.Checked = true;
                this.tsbtnTestTrainingSet.Checked = false;
                this.tsbtnTestValidationSet.Checked = false;

                var v_dt_test_set = m_dAnalysis_obj.TestSet;
                tsLabelMeanCCR.Text = string.Format("Mean CCR: {0}", TestDataset(v_dt_test_set));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Query Tab Handler

        private void btnManualQuery_Click(object sender, EventArgs e)
        {
            try
            {
                var v_dt_table = DataQueryBlo.GetUserData(this.c1ManualQueryFlexGrid, 1);
                var input = m_dPreprocessing_obj.Preprocessing(v_dt_table);
                var output = chkUseBestNetwork.Checked == true ? m_best_forecast.ComputeOutput(input.ToDoubles()) : m_dropOutForecast.ComputeOutputs(input);

                var v_column_details = m_dAnalysis_obj.GetOuput();
                v_dt_table = m_dPreprocessing_obj.DecodeCategoricalColumnByBinary(output, v_column_details);

                if (this.c1ManualQueryResultFlexGrid.DataSource == null)
                {
                    this.c1ManualQueryResultFlexGrid.Rows.Count = 2;
                    this.c1ManualQueryResultFlexGrid.Rows.Fixed = 1;
                    //this.c1ManualQueryResultFlexGrid[1, 0] = string.Empty;
                }
                this.c1ManualQueryResultFlexGrid.DataSource = v_dt_table;
                var v_cstyle = this.c1ManualQueryResultFlexGrid.Styles["Bold"];
                this.c1ManualQueryResultFlexGrid.Rows[1].Height = 60;
                this.c1ManualQueryResultFlexGrid.SetCellStyle(1, 0, v_cstyle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnQueryFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dropOutForecast != null)
                {
                    var v_openFileDialog = new OpenFileDialog();
                    v_openFileDialog.Filter = "All Data Format (*.csv, *.txt)|*.csv;*.txt|Comma Separated Values(*.csv)|*.csv|All Files (*.*)|*.*";
                    var v_dialogResult = v_openFileDialog.ShowDialog();
                    if (v_dialogResult == DialogResult.OK)
                    {
                        var v_dt_input = CsvDataAccess.OpenCommaDelimitedFile(v_openFileDialog.FileName);
                        DataQueryBlo.LabelDataWithOutAnalysis(ref v_dt_input, m_dAnalysis_obj.AnalyzedDataSet.Columns);
                        var input = m_dPreprocessing_obj.Preprocessing(v_dt_input);
                        var output = chkUseBestNetwork.Checked == true ? m_best_forecast.ComputeOutput(input.ToDoubles()) : m_dropOutForecast.ComputeOutputs(input);

                        var v_column_details = m_dAnalysis_obj.GetOuput();
                        var v_dt_output = m_dPreprocessing_obj.DecodeCategoricalColumnByBinary(output, v_column_details);
                        DataHelper.MergeTableColumns(ref v_dt_input, v_dt_output);
                        this.c1TableQueryFlexGrid.DataSource = null;
                        this.c1TableQueryFlexGrid.Rows.Count = 1;
                        this.c1TableQueryFlexGrid.Rows.Fixed = 1;
                        this.c1TableQueryFlexGrid.DataSource = v_dt_input;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetOutputResultQueryTable(DataTable ip_dt_input, DataTable ip_dt_output, bool ip_bl_clean)
        {
            if (ip_bl_clean == true)
            {
                this.c1TableQueryFlexGrid.DataSource = null;
                this.c1TableQueryFlexGrid.Rows.Count = 1;
            }
        }

        private void tsbtnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dropOutForecast != null)
                    tabControl1.SelectedTab = this.tabQueryPage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Thông số lớp nghiệp vụ
        /// <summary>
        /// Đối tượng xử lý nghiệp vụ phân tích, phân chia tập dữ liệu
        /// </summary>
        private DataAnalysisBlo m_dAnalysis_obj = null;
        /// <summary>
        /// Đối tượng xử lý nghiệp vụ tiền xử lý, biến đổi kiểu dữ liệu thô
        /// </summary>
        private DataPreprocessingBlo m_dPreprocessing_obj = null;
        /// <summary>
        /// Kiểm tra dữ liệu mới đưa vào đã được luyện chưa
        /// </summary>

        #endregion

        #region Xử lý hiển thị dữ liệu

        private void PartitionDataset(DataPartitionOptions ip_training_option)
        {
            if (ip_training_option.TrainPcent + ip_training_option.ValidPcent + ip_training_option.TestPcent > 1)
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
            m_dAnalysis_obj.Partition(ip_training_option.TrainPcent, ip_training_option.ValidPcent, ip_training_option.TestPcent);
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

        #region Analysis Tab Handler: Xử lý các sự kiện phân tích dữ liệu

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
                    if (this.c1RawDataFlexGrid.DataSource == null)
                        this.c1RawDataFlexGrid.Rows.Count = 1;
                    this.c1RawDataFlexGrid.Rows.Fixed = 1;
                    this.c1RawDataFlexGrid.Cols.Count = 1;
                    this.c1RawDataFlexGrid.Cols.Fixed = 1;
                    C1Helper.LoadDataTableToC1Grid(this.c1RawDataFlexGrid, v_table);

                    tscboTarget.Items.Clear();
                    foreach (var v_str_item in m_dAnalysis_obj.ColumnName)
                    {
                        this.tscboTarget.Items.Add(v_str_item);
                    }
                    if (this.tscboTarget.Items.Count > 0)
                        this.tscboTarget.SelectedIndex = this.tscboTarget.Items.Count - 1;
                    this.PartitionDataset(this.m_traing_partition);
                    this.PreprocessData();
                    this.Text = string.Format("{0} - DropOut Forecast", Path.GetFileName(v_openFileDialog.FileName));
                    this.tabControl1.SelectedTab = this.tabAnalysisPage;
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
                    PartitionDataset(this.m_traing_partition);
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

        #region Form Handler: Xử lý các sự kiện form

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
            this.chartErrorTraining.AddDataSeries("TrnError", Color.Red, AForge.Controls.Chart.SeriesType.Line, 1);
            this.chartErrorTraining.AddDataSeries("VldError", Color.Green, AForge.Controls.Chart.SeriesType.Line, 1);   // mặc định là true;
            this.chartErrorTraining.AddDataSeries("Overfit", Color.Blue, AForge.Controls.Chart.SeriesType.Line, 1, false);     // dữ liệu này không làm thay đổi hiển thị trên trục Y

            // form
            this.FormClosing += new FormClosingEventHandler(F001_MainProgram_FormClosing);

            // Dropout Forecast
            //m_dAnalysis_obj = new DataAnalysisBlo();
            //m_dPreprocessing_obj = new DataPreprocessingBlo();
            //m_dropOutForecast = new DropOutForecast();

            #region Partition Options
            m_traing_partition.TrainPcent = 0.68;
            m_traing_partition.ValidPcent = 0.16;
            m_traing_partition.TestPcent = 0.16;
            #endregion
        }

        void F001_MainProgram_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                btnStop_Click(btnStop, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        #region Preprocessing Tab Handler: Tiền xử lý dữ liệu

        private void PreprocessData()
        {
            m_dPreprocessing_obj = new DataPreprocessingBlo();
            m_dPreprocessing_obj.CategoricalEncoding = CategoricalEncoding.Binary;
            m_dPreprocessing_obj.DateEncoding = DateEncoding.Weekly;
            m_dPreprocessing_obj.TrainingSet = m_dAnalysis_obj.TrainingSet;
            m_dPreprocessing_obj.ValidationSet = m_dAnalysis_obj.ValidationSet;
            m_dPreprocessing_obj.TestSet = m_dAnalysis_obj.TestSet;
            m_dPreprocessing_obj.Preprocessing();
            // Dữ liệu chưa được luyện
            m_dropOutForecast = new DropOutForecast();
            m_dropOutForecast.IsTrainedData = false;

            var v_table = m_dPreprocessing_obj.EncodedData;
            if (this.c1ProcessedDataFlexGrid.DataSource == null)
                this.c1ProcessedDataFlexGrid.Rows.Count = 1;
            this.c1ProcessedDataFlexGrid.Rows.Fixed = 1;
            this.c1ProcessedDataFlexGrid.Cols.Count = 0;
            C1Helper.LoadDataTableToC1Grid(c1ProcessedDataFlexGrid, v_table);
        }

        private void tsbtnPreprocess1_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (m_dAnalysis_obj != null)
                {
                    this.tabControl1.SelectedTab = this.tabPreprocessingPage;
                    this.PreprocessData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void btnPartitionOptions_Click(object sender, EventArgs e)
        {
            try
            {
                var frmPartitionOptions = new F004_DataPartitionOptions();
                frmPartitionOptions.LoadPartition(m_traing_partition);
                var v_dr_result = frmPartitionOptions.ShowDialog(this);
                if (v_dr_result == DialogResult.OK)
                {
                    this.m_traing_partition.TrainPcent = frmPartitionOptions.TrainingPercentage;
                    this.m_traing_partition.ValidPcent = frmPartitionOptions.ValidtionPercentage;
                    this.m_traing_partition.TestPcent = frmPartitionOptions.TestPercentage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsbtnTestMainTab_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dropOutForecast != null)
                    this.tabControl1.SelectedTab = this.tabTestingPage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}

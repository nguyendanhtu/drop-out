using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using C1.Win.C1FlexGrid;
using System.Data;
using System.IO;
using DemoDropOut.Apps.Objects;
using System.Globalization;
using System.Collections;

namespace DemoDropOut.Apps.BussinessLogicLayer
{
    public class DataAnalysisBlo
    {
        private string m_str_date_format = "MM/dd/yyyy";
        private IList<ColumnDetails> m_list_cols_name;
        private bool m_bl_specific_order;

        public bool SpecificOrder
        {
            get { return m_bl_specific_order; }
            set { m_bl_specific_order = value; }
        }

        // Lưu thông tin chỉ số của mẫu hợp lệ & không hợp lệ
        private List<int> m_list_valid_population;
        private Hashtable m_list_invalid_population;
        // Lưu thông tin chỉ số của tập mẫu dùng làm: mẫu luyện, mẫu khớp, mẫu kiểm tra.
        private List<int> m_list_training_set;
        private List<int> m_list_validation_set;
        private List<int> m_list_test_set;

        // Phân chia tập dữ liệu
        private DataTable m_dt_analyzed_set;
        //private DataTable m_dt_training_set;
        //private DataTable m_dt_validation_set;
        //private DataTable m_dt_test_set;

        public DataTable AnalyzedDataSet
        {
            get
            {
                if (m_dt_analyzed_set == null)
                    throw new Exception("Chưa phân tích tập dữ liệu");
                return m_dt_analyzed_set;
            }
        }

        public DataTable TrainingSet
        {
            get
            {
                if (m_dt_analyzed_set == null)
                    throw new Exception("Chưa thực hiện phân tích dữ liệu");
                var v_table = m_dt_analyzed_set.Clone();
                for (int i = 0; i < m_list_training_set.Count; i++)
                {
                    v_table.Rows.Add(m_dt_analyzed_set.Rows[m_list_training_set[i]].ItemArray);
                }
                return v_table;
            }
        }

        public DataTable ValidationSet
        {
            get
            {
                if (m_dt_analyzed_set == null)
                    throw new Exception("Chưa thực hiện phân tích dữ liệu");
                var v_table = m_dt_analyzed_set.Clone();
                for (int i = 0; i < m_list_validation_set.Count; i++)
                {
                    v_table.Rows.Add(m_dt_analyzed_set.Rows[m_list_validation_set[i]].ItemArray);
                }
                return v_table;
            }
        }

        public DataTable TestSet
        {
            get
            {
                if (m_dt_analyzed_set == null)
                    throw new Exception("Chưa thực hiện phân tích dữ liệu");
                var v_table = m_dt_analyzed_set.Clone();
                for (int i = 0; i < m_list_test_set.Count; i++)
                {
                    v_table.Rows.Add(m_dt_analyzed_set.Rows[m_list_test_set[i]].ItemArray);
                }
                return v_table;
            }
        }

        public IList<int> TrainingSetIndex
        {
            get { return m_list_training_set; }
        }

        public IList<int> ValidationSetIndex
        {
            get { return m_list_validation_set; }
        }

        public IList<int> TestSetIndex
        {
            get { return m_list_test_set; }
        }

        public IList<int> ValidPopulationRows
        {
            get { return m_list_valid_population; }
        }

        public Hashtable InvalidPopulationRows
        {
            get { return m_list_invalid_population; }
        }

        public IList<ColumnDetails> ColumnName
        {
            get { return m_list_cols_name; }
        }

        public string DateFormat
        {
            get { return m_str_date_format; }
            set { m_str_date_format = value; }
        }

        public void Partition(double ip_training_percent, double ip_validation_percent, double ip_test_percent)
        {
            // Khởi tạo lại danh sách tập mẫu
            m_list_training_set = new List<int>();
            m_list_validation_set = new List<int>();
            m_list_test_set = new List<int>();

            var v_partition_population = m_list_valid_population.ToList();
            var v_train_count = (int)(v_partition_population.Count * ip_training_percent);
            var v_validation_count = (int)Math.Ceiling(v_partition_population.Count * ip_validation_percent);
            // var v_test_count = (int)(v_valid_population.Count * ip_test_percent);

            if (m_bl_specific_order == true)
            {

            }
            else // Random approach (Simple random sample)
            {
                // Lấy ngẫu nhiên tập dữ liệu training trước
                var v_random = new Random();
                for (int i = 0; i < v_train_count; i++)
                {
                    var v_rnd_index = v_random.Next(v_partition_population.Count - 1);
                    AddTrainingSetIndex(v_partition_population[v_rnd_index]);
                    v_partition_population.RemoveAt(v_rnd_index);
                }
                for (int i = 0; i < v_validation_count; i++)
                {
                    var v_rnd_index = v_random.Next(v_partition_population.Count - 1);
                    AddValidationSetIndex(v_partition_population[v_rnd_index]);
                    v_partition_population.RemoveAt(v_rnd_index);
                }
                // for ... test count
                m_list_test_set = v_partition_population;
            }
        }

        #region Thêm vào hoặc loại bỏ chỉ mục trong các danh sách lưu trữ chỉ mục
        private void AddValidPopulationIndex(int row)
        {
            m_list_valid_population.Add(row);
        }

        private void RemoveValidPopulationIndex(int row)
        {
            m_list_valid_population.RemoveAt(row);
        }

        private void AddInvalidPopulationIndex(int row, int col)
        {
            if (m_list_invalid_population[row] == null)
            {
                var v_list_col_error = new List<int>();
                v_list_col_error.Add(col);
                m_list_invalid_population[row] = v_list_col_error;
            }
            else
            {
                ((IList<int>)(m_list_invalid_population[row])).Add(col);
            }
        }

        private void RemoveInvalidPopulationIndex(int index)
        {
            m_list_invalid_population.Remove(index);
        }

        private void AddTrainingSetIndex(int index)
        {
            m_list_training_set.Add(index);
        }

        private void RemoveTrainingSetIndex(int index)
        {
            m_list_training_set.RemoveAt(index);
        }

        private void AddValidationSetIndex(int index)
        {
            m_list_validation_set.Add(index);
        }

        private void RemoveValidationSetIndex(int index)
        {
            m_list_validation_set.RemoveAt(index);
        }

        private void AddTestSetIndex(int index)
        {
            m_list_test_set.Add(index);
        }

        private void RemoveTestSetIndex(int index)
        {
            m_list_test_set.RemoveAt(index);
        }
        #endregion

        private void InitializeComponents()
        {
            m_list_invalid_population = new Hashtable();
            m_list_valid_population = new List<int>();
        }

        private void SetOutput(DataTable table, int index)
        {
            if (table.ExtendedProperties["OutputIndex"] != null)
            {
                var v_col_index = (int)table.ExtendedProperties["OutputIndex"];
                ((ColumnDetails)table.Columns[v_col_index].ExtendedProperties["Details"]).Type = ColumnType.Input;
            }
            ((ColumnDetails)table.Columns[index].ExtendedProperties["Details"]).Type = ColumnType.Ouput;
            table.ExtendedProperties["OutputIndex"] = index;
            table.ExtendedProperties["OutputCount"] = 1;
        }

        public void SetOutput(int index)
        {
            if (m_dt_analyzed_set != null)
                SetOutput(m_dt_analyzed_set, index);
            else
            {
                throw new NullReferenceException("Dữ liệu mẫu đưa vào chưa được phân tích trước khi SetOutput");
            }
        }

        public ColumnDetails GetOuputColumn()
        {
            var v_output_index = (int)m_dt_analyzed_set.ExtendedProperties["OutputIndex"];
            return m_dt_analyzed_set.Columns[v_output_index].ExtendedProperties["Details"] as ColumnDetails;
        }

        public DataTable Analyze(DataTable ip_rawDataTable)
        {
            InitializeComponents();
            var v_dataTable = new DataTable(ip_rawDataTable.TableName);
            // Get table columns
            var v_cols_ = ip_rawDataTable.Columns.Count;
            for (int i = 0; i < v_cols_; i++)
            {
                v_dataTable.Columns.Add(ip_rawDataTable.Columns[i].ColumnName);
            }

            #region Đọc một hàng để xác định kiểu cho cột dữ liệu
            var v_dataRow = v_dataTable.NewRow();
            var v_str_tokens = ip_rawDataTable.Rows[0];
            var v_db_value = default(Double);
            var v_dt_date = default(DateTime);
            for (int j = 0; j < v_cols_; j++)
            {
                var v_column_details = new ColumnDetails();
                var v_str_value = v_str_tokens[j].ToString();
                v_column_details.ColumnName = v_dataTable.Columns[j].ColumnName;
                if (double.TryParse(v_str_value, out v_db_value) == true)
                {
                    v_column_details.MaxMinUpdateValue = v_db_value;
                    v_column_details.Format = ColumnFormat.Numerical;
                }
                else if (DateTime.TryParseExact(v_str_value, m_str_date_format, CultureInfo.CurrentCulture, DateTimeStyles.None, out v_dt_date) == true)
                {
                    v_column_details.Format = ColumnFormat.Date;
                    v_column_details.Categories.Add(v_dt_date.ToShortDateString());
                }
                else if (string.IsNullOrEmpty(v_str_value) == true)
                {
                    v_column_details.Format = ColumnFormat.Unknow;
                    v_str_tokens[j] = "null";
                    v_dataRow.RowError = "Anomaly"; // Đánh dấu hàng này có lỗi
                    v_dataTable.Columns[j].ExtendedProperties.Add("Error", true);
                }
                else if (v_column_details.Categories.Contains(v_str_value) == false)
                {
                    v_column_details.Categories.Add(v_str_value);
                    v_column_details.Format = ColumnFormat.Categorical;
                }
                v_dataRow[j] = v_str_value;
                // CD := Column Details
                v_dataTable.Columns[j].ExtendedProperties.Add("Details", v_column_details);
            }

            // thêm được 1 mẫu mới: v_dataRow
            v_dataTable.Rows.Add(v_dataRow);
            #endregion

            #region Đọc các mẫu dữ liệu tiếp theo
            for (int j = 0; j < v_cols_; j++)
            {
                var v_column_details = (ColumnDetails)v_dataTable.Columns[j].ExtendedProperties["Details"];
                var v_str_value = v_str_tokens[j].ToString();
                if (string.IsNullOrEmpty(v_str_value) == false)
                {
                    if (double.TryParse(v_str_value, out v_db_value) == true)
                    {
                        // Kiểm tra lại giá trị v_str_tokens[j] này có thuộc nhóm Numerical
                        // Nếu ko thuộc thì đây là lỗi, khác thì cập nhật giá trị max - min
                        if (v_column_details.Format == ColumnFormat.Numerical)
                        {
                            v_column_details.MaxMinUpdateValue = v_db_value;
                        }
                        else
                        {
                            v_dataRow.RowError = "Anomaly"; // Đánh dấu hàng này có lỗi
                        }
                    }
                    else if (DateTime.TryParseExact(v_str_value, m_str_date_format, CultureInfo.CurrentCulture, DateTimeStyles.None, out v_dt_date) == true)
                    {
                        if (v_column_details.Format == ColumnFormat.Date)
                        {
                            var v_str_date = v_dt_date.ToShortDateString();
                            if (v_column_details.Categories.Contains(v_str_date) == false)
                            {
                                v_column_details.Categories.Add(v_str_date);
                            }
                        }
                        else
                        {
                            v_dataRow.RowError = "Anomaly"; // Đánh dấu hàng này có lỗi
                        }
                    }
                    else if (v_column_details.Categories.Contains(v_str_value) == false)
                    {
                        if (v_column_details.Format == ColumnFormat.Categorical)
                        {
                            v_column_details.Categories.Add(v_str_value);
                        }
                        else
                        {
                            v_dataRow.RowError = "Anomaly"; // Đánh dấu hàng này có lỗi
                        }
                    }
                }
                else
                {
                    v_str_value = "null";
                    v_dataRow.RowError = "Anomaly"; // Đánh dấu hàng này có lỗi
                }
                v_dataRow[j] = v_str_value;
            }
            // thêm được 1 mẫu mới: v_dataRow
            v_dataTable.Rows.Add(v_dataRow);

            #endregion

            #region Format tên cột của bảng
            // Kiểm tra số trạng thái của cột số
            // Nếu số giá trị có chứa trong cột < 5 thì mặc định đặt nó là kiểu (categorical)
            m_list_cols_name = new List<ColumnDetails>();
            for (int i = 0; i < v_dataTable.Columns.Count; i++)
            {
                var v_column_details = (ColumnDetails)v_dataTable.Columns[i].ExtendedProperties["Details"];
                m_list_cols_name.Add(v_column_details);
                if (v_column_details.Format == ColumnFormat.Numerical && v_column_details.NumericCount < 5)
                {
                    for (int j = 0; j < v_dataTable.Rows.Count; j++)
                    {
                        var v_cate_item = v_dataTable.Rows[j][i].ToString();
                        if (string.IsNullOrEmpty(v_cate_item) == false && v_column_details.Categories.Contains(v_cate_item) == false)
                        {
                            v_column_details.Categories.Add(v_cate_item);
                        }
                    }
                    v_column_details.Format = ColumnFormat.Categorical;
                }
                // Đặt lại tên cột
                if (v_column_details.Format == ColumnFormat.Categorical)
                {
                    v_dataTable.Columns[i].Caption = string.Format("(C{0}) {1}", v_column_details.Categories.Count, v_dataTable.Columns[i].Caption);
                }
                else if (v_column_details.Format == ColumnFormat.Numerical)
                {
                    v_dataTable.Columns[i].Caption = string.Format("(N) {1}", v_column_details.Categories.Count, v_dataTable.Columns[i].Caption);
                }
                else if (v_column_details.Format == ColumnFormat.Date)
                {
                    v_dataTable.Columns[i].Caption = string.Format("(D) {1}", v_column_details.Categories.Count, v_dataTable.Columns[i].Caption);
                }
                else if (v_column_details.Format == ColumnFormat.Time)
                {
                    v_dataTable.Columns[i].Caption = string.Format("(T) {1}", v_column_details.Categories.Count, v_dataTable.Columns[i].Caption);
                }
                else if (v_column_details.Format == ColumnFormat.Unknow)
                {
                    v_dataTable.Columns[i].Caption = string.Format("(U) {1}", v_column_details.Categories.Count, v_dataTable.Columns[i].Caption);
                }
            }
            #endregion
            // data table
            m_dt_analyzed_set = v_dataTable;
            return v_dataTable;
        }
        /// <summary>
        /// Phân tích tập dữ liệu từ tệp tin định dạng: Comma delimited
        /// </summary>
        /// <param name="ip_str_fname">Tên đường dẫn tệp tin</param>
        /// <returns>DataTable</returns>
        public DataTable Analyze(string ip_str_fname)
        {
            InitializeComponents();
            StreamReader v_reader = null;
            try
            {
                var v_stream = new FileStream(ip_str_fname, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                v_reader = new StreamReader(v_stream); // File.OpenText(ip_str_fname);
                var v_str_line = string.Empty;
                var v_db_value = default(Double);
                var v_dt_date = default(DateTime);
                var v_dataTable = new DataTable("Raw Data");

                //var v_bl_header_ok = false;
                #region Đọc thông tin header file
                if ((v_str_line = v_reader.ReadLine()) != null)
                {
                    var v_str_tokens = v_str_line.Split(',');
                    // đọc các cột hiện có (header)
                    for (int j = 0; j < v_str_tokens.Length; j++)
                    {
                        var v_dataCol = new DataColumn(v_str_tokens[j]);
                        if (double.TryParse(v_str_tokens[j], out v_db_value) == true || DateTime.TryParseExact(v_str_tokens[j], m_str_date_format, CultureInfo.CurrentCulture, DateTimeStyles.None, out v_dt_date) == true)
                        {
                            // Xóa số cột đang hiện có
                            v_dataTable.Columns.Clear();
                            // Tạo header mới
                            for (j = 0; j < v_str_tokens.Length; j++)
                            {
                                var v_str_colName = "Column #" + (j + 1).ToString();
                                v_dataCol = new DataColumn(v_str_colName);
                                // v_dataCol.Caption = v_str_colName;
                                v_dataTable.Columns.Add(v_dataCol);
                            }

                            // Thoát khỏi vòng lặp đọc header
                            // break;
                            goto lbl_header_ok;
                        }
                        // else
                        // v_dataCol.Caption = v_str_tokens[j];
                        v_dataTable.Columns.Add(v_dataCol);
                    }
                }
                // đọc thêm 1 dòng nếu lần đọc ở trên chứa thông tin (header) dữ liệu
                v_str_line = v_reader.ReadLine();
                #endregion

            lbl_header_ok: ;// Hoàn tất việc tạo header cho tập dữ liệu
                #region Đọc 01 mẫu dữ liệu đầu tiên của tập dữ liệu
                if (v_str_line != null)
                {
                    // đọc & đưa dữ liệu là 1 bản ghi (mẫu)
                    var v_dataRow = v_dataTable.NewRow();
                    var v_str_tokens = v_str_line.Split(',');
                    for (int j = 0; j < v_str_tokens.Length; j++)
                    {
                        var v_column_details = new ColumnDetails();
                        v_column_details.ColumnName = v_dataTable.Columns[j].ColumnName;
                        if (string.IsNullOrEmpty(v_str_tokens[j]) == true)
                        {
                            v_str_tokens[j] = "null";
                            v_dataRow.RowError = "Anomaly"; // Đánh dấu hàng này có lỗi
                            throw new Exception("Mẫu dữ liệu đầu tiên không thể phân tích được. Cần được chuẩn hóa lại");
                        }
                        else if (double.TryParse(v_str_tokens[j], out v_db_value) == true)
                        {
                            v_column_details.MaxMinUpdateValue = v_db_value;
                            v_column_details.Format = ColumnFormat.Numerical;
                        }
                        else if (DateTime.TryParseExact(v_str_tokens[j], m_str_date_format, CultureInfo.CurrentCulture, DateTimeStyles.None, out v_dt_date) == true)
                        {
                            v_column_details.Format = ColumnFormat.Date;
                            if (v_column_details.Categories.Contains(v_dt_date.ToShortDateString()) == false)
                            {
                                v_column_details.Categories.Add(v_dt_date.ToShortDateString());
                            }
                        }
                        else if (v_column_details.Categories.Contains(v_str_tokens[j]) == false)
                        {
                            v_column_details.Categories.Add(v_str_tokens[j]);
                            v_column_details.Format = ColumnFormat.Categorical;
                        }
                        v_dataRow[j] = v_str_tokens[j];
                        // CD := Column Details
                        v_dataTable.Columns[j].ExtendedProperties.Add("Details", v_column_details);
                    }
                    // thêm được 1 mẫu mới: v_dataRow
                    v_dataTable.Rows.Add(v_dataRow);
                    AddValidPopulationIndex(0); //v_dataTable.Rows.Count := 0;
                }
                #endregion


                #region Đọc nốt các mẫu dữ liệu còn lại
                while ((v_str_line = v_reader.ReadLine()) != null)
                {
                    // Chỉ đọc định dạng được ngăn cách bởi dấu ',' ở mỗi dòng
                    var v_str_tokens = v_str_line.Split(',');
                    var v_dataRow = v_dataTable.NewRow();
                    // Đọc dữ liệu của mẫu: inputsCount
                    for (int v_col = 0; v_col < v_str_tokens.Length; v_col++)
                    {
                        var v_column_details = (ColumnDetails)v_dataTable.Columns[v_col].ExtendedProperties["Details"];
                        if (string.IsNullOrEmpty(v_str_tokens[v_col]) == true)
                        {
                            //v_str_tokens[j] = "null";
                            //v_dataRow.RowError = "Anomaly"; // Đánh dấu hàng này có lỗi
                            AddInvalidPopulationIndex(v_dataTable.Rows.Count, v_col);
                        }
                        else if (double.TryParse(v_str_tokens[v_col], out v_db_value) == true)
                        {
                            // Kiểm tra lại giá trị v_str_tokens[j] này có thuộc nhóm Numerical
                            // Nếu ko thuộc thì đây là lỗi, khác thì cập nhật giá trị max - min
                            if (v_column_details.Format == ColumnFormat.Numerical)
                            {
                                v_column_details.MaxMinUpdateValue = v_db_value;
                            }
                            else
                            {
                                // v_dataRow.RowError = "Anomaly"; // Đánh dấu hàng này có lỗi
                                AddInvalidPopulationIndex(v_dataTable.Rows.Count, v_col);
                            }
                        }
                        else if (DateTime.TryParseExact(v_str_tokens[v_col], m_str_date_format, CultureInfo.CurrentCulture, DateTimeStyles.None, out v_dt_date) == true)
                        {
                            if (v_column_details.Format == ColumnFormat.Date)
                            {
                                var v_str_date = v_dt_date.ToShortDateString();
                                if (v_column_details.Categories.Contains(v_str_date) == false)
                                {
                                    v_column_details.Categories.Add(v_str_date);
                                }
                            }
                            else
                            {
                                // v_dataRow.RowError = "Anomaly"; // Đánh dấu hàng này có lỗi
                                AddInvalidPopulationIndex(v_dataTable.Rows.Count, v_col);
                            }
                        }
                        else if (v_column_details.Categories.Contains(v_str_tokens[v_col]) == false)
                        {
                            if (v_column_details.Format == ColumnFormat.Categorical)
                            {
                                v_column_details.Categories.Add(v_str_tokens[v_col]);
                            }
                            else
                            {
                                // v_dataRow.RowError = "Anomaly"; // Đánh dấu hàng này có lỗi
                                AddInvalidPopulationIndex(v_dataTable.Rows.Count, v_col);
                            }
                        }
                        //else
                        //{
                        //    AddInvalidPopulationIndex(v_dataTable.Rows.Count, v_col);
                        //}
                        v_dataRow[v_col] = v_str_tokens[v_col];
                    }
                    if (m_list_invalid_population[v_dataTable.Rows.Count] == null)
                    {
                        // Chú ý: ta lấy v_dataTable.Rows.Count bởi vì
                        // trước vòng lặp while ta đã sẵn có 1 bản ghi ở trong v_dataTable
                        // --> Count chính bằng chỉ số của v_dataRow ta cần lấy.
                        AddValidPopulationIndex(v_dataTable.Rows.Count);
                    }
                    else
                    {
                        v_dataRow.RowError = "true";
                    }
                    // thêm được 1 mẫu mới: v_dataRow
                    v_dataTable.Rows.Add(v_dataRow);
                }
                #endregion

                #region Đặt lại tên cột dữ liệu
                // Kiểm tra số trạng thái của cột số
                // Nếu số giá trị có chứa trong cột < 5 thì mặc định đặt nó là kiểu (categorical)
                m_list_cols_name = new List<ColumnDetails>();
                for (int i = 0; i < v_dataTable.Columns.Count; i++)
                {
                    var v_column_details = (ColumnDetails)v_dataTable.Columns[i].ExtendedProperties["Details"];
                    m_list_cols_name.Add(v_column_details);
                    if (v_column_details.Format == ColumnFormat.Numerical && v_column_details.NumericCount < 5)
                    {
                        for (int j = 0; j < v_dataTable.Rows.Count; j++)
                        {
                            var v_cate_item = v_dataTable.Rows[j][i].ToString();
                            if (string.IsNullOrEmpty(v_cate_item) == false && v_column_details.Categories.Contains(v_cate_item) == false)
                            {
                                v_column_details.Categories.Add(v_cate_item);
                            }
                        }
                        v_column_details.Format = ColumnFormat.Categorical;
                    }
                    // Đặt lại tên cột
                    if (v_column_details.Format == ColumnFormat.Categorical)
                    {
                        v_dataTable.Columns[i].Caption = string.Format("(C{0}) {1}", v_column_details.Categories.Count, v_dataTable.Columns[i].ColumnName);
                    }
                    else if (v_column_details.Format == ColumnFormat.Numerical)
                    {
                        v_dataTable.Columns[i].Caption = string.Format("(N) {1}", v_column_details.Categories.Count, v_dataTable.Columns[i].ColumnName);
                    }
                    else if (v_column_details.Format == ColumnFormat.Date)
                    {
                        v_dataTable.Columns[i].Caption = string.Format("(D) {1}", v_column_details.Categories.Count, v_dataTable.Columns[i].ColumnName);
                    }
                    else if (v_column_details.Format == ColumnFormat.Time)
                    {
                        v_dataTable.Columns[i].Caption = string.Format("(T) {1}", v_column_details.Categories.Count, v_dataTable.Columns[i].ColumnName);
                    }
                    else if (v_column_details.Format == ColumnFormat.Unknow)
                    {
                        v_dataTable.Columns[i].Caption = string.Format("(U) {1}", v_column_details.Categories.Count, v_dataTable.Columns[i].ColumnName);
                    }
                }

                #endregion

                #region Mặc định cột cuối cùng là đầu ra
                var v_output_index = v_dataTable.Columns.Count - 1;
                ((ColumnDetails)v_dataTable.Columns[v_output_index].ExtendedProperties["Details"]).Type = ColumnType.Ouput;
                v_dataTable.ExtendedProperties["OutputIndex"] = v_output_index;
                v_dataTable.ExtendedProperties["OutputCount"] = 1;
                #endregion
                // data table
                m_dt_analyzed_set = v_dataTable;
                return v_dataTable;
            }
            catch (IOException ex)
            {
                throw new IOException("Failed reading the file", ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (v_reader != null)
                    v_reader.Close();
            }
        }
    }
}

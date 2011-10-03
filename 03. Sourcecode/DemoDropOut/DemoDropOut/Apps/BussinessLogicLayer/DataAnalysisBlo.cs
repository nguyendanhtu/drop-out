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

namespace DemoDropOut.Apps.BussinessLogicLayer
{
    public class DataAnalysisBlo
    {
        private string m_str_date_format = "MM/dd/yyyy";
        private IList<ColumnDetails> m_list_cols_name;
        private bool m_bl_specific_order;

        public bool SpecificOrder
        {
            get { return m_bl_specific_order;}
            set { m_bl_specific_order = value;}
        }

        private DataTable m_dt_analyzed;

        public DataTable Analyzed
        {
            get { return m_dt_analyzed; }
        }
        // Lư thông tin chỉ số của mẫu hợp lệ & không hợp lệ
        private IList<int> m_list_valid_population;
        private IList<int> m_list_invalid_population;
        // Lưu thông tin chỉ số của tập mẫu dùng làm: mẫu luyện, mẫu khớp, mẫu kiểm tra.
        private IList<int> m_list_training_set;
        private IList<int> m_list_validation_set;
        private IList<int> m_list_test_set;

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

        public IList<int> ValidPopulation
        {
            get { return m_list_valid_population; }
        }

        public IList<int> InvalidPopulation
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

        public void Partition(int ip_training_percent, int ip_validation_percent, int ip_test_percent)
        {
            // Khởi tạo lại danh sách tập mẫu
            m_list_training_set = new List<int>();
            m_list_validation_set = new List<int>();
            m_list_test_set = new List<int>();

            if (m_bl_specific_order == true)
            {

            }
            else // Random approach (Simple random sample)
            {

            }
        }

        #region Thêm vào hoặc loại bỏ chỉ mục trong các danh sách lưu trữ chỉ mục
        private void AddValidPopulationIndex(int index)
        {
            m_list_valid_population.Add(index);
        }

        private void RemoveValidPopulationIndex(int index)
        {
            m_list_valid_population.RemoveAt(index);
        }

        private void AddInvalidPopulationIndex(int index)
        {
            m_list_invalid_population.Add(index);
        }

        public void RemoveInvalidPopulationIndex(int index)
        {
            m_list_invalid_population.RemoveAt(index);
        }

        public void AddTrainingSetIndex(int index)
        {
            m_list_training_set.Add(index);
        }

        public void RemoveTrainingSetIndex(int index)
        {
            m_list_training_set.RemoveAt(index);
        }

        public void AddValidationSetIndex(int index)
        {
            m_list_validation_set.Add(index);
        }

        public void RemoveValidationSetIndex(int index)
        {
            m_list_validation_set.RemoveAt(index);
        }

        public void AddTestSetIndex(int index)
        {
            m_list_test_set.Add(index);
        }

        public void RemoveTestSetIndex(int index)
        {
            m_list_test_set.RemoveAt(index);
        }
        #endregion

        public DataTable Analyze(DataTable ip_rawDataTable)
        {
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
            m_dt_analyzed = v_dataTable;
            return v_dataTable;
        }
        /// <summary>
        /// Phân tích tập dữ liệu từ tệp tin định dạng: Comma delimited
        /// </summary>
        /// <param name="ip_str_fname">Tên đường dẫn tệp tin</param>
        /// <returns>DataTable</returns>
        public DataTable Analyze(string ip_str_fname)
        {
            StreamReader v_reader = null;
            try
            {
                var v_dataTable = new DataTable("Raw Data");
                var v_stream = new FileStream(ip_str_fname, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                v_reader = new StreamReader(v_stream); // File.OpenText(ip_str_fname);
                var v_str_line = string.Empty;
                var v_db_value = default(Double);
                var v_dt_date = default(DateTime);
                //var v_bl_header_ok = false;
                // Khởi tạo bảng thông tin
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
                                v_dataCol.Caption = v_str_colName;
                                v_dataTable.Columns.Add(v_dataCol);
                            }

                            // Thoát khỏi vòng lặp đọc header
                            // break;
                            goto lbl_header_ok;
                        }
                        // else
                        v_dataCol.Caption = v_str_tokens[j];
                        v_dataTable.Columns.Add(v_dataCol);
                    }
                }
                // đọc thêm 1 dòng nếu lần đọc ở trên chứa thông tin (header) dữ liệu
                v_str_line = v_reader.ReadLine();

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
                }
                #endregion


                #region Đọc nốt các mẫu dữ liệu còn lại
                while ((v_str_line = v_reader.ReadLine()) != null)
                {
                    // Chỉ đọc định dạng được ngăn cách bởi dấu ',' ở mỗi dòng
                    var v_str_tokens = v_str_line.Split(',');
                    var v_dataRow = v_dataTable.NewRow();
                    // Đọc dữ liệu của mẫu: inputsCount
                    for (int j = 0; j < v_str_tokens.Length; j++)
                    {
                        var v_column_details = (ColumnDetails)v_dataTable.Columns[j].ExtendedProperties["Details"];
                        if (string.IsNullOrEmpty(v_str_tokens[j]) == true)
                        {
                            v_str_tokens[j] = "null";
                            v_dataRow.RowError = "Anomaly"; // Đánh dấu hàng này có lỗi
                        }
                        else if (double.TryParse(v_str_tokens[j], out v_db_value) == true)
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
                        else if (DateTime.TryParseExact(v_str_tokens[j], m_str_date_format, CultureInfo.CurrentCulture, DateTimeStyles.None, out v_dt_date) == true)
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
                        else if (v_column_details.Categories.Contains(v_str_tokens[j]) == false)
                        {
                            if (v_column_details.Format == ColumnFormat.Categorical)
                            {
                                v_column_details.Categories.Add(v_str_tokens[j]);
                            }
                            else
                            {
                                v_dataRow.RowError = "Anomaly"; // Đánh dấu hàng này có lỗi
                            }
                        }
                        v_dataRow[j] = v_str_tokens[j];
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
                m_dt_analyzed = v_dataTable;
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

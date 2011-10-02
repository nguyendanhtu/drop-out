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

        public string DateFormat
        {
            get { return m_str_date_format; }
            set { m_str_date_format = value; }
        }

        public DataTable Analyze(DataTable ip_rawDataTable)
        {
            throw new NotImplementedException();
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
                v_reader = File.OpenText(ip_str_fname);
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
                        if (string.IsNullOrEmpty(v_str_tokens[j]) == true)
                        {
                            v_str_tokens[j] = "null";
                            v_column_details.Format = ColumnFormat.Unknow;
                            v_dataRow.RowError = "Anomaly"; // Đánh dấu hàng này có lỗi
                            v_dataTable.Columns[j].ExtendedProperties.Add("Error", true);
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

                while ((v_str_line = v_reader.ReadLine()) != null)
                {
                    // Chỉ đọc định dạng được ngăn cách bởi dấu ',' ở mỗi dòng
                    var v_str_tokens = v_str_line.Split(',');
                    var v_dataRow = v_dataTable.NewRow();
                    // Đọc dữ liệu của mẫu: inputsCount
                    for (int j = 0; j < v_str_tokens.Length; j++)
                    {
                        v_dataRow[j] = v_str_tokens[j];
                    }
                    // thêm được 1 mẫu mới: v_dataRow
                    v_dataTable.Rows.Add(v_dataRow);
                }
                // data table
                return v_dataTable;
            }
            catch (IOException ex)
            {
                throw new IOException("Failed reading the file", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("File format is not correct", ex);
            }
            finally
            {
                if (v_reader != null)
                    v_reader.Close();
            }

            throw new NotImplementedException();
        }
    }
}

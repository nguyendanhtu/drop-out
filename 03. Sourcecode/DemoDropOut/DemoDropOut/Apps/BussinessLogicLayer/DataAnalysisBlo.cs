using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using C1.Win.C1FlexGrid;
using System.Data;
using System.IO;

namespace DemoDropOut.Apps.BussinessLogicLayer
{
    public class DataAnalysisBlo
    {
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
                // Khởi tạo bảng thông tin
                if ((v_str_line = v_reader.ReadLine()) != null)
                {
                    var v_str_tokens = v_str_line.Split(',');
                    // đọc các cột hiện có (header)
                    double v_db_value = 0;
                    for (int j = 0; j < v_str_tokens.Length; j++)
                    {
                        var v_dataCol = new DataColumn(v_str_tokens[j]);
                        if (double.TryParse(v_str_tokens[j], out v_db_value) == true)
                        {
                            #region Đọc là mẫu 1 nếu không có header
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
                            // đọc & đưa dữ liệu là 1 bản ghi (mẫu)
                            var v_dataRow = v_dataTable.NewRow();
                            for (j = 0; j < v_str_tokens.Length; j++)
                            {
                                v_dataRow[j] = double.Parse(v_str_tokens[j]);
                            }

                            // thêm được 1 mẫu mới: v_dataRow
                            v_dataTable.Rows.Add(v_dataRow);

                            #endregion
                            // Thoát khỏi vòng lặp đọc header
                            break;
                        }
                        // else
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

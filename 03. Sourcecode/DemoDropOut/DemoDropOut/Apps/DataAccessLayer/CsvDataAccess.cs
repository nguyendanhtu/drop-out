using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace DemoDropOut.Apps.DataAccessLayer
{
    public static class CsvDataAccess
    {
        /// <summary>
        /// Đọc dữ liệu định dạng comma delimited
        /// </summary>
        /// <param name="ip_fileName">Tên file ?</param>
        /// <returns></returns>
        public static DataTable OpenCommaDelimitedFile(string ip_fileName)
        {
            StreamReader v_reader = null;
            try
            {
                var v_dataTable = new DataTable("Encoded Data");
                v_reader = File.OpenText(ip_fileName);
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
        }

        public static DataTable OpenEncodedDataFormFile(string ip_fileName)
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
        }

        public static double[][] ToDoubles(DataTable ip_table_inputs)
        {
            var _samples = ip_table_inputs.Rows.Count;
            var _variables = ip_table_inputs.Columns.Count;

            var _input = new double[_samples][];

            for (int i = 0; i < _samples; i++)
            {
                _input[i] = new double[_variables];

                // set value output
                for (int j = 0; j < _variables; j++)
                {
                    var _value = ip_table_inputs.Rows[i][j].ToString();
                    _input[i][j] = double.Parse(_value);
                }
            }
            return _input;
        }
    }
}

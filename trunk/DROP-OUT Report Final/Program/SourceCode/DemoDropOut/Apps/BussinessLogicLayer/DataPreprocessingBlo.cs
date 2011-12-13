using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.Win.C1FlexGrid;
using System.Data;
using DemoDropOut.Apps.Objects;
using DemoDropOut.Common;
using System.Globalization;
using System.Collections;

namespace DemoDropOut.Apps.BussinessLogicLayer
{
    public class DataPreprocessingBlo
    {
        public DataPreprocessingBlo()
        {

        }
        /// <summary>
        ///Julian date start
        /// </summary>
        private static DateTime m_Julian_Date = DateTime.ParseExact("01/01/1900", "MM/dd/yyyy", CultureInfo.CurrentCulture);
        /// <summary>
        /// Date time format
        /// </summary>
        private string m_str_date_format = "MM/dd/yyyy";
        private CategoricalEncoding m_cate_enc = CategoricalEncoding.Binary;
        private DateEncoding m_date_enc = DateEncoding.Weekly;
        /// <summary>
        /// Categorical Encoding
        /// </summary>
        public CategoricalEncoding CategoricalEncoding
        {
            set { m_cate_enc = value; }
            get { return m_cate_enc; }
        }
        /// <summary>
        /// Date Encoding
        /// </summary>
        public DateEncoding DateEncoding
        {
            set { m_date_enc = value; }
        }

        private DataTable m_dt_training_set_enc = null;
        private DataTable m_dt_validation_set_enc = null;
        private DataTable m_dt_test_set_enc = null;
        private bool m_bl_processedData = false;

        public bool IsProcessedData
        {
            get { return m_bl_processedData; }
        }

        public DataTable TrainingSet
        {
            get { return m_dt_training_set_enc; }
            set
            {
                m_dt_training_set_enc = value;
                m_bl_processedData = false;
            }
        }

        public DataTable ValidationSet
        {
            get { return m_dt_validation_set_enc; }
            set { m_dt_validation_set_enc = value; }
        }

        public DataTable TestSet
        {
            get { return m_dt_test_set_enc; }
            set { m_dt_test_set_enc = value; }
        }


        /// <summary>
        /// Tập dữ liệu đã được tiền xử lý
        /// Bao gồm: training set, validation set, test set
        /// </summary>
        public DataTable EncodedData
        {
            get
            {
                var v_processed_data = m_dt_training_set_enc.Copy();
                v_processed_data.MergeTableRows(m_dt_validation_set_enc);
                v_processed_data.MergeTableRows(m_dt_test_set_enc);
                return v_processed_data;
            }
        }
        /// <summary>
        /// Chuỗi định dạng ngày tháng
        /// </summary>
        private string DateTimeFormat
        {
            set { m_str_date_format = value; }
        }
        /// <summary>
        /// Scaling numeric
        /// </summary>
        /// <param name="Xactual">Giá trị X</param>
        /// <param name="Xmax">X max</param>
        /// <param name="Xmin">X min</param>
        /// <param name="SRlower">Scaling range lower</param>
        /// <param name="SRupper">Scaling range upper</param>
        /// <returns></returns>
        private double ScalingNumeric(double Xactual, double Xmax, double Xmin, double SRlower, double SRupper)
        {
            var sf = (SRupper - SRlower) / (Xmax - Xmin); // scaling factor
            return (SRlower + (Xactual - Xmin) * sf);
        }

        private double ScalingNumeric(double Xactual, double Xmin, double sf, double SRlower)
        {
            return (SRlower + (Xactual - Xmin) * sf);
        }

        private DataTable EncodeNumeric(DataColumn column)
        {
            var v_table = column.Table;
            var v_col = v_table.Columns.IndexOf(column);
            var v_table_ = new DataTable();
            v_table_.Columns.Add(column.ColumnName);
            var v_column_details = (DataColumnDetails)column.ExtendedProperties["Details"];
            var sf = v_column_details.ScalingFactor; // scaling factor
            for (int i = 0; i < v_table.Rows.Count; i++)
            {
                var v_str_value = v_table.Rows[i][v_col].ToString();
                var v_actual_value = double.Parse(v_str_value);
                var v_new_row = v_table_.NewRow();
                v_new_row[0] = ScalingNumeric(v_actual_value, v_column_details.MinValue, sf, v_column_details.ScalingRange.Lower);
                #region Backup
                //if (v_column_details.Type == ColumnType.Input)
                //{
                //    //v_new_row[0] = ScalingNumeric(v_actual_value, v_column_details.MaxValue, v_column_details.MinValue, -1, 1);
                //    v_new_row[0] = ScalingNumeric(v_actual_value, v_column_details.MinValue, sf, v_column_details.ScalingRange.Lower);
                //}
                //else
                //{
                //    //v_new_row[0] = ScalingNumeric(v_actual_value, v_column_details.MaxValue, v_column_details.MinValue, 0, 1);
                //    v_new_row[0] = ScalingNumeric(v_actual_value, v_column_details.MinValue, sf, v_column_details.ScalingRange.Lower);
                //}

                #endregion
                v_table_.Rows.Add(v_new_row);
            }
            return v_table_;
        }

        /// <summary>
        /// Encode value by sine function
        /// </summary>
        /// <param name="value">Giá trị cột</param>
        /// <param name="periodicity">Giá trị chu kỳ</param>
        /// <returns>double</returns>
        private double EncodeDateTimeBySin(DateTime date, double periodicity)
        {
            var v_span_ = date - m_Julian_Date;
            var v_serial_ = v_span_.Days + 2;
            return Math.Sin(Math.PI * ((v_serial_ >> 1) / periodicity));
        }
        /// <summary>
        /// Encode value by cosine function
        /// </summary>
        /// <param name="value">Giá trị cột</param>
        /// <param name="periodicity">Giá trị chu kỳ</param>
        /// <returns>double</returns>
        private double EncodeDateTimeByCos(DateTime date, double periodicity)
        {
            var v_span_ = date - m_Julian_Date;
            var v_serial_ = v_span_.Days + 2;
            return Math.Cos(Math.PI * ((v_serial_ >> 1) / periodicity));
        }
        /// <summary>
        /// Mã hóa dạng ngày tháng
        /// Viết thêm module tự động nhận dạng kiểu ngày tháng nhập vào: MM/dd/yyyy hay dd/MM/yyyy bằng cách so sánh (tồn tại trường dữ liệu 24 >= value >= 12 thì là ngày khác thì value là tháng).
        /// </summary>
        /// <param name="dateTimeColumn"></param>
        /// <param name="rowsCount"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        private DataTable EncodeDateTime(DataColumn column, DataColumnFormat format)
        {
            var v_table = column.Table;
            var v_col = v_table.Columns.IndexOf(column);
            var v_table_enc = new DataTable(v_table.TableName);
            v_table_enc.Columns.Add(column.ColumnName + ": sin"); // sine column
            v_table_enc.Columns.Add(column.ColumnName + ": cos"); // cosine column
            // Format column
            var periodicity = 7;
            if (format == DataColumnFormat.Date)
            {
                periodicity = 7;
                //goto lbl_parse_date;
            }
            else if (format == DataColumnFormat.Time)
            {
                periodicity = 24;
                //goto lbl_parse_time;
            }
            else
            {
                throw new Exception("Dữ liệu mã hóa cần phải là kiểu ngày tháng");
            }
            //lbl_parse_date: ;
            for (int i = 0; i < v_table.Rows.Count; i++)
            {
                var v_str_value = v_table.Rows[i][v_col] as string;
                var v_dt_value = DateTime.ParseExact(v_str_value, m_str_date_format, CultureInfo.CurrentCulture);
                var v_new_row = v_table_enc.NewRow();
                // encode ...
                v_new_row[0] = EncodeDateTimeBySin(v_dt_value, periodicity);
                v_new_row[1] = EncodeDateTimeByCos(v_dt_value, periodicity);
                v_table_enc.Rows.Add(v_new_row);
            }
            //lbl_parse_time: ;
            //for (int i = 0; i < rowsCount; i++)
            //{
            //    var v_str_value = dateTimeColumn[i] as string;
            //    var v_new_row = v_table_enc.NewRow();
            //}
            //throw new NotImplementedException();
            return v_table_enc;
        }

        /// <summary>
        /// Module: Tiền xử lý dữ liệu dạng categorical
        /// Mã hóa dạng: One of N
        /// </summary>
        /// <param name="categoricalColumn">Cột dữ liệu</param>
        /// <param name="rowsCount">Số hàng có trong cột</param>
        /// <param name="category">Dữ liệu văn bản có trong cột</param>
        /// <returns></returns>
        private DataTable EncodeCategoricalColumnByOneOfN(DataColumn column, IList<string> category)
        {
            var v_table = column.Table;
            var v_col = v_table.Columns.IndexOf(column);
            var v_cat_count = category.Count;
            var v_table_enc = new DataTable(column.ColumnName);

            // Đặt tên cột
            for (int i = 0; i < v_cat_count; i++)
            {
                var v_new_colName = string.Format("{0}: {1}", column.ColumnName, category[i]);
                v_table_enc.Columns.Add(v_new_colName);
            }

            for (int i = 0; i < v_table.Rows.Count; i++)
            {
                var v_str_value = v_table.Rows[i][v_col] as string;
                var v_new_row = v_table_enc.NewRow();
                for (int j = 0; j < v_cat_count; j++)
                {
                    if (v_str_value == category[j])
                    {
                        v_new_row[j] = 1;
                    }
                    else
                    {
                        v_new_row[j] = 0;
                    }
                }
                v_table_enc.Rows.Add(v_new_row);
            }

            return v_table_enc;
        }
        /// <summary>
        /// Khuyến cáo sử dụng mã hóa dạng One-Of-N
        /// Bởi vì tính tự do của mạng
        /// Và quá trính giải mã không bị mất dữ liệu
        /// ( Không giống như trường hợp Binary chỉ số vượt khỏi danh sách
        /// </summary>
        /// <param name="ip_db_outputs"></param>
        /// <param name="ip_column_details"></param>
        /// <returns></returns>
        private DataTable DecodeCategoricalColumnByOneOfN(double[][] ip_db_outputs, DataColumnDetails ip_column_details)
        {
            var category = ip_column_details.Categories;
            var v_ouputs_count = ip_db_outputs.GetLength(0);
            var v_cat_count = category.Count;

            var v_dt_table = new DataTable();
            v_dt_table.Columns.Add(ip_column_details.ColumnName);
            for (int i = 0; i < v_ouputs_count; i++)
            {
                var v_new_row = v_dt_table.NewRow();

                #region Version 1: Có thể không tìm được lời giải
                //for (int j = 0; j < v_cat_count; j++)
                //{
                //    var v_db_ouput = (int)Math.Round(ip_db_outputs[i][j]);
                //    if (v_db_ouput == 1)
                //    {
                //        v_new_row[0] = category[j];
                //        break;
                //    }
                //}
                #endregion

                #region Version 2: Chắc chắn là tìm được lời giải
                // Tìm thằng có chỉ số lớn nhất trong category. :D (One Of N)
                var index = 0;    // Chỉ số có giá trị lớn nhất
                var max_value = ip_db_outputs[i][index];    // Gán giá trị lớn nhất (giả sử)

                for (int j = index + 1; j < v_cat_count; j++)
                {
                    if (ip_db_outputs[i][j] > max_value)
                    {
                        index = j;
                        max_value = ip_db_outputs[i][j];
                    }
                }
                v_new_row[0] = category[index];
                #endregion
                // Add row was decoded.
                v_dt_table.Rows.Add(v_new_row);
            }

            return v_dt_table;
        }

        private DataTable EncodeCategoricalComlumnByBinary(DataColumn column, IList<string> category)
        {
            // Lấy cột thứ columnIndex để mã
            var v_table = column.Table;
            var v_col = column.Ordinal;// v_table.Columns.IndexOf(column);
            // Số bit dùng để lưu category.Count giá trị: v_cat_enc
            // Tương ứng với số cột được sử dụng để lưu trạng thái của category item
            var v_cat_enc = Math.Log(category.Count, 2);
            // Tạo bảng dữ liệu lưu cột mới
            var v_table_enc = new DataTable(column.ColumnName);

            // Đặt tên cột bằng với số bit dùng
            for (int i = 0; i < v_cat_enc; i++)
            {
                var v_new_caption = string.Format("{0}: B{1}", column.ColumnName, i + 1);
                var v_new_column = new DataColumn(v_new_caption);
                v_new_column.Caption = v_new_caption;
                v_table_enc.Columns.Add(v_new_column);
            }

            for (int i = 0; i < v_table.Rows.Count; i++)
            {
                var v_str_value = v_table.Rows[i][v_col] as string;
                var v_new_row = v_table_enc.NewRow();
                for (int j = 0; j < category.Count; j++)
                {
                    if (v_str_value == category[j])
                    {
                        // Tìm thấy value tại vị trí j
                        // Biểu diễn j dưới dạng nhị phân rồi lưu tương ứng vào các cột
                        for (var k = 0; k < v_cat_enc; k++)
                        {
                            // Lưu giá trị các cột tương ứng giá trị nhị phân của j
                            v_new_row[k] = (j >> k) & 1;
                        }
                        break;
                    }
                }
                v_table_enc.Rows.Add(v_new_row);
            }

            return v_table_enc;
        }

        public DataTable DecodeCategoricalColumn(double[][] ip_db_ouputs, DataColumnDetails ip_column_details, CategoricalEncoding enc)
        {
            if (enc == CategoricalEncoding.OneOfN)
            {
                return DecodeCategoricalColumnByOneOfN(ip_db_ouputs, ip_column_details);
            }
            else if (enc == CategoricalEncoding.Binary)
            {
                return DecodeCategoricalColumnByBinary(ip_db_ouputs, ip_column_details);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public DataTable DecodeCategoricalColumnByBinary(double[][] ip_db_outputs, DataColumnDetails ip_column_details)
        {
            var category = ip_column_details.Categories;
            var v_outputs_count = ip_db_outputs.GetLength(0);
            var v_cate_enc = Math.Log(category.Count, 2);
            //var v_cate_enc = ip_db_outputs.GetLength(1);
            //var v_actual_enc = Math.Log(category.Count, 2);
            //System.Diagnostics.Debug.Assert(v_cate_enc == v_actual_enc, "Không đồng bộ cột dữ liệu ra với category list");
            var v_dt_table = new DataTable();
            v_dt_table.Columns.Add(ip_column_details.ColumnName);

            for (int i = 0; i < v_outputs_count; i++)
            {
                var v_cate_index = 0; // j value
                var k = ip_db_outputs[i].GetLength(0) - 1;  // get index array
                // Duyệt các phần tử mảng bởi chỉ số k
                for (; k >= 0; k--)
                {
                    var v_output_value = ip_db_outputs[i][k];   // actual value
                    var v_binary_value = (int)Math.Round(v_output_value);// binary value (rounded)
                    v_cate_index = (v_cate_index << 1) | v_binary_value;
                    if (v_cate_index >= category.Count)
                    {
                        // Không tìm được lời giải
                        // --> Sử dụng hàm Predict()
                    }
                }
                var v_dt_row = v_dt_table.NewRow();
                var v_str_cname = Predict(category, v_cate_index);  // category[v_cate_index];
                v_dt_row[0] = v_str_cname;
                v_dt_table.Rows.Add(v_dt_row);
            }
            return v_dt_table;
        }

        // Đệ quy lấy giá trị gần nhất với v_cate_index nếu nó vượt quá chỉ số mảng
        private string Predict(IList<string> source, int ip_over_index)
        {
            try
            {
                return source[ip_over_index];
            }
            catch (Exception) //IndexOutOfRangeException
            {
                return Predict(source, ip_over_index - 1);
            }
        }

        public void Preprocessing()
        {
            m_dt_training_set_enc = Preprocessing(m_dt_training_set_enc);
            m_dt_validation_set_enc = Preprocessing(m_dt_validation_set_enc);
            m_dt_test_set_enc = Preprocessing(m_dt_test_set_enc);
            m_bl_processedData = true;
        }

        public DataTable Preprocessing(DataTable ip_raw_data)
        {
            if (ip_raw_data == null)
                throw new ArgumentNullException("ip_raw_data");
            var v_enc_data = new DataTable(ip_raw_data.TableName);

            for (int i = 0; i < ip_raw_data.Columns.Count; i++)
            {
                var v_column_ = ip_raw_data.Columns[i];
                var v_column_details = (DataColumnDetails)v_column_.ExtendedProperties["Details"];
                DataTable v_table_ = null;
                switch (v_column_details.Format)
                {
                    case DataColumnFormat.Numerical:
                        v_table_ = EncodeNumeric(v_column_);
                        break;
                    case DataColumnFormat.Date:
                        v_table_ = EncodeDateTime(v_column_, DataColumnFormat.Date);
                        break;
                    case DataColumnFormat.Time:
                        v_table_ = EncodeDateTime(v_column_, DataColumnFormat.Time);
                        break;
                    case DataColumnFormat.Categorical:
                    default:
                        if (this.m_cate_enc == CategoricalEncoding.Binary)
                        {
                            v_table_ = EncodeCategoricalComlumnByBinary(v_column_, v_column_details.Categories);
                        }
                        else if (this.m_cate_enc == CategoricalEncoding.OneOfN)
                        {
                            v_table_ = EncodeCategoricalColumnByOneOfN(v_column_, v_column_details.Categories);
                        }
                        break;
                }
                if (v_table_ == null)
                    continue;
                if (v_column_details.Type == DataColumnType.Ouput)
                {
                    v_enc_data.ExtendedProperties["OutputIndex"] = v_enc_data.Columns.Count;
                    v_enc_data.ExtendedProperties["OutputCount"] = v_table_.Columns.Count;
                }
                DataHelper.MergeTableColumns(ref v_enc_data, v_table_);
            }
            return v_enc_data;
        }

        #region Chuyển đổi dữ liệu bảng sang mảng double 2 chiều

        public double[][] TrainingSetToDoubles()
        {
            return m_dt_training_set_enc.ToDoubles();
        }

        public double[][] TrainingSetInputToDoubles()
        {
            var v_output_index = (int)m_dt_training_set_enc.ExtendedProperties["OutputIndex"];
            var v_output_count = (int)m_dt_training_set_enc.ExtendedProperties["OutputCount"];
            var v_index2 = v_output_index + v_output_count;
            return m_dt_training_set_enc.ToDoubles(0, v_output_index, v_index2, m_dt_training_set_enc.Columns.Count - v_index2);
        }

        public double[][] TrainingSetOutputToDoubles()
        {
            var v_output_index = (int)m_dt_training_set_enc.ExtendedProperties["OutputIndex"];
            var v_output_count = (int)m_dt_training_set_enc.ExtendedProperties["OutputCount"];
            return m_dt_training_set_enc.ToDoubles(v_output_index, v_output_count);
        }

        public double[][] ValidationSetToDoubles()
        {
            return m_dt_validation_set_enc.ToDoubles();
        }

        public double[][] ValidationSetInputToDoubles()
        {
            var v_output_index = (int)m_dt_training_set_enc.ExtendedProperties["OutputIndex"];
            var v_output_count = (int)m_dt_training_set_enc.ExtendedProperties["OutputCount"];
            var v_index2 = v_output_index + v_output_count;
            return m_dt_validation_set_enc.ToDoubles(0, v_output_index, v_index2, m_dt_validation_set_enc.Columns.Count - v_index2);
        }

        public double[][] ValidationSetOutputToDoubles()
        {
            var v_output_index = (int)m_dt_validation_set_enc.ExtendedProperties["OutputIndex"];
            var v_output_count = (int)m_dt_validation_set_enc.ExtendedProperties["OutputCount"];
            return m_dt_validation_set_enc.ToDoubles(v_output_index, v_output_count);
        }

        public double[][] TestSetToDoubles()
        {
            return m_dt_test_set_enc.ToDoubles();
        }

        public double[][] TestSetInputToDoubles()
        {
            var v_output_index = (int)m_dt_test_set_enc.ExtendedProperties["OutputIndex"];
            var v_output_count = (int)m_dt_test_set_enc.ExtendedProperties["OutputCount"];
            var v_index2 = v_output_index + v_output_count;
            return m_dt_test_set_enc.ToDoubles(0, v_output_index, v_index2, m_dt_test_set_enc.Columns.Count - v_index2);
        }

        public double[][] TestSetOutputToDoubles()
        {
            var v_output_index = (int)m_dt_test_set_enc.ExtendedProperties["OutputIndex"];
            var v_output_count = (int)m_dt_test_set_enc.ExtendedProperties["OutputCount"];
            return m_dt_test_set_enc.ToDoubles(v_output_index, v_output_count);
        }

        #endregion

        #region Phác thảo tham số luyện mạng

        public int Classes
        {
            get
            {
                return (int)m_dt_training_set_enc.ExtendedProperties["OutputCount"];
            }
        }

        public int Variables
        {
            get
            {
                return m_dt_training_set_enc.Columns.Count - (int)m_dt_training_set_enc.ExtendedProperties["OutputCount"];
            }
        }

        public int HiddenNeurons
        {
            get
            {
                return (Variables >> 1) + 1;
            }
        }
        #endregion
    }
}

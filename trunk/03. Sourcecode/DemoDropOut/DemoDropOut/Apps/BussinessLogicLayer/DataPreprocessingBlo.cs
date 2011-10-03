using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.Win.C1FlexGrid;
using System.Data;
using DemoDropOut.Apps.Objects;
using System.Globalization;
using System.Collections;

namespace DemoDropOut.Apps.BussinessLogicLayer
{
    public class DataPreprocessingBlo
    {
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
        }
        /// <summary>
        /// Date Encoding
        /// </summary>
        public DateEncoding DateEncoding
        {
            set { m_date_enc = value; }
        }

        private C1FlexGrid m_gr_raw_data = null;
        private DataTable m_dt_enc_data = null;
        /// <summary>
        /// Tập dữ liệu đã được tiền xử lý
        /// </summary>
        public DataTable EncodedData
        {
            get { return m_dt_enc_data; }
            set { m_dt_enc_data = value; }
        }
        /// <summary>
        /// Tập dữ liệu thô
        /// </summary>
        public C1FlexGrid RawData
        {
            get { return m_gr_raw_data; }
            set { m_gr_raw_data = value; }
        }
        /// <summary>
        /// Chuỗi định dạng ngày tháng
        /// </summary>
        private string DateTimeFormat
        {
            set { m_str_date_format = value; }
        }

        private double ScalingNumeric(double actual, double Xmax, double Xmin, double lower, double upper)
        {
            var sf = (upper - lower) / (Xmax - Xmin); // scaling factor
            return (lower + (actual - Xmin) * sf);
        }

        private DataTable EncodeNumeric(int columnIndex)
        {
            var v_numeric_column = m_gr_raw_data.Cols[columnIndex];
            var v_table_ = new DataTable();
            v_table_.Columns.Add(v_numeric_column.Name);
            var v_column_details = (ColumnDetails)v_numeric_column.UserData;
            var v_rows = m_gr_raw_data.Rows.Count - m_gr_raw_data.Rows.Fixed;
            for (int i = m_gr_raw_data.Rows.Fixed; i < m_gr_raw_data.Rows.Count; i++)
            {
                var v_actual_value = double.Parse(v_numeric_column[i].ToString());
                var v_new_row = v_table_.NewRow();
                if (v_column_details.Type == ColumnType.Input)
                {
                    v_new_row[0] = ScalingNumeric(v_actual_value, v_column_details.MaxValue, v_column_details.MinValue, -1, 1);
                }
                else
                {
                    v_new_row[0] = ScalingNumeric(v_actual_value, v_column_details.MaxValue, v_column_details.MinValue, 0, 1);
                }
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
        private DataTable EncodeDateTime(int columnIndex, ColumnFormat format)
        {
            var dateTimeColumn = m_gr_raw_data.Cols[columnIndex];
            var v_table_enc = new DataTable(dateTimeColumn.Name);
            v_table_enc.Columns.Add(dateTimeColumn.Name + ": sin"); // sine column
            v_table_enc.Columns.Add(dateTimeColumn.Name + ": cos"); // cosine column
            // Format column
            var periodicity = 7;
            if (format == ColumnFormat.Date)
            {
                periodicity = 7;
                //goto lbl_parse_date;
            }
            else if (format == ColumnFormat.Time)
            {
                periodicity = 24;
                //goto lbl_parse_time;
            }
            else
            {
                throw new Exception("Dữ liệu mã hóa cần phải là kiểu ngày tháng");
            }
            //lbl_parse_date: ;
            for (int i = m_gr_raw_data.Rows.Fixed; i < m_gr_raw_data.Rows.Count; i++)
            {
                var v_str_value = dateTimeColumn[i] as string;
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
        private DataTable EncodeCategoricalColumnByOneOfN(int columnIndex, IList<string> category)
        {
            var categoricalColumn = m_gr_raw_data.Cols[columnIndex];
            var v_cat_count = category.Count;
            // var v_enc_cols = new ColumnCollection(v_cat_count, 0, 10, 500, 150, "Chưa biết là gì?");
            var v_table_enc = new DataTable(categoricalColumn.Name);

            // Đặt tên cột
            for (int i = 0; i < v_cat_count; i++)
            {
                var v_new_colName = string.Format("{0}: {1}", categoricalColumn.Name, category[i]);
                v_table_enc.Columns.Add(v_new_colName);
            }

            for (int i = m_gr_raw_data.Rows.Fixed; i < m_gr_raw_data.Rows.Count; i++)
            {
                var v_str_value = categoricalColumn[i] as string;
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
            }

            return v_table_enc;
        }

        private Column DecodeCategoricalColumnByOneOfN(DataColumnCollection columnCollection, int rowsCount, params string[] category)
        {
            throw new NotImplementedException();
        }

        private DataTable EncodeCategoricalComlumnByBinary(int columnIndex, IList<string> category)
        {
            // Lấy cột thứ columnIndex để mã
            var categoricalColumn = m_gr_raw_data.Cols[columnIndex];
            // Số bit dùng để lưu category.Count giá trị: v_cat_enc
            // Tương ứng với số cột được sử dụng để lưu trạng thái của category item
            var v_cat_enc = Math.Log(category.Count, 2);
            // Tạo bảng dữ liệu lưu cột mới
            var v_table_enc = new DataTable(categoricalColumn.Name);

            // Đặt tên cột
            for (int i = 0; i < v_cat_enc; i++)
            {
                var v_new_caption = string.Format("{0}: B{1}", categoricalColumn.Name, i);
                var v_new_column = new DataColumn(v_new_caption);
                v_new_column.Caption = v_new_caption;
                v_table_enc.Columns.Add(v_new_column);
            }

            for (int i = m_gr_raw_data.Rows.Fixed; i < m_gr_raw_data.Rows.Count; i++)
            {
                var v_str_value = categoricalColumn[i] as string;
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

        private Column DecodeCategoricalColumnByBinary(DataColumnCollection columnCollection, int rowsCount, params string[] category)
        {
            throw new NotImplementedException();
        }

        public DataTable Preprocessing()
        {
            if (m_gr_raw_data == null)
                throw new Exception("Dữ liệu trống (null). Không thể tiền xử lý");
            m_dt_enc_data = new DataTable("Encoded Data");
            //var v_cols_ = m_gr_raw_data.Cols.Count - m_gr_raw_data.Cols.Fixed;
            //var v_rows_ = m_gr_raw_data.Rows.Count - m_gr_raw_data.Rows.Fixed;
            for (int i = m_gr_raw_data.Cols.Fixed; i < m_gr_raw_data.Cols.Count; i++)
            {
                var v_column_ = m_gr_raw_data.Cols[i];
                var v_column_details = (ColumnDetails)v_column_.UserData;
                DataTable v_table_ = null;
                switch (v_column_details.Format)
                {
                    case ColumnFormat.Numerical:
                        v_table_ = EncodeNumeric(i);
                        break;
                    case ColumnFormat.Date:
                        v_table_ = EncodeDateTime(i, ColumnFormat.Date);
                        break;
                    case ColumnFormat.Time:
                        v_table_ = EncodeDateTime(i, ColumnFormat.Time);
                        break;
                    case ColumnFormat.Categorical:
                    default:
                        if (this.m_cate_enc == CategoricalEncoding.Binary)
                        {
                            v_table_ = EncodeCategoricalComlumnByBinary(i, v_column_details.Categories);
                        }
                        else if (this.m_cate_enc == CategoricalEncoding.OneOfN)
                        {
                            v_table_ = EncodeCategoricalColumnByOneOfN(i, v_column_details.Categories);
                        }
                        break;
                }
                if (v_table_ == null)
                    continue;
                MergeTable(ref m_dt_enc_data, v_table_);
            }
            //throw new NotImplementedException();
            return m_dt_enc_data;
        }

        public DataTable Preprocessing(C1FlexGrid ip_grid_raw_data)
        {
            this.m_gr_raw_data = ip_grid_raw_data;
            return Preprocessing();
        }

        private void MergeTable(ref DataTable ip_dt_dest, DataTable ip_dt_src)
        {
            if(ip_dt_dest.Rows.Count <= 0)
            {
                var v_str_bak = ip_dt_dest.TableName;
                ip_dt_dest = ip_dt_src;
                ip_dt_dest.TableName = v_str_bak;
                return;
            }
            var v_col_bak = ip_dt_dest.Columns.Count;
            for (int j = 0; j < ip_dt_src.Columns.Count; j++)
            {
                ip_dt_dest.Columns.Add(ip_dt_src.Columns[j].ColumnName, ip_dt_src.Columns[j].DataType);
                for (int i = 0; i < ip_dt_src.Rows.Count; i++)
                {
                    ip_dt_dest.Rows[i][v_col_bak + j] = ip_dt_src.Rows[i][j];
                }
            }
        }
    }
}

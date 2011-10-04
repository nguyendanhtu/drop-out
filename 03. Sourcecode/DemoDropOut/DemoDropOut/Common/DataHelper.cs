using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DemoDropOut.Common
{
    public static class DataHelper
    {
        /// <summary>
        /// Kết hợp 2 bảng dữ liệu có cùng chung các cột tương ứng
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static void MergeTableRows(this DataTable owner, DataTable source)
        {
            owner.Merge(source);
            owner.AcceptChanges();
        }

        public static void MergeTableColumns(ref DataTable ip_dt_dest, DataTable ip_dt_src)
        {
            if (ip_dt_dest.Rows.Count <= 0)
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
            //ip_dt_dest.AcceptChanges();
        }


        public static double[][] ToDoubles(this DataTable ip_table_inputs)
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
        /// <summary>
        /// Convert column at ip_column_index to double array
        /// </summary>
        /// <param name="ip_table_inputs"></param>
        /// <param name="ip_column_index"></param>
        /// <returns></returns>
        public static double[][] ToDoubles(this DataTable ip_table_inputs, int ip_column_index)
        {
            if (ip_column_index >= ip_table_inputs.Columns.Count)
            {
                throw new IndexOutOfRangeException("Chỉ số cột vượt quá số cột bảng dữ liệu có");
            }
            var _samples = ip_table_inputs.Rows.Count;
            var _output = new double[_samples][];

            for (int i = 0; i < _samples; i++)
            {
                _output[i] = new double[1];

                // set value output
                var _value = ip_table_inputs.Rows[i][ip_column_index].ToString();
                _output[i][0] = double.Parse(_value);
            }
            return _output;
        }
        /// <summary>
        /// Convert ip_count column to double array start at ip_index
        /// </summary>
        /// <param name="ip_table_inputs"></param>
        /// <param name="ip_index">Index column to convert</param>
        /// <param name="ip_count">Count of column would be converted</param>
        /// <returns></returns>
        public static double[][] ToDoubles(this DataTable ip_table_inputs, int ip_index, int ip_count)
        {
            if (ip_index + ip_count > ip_table_inputs.Columns.Count)
            {
                throw new IndexOutOfRangeException("Số lượng cột trích xuất vượt quá số cột bảng dữ liệu có");
            }
            var _samples = ip_table_inputs.Rows.Count;
            var _output = new double[_samples][];

            for (int i = 0; i < _samples; i++)
            {
                _output[i] = new double[ip_count];

                // set value output
                for (int j = 0; j < ip_count; j++)
                {
                    var _value = ip_table_inputs.Rows[i][ip_index + j].ToString();
                    _output[i][j] = double.Parse(_value);
                }
            }
            return _output;
        }

        public static double[][] ToDoubles(this DataTable ip_table_inputs, int ip_index1, int ip_count1, int ip_index2, int ip_count2)
        {
            if (ip_index1 + ip_count1 > ip_table_inputs.Columns.Count)
            {
                throw new IndexOutOfRangeException("Số lượng cột trích xuất vượt quá số cột bảng dữ liệu có");
            }
            else if (ip_index2 + ip_count2 > ip_table_inputs.Columns.Count)
            {
                throw new IndexOutOfRangeException("Số lượng cột trích xuất vượt quá số cột bảng dữ liệu có");
            }

            var _samples = ip_table_inputs.Rows.Count;
            var _output = new double[_samples][];

            for (int i = 0; i < _samples; i++)
            {
                _output[i] = new double[ip_count1 + ip_count2];

                // set value output 1
                for (int j = 0; j < ip_count1; j++)
                {
                    var _value = ip_table_inputs.Rows[i][ip_index1 + j].ToString();
                    _output[i][j] = double.Parse(_value);
                }
                // set value output 2
                for (int j = 0; j < ip_count2; j++)
                {
                    var _value = ip_table_inputs.Rows[i][ip_index2 + j].ToString();
                    _output[i][ip_index1 + j] = double.Parse(_value);
                }
            }
            return _output;
        }
    }
}

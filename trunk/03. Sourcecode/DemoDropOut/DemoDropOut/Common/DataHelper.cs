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
    }
}

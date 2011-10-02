using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;
using C1.Win.C1FlexGrid;

namespace DemoDropOut.Common
{
    public class C1Utils
    {
        public static void LoadDataTableToC1Grid(C1FlexGrid ip_c1Grid, DataTable ip_table)
        {
            Debug.Assert(ip_c1Grid != null, "Chưa khởi tạo C1 grid control: Null");
            Debug.Assert(ip_table != null, "Bảng không chứa dữ liệu: Null");
            // Xóa dữ liệu trong bảng (bằng cách đặt lại số hàng, số cột)
            ip_c1Grid.Rows.Count = ip_c1Grid.Rows.Fixed; // xóa dữ liệu bằng cách đặt lại số row = fixed
            ip_c1Grid.Cols.Count = ip_c1Grid.Cols.Fixed + ip_table.Columns.Count; //Số cột = số cột fixed + số cột dữ liệu
            ip_c1Grid.Tag = ip_table;

            // Đọc số cột & ghi tiêu đề
            for (int i = ip_c1Grid.Cols.Fixed, table_index = 0; table_index < ip_table.Columns.Count; i++, table_index++)
            {
                //var v_str_caption = ip_table.Columns[table_index].Caption;
                //ip_c1Grid[0, i] = v_str_caption;
                ip_c1Grid.Cols[i].Caption = ip_table.Columns[table_index].Caption;
                ip_c1Grid.Cols[i].Name = ip_table.Columns[table_index].ColumnName;
            }
            for (int i = 0; i < ip_table.Rows.Count; i++)
            {
                // Đọc từng mẫu dữ liệu
                var v_dataRow = ip_table.Rows[i];
                ip_c1Grid.Rows.Add();
                for (int j = 0; j < ip_table.Columns.Count; j++)
                {
                    //ip_c1Grid.Rows[j].UserData = v_dataRow[j];
                    ip_c1Grid[ip_c1Grid.Rows.Count - 1, ip_c1Grid.Cols.Fixed + j] = v_dataRow[j];
                }
            }
        }
    }
}

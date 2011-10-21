using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.Win.C1FlexGrid;
using System.Data;
using System.Diagnostics;

namespace DemoDropOut.Apps.BussinessLogicLayer
{
    public class DataTestingBlo
    {
        /// <summary>
        /// So sánh output so với dữ liệu mẫu
        /// </summary>
        /// <param name="ip_dt_output">Output Result</param>
        /// <param name="ip_dt_sample">Input Sample</param>
        /// <returns>DataTable</returns>
        public static DataTable CompareResultTable(DataTable ip_dt_output, DataTable ip_dt_target)
        {
            var v_dt_result = new DataTable("Testing Table");
            var v_list_index = ip_dt_target.ExtendedProperties["IndexList"] as List<int>;
            Debug.Assert(ip_dt_output.Rows.Count == ip_dt_target.Rows.Count, "Số lượng mẫu đầu vào và ra không hợp lệ");
            Debug.Assert(ip_dt_target.Rows.Count == v_list_index.Count, "Số lượng mẫu với số phần tử danh sách chỉ số mảng không hợp lệ");
            Debug.Assert(v_list_index != null, "Tham số không chứa danh sách chỉ số mảng");
            v_dt_result.Columns.Add("Row", typeof(int));
            v_dt_result.Columns.Add("Target");
            v_dt_result.Columns.Add("Output");
            v_dt_result.Columns.Add("Match");
            // thông tin mẫu
            var v_ouput_target_index = (int)ip_dt_target.ExtendedProperties["OutputIndex"];
            var v_error_count = 0;
            for (int i = 0; i < v_list_index.Count; i++)
            {
                var v_target = ip_dt_target.Rows[i][v_ouput_target_index].ToString();
                var v_output = ip_dt_output.Rows[i][0].ToString(); //ip_dt_output.Rows[i]["Output"].ToString();

                var v_dt_row = v_dt_result.NewRow();
                v_dt_row["Row"] = v_list_index[i];
                v_dt_row["Target"] = v_target;
                v_dt_row["Output"] = v_output;
                v_dt_row["Match"] = "OK";
                if (v_output.Equals(v_target) == false)
                {
                    v_dt_row["Match"] = "Wrong";
                    v_error_count++;
                }
                v_dt_result.Rows.Add(v_dt_row);
            }
            var v_db_ccr = ((double)(v_list_index.Count - v_error_count) * 100) / v_list_index.Count;
            v_dt_result.ExtendedProperties["CCR"] = v_db_ccr;
            return v_dt_result;
        }
        public static void FormatTestingTable(C1FlexGrid ip_c1flex_grid, DataTable ip_dt_result)
        {
            // format grid
            ip_c1flex_grid.DataSource = null;
            ip_c1flex_grid.Rows.Count = 1;
            ip_c1flex_grid.Rows.Fixed = 1;
            ip_c1flex_grid.Cols.Count = 1;
            ip_c1flex_grid.Cols.Fixed = 1;

            ip_c1flex_grid.DataSource = ip_dt_result;
        }
    }
}

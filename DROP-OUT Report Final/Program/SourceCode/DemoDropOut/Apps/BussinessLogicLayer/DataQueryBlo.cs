using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DemoDropOut.Apps.Objects;
using System.Drawing;
using C1.Win.C1FlexGrid;
using System.Windows.Forms;

namespace DemoDropOut.Apps.BussinessLogicLayer
{
    public class DataQueryBlo
    {
        public static void FormatResultTable(C1FlexGrid ip_grid_c1flex, DataTable ip_dt_samples)
        {

        }

        public static void FormatManualC1FlexGrid(C1.Win.C1FlexGrid.C1FlexGrid ip_gird_c1flex, DataTable ip_dt_samples)
        {
            //if (ip_dt_samples.ExtendedProperties["DropOutDataSamples"] == null)
            //{
            //    throw new ArgumentException("DataTable không chứa dữ liệu DropOut", "ip_dt_samples");
            //}
            //else
            {
                var v_output_index = (int)ip_dt_samples.ExtendedProperties["OutputIndex"];
                var v_output_count = (int)ip_dt_samples.ExtendedProperties["OutputCount"];
                var v_input_index = 0;
                var v_input_index_2 = v_output_index + v_output_count;
                var v_input_count = ip_dt_samples.Columns.Count - v_output_count;
                // reset c1flex grid
                ip_gird_c1flex.Cols.Count = 0;
                ip_gird_c1flex.Rows.Count = 0;
                ip_gird_c1flex.Cols.Count = v_input_count;
                ip_gird_c1flex.Cols.Fixed = 0;
                ip_gird_c1flex.Rows.Count = 4;  // 4 rows
                ip_gird_c1flex.Rows.Fixed = 1;
                // add styles
                var v_excess_style = ip_gird_c1flex.Styles.Add("ExcessValue");
                v_excess_style.BackColor = Color.Gold;
                ip_gird_c1flex.AllowSorting = AllowSortingEnum.None;
                // Không thích viền focus và màu highlight
                ip_gird_c1flex.HighLight = HighLightEnum.Never;
                ip_gird_c1flex.FocusRect = FocusRectEnum.Solid;
                //ip_gird_c1flex.SelectionMode = SelectionModeEnum.Cell;
                // set event
                ip_gird_c1flex.CellChanged -= ip_gird_c1flex_CellChanged;
                ip_gird_c1flex.CellChanged += ip_gird_c1flex_CellChanged;
                ip_gird_c1flex.BeforeEdit -= ip_gird_c1flex_BeforeEdit;
                ip_gird_c1flex.BeforeEdit += ip_gird_c1flex_BeforeEdit;
                ip_gird_c1flex.Click += new EventHandler(ip_gird_c1flex_Click);
                //ip_gird_c1flex.KeyDown -= ip_gird_c1flex_KeyDown;
                //ip_gird_c1flex.KeyDown += ip_gird_c1flex_KeyDown;
                //ip_gird_c1flex.KeyPress -= ip_gird_c1flex_KeyPress;
                //ip_gird_c1flex.KeyPress += ip_gird_c1flex_KeyPress;
                // Duyệt cột vào trước cột ra
                for (; v_input_index < v_output_index; v_input_index++)
                {
                    var v_column_details = ip_dt_samples.Columns[v_input_index].ExtendedProperties["Details"] as DataColumnDetails;
                    ip_gird_c1flex.Cols[v_input_index].UserData = v_column_details;
                    ip_gird_c1flex.Cols[v_input_index].Name = ip_dt_samples.Columns[v_input_index].ColumnName;
                    ip_gird_c1flex.Cols[v_input_index].Caption = ip_dt_samples.Columns[v_input_index].ColumnName;

                    if (v_column_details.Format == DataColumnFormat.Categorical)
                    {
                        ip_gird_c1flex.Cols[v_input_index].ComboList = v_column_details.GetCateComboList();
                        ip_gird_c1flex.Cols[v_input_index][2] = "max: n/a";
                        ip_gird_c1flex.Cols[v_input_index][3] = "min: n/a";
                    }
                    else if (v_column_details.Format == DataColumnFormat.Numerical)
                    {
                        // Chưa có chế độ cảnh báo người dùng nhập quá giới hạn min max
                        ip_gird_c1flex.Cols[v_input_index][2] = "max: " + v_column_details.MaxValue;
                        ip_gird_c1flex.Cols[v_input_index][3] = "min: " + v_column_details.MinValue;
                    }
                    else
                    {
                        ip_gird_c1flex.Cols[v_input_index][2] = "max: n/a";
                        ip_gird_c1flex.Cols[v_input_index][3] = "min: n/a";
                    }
                }
                // Duyệt cột vào sau cột ra
                for (v_input_index = v_output_index + v_output_count; v_input_index < v_input_count; v_input_index++)
                {
                    var v_column_details = ip_dt_samples.Columns[v_input_index].ExtendedProperties["Details"] as DataColumnDetails;
                    ip_gird_c1flex.Cols[v_input_index].Name = ip_dt_samples.Columns[v_input_index].ColumnName;
                    ip_gird_c1flex.Cols[v_input_index].Caption = ip_dt_samples.Columns[v_input_index].Caption;
                    if (v_column_details.Format == DataColumnFormat.Categorical)
                    {
                        ip_gird_c1flex.Cols[v_input_index].ComboList = v_column_details.GetCateComboList();
                        ip_gird_c1flex.Cols[v_input_index][2] = "max: n/a";
                        ip_gird_c1flex.Cols[v_input_index][3] = "min: n/a";
                    }
                    else if (v_column_details.Format == DataColumnFormat.Numerical)
                    {
                        // Chưa có chế độ cảnh báo người dùng nhập quá giới hạn min max
                        ip_gird_c1flex.Cols[v_input_index][2] = "max: " + v_column_details.MaxValue;
                        ip_gird_c1flex.Cols[v_input_index][3] = "min: " + v_column_details.MinValue;
                    }
                    else
                    {
                        ip_gird_c1flex.Cols[v_input_index][2] = "max: n/a";
                        ip_gird_c1flex.Cols[v_input_index][3] = "min: n/a";
                    }
                }
                ip_gird_c1flex.Rows[2].ComboList = string.Empty;
                ip_gird_c1flex.Rows[3].ComboList = string.Empty;
                ip_gird_c1flex.Rows[2].Style = ip_gird_c1flex.Rows[0].StyleFixed;
            }
        }

        private static void ip_gird_c1flex_Click(object sender, EventArgs e)
        {
            var v_flex = sender as C1FlexGrid;
            if (v_flex.RowSel == 1)
                v_flex.StartEditing(v_flex.RowSel, v_flex.ColSel);
        }

        private static void ip_gird_c1flex_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)  // ignore enter key
                e.Handled = true; // (don't move the cursor)
        }

        private static void ip_gird_c1flex_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Return)
            //{
            //    e.Handled = true; // ignore the event, we'll handle it ourselves
            //    var _flex = sender as C1FlexGrid;
            //    _flex.StartEditing(); // put the grid in edit mode

            //    TextBox tb = _flex.Editor as TextBox; // if we got a textbox, then
            //    if (tb != null) tb.Select(tb.Text.Length, 0); // move cursor to end

            //}
            //else if (e.KeyCode == Keys.Tab)
            //{
            //    e.Handled = true;
            //    var v_c1flex = sender as C1FlexGrid;
            //    v_c1flex.StartEditing(v_c1flex.RowSel, v_c1flex.ColSel + 1);
            //    TextBox tb = v_c1flex.Editor as TextBox; // if we got a textbox, then
            //    if (tb != null) tb.Select(tb.Text.Length, 0); // move cursor to end
            //}
        }

        private static void ip_gird_c1flex_BeforeEdit(object sender, RowColEventArgs e)
        {
            // Thiết đặt thuộc tính read-only cho hàng max(2), min(3)
            if (e.Row > 1)
            {
                e.Cancel = true;
            }
        }

        private static void ip_gird_c1flex_CellChanged(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            var v_c1flex_grid = sender as C1FlexGrid;
            var v_column_details = v_c1flex_grid.Cols[e.Col].UserData as DataColumnDetails;
            if (v_column_details.Format != DataColumnFormat.Numerical)
                return;
            var v_user_value = (double)0;
            if (double.TryParse(v_c1flex_grid[e.Row, e.Col].ToString(), out v_user_value) == false)
                return;
            var cs = v_c1flex_grid.Styles["ExcessValue"];
            if (v_user_value > v_column_details.MaxValue || v_user_value < v_column_details.MinValue)
            {
                v_c1flex_grid.SetCellStyle(e.Row, e.Col, cs);
            }
            else
            {
                v_c1flex_grid.Clear(ClearFlags.Style, e.Row, e.Col);
            }
        }

        public static DataTable GetUserData(C1FlexGrid ip_c1flex_grid, int ip_row_index)
        {
            var v_dt_table = new DataTable();
            for (int i = ip_c1flex_grid.Cols.Fixed; i < ip_c1flex_grid.Cols.Count; i++)
            {
                var v_dt_column = new DataColumn(ip_c1flex_grid.Cols[i].Name);
                v_dt_column.ExtendedProperties["Details"] = ip_c1flex_grid.Cols[i].UserData;
                v_dt_table.Columns.Add(v_dt_column);
            }
            var v_dt_row = v_dt_table.NewRow();
            for (int i = ip_c1flex_grid.Cols.Fixed; i < ip_c1flex_grid.Cols.Count; i++)
            {
                v_dt_row[i] = ip_c1flex_grid[ip_row_index, i];
            }
            v_dt_table.Rows.Add(v_dt_row);
            return v_dt_table;
        }

        public static DataTable GetUserData(C1FlexGrid ip_c1flex_grid)
        {
            return GetUserData(ip_c1flex_grid, 1);
        }

        public static void LabelDataWithOutAnalysis(ref DataTable ip_dt_table, DataColumnCollection ip_dt_labeled)
        {
            for (int i = 0; i < ip_dt_labeled.Count; i++)
            {
                var v_cdetails = ip_dt_labeled[i].ExtendedProperties["Details"] as DataColumnDetails;
                if (v_cdetails.Type == DataColumnType.Ouput)
                {
                    continue;
                }
                ip_dt_table.Columns[i].ExtendedProperties["Details"] = v_cdetails;
            }
        }
    }
}

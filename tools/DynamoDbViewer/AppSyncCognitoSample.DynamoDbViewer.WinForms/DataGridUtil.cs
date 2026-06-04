using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppSyncCognitoSample.DynamoDbViewer.WinForms
{
    public static class DataGridUtil
    {
        //========================================================================================
        //	関数名	：InitDataGrid
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        public static void InitDataGrid(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.RowHeadersVisible = false;
            //grid.AutoGenerateColumns = false;

            var toolMenuClipBorad = new ToolStripMenuItem();
            toolMenuClipBorad.Text = "選択行コピー";
            toolMenuClipBorad.Click += toolMenuClipBoradSelect_Click!;
            toolMenuClipBorad.Tag = grid;

            var toolMenuClipBorad2 = new ToolStripMenuItem();
            toolMenuClipBorad2.Text = "全行コピー";
            toolMenuClipBorad2.Click += toolMenuClipBoradAll_Click!;
            toolMenuClipBorad2.Tag = grid;

            var toolMenuExcel = new ToolStripMenuItem();
            toolMenuExcel.Text = "タブ区切りCSV出力...";
            toolMenuExcel.Click += toolMenuCsv_Click!;
            toolMenuExcel.Tag = grid;


            grid.ContextMenuStrip = new ContextMenuStrip();
            grid.ContextMenuStrip.Items.AddRange(new ToolStripItem[] {
            toolMenuClipBorad,
			toolMenuClipBorad2,
			toolMenuExcel});

        }

        //========================================================================================
        //	関数名	：toolMenuCsv_Click
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        static void toolMenuCsv_Click(object sender, EventArgs e)
        {
            try
            {
                var menuItem = sender as ToolStripMenuItem;
                if (menuItem == null || menuItem.Tag as DataGridView == null)
                    return;
                var grid = menuItem.Tag as DataGridView;

                string data = GetAllText(grid!);

                // ファイル保存ダイアログを開く
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                    sfd.Title = "タブ区切りCSVファイル保存";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        // cData を CSV ファイルに保存
                        File.WriteAllText(sfd.FileName, data, Encoding.UTF8);
                    }

                    MessageBox.Show("保存しました。", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //========================================================================================
        //	関数名	：toolMenuClipBoradAll_Click
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        static void toolMenuClipBoradAll_Click(object sender, EventArgs e)
        {
            try
            {
                var menuItem = sender as ToolStripMenuItem;
                if (menuItem == null || menuItem.Tag as DataGridView == null)
                    return;
                var grid = menuItem.Tag as DataGridView;

                Clipboard.Clear();
                Clipboard.SetText(GetAllText(grid!));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //========================================================================================
        //	関数名	：toolMenuClipBoradSelect_Click
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        static void toolMenuClipBoradSelect_Click(object sender, EventArgs e)
        {
            try
            {
                var menuItem = sender as ToolStripMenuItem;
                if (menuItem == null || menuItem.Tag as DataGridView == null)
                    return;
                var grid = menuItem.Tag as DataGridView;

                Clipboard.Clear();
                Clipboard.SetText(GetSelectText(grid!));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //========================================================================================
        //	関数名	：GetAllText
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private static string GetAllText(DataGridView grid)
        {
            StringBuilder builder = new StringBuilder();

            foreach (DataGridViewColumn col in grid.Columns)
            {
                builder.Append(col.HeaderText);
                if (col != grid.Columns[grid.Columns.Count - 1])
                {
                    builder.Append("\t");
                }

            }

            builder.Append("\n");
            foreach (DataGridViewRow row in grid.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    builder.Append(cell.Value);
                    if (cell != row.Cells[row.Cells.Count - 1])
                    {
                        builder.Append("\t");
                    }
                }
                builder.Append("\n");
            }

            return builder.ToString();
        }

        //========================================================================================
        //	関数名	：GetSelectText
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private static string GetSelectText(DataGridView grid)
        {
            StringBuilder selectedData = new StringBuilder();

            foreach (DataGridViewColumn column in grid.Columns)
            {
                selectedData.Append(column.HeaderText);
                if (column != grid.Columns[grid.Columns.Count - 1])
                {
                    selectedData.Append("\t");
                }
            }
            selectedData.Append("\n");

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Selected)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        selectedData.Append(cell.Value);
                        if (cell != row.Cells[row.Cells.Count - 1])
                        {
                            selectedData.Append("\t");
                        }
                    }
                    selectedData.Append("\n");
                }
            }
            string selectedDataString = selectedData.ToString();
            return selectedDataString;
        }

    }
}

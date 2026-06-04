using Amazon.DynamoDBv2.Model;
using AppSyncCognitoSample.DynamoDbViewer.Core;
using AppSyncCognitoSample.DynamoDbViewer.Core.Utilities;
using System.Data;

namespace AppSyncCognitoSample.DynamoDbViewer.WinForms
{
    public partial class Form1 : Form
    {
        private readonly IDynamoDbViewerService _viewerService;
        private readonly DynamoDbViewerOptions _options;
        private bool _isLoading;

        private DynamoDbTableSchema? _currentSchema;
        private Dictionary<string, AttributeValue>? _lastEvaluatedKey;
        private readonly List<DynamoDbItemRow> _loadedRows = new();

        //========================================================================================
        //	関数名	：Form1
        //
        //	戻り値	：
        //
        //	説明	：
        //========================================================================================
        public Form1(IDynamoDbViewerService viewerService, DynamoDbViewerOptions options)
        {
            _viewerService = viewerService;
            _options = options;

            InitializeComponent();
        }

        //========================================================================================
        //	関数名	：Form1_Load
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private async void Form1_Load(object sender, EventArgs e)
        {
            _lblRegion.Text = _options.Region;
            DataGridUtil.InitDataGrid(_grid);

            try
            {
                await LoadTablesAsync().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //========================================================================================
        //	関数名	：btnClose_Click
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //========================================================================================
        //	関数名	：btnReloadTables_Click
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private async void btnReloadTables_Click(object sender, EventArgs e)
        {
            try
            {
                await LoadTablesAsync().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //========================================================================================
        //	関数名	：btnLoadNext_Click
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private async void btnLoadNext_Click(object sender, EventArgs e)
        {
            try
            {
                await LoadSelectedTableAsync(reset: false).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //========================================================================================
        //	関数名	：btnReloadItems_Click
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private async void btnReloadItems_Click(object sender, EventArgs e)
        {
            try
            {
                await LoadSelectedTableAsync(reset: true).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //========================================================================================
        //	関数名	：btnDeleteRow_Click
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private async void btnDeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteSelectedAsync().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //========================================================================================
        //	関数名	：_tableComboBox_SelectedIndexChanged
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private async void _tableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await LoadSelectedTableAsync(reset: true).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //========================================================================================
        //	関数名	：_grid_SelectionChanged
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private void _grid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var dataGridViewRow = _grid.CurrentRow.DataBoundItem as DataRowView;
                if (dataGridViewRow != null)
                {
                    _propertyGrid.SelectedObject = dataGridViewRow;
                }
                else
                {
                    _propertyGrid.SelectedObject = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //========================================================================================
        //	関数名	：LoadTablesAsync
        //
        //	戻り値	：Task
        //
        //	説明	：
        //========================================================================================
        private async Task LoadTablesAsync()
        {
            if (_isLoading)
            {
                return;
            }

            await RunUiOperationAsync(async () =>
            {
                SetStatus("Loading table list...");
                _tableComboBox.Items.Clear();
                _loadedRows.Clear();
                _grid.DataSource = null;
                _currentSchema = null;
                _lastEvaluatedKey = null;
                _keyLabel.Text = "-";

                var tables = await _viewerService.ListTablesAsync().ConfigureAwait(true);
                foreach (var table in tables)
                {
                    _tableComboBox.Items.Add(table);
                }

                SetStatus($"{tables.Count} table(s) loaded.");

                if (_tableComboBox.Items.Count > 0)
                {
                    _tableComboBox.SelectedIndex = 0;
                }
            }).ConfigureAwait(true);

            if (_tableComboBox.SelectedItem is not null)
            {
                await LoadSelectedTableAsync(reset: true).ConfigureAwait(true);
            }
        }

        //========================================================================================
        //	関数名	：LoadSelectedTableAsync
        //
        //	戻り値	：Task
        //
        //	説明	：
        //========================================================================================
        private async Task LoadSelectedTableAsync(bool reset)
        {
            if (_isLoading || _tableComboBox.SelectedItem is not DynamoDbTableSummary table)
            {
                return;
            }

            await RunUiOperationAsync(async () =>
            {
                if (reset)
                {
                    _loadedRows.Clear();
                    _lastEvaluatedKey = null;
                    _grid.DataSource = null;
                    _currentSchema = await _viewerService.DescribeTableAsync(table.TableName).ConfigureAwait(true);
                    _keyLabel.Text = $"{_currentSchema.KeySummary}";
                }

                SetStatus(reset ? $"Scanning {table.TableName}..." : $"Loading more items from {table.TableName}...");

                var result = await _viewerService.ScanAsync(
                    table.TableName,
                    Math.Max(1, _options.ScanLimit),
                    _lastEvaluatedKey).ConfigureAwait(true);

                _loadedRows.AddRange(result.Items);
                _lastEvaluatedKey = result.LastEvaluatedKey;
                BindRowsToGrid();

                btnLoadNext.Enabled = result.HasMore;
                SetStatus($"{_loadedRows.Count} item(s) loaded. More items: {(result.HasMore ? "Yes" : "No")}");
            }).ConfigureAwait(true);
        }

        //========================================================================================
        //	関数名	：DeleteSelectedAsync
        //
        //	戻り値	：Task
        //
        //	説明	：
        //========================================================================================
        private async Task DeleteSelectedAsync()
        {
            if (_tableComboBox.SelectedItem is not DynamoDbTableSummary table || _currentSchema is null)
            {
                return;
            }

            var selectedRow = _grid.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault();
            if (selectedRow is null)
            {
                return;
            }

            var rowIndexValue = selectedRow.Cells["__RowIndex"].Value?.ToString();
            if (!int.TryParse(rowIndexValue, out var rowIndex) || rowIndex < 0 || rowIndex >= _loadedRows.Count)
            {
                MessageBox.Show("Could not identify the selected item.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = _loadedRows[rowIndex];
            var key = BuildKey(item.RawItem, _currentSchema.KeyAttributeNames);
            var keyText = string.Join(
                Environment.NewLine,
                key.Select(x => $"{x.Key}: {AttributeValueFormatter.Format(x.Value)}"));

            var confirmation = MessageBox.Show(
                $"The following item will be deleted from table '{table.TableName}'.\n\n{keyText}\n\nContinue?",
                "Confirm delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            await RunUiOperationAsync(async () =>
            {
                SetStatus("Deleting selected item...");
                await _viewerService.DeleteItemAsync(table.TableName, key).ConfigureAwait(true);

                SetStatus("Item deleted. Reloading table...");
                _loadedRows.Clear();
                _lastEvaluatedKey = null;
                _grid.DataSource = null;
                _currentSchema = await _viewerService.DescribeTableAsync(table.TableName).ConfigureAwait(true);
                _keyLabel.Text = $"{_currentSchema.KeySummary}";

                var result = await _viewerService.ScanAsync(
                    table.TableName,
                    Math.Max(1, _options.ScanLimit),
                    _lastEvaluatedKey).ConfigureAwait(true);

                _loadedRows.AddRange(result.Items);
                _lastEvaluatedKey = result.LastEvaluatedKey;
                BindRowsToGrid();
                btnLoadNext.Enabled = result.HasMore;
                SetStatus($"Item deleted. {_loadedRows.Count} item(s) loaded. More items: {(result.HasMore ? "Yes" : "No")}");
            }).ConfigureAwait(true);
        }

        //========================================================================================
        //	関数名	：BuildKey
        //
        //	戻り値	：IReadOnlyDictionary<string, AttributeValue>
        //
        //	説明	：
        //========================================================================================
        private static IReadOnlyDictionary<string, AttributeValue> BuildKey(
            IReadOnlyDictionary<string, AttributeValue> rawItem,
            IReadOnlyList<string> keyAttributeNames)
        {
            var key = new Dictionary<string, AttributeValue>();

            foreach (var keyName in keyAttributeNames)
            {
                if (!rawItem.TryGetValue(keyName, out var value))
                {
                    throw new InvalidOperationException(
                        $"The selected item does not contain key attribute '{keyName}'.");
                }

                key[keyName] = value;
            }

            return key;
        }

        //========================================================================================
        //	関数名	：BindRowsToGrid
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private void BindRowsToGrid()
        {
            var table = new DataTable();
            table.Columns.Add("__RowIndex", typeof(int));

            var columnNames = _loadedRows
                .SelectMany(x => x.DisplayValues.Keys)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => GetColumnPriority(x))
                .ThenBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            foreach (var columnName in columnNames)
            {
                table.Columns.Add(columnName, typeof(string));
            }

            for (var i = 0; i < _loadedRows.Count; i++)
            {
                var dataRow = table.NewRow();
                dataRow["__RowIndex"] = i;

                foreach (var columnName in columnNames)
                {
                    if (_loadedRows[i].DisplayValues.TryGetValue(columnName, out var value))
                    {
                        dataRow[columnName] = value ?? string.Empty;
                    }
                }

                table.Rows.Add(dataRow);
            }

            _grid.DataSource = table;
            btnDeleteRow.Enabled = _grid.SelectedRows.Count > 0;
        }

        //========================================================================================
        //	関数名	：GetColumnPriority
        //
        //	戻り値	：int
        //
        //	説明	：
        //========================================================================================
        private static int GetColumnPriority(string columnName)
        {
            return columnName switch
            {
                "pk" => 0,
                "sk" => 1,
                "id" => 2,
                "userSub" => 3,
                "username" => 4,
                "email" => 5,
                "title" => 6,
                "body" => 7,
                "createdAt" => 8,
                _ => 100
            };
        }

        //========================================================================================
        //	関数名	：RunUiOperationAsync
        //
        //	戻り値	：Task
        //
        //	説明	：
        //========================================================================================
        private async Task RunUiOperationAsync(Func<Task> operation)
        {
            try
            {
                SetLoading(true);
                await operation().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                SetStatus("Error occurred.");
                MessageBox.Show(ex.ToString(), "DynamoDB Viewer error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoading(false);
            }
        }

        //========================================================================================
        //	関数名	：SetLoading
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private void SetLoading(bool loading)
        {
            _isLoading = loading;
            UseWaitCursor = loading;
            _tableComboBox.Enabled = !loading;
            btnReloadTables.Enabled = !loading;
            btnReloadItems.Enabled = !loading && _tableComboBox.SelectedItem is not null;
            btnLoadNext.Enabled = !loading && _lastEvaluatedKey is { Count: > 0 };
            btnDeleteRow.Enabled = !loading && _grid.SelectedRows.Count > 0;
        }

        //========================================================================================
        //	関数名	：SetStatus
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private void SetStatus(string message)
        {
            _statusLabel.Text = message;
        }
    }
}

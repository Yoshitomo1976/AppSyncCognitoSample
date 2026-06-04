using System.Windows.Forms;
using System.Data;
using Amazon.DynamoDBv2.Model;
using AppSyncCognitoSample.DynamoDbViewer.Core;
using AppSyncCognitoSample.DynamoDbViewer.Core.Utilities;

namespace AppSyncCognitoSample.DynamoDbViewer.WinForms;

public sealed class MainForm : Form
{
    private readonly IDynamoDbViewerService _viewerService;
    private readonly DynamoDbViewerOptions _options;

    private readonly ComboBox _tableComboBox = new();
    private readonly Button _reloadTablesButton = new();
    private readonly Button _reloadItemsButton = new();
    private readonly Button _loadMoreButton = new();
    private readonly Button _deleteButton = new();
    private readonly DataGridView _grid = new();
    private readonly Label _statusLabel = new();
    private readonly Label _keyLabel = new();

    private DynamoDbTableSchema? _currentSchema;
    private Dictionary<string, AttributeValue>? _lastEvaluatedKey;
    private readonly List<DynamoDbItemRow> _loadedRows = new();
    private bool _isLoading;

    public MainForm(IDynamoDbViewerService viewerService, DynamoDbViewerOptions options)
    {
        _viewerService = viewerService;
        _options = options;

        Text = "DynamoDB Viewer - AppSync Cognito Sample";
        Width = 1200;
        Height = 760;
        StartPosition = FormStartPosition.CenterScreen;

        InitializeControls();
    }

    protected override async void OnShown(EventArgs e)
    {
        base.OnShown(e);
        await LoadTablesAsync().ConfigureAwait(true);
    }

    private void InitializeControls()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(8)
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var topPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = false
        };

        var tableLabel = new Label
        {
            Text = $"Region: {_options.Region}   Table:",
            AutoSize = true,
            Padding = new Padding(0, 8, 0, 0)
        };

        _tableComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _tableComboBox.Width = 420;
        _tableComboBox.SelectedIndexChanged += async (_, _) => await LoadSelectedTableAsync(reset: true).ConfigureAwait(true);

        _reloadTablesButton.Text = "Reload Tables";
        _reloadTablesButton.AutoSize = true;
        _reloadTablesButton.Click += async (_, _) => await LoadTablesAsync().ConfigureAwait(true);

        _reloadItemsButton.Text = "Reload Items";
        _reloadItemsButton.AutoSize = true;
        _reloadItemsButton.Click += async (_, _) => await LoadSelectedTableAsync(reset: true).ConfigureAwait(true);

        _loadMoreButton.Text = "Load More";
        _loadMoreButton.AutoSize = true;
        _loadMoreButton.Enabled = false;
        _loadMoreButton.Click += async (_, _) => await LoadSelectedTableAsync(reset: false).ConfigureAwait(true);

        _deleteButton.Text = "Delete Selected";
        _deleteButton.AutoSize = true;
        _deleteButton.Enabled = false;
        _deleteButton.Click += async (_, _) => await DeleteSelectedAsync().ConfigureAwait(true);

        topPanel.Controls.Add(tableLabel);
        topPanel.Controls.Add(_tableComboBox);
        topPanel.Controls.Add(_reloadTablesButton);
        topPanel.Controls.Add(_reloadItemsButton);
        topPanel.Controls.Add(_loadMoreButton);
        topPanel.Controls.Add(_deleteButton);

        _keyLabel.AutoSize = true;
        _keyLabel.Text = "Key: -";
        _keyLabel.Padding = new Padding(0, 4, 0, 4);

        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        _grid.RowHeadersVisible = false;
        _grid.DataBindingComplete += (_, _) =>
        {
            foreach (DataGridViewColumn column in _grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (column.Name == "__RowIndex")
                {
                    column.Visible = false;
                }
            }
        };
        _grid.SelectionChanged += (_, _) => _deleteButton.Enabled = !_isLoading && _grid.SelectedRows.Count > 0;

        _statusLabel.AutoSize = true;
        _statusLabel.Text = "Ready";
        _statusLabel.Padding = new Padding(0, 6, 0, 0);

        root.Controls.Add(topPanel, 0, 0);
        root.Controls.Add(_keyLabel, 0, 1);
        root.Controls.Add(_grid, 0, 2);
        root.Controls.Add(_statusLabel, 0, 3);

        Controls.Add(root);
    }

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
            _keyLabel.Text = "Key: -";

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
                _keyLabel.Text = $"Key: {_currentSchema.KeySummary}";
            }

            SetStatus(reset ? $"Scanning {table.TableName}..." : $"Loading more items from {table.TableName}...");

            var result = await _viewerService.ScanAsync(
                table.TableName,
                Math.Max(1, _options.ScanLimit),
                _lastEvaluatedKey).ConfigureAwait(true);

            _loadedRows.AddRange(result.Items);
            _lastEvaluatedKey = result.LastEvaluatedKey;
            BindRowsToGrid();

            _loadMoreButton.Enabled = result.HasMore;
            SetStatus($"{_loadedRows.Count} item(s) loaded. More items: {(result.HasMore ? "Yes" : "No")}");
        }).ConfigureAwait(true);
    }

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
            _keyLabel.Text = $"Key: {_currentSchema.KeySummary}";

            var result = await _viewerService.ScanAsync(
                table.TableName,
                Math.Max(1, _options.ScanLimit),
                _lastEvaluatedKey).ConfigureAwait(true);

            _loadedRows.AddRange(result.Items);
            _lastEvaluatedKey = result.LastEvaluatedKey;
            BindRowsToGrid();
            _loadMoreButton.Enabled = result.HasMore;
            SetStatus($"Item deleted. {_loadedRows.Count} item(s) loaded. More items: {(result.HasMore ? "Yes" : "No")}");
        }).ConfigureAwait(true);
    }

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
        _deleteButton.Enabled = _grid.SelectedRows.Count > 0;
    }

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

    private void SetLoading(bool loading)
    {
        _isLoading = loading;
        UseWaitCursor = loading;
        _tableComboBox.Enabled = !loading;
        _reloadTablesButton.Enabled = !loading;
        _reloadItemsButton.Enabled = !loading && _tableComboBox.SelectedItem is not null;
        _loadMoreButton.Enabled = !loading && _lastEvaluatedKey is { Count: > 0 };
        _deleteButton.Enabled = !loading && _grid.SelectedRows.Count > 0;
    }

    private void SetStatus(string message)
    {
        _statusLabel.Text = message;
    }
}

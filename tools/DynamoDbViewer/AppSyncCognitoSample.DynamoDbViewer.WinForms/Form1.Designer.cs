namespace AppSyncCognitoSample.DynamoDbViewer.WinForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            btnClose = new Button();
            _statusLabel = new Label();
            panel2 = new Panel();
            btnDeleteRow = new Button();
            btnLoadNext = new Button();
            btnReloadItems = new Button();
            btnReloadTables = new Button();
            _keyLabel = new Label();
            label3 = new Label();
            _tableComboBox = new ComboBox();
            label2 = new Label();
            _lblRegion = new Label();
            label1 = new Label();
            splitContainer1 = new SplitContainer();
            _grid = new DataGridView();
            _propertyGrid = new PropertyGrid();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnClose);
            panel1.Controls.Add(_statusLabel);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 373);
            panel1.Name = "panel1";
            panel1.Size = new Size(734, 38);
            panel1.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Location = new Point(647, 6);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 0;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // _statusLabel
            // 
            _statusLabel.Location = new Point(12, 6);
            _statusLabel.Name = "_statusLabel";
            _statusLabel.Size = new Size(327, 23);
            _statusLabel.TabIndex = 4;
            _statusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnDeleteRow);
            panel2.Controls.Add(btnLoadNext);
            panel2.Controls.Add(btnReloadItems);
            panel2.Controls.Add(btnReloadTables);
            panel2.Controls.Add(_keyLabel);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(_tableComboBox);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(_lblRegion);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(734, 74);
            panel2.TabIndex = 1;
            // 
            // btnDeleteRow
            // 
            btnDeleteRow.Location = new Point(619, 40);
            btnDeleteRow.Name = "btnDeleteRow";
            btnDeleteRow.Size = new Size(98, 23);
            btnDeleteRow.TabIndex = 9;
            btnDeleteRow.Text = "Delete Row";
            btnDeleteRow.UseVisualStyleBackColor = true;
            btnDeleteRow.Click += btnDeleteRow_Click;
            // 
            // btnLoadNext
            // 
            btnLoadNext.Location = new Point(619, 11);
            btnLoadNext.Name = "btnLoadNext";
            btnLoadNext.Size = new Size(98, 23);
            btnLoadNext.TabIndex = 8;
            btnLoadNext.Text = "Load Next";
            btnLoadNext.UseVisualStyleBackColor = true;
            btnLoadNext.Click += btnLoadNext_Click;
            // 
            // btnReloadItems
            // 
            btnReloadItems.Location = new Point(520, 40);
            btnReloadItems.Name = "btnReloadItems";
            btnReloadItems.Size = new Size(98, 23);
            btnReloadItems.TabIndex = 7;
            btnReloadItems.Text = "Reload Items";
            btnReloadItems.UseVisualStyleBackColor = true;
            btnReloadItems.Click += btnReloadItems_Click;
            // 
            // btnReloadTables
            // 
            btnReloadTables.Location = new Point(520, 11);
            btnReloadTables.Name = "btnReloadTables";
            btnReloadTables.Size = new Size(98, 23);
            btnReloadTables.TabIndex = 6;
            btnReloadTables.Text = "Reload Tables";
            btnReloadTables.UseVisualStyleBackColor = true;
            btnReloadTables.Click += btnReloadTables_Click;
            // 
            // _keyLabel
            // 
            _keyLabel.BackColor = SystemColors.ControlLightLight;
            _keyLabel.BorderStyle = BorderStyle.Fixed3D;
            _keyLabel.Location = new Point(74, 40);
            _keyLabel.Name = "_keyLabel";
            _keyLabel.Size = new Size(150, 23);
            _keyLabel.TabIndex = 5;
            _keyLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.Location = new Point(12, 40);
            label3.Name = "label3";
            label3.Size = new Size(56, 23);
            label3.TabIndex = 4;
            label3.Text = "Keys:";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _tableComboBox
            // 
            _tableComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _tableComboBox.FormattingEnabled = true;
            _tableComboBox.Location = new Point(241, 10);
            _tableComboBox.Name = "_tableComboBox";
            _tableComboBox.Size = new Size(273, 23);
            _tableComboBox.TabIndex = 3;
            _tableComboBox.SelectedIndexChanged += _tableComboBox_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.Location = new Point(187, 9);
            label2.Name = "label2";
            label2.Size = new Size(48, 23);
            label2.TabIndex = 2;
            label2.Text = "Table:";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _lblRegion
            // 
            _lblRegion.BackColor = SystemColors.ControlLightLight;
            _lblRegion.BorderStyle = BorderStyle.Fixed3D;
            _lblRegion.Location = new Point(74, 9);
            _lblRegion.Name = "_lblRegion";
            _lblRegion.Size = new Size(107, 23);
            _lblRegion.TabIndex = 1;
            _lblRegion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(56, 23);
            label1.TabIndex = 0;
            label1.Text = "Region:";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 74);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(_grid);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(_propertyGrid);
            splitContainer1.Size = new Size(734, 299);
            splitContainer1.SplitterDistance = 516;
            splitContainer1.TabIndex = 2;
            // 
            // _grid
            // 
            _grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _grid.Dock = DockStyle.Fill;
            _grid.Location = new Point(0, 0);
            _grid.Name = "_grid";
            _grid.Size = new Size(516, 299);
            _grid.TabIndex = 0;
            _grid.SelectionChanged += _grid_SelectionChanged;
            // 
            // _propertyGrid
            // 
            _propertyGrid.Dock = DockStyle.Fill;
            _propertyGrid.HelpVisible = false;
            _propertyGrid.Location = new Point(0, 0);
            _propertyGrid.Name = "_propertyGrid";
            _propertyGrid.Size = new Size(214, 299);
            _propertyGrid.TabIndex = 0;
            _propertyGrid.ToolbarVisible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(734, 411);
            Controls.Add(splitContainer1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            MinimumSize = new Size(750, 450);
            Name = "Form1";
            Text = "DynamoDB Viewer";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private SplitContainer splitContainer1;
        private Button btnClose;
        private Label label1;
        private ComboBox _tableComboBox;
        private Label label2;
        private Label _lblRegion;
        private Button btnDeleteRow;
        private Button btnLoadNext;
        private Button btnReloadItems;
        private Button btnReloadTables;
        private Label _keyLabel;
        private Label label3;
        private DataGridView _grid;
        private PropertyGrid _propertyGrid;
        private Label _statusLabel;
    }
}

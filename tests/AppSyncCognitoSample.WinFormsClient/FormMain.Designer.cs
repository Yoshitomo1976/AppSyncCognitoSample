namespace AppSyncCognitoSample.WinFormsClient
{
    partial class FormMain
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
            panel2 = new Panel();
            btnHostedUiLogin = new Button();
            btnLogin = new Button();
            _txtUserName = new TextBox();
            _txtPassword = new TextBox();
            label2 = new Label();
            label1 = new Label();
            _lblToken = new Label();
            label3 = new Label();
            panel3 = new Panel();
            btnSave = new Button();
            _txtTitle = new TextBox();
            _txtBody = new TextBox();
            label4 = new Label();
            label5 = new Label();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            btnHostedUiLogout = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnClose);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 335);
            panel1.Name = "panel1";
            panel1.Size = new Size(501, 43);
            panel1.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(414, 6);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 0;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(groupBox2);
            panel2.Controls.Add(groupBox1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(501, 197);
            panel2.TabIndex = 1;
            // 
            // btnHostedUiLogin
            // 
            btnHostedUiLogin.Location = new Point(6, 44);
            btnHostedUiLogin.Name = "btnHostedUiLogin";
            btnHostedUiLogin.Size = new Size(172, 23);
            btnHostedUiLogin.TabIndex = 4;
            btnHostedUiLogin.Text = "ログイン";
            btnHostedUiLogin.UseVisualStyleBackColor = true;
            btnHostedUiLogin.Click += btnHostedUiLogin_Click;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(102, 126);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(91, 23);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "ログイン";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // _txtUserName
            // 
            _txtUserName.Location = new Point(21, 45);
            _txtUserName.Name = "_txtUserName";
            _txtUserName.Size = new Size(172, 23);
            _txtUserName.TabIndex = 2;
            // 
            // _txtPassword
            // 
            _txtPassword.Location = new Point(21, 97);
            _txtPassword.Name = "_txtPassword";
            _txtPassword.PasswordChar = '*';
            _txtPassword.Size = new Size(172, 23);
            _txtPassword.TabIndex = 1;
            // 
            // label2
            // 
            label2.Location = new Point(21, 71);
            label2.Name = "label2";
            label2.Size = new Size(100, 23);
            label2.TabIndex = 0;
            label2.Text = "パスワード";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.Location = new Point(21, 19);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 0;
            label1.Text = "Eメール";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _lblToken
            // 
            _lblToken.BorderStyle = BorderStyle.Fixed3D;
            _lblToken.Location = new Point(132, 9);
            _lblToken.Name = "_lblToken";
            _lblToken.Size = new Size(257, 23);
            _lblToken.TabIndex = 5;
            _lblToken.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.Location = new Point(26, 9);
            label3.Name = "label3";
            label3.Size = new Size(100, 23);
            label3.TabIndex = 4;
            label3.Text = "Access Token";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            panel3.Controls.Add(_lblToken);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(btnSave);
            panel3.Controls.Add(_txtTitle);
            panel3.Controls.Add(_txtBody);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(label5);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 197);
            panel3.Name = "panel3";
            panel3.Size = new Size(501, 138);
            panel3.TabIndex = 2;
            // 
            // btnSave
            // 
            btnSave.Enabled = false;
            btnSave.Location = new Point(229, 100);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 8;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // _txtTitle
            // 
            _txtTitle.Location = new Point(132, 42);
            _txtTitle.Name = "_txtTitle";
            _txtTitle.Size = new Size(172, 23);
            _txtTitle.TabIndex = 7;
            // 
            // _txtBody
            // 
            _txtBody.Location = new Point(132, 71);
            _txtBody.Name = "_txtBody";
            _txtBody.Size = new Size(172, 23);
            _txtBody.TabIndex = 6;
            // 
            // label4
            // 
            label4.Location = new Point(26, 71);
            label4.Name = "label4";
            label4.Size = new Size(100, 23);
            label4.TabIndex = 4;
            label4.Text = "Body";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.Location = new Point(26, 41);
            label5.Name = "label5";
            label5.Size = new Size(100, 23);
            label5.TabIndex = 5;
            label5.Text = "Title";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(btnLogin);
            groupBox1.Controls.Add(_txtPassword);
            groupBox1.Controls.Add(_txtUserName);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(214, 164);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Eメール/パスワードでログイン";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnHostedUiLogout);
            groupBox2.Controls.Add(btnHostedUiLogin);
            groupBox2.Location = new Point(232, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(200, 164);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "Webブラウザを起動してログイン";
            // 
            // btnHostedUiLogout
            // 
            btnHostedUiLogout.Location = new Point(6, 73);
            btnHostedUiLogout.Name = "btnHostedUiLogout";
            btnHostedUiLogout.Size = new Size(172, 23);
            btnHostedUiLogout.TabIndex = 5;
            btnHostedUiLogout.Text = "ログアウト";
            btnHostedUiLogout.UseVisualStyleBackColor = true;
            btnHostedUiLogout.Click += btnHostedUiLogout_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(501, 378);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "FormMain";
            Text = "AppSyncSample";
            Load += FormMain_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnClose;
        private Panel panel2;
        private Label _lblToken;
        private Label label3;
        private Button btnLogin;
        private TextBox _txtUserName;
        private TextBox _txtPassword;
        private Label label2;
        private Label label1;
        private Panel panel3;
        private Button btnSave;
        private TextBox _txtTitle;
        private TextBox _txtBody;
        private Label label4;
        private Label label5;
        private Button btnHostedUiLogin;
        private GroupBox groupBox2;
        private Button btnHostedUiLogout;
        private GroupBox groupBox1;
    }
}

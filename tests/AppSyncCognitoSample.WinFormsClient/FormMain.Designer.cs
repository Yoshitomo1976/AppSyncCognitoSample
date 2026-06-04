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
            _lblToken = new Label();
            label3 = new Label();
            btnLogin = new Button();
            _txtUserName = new TextBox();
            _txtPassword = new TextBox();
            label2 = new Label();
            label1 = new Label();
            panel3 = new Panel();
            btnSave = new Button();
            _txtTitle = new TextBox();
            _txtBody = new TextBox();
            label4 = new Label();
            label5 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnClose);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 335);
            panel1.Name = "panel1";
            panel1.Size = new Size(401, 43);
            panel1.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(314, 8);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 0;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(_lblToken);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(btnLogin);
            panel2.Controls.Add(_txtUserName);
            panel2.Controls.Add(_txtPassword);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(401, 115);
            panel2.TabIndex = 1;
            // 
            // _lblToken
            // 
            _lblToken.BorderStyle = BorderStyle.Fixed3D;
            _lblToken.Location = new Point(132, 83);
            _lblToken.Name = "_lblToken";
            _lblToken.Size = new Size(257, 23);
            _lblToken.TabIndex = 5;
            _lblToken.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.Location = new Point(26, 83);
            label3.Name = "label3";
            label3.Size = new Size(100, 23);
            label3.TabIndex = 4;
            label3.Text = "Access Token";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(314, 46);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(75, 23);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // _txtUserName
            // 
            _txtUserName.Location = new Point(132, 18);
            _txtUserName.Name = "_txtUserName";
            _txtUserName.Size = new Size(172, 23);
            _txtUserName.TabIndex = 2;
            // 
            // _txtPassword
            // 
            _txtPassword.Location = new Point(132, 47);
            _txtPassword.Name = "_txtPassword";
            _txtPassword.PasswordChar = '*';
            _txtPassword.Size = new Size(172, 23);
            _txtPassword.TabIndex = 1;
            // 
            // label2
            // 
            label2.Location = new Point(26, 47);
            label2.Name = "label2";
            label2.Size = new Size(100, 23);
            label2.TabIndex = 0;
            label2.Text = "Password";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.Location = new Point(26, 17);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 0;
            label1.Text = "UserName";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnSave);
            panel3.Controls.Add(_txtTitle);
            panel3.Controls.Add(_txtBody);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(label5);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 115);
            panel3.Name = "panel3";
            panel3.Size = new Size(401, 220);
            panel3.TabIndex = 2;
            // 
            // btnSave
            // 
            btnSave.Enabled = false;
            btnSave.Location = new Point(314, 46);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 8;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // _txtTitle
            // 
            _txtTitle.Location = new Point(132, 18);
            _txtTitle.Name = "_txtTitle";
            _txtTitle.Size = new Size(172, 23);
            _txtTitle.TabIndex = 7;
            // 
            // _txtBody
            // 
            _txtBody.Location = new Point(132, 47);
            _txtBody.Name = "_txtBody";
            _txtBody.Size = new Size(172, 23);
            _txtBody.TabIndex = 6;
            // 
            // label4
            // 
            label4.Location = new Point(26, 47);
            label4.Name = "label4";
            label4.Size = new Size(100, 23);
            label4.TabIndex = 4;
            label4.Text = "Body";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.Location = new Point(26, 17);
            label5.Name = "label5";
            label5.Size = new Size(100, 23);
            label5.TabIndex = 5;
            label5.Text = "Title";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(401, 378);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "FormMain";
            Text = "AppSyncSample";
            Load += FormMain_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
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
    }
}

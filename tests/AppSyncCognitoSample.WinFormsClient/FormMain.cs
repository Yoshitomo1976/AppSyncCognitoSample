using AppSyncCognitoSample.ClientCore.AppSync;
using AppSyncCognitoSample.ClientCore.Authentication;
using AppSyncCognitoSample.ClientCore.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace AppSyncCognitoSample.WinFormsClient
{
    public partial class FormMain : Form
    {
        private readonly ICognitoAuthService _authService;
        private readonly IAppSyncClient _appSyncClient;

        //========================================================================================
        //	関数名	：FormMain
        //
        //	戻り値	：
        //
        //	説明	：
        //========================================================================================
        public FormMain(ICognitoAuthService authService, IAppSyncClient appSyncClient)
        {
            _appSyncClient = appSyncClient;
            _authService = authService;
            InitializeComponent();
        }

        //========================================================================================
        //	関数名	：FormMain_Load
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private void FormMain_Load(object sender, EventArgs e)
        {

            try
            {
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
        //	関数名	：btnLogin_Click
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                LoginResult result = await _authService.LoginAsync(new LoginRequest
                {
                    Username = _txtUserName.Text ?? string.Empty,
                    Password = _txtPassword.Text ?? string.Empty
                });

                _lblToken.Text = result.IdToken;
                btnSave.Enabled = true;
            }
            catch (Exception ex)
            {
                _lblToken.Text = string.Empty;
                btnSave.Enabled = false;
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //========================================================================================
        //	関数名	：btnSave_Click
        //
        //	戻り値	：void
        //
        //	説明	：
        //========================================================================================
        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var result = await _appSyncClient.SaveUserInputAsync(
                    new UserInputPayload
                    {
                        Title = _txtTitle.Text  ?? string.Empty,
                        Body = _txtBody.Text ?? string.Empty
                    });


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}

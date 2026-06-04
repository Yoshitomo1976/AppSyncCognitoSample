using AppSyncCognitoSample.ClientCore.AppSync;
using AppSyncCognitoSample.ClientCore.Authentication;
using AppSyncCognitoSample.ClientCore.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppSyncCognitoSample.WinFormsClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ICognitoAuthService authService;
            IAppSyncClient appSyncClient;
            try
            {
                var configuration = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json", optional: false)
                   .AddJsonFile("appsettings.Development.json", optional: true)
                   .Build();

                var services = new ServiceCollection();

                services.AddAppSyncCognitoClient(configuration);

                var serviceProvider = services.BuildServiceProvider();

                authService = serviceProvider.GetRequiredService<ICognitoAuthService>();
                appSyncClient = serviceProvider.GetRequiredService<IAppSyncClient>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new FormMain(authService, appSyncClient));
        }
    }
}
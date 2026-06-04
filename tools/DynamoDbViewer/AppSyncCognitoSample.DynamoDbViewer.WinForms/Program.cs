using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using AppSyncCognitoSample.DynamoDbViewer.Core;
using AppSyncCognitoSample.DynamoDbViewer.Core.Services;
using Microsoft.Extensions.Configuration;
using System.Windows.Forms;

namespace AppSyncCognitoSample.DynamoDbViewer.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        //System.Windows.Forms.ApplicationConfiguration.Initialize();

        try
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
                .Build();

            var options = configuration
                .GetSection("DynamoDbViewer")
                .Get<DynamoDbViewerOptions>() ?? new DynamoDbViewerOptions();

            using var dynamoDbClient = CreateDynamoDbClient(options);
            var viewerService = new DynamoDbViewerService(dynamoDbClient, options);

            //System.Windows.Forms.Application.Run(new MainForm(viewerService, options));
            System.Windows.Forms.Application.Run(new Form1(viewerService, options));
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.ToString(),
                "Startup error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private static AmazonDynamoDBClient CreateDynamoDbClient(DynamoDbViewerOptions options)
    {
        var region = RegionEndpoint.GetBySystemName(options.Region);

        if (string.IsNullOrWhiteSpace(options.ProfileName))
        {
            return new AmazonDynamoDBClient(region);
        }

        var chain = new CredentialProfileStoreChain();
        if (!chain.TryGetAWSCredentials(options.ProfileName, out AWSCredentials credentials))
        {
            throw new InvalidOperationException(
                $"AWS profile '{options.ProfileName}' was not found. Check appsettings.json or your AWS profile configuration.");
        }

        return new AmazonDynamoDBClient(credentials, region);
    }
}

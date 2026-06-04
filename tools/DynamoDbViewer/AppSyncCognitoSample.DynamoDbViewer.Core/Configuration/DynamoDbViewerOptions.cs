namespace AppSyncCognitoSample.DynamoDbViewer.Core;

public sealed class DynamoDbViewerOptions
{
    public string Region { get; set; } = "ap-northeast-1";

    /// <summary>
    /// Optional AWS profile name. When blank, the AWS SDK default credential chain is used.
    /// </summary>
    public string? ProfileName { get; set; }

    /// <summary>
    /// Optional prefix used to narrow the table list for verification work.
    /// </summary>
    public string? TableNamePrefix { get; set; }

    public int ScanLimit { get; set; } = 100;
}

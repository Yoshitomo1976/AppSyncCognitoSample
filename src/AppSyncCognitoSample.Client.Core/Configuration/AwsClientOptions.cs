namespace AppSyncCognitoSample.ClientCore.Configuration;

public sealed class AwsClientOptions
{
    public string Region { get; set; } = "ap-northeast-1";

    public string CognitoClientId { get; set; } = string.Empty;

    public string AppSyncGraphQlUrl { get; set; } = string.Empty;
}

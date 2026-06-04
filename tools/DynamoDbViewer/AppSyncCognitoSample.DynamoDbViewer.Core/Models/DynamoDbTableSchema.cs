namespace AppSyncCognitoSample.DynamoDbViewer.Core;

public sealed class DynamoDbTableSchema
{
    public required string TableName { get; init; }

    public required IReadOnlyList<string> KeyAttributeNames { get; init; }

    public string KeySummary => string.Join(", ", KeyAttributeNames);
}

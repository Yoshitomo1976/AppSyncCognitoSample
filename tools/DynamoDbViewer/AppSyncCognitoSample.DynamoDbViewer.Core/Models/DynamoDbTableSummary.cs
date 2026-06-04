namespace AppSyncCognitoSample.DynamoDbViewer.Core;

public sealed class DynamoDbTableSummary
{
    public required string TableName { get; init; }

    public override string ToString() => TableName;
}

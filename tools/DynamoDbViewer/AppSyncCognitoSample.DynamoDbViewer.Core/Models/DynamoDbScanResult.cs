using Amazon.DynamoDBv2.Model;

namespace AppSyncCognitoSample.DynamoDbViewer.Core;

public sealed class DynamoDbScanResult
{
    public required IReadOnlyList<DynamoDbItemRow> Items { get; init; }

    public Dictionary<string, AttributeValue>? LastEvaluatedKey { get; init; }

    public bool HasMore => LastEvaluatedKey is { Count: > 0 };
}
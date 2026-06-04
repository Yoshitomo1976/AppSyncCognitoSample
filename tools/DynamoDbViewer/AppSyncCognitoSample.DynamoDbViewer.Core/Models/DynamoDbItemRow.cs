using Amazon.DynamoDBv2.Model;

namespace AppSyncCognitoSample.DynamoDbViewer.Core;

public sealed class DynamoDbItemRow
{
    public required Dictionary<string, AttributeValue> RawItem { get; init; }

    public required Dictionary<string, string?> DisplayValues { get; init; }
}
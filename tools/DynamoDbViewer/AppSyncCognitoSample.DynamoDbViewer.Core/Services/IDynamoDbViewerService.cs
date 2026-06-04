using Amazon.DynamoDBv2.Model;

namespace AppSyncCognitoSample.DynamoDbViewer.Core;

public interface IDynamoDbViewerService
{
    Task<IReadOnlyList<DynamoDbTableSummary>> ListTablesAsync(
        CancellationToken cancellationToken = default);

    Task<DynamoDbTableSchema> DescribeTableAsync(
        string tableName,
        CancellationToken cancellationToken = default);

    Task<DynamoDbScanResult> ScanAsync(
        string tableName,
        int limit = 100,
        Dictionary<string, AttributeValue>? exclusiveStartKey = null,
        CancellationToken cancellationToken = default);

    Task DeleteItemAsync(
        string tableName,
        IReadOnlyDictionary<string, AttributeValue> key,
        CancellationToken cancellationToken = default);
}
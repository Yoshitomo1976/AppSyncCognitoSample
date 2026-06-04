using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AppSyncCognitoSample.DynamoDbViewer.Core;
using AppSyncCognitoSample.DynamoDbViewer.Core.Utilities;

namespace AppSyncCognitoSample.DynamoDbViewer.Core.Services;

public sealed class DynamoDbViewerService : IDynamoDbViewerService
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly DynamoDbViewerOptions _options;

    public DynamoDbViewerService(IAmazonDynamoDB dynamoDb, DynamoDbViewerOptions options)
    {
        _dynamoDb = dynamoDb;
        _options = options;
    }

    public async Task<IReadOnlyList<DynamoDbTableSummary>> ListTablesAsync(
        CancellationToken cancellationToken = default)
    {
        var result = new List<DynamoDbTableSummary>();
        string? lastEvaluatedTableName = null;

        do
        {
            var response = await _dynamoDb.ListTablesAsync(
                new ListTablesRequest
                {
                    ExclusiveStartTableName = lastEvaluatedTableName,
                    Limit = 100
                },
                cancellationToken).ConfigureAwait(false);

            foreach (var tableName in response.TableNames)
            {
                if (!string.IsNullOrWhiteSpace(_options.TableNamePrefix)
                    && !tableName.StartsWith(_options.TableNamePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                result.Add(new DynamoDbTableSummary { TableName = tableName });
            }

            lastEvaluatedTableName = response.LastEvaluatedTableName;
        }
        while (!string.IsNullOrWhiteSpace(lastEvaluatedTableName));

        return result.OrderBy(x => x.TableName, StringComparer.OrdinalIgnoreCase).ToArray();
    }

    public async Task<DynamoDbTableSchema> DescribeTableAsync(
        string tableName,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tableName);

        var response = await _dynamoDb.DescribeTableAsync(
            new DescribeTableRequest { TableName = tableName },
            cancellationToken).ConfigureAwait(false);

        return new DynamoDbTableSchema
        {
            TableName = tableName,
            KeyAttributeNames = response.Table.KeySchema
                .OrderBy(x => x.KeyType == KeyType.HASH ? 0 : 1)
                .Select(x => x.AttributeName)
                .ToArray()
        };
    }

    public async Task<DynamoDbScanResult> ScanAsync(
        string tableName,
        int limit = 100,
        Dictionary<string, AttributeValue>? exclusiveStartKey = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tableName);

        var request = new ScanRequest
        {
            TableName = tableName,
            Limit = Math.Clamp(limit, 1, 500),
            ExclusiveStartKey = exclusiveStartKey
        };

        var response = await _dynamoDb.ScanAsync(request, cancellationToken)
            .ConfigureAwait(false);

        return new DynamoDbScanResult
        {
            Items = response.Items
                .Select(item => new DynamoDbItemRow
                {
                    RawItem = item,
                    DisplayValues = item.ToDictionary(
                        x => x.Key,
                        x => AttributeValueFormatter.Format(x.Value))
                })
                .ToArray(),
            LastEvaluatedKey = response.LastEvaluatedKey
        };
    }

    public async Task DeleteItemAsync(
        string tableName,
        IReadOnlyDictionary<string, AttributeValue> key,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tableName);

        if (key.Count == 0)
        {
            throw new ArgumentException("Delete key is empty.", nameof(key));
        }

        await _dynamoDb.DeleteItemAsync(
            new DeleteItemRequest
            {
                TableName = tableName,
                Key = key.ToDictionary(x => x.Key, x => x.Value)
            },
            cancellationToken).ConfigureAwait(false);
    }
}

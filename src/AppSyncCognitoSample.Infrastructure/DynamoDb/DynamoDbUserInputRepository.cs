using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AppSyncCognitoSample.Application.Interfaces;
using AppSyncCognitoSample.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSyncCognitoSample.Infrastructure.DynamoDb
{
    public sealed class DynamoDbUserInputRepository : IUserInputRepository
    {
        public const string TableNameEnvironmentVariable = "USER_INPUT_TABLE_NAME";

        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly string _tableName;

        public DynamoDbUserInputRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
            _tableName = Environment.GetEnvironmentVariable(TableNameEnvironmentVariable)
                ?? throw new InvalidOperationException($"Environment variable '{TableNameEnvironmentVariable}' is not set.");
        }

        public async Task SaveAsync(UserInputRecord record, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(record);

            var item = new Dictionary<string, AttributeValue>
            {
                ["pk"] = new(record.Pk),
                ["sk"] = new(record.Sk),
                ["id"] = new(record.Id),
                ["userSub"] = new(record.UserSub),
                ["title"] = new(record.Title),
                ["createdAt"] = new(record.CreatedAt.ToString("O"))
            };

            AddIfNotEmpty(item, "username", record.Username);
            AddIfNotEmpty(item, "email", record.Email);
            AddIfNotEmpty(item, "body", record.Body);

            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = item,
                ConditionExpression = "attribute_not_exists(pk) AND attribute_not_exists(sk)"
            };

            await _dynamoDb.PutItemAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private static void AddIfNotEmpty(IDictionary<string, AttributeValue> item, string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                item[key] = new AttributeValue(value);
            }
        }
    }
}

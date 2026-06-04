using System.Text.Json;
using Amazon.DynamoDBv2.Model;

namespace AppSyncCognitoSample.DynamoDbViewer.Core.Utilities;

public static class AttributeValueFormatter
{
    public static string? Format(AttributeValue? value)
    {
        if (value is null)
        {
            return null;
        }

        if (value.NULL == true)
        {
            return "<null>";
        }

        if (value.S is not null)
        {
            return value.S;
        }

        if (value.N is not null)
        {
            return value.N;
        }

        if (value.BOOL.HasValue)
        {
            return value.BOOL.Value ? "true" : "false";
        }

        if (value.SS is { Count: > 0 })
        {
            return string.Join(", ", value.SS);
        }

        if (value.NS is { Count: > 0 })
        {
            return string.Join(", ", value.NS);
        }

        if (value.B is not null)
        {
            return $"<binary:{value.B.Length} bytes>";
        }

        if (value.BS is { Count: > 0 })
        {
            return $"<binary set:{value.BS.Count}>";
        }

        if (value.L is { Count: > 0 })
        {
            return JsonSerializer.Serialize(value.L.Select(Format).ToArray());
        }

        if (value.M is { Count: > 0 })
        {
            var map = value.M.ToDictionary(x => x.Key, x => Format(x.Value));
            return JsonSerializer.Serialize(map);
        }

        return string.Empty;
    }
}

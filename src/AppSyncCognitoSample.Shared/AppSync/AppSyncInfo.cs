using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppSyncCognitoSample.Shared.AppSync
{
    public sealed class AppSyncInfo
    {
        [JsonPropertyName("fieldName")]
        public string? FieldName { get; set; }

        [JsonPropertyName("parentTypeName")]
        public string? ParentTypeName { get; set; }
    }
}

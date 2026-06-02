using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppSyncCognitoSample.Shared.AppSync
{
    public sealed class AppSyncRequest
    {
        [JsonPropertyName("arguments")]
        public AppSyncArguments Arguments { get; set; } = new();

        [JsonPropertyName("identity")]
        public AppSyncIdentity? Identity { get; set; }

        [JsonPropertyName("info")]
        public AppSyncInfo? Info { get; set; }
    }
}

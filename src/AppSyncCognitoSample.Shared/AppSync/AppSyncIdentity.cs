using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppSyncCognitoSample.Shared.AppSync
{
    public sealed class AppSyncIdentity
    {
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("claims")]
        public AppSyncClaims Claims { get; set; } = new();
    }
}

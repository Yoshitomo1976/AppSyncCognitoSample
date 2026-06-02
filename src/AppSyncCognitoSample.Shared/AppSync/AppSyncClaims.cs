using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppSyncCognitoSample.Shared.AppSync
{
    public sealed class AppSyncClaims
    {
        [JsonPropertyName("sub")]
        public string? Sub { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("cognito:username")]
        public string? CognitoUsername { get; set; }

        // Keeps unknown Cognito claims such as email_verified, token_use, auth_time, etc.
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtensionData { get; set; }
    }
}

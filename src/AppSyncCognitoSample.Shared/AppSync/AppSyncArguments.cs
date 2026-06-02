using AppSyncCognitoSample.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppSyncCognitoSample.Shared.AppSync
{
    public sealed class AppSyncArguments
    {
        [JsonPropertyName("input")]
        public SaveUserInput Input { get; set; } = new();
    }
}

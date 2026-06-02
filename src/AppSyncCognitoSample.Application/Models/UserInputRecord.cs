using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSyncCognitoSample.Application.Models
{
    public sealed class UserInputRecord
    {
        public string Pk => $"USER#{UserSub}";

        public string Sk => $"INPUT#{CreatedAt:O}#{Id}";

        public string Id { get; init; } = string.Empty;

        public string UserSub { get; init; } = string.Empty;

        public string? Username { get; init; }

        public string? Email { get; init; }

        public string Title { get; init; } = string.Empty;

        public string? Body { get; init; }

        public DateTimeOffset CreatedAt { get; init; }
    }
}

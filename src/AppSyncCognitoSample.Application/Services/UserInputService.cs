using AppSyncCognitoSample.Application.Interfaces;
using AppSyncCognitoSample.Application.Models;
using AppSyncCognitoSample.Shared.AppSync;
using AppSyncCognitoSample.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSyncCognitoSample.Application.Services
{
    public sealed class UserInputService : IUserInputService
    {
        private readonly IUserInputRepository _repository;

        public UserInputService(IUserInputRepository repository)
        {
            _repository = repository;
        }

        public async Task<SaveUserInputResult> SaveAsync(AppSyncRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var input = request.Arguments?.Input
                ?? throw new ArgumentException("arguments.input is required.", nameof(request));

            var title = NormalizeRequired(input.Title, "title");
            var userSub = NormalizeRequired(request.Identity?.Claims?.Sub, "identity.claims.sub");

            var username = request.Identity?.Username
                ?? request.Identity?.Claims?.CognitoUsername;

            var createdAt = DateTimeOffset.UtcNow;
            var record = new UserInputRecord
            {
                Id = Guid.NewGuid().ToString("N"),
                UserSub = userSub,
                Username = username,
                Email = request.Identity?.Claims?.Email,
                Title = title,
                Body = string.IsNullOrWhiteSpace(input.Body) ? null : input.Body.Trim(),
                CreatedAt = createdAt
            };

            await _repository.SaveAsync(record, cancellationToken).ConfigureAwait(false);

            return new SaveUserInputResult
            {
                Id = record.Id,
                UserSub = record.UserSub,
                Username = record.Username,
                Email = record.Email,
                Title = record.Title,
                Body = record.Body,
                CreatedAt = record.CreatedAt.ToString("O")
            };
        }

        private static string NormalizeRequired(string? value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{name} is required.", name);
            }

            return value.Trim();
        }
    }
}

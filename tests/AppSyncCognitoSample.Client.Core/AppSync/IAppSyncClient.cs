using AppSyncCognitoSample.Client.Core;
using AppSyncCognitoSample.ClientCore.Models;

namespace AppSyncCognitoSample.ClientCore.AppSync;

public interface IAppSyncClient
{
    Task<SaveUserInputResult> SaveUserInputAsync(
        UserInputPayload input,
        CancellationToken cancellationToken = default);
}
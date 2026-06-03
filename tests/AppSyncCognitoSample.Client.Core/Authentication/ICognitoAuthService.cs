using AppSyncCognitoSample.Client.Core;

namespace AppSyncCognitoSample.ClientCore.Authentication;

public interface ICognitoAuthService
{
    Task<LoginResult> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default);

    Task<LoginResult> RefreshAsync(
        CancellationToken cancellationToken = default);

    Task LogoutAsync(
        CancellationToken cancellationToken = default);

    Task<bool> IsLoggedInAsync(
        CancellationToken cancellationToken = default);
}
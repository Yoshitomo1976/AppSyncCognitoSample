namespace AppSyncCognitoSample.ClientCore.Authentication.HostedUi;

public interface IHostedUiAuthService
{
    /// <summary>
    /// 既定ブラウザで Cognito Hosted UI を開き、Authorization Code Flow with PKCE でトークンを取得します。
    /// </summary>
    Task<OAuthTokenResult> LoginAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Refresh Token を使って ID Token / Access Token を再取得します。
    /// </summary>
    Task<OAuthTokenResult> RefreshAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);
}

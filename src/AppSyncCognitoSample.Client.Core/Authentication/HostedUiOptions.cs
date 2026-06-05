namespace AppSyncCognitoSample.ClientCore.Authentication.HostedUi;

/// <summary>
/// Cognito Hosted UI の認証に必要な設定です。
/// </summary>
public sealed class HostedUiOptions
{
    /// <summary>
    /// Cognito Hosted UI のドメインです。
    /// 例: https://xxxxx.auth.ap-northeast-1.amazoncognito.com
    /// </summary>
    public string Domain { get; set; } = string.Empty;

    /// <summary>
    /// Cognito User Pool App Client ID です。
    /// Public client 前提のため、Client secret は使いません。
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Cognito App Client の Allowed callback URLs に登録した Redirect URI です。
    /// 例: http://localhost:51789/callback/
    /// </summary>
    public string RedirectUri { get; set; } = "http://localhost:51789/callback/";

    public string LogoutUri { get; set; } = "http://localhost:51789/signout/";

    /// <summary>
    /// 要求する OAuth scope です。
    /// 例: openid, email, profile
    /// </summary>
    public string[] Scopes { get; set; } = ["openid", "email", "profile"];

    /// <summary>
    /// ローカルコールバック待受のタイムアウトです。
    /// </summary>
    public TimeSpan CallbackTimeout { get; set; } = TimeSpan.FromMinutes(3);

    /// <summary>
    /// 認証エンドポイントです。通常は未設定で構いません。
    /// 未設定の場合は {Domain}/oauth2/authorize を使います。
    /// </summary>
    public string? AuthorizationEndpoint { get; set; }

    /// <summary>
    /// トークンエンドポイントです。通常は未設定で構いません。
    /// 未設定の場合は {Domain}/oauth2/token を使います。
    /// </summary>
    public string? TokenEndpoint { get; set; }

    /// <summary>
    /// 認証成功後にブラウザへ返す HTML タイトルです。
    /// </summary>
    public string SuccessPageTitle { get; set; } = "Login completed";

    /// <summary>
    /// 認証成功後にブラウザへ返す HTML 本文です。
    /// </summary>
    public string SuccessPageMessage { get; set; } = "Login completed. You can close this browser window.";

    public string GetAuthorizationEndpoint()
    {
        return string.IsNullOrWhiteSpace(AuthorizationEndpoint)
            ? CombineUrl(Domain, "/oauth2/authorize")
            : AuthorizationEndpoint;
    }

    public string GetTokenEndpoint()
    {
        return string.IsNullOrWhiteSpace(TokenEndpoint)
            ? CombineUrl(Domain, "/oauth2/token")
            : TokenEndpoint;
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Domain))
        {
            throw new InvalidOperationException("HostedUiOptions.Domain is required.");
        }

        if (string.IsNullOrWhiteSpace(ClientId))
        {
            throw new InvalidOperationException("HostedUiOptions.ClientId is required.");
        }

        if (string.IsNullOrWhiteSpace(RedirectUri))
        {
            throw new InvalidOperationException("HostedUiOptions.RedirectUri is required.");
        }

        if (!Uri.TryCreate(RedirectUri, UriKind.Absolute, out var redirectUri))
        {
            throw new InvalidOperationException("HostedUiOptions.RedirectUri must be an absolute URI.");
        }

        if (!string.Equals(redirectUri.Scheme, "http", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("For this sample, RedirectUri must use http://localhost.");
        }

        if (!string.Equals(redirectUri.Host, "localhost", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(redirectUri.Host, "127.0.0.1", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("For this sample, RedirectUri host must be localhost or 127.0.0.1.");
        }

        if (Scopes is null || Scopes.Length == 0)
        {
            throw new InvalidOperationException("At least one scope is required.");
        }
    }

    private static string CombineUrl(string baseUrl, string path)
    {
        return baseUrl.TrimEnd('/') + "/" + path.TrimStart('/');
    }
}

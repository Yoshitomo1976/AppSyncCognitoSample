using System.Diagnostics;
using System.Net;

namespace AppSyncCognitoSample.ClientCore.Authentication.HostedUi;

/// <summary>
/// Cognito Hosted UI + Authorization Code Flow with PKCE を実行するサービスです。
/// </summary>
public sealed class HostedUiAuthService : IHostedUiAuthService
{
    private readonly HostedUiOptions _options;
    private readonly LocalCallbackServer _callbackServer;
    private readonly OAuthTokenClient _tokenClient;

    public HostedUiAuthService(HostedUiOptions options)
        : this(options, new LocalCallbackServer(), new OAuthTokenClient())
    {
    }

    public HostedUiAuthService(
        HostedUiOptions options,
        LocalCallbackServer callbackServer,
        OAuthTokenClient tokenClient)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _callbackServer = callbackServer ?? throw new ArgumentNullException(nameof(callbackServer));
        _tokenClient = tokenClient ?? throw new ArgumentNullException(nameof(tokenClient));
    }

    public async Task<OAuthTokenResult> LoginAsync(CancellationToken cancellationToken = default)
    {
        _options.Validate();

        var pkce = PkceGenerator.Generate();
        var state = PkceGenerator.GenerateState();
        var authorizationUrl = BuildAuthorizationUrl(pkce.CodeChallenge, state);

        var callbackTask = _callbackServer.WaitForCallbackAsync(
            _options.RedirectUri,
            state,
            _options.CallbackTimeout,
            _options.SuccessPageTitle,
            _options.SuccessPageMessage,
            cancellationToken);

        OpenBrowser(authorizationUrl);

        var callbackResult = await callbackTask.ConfigureAwait(false);

        return await _tokenClient.ExchangeCodeAsync(
            _options.GetTokenEndpoint(),
            _options.ClientId,
            _options.RedirectUri,
            callbackResult.Code,
            pkce.CodeVerifier,
            cancellationToken).ConfigureAwait(false);
    }

    public async Task<OAuthTokenResult> RefreshAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        _options.Validate();

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new ArgumentException("Refresh token is required.", nameof(refreshToken));
        }

        return await _tokenClient.RefreshAsync(
            _options.GetTokenEndpoint(),
            _options.ClientId,
            refreshToken,
            cancellationToken).ConfigureAwait(false);
    }

    private string BuildAuthorizationUrl(string codeChallenge, string state)
    {
        var parameters = new Dictionary<string, string>
        {
            ["response_type"] = "code",
            ["client_id"] = _options.ClientId,
            ["redirect_uri"] = _options.RedirectUri,
            ["scope"] = string.Join(' ', _options.Scopes),
            ["code_challenge_method"] = "S256",
            ["code_challenge"] = codeChallenge,
            ["state"] = state
        };

        var query = string.Join("&", parameters.Select(kvp =>
            $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

        return _options.GetAuthorizationEndpoint() + "?" + query;
    }

    private static void OpenBrowser(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to open the default browser.", ex);
        }
    }
}

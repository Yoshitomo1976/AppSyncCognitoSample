using AppSyncCognitoSample.ClientCore.Configuration;
using AppSyncCognitoSample.ClientCore.TokenStorage;
using Microsoft.Extensions.Options;
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
    private readonly ITokenStore _tokenStore;

    public HostedUiAuthService(
        ITokenStore tokenStore,
        IOptions<HostedUiOptions> options)
        : this(tokenStore, options, new LocalCallbackServer(), new OAuthTokenClient())
    {
    }

    public HostedUiAuthService(
        ITokenStore tokenStore,
        IOptions<HostedUiOptions> options,
        LocalCallbackServer callbackServer,
        OAuthTokenClient tokenClient)
    {
        _tokenStore = tokenStore ?? throw new ArgumentNullException(nameof(tokenStore));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _callbackServer = callbackServer ?? throw new ArgumentNullException(nameof(callbackServer));
        _tokenClient = tokenClient ?? throw new ArgumentNullException(nameof(tokenClient));
    }

    public async Task<OAuthTokenResult> LoginAsync(CancellationToken cancellationToken = default)
    {
        _options.Validate();

        var pkce = PkceGenerator.Generate();
        var state = PkceGenerator.GenerateState();
        var authorizationUrl = BuildAuthorizationUrl(pkce.CodeChallenge, state);

        var callbackTask = _callbackServer.WaitForAuthorizationCodeAsync(
            _options.RedirectUri,
            state,
            _options.CallbackTimeout,
            _options.SuccessPageTitle,
            _options.SuccessPageMessage,
            cancellationToken);

        OpenBrowser(authorizationUrl);

        var callbackResult = await callbackTask.ConfigureAwait(false);

        var tokenResult = await _tokenClient.ExchangeCodeAsync(
            _options.GetTokenEndpoint(),
            _options.ClientId,
            _options.RedirectUri,
            callbackResult.Code,
            pkce.CodeVerifier,
            cancellationToken).ConfigureAwait(false);

        await _tokenStore.SaveAsync(
            new StoredTokenSet
            {
                IdToken = tokenResult.IdToken,
                AccessToken = tokenResult.AccessToken,
                RefreshToken = tokenResult.RefreshToken,
                ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(tokenResult.ExpiresIn),
            },
            cancellationToken).ConfigureAwait(false);

        return tokenResult;
    }

    public async Task LogoutAsync(
        CancellationToken cancellationToken = default)
    {
        await _tokenStore.ClearAsync(cancellationToken).ConfigureAwait(false); 
        OpenBrowser(BuildLogoutUrl());
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

        var tokenResult = await _tokenClient.RefreshAsync(
            _options.GetTokenEndpoint(),
            _options.ClientId,
            refreshToken,
            cancellationToken).ConfigureAwait(false);

        await _tokenStore.SaveAsync(
            new StoredTokenSet
            {
                IdToken = tokenResult.IdToken,
                AccessToken = tokenResult.AccessToken,
                RefreshToken = tokenResult.RefreshToken,
                ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(tokenResult.ExpiresIn),
            },
            cancellationToken).ConfigureAwait(false);

        return tokenResult;
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

    private string BuildLogoutUrl()
    {
        var query = new Dictionary<string, string>
        {
            ["client_id"] = _options.ClientId,
            ["logout_uri"] = _options.LogoutUri
        };

        return $"{_options.Domain.TrimEnd('/')}/logout?{BuildQueryString(query)}";
    }

    private static string BuildQueryString(
        IReadOnlyDictionary<string, string> values)
    {
        return string.Join(
            "&",
            values.Select(x =>
                $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value)}"));
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

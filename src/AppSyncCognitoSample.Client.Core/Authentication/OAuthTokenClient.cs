using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace AppSyncCognitoSample.ClientCore.Authentication.HostedUi;

public sealed class OAuthTokenResult
{
    public required string IdToken { get; init; }

    public required string AccessToken { get; init; }

    public string? RefreshToken { get; init; }

    public int ExpiresIn { get; init; }

    public string? TokenType { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }
}

internal sealed class CognitoTokenEndpointResponse
{
    [JsonPropertyName("id_token")]
    public string? IdToken { get; init; }

    [JsonPropertyName("access_token")]
    public string? AccessToken { get; init; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; init; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; init; }

    [JsonPropertyName("error")]
    public string? Error { get; init; }

    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; init; }
}

/// <summary>
/// Cognito Token Endpoint と通信し、authorization code をトークンへ交換します。
/// </summary>
public sealed class OAuthTokenClient
{
    private readonly HttpClient _httpClient;

    public OAuthTokenClient()
        : this(new HttpClient())
    {
    }

    public OAuthTokenClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OAuthTokenResult> ExchangeCodeAsync(
        string tokenEndpoint,
        string clientId,
        string redirectUri,
        string code,
        string codeVerifier,
        CancellationToken cancellationToken = default)
    {
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = clientId,
            ["redirect_uri"] = redirectUri,
            ["code"] = code,
            ["code_verifier"] = codeVerifier
        });

        using var response = await _httpClient.PostAsync(
            tokenEndpoint,
            content,
            cancellationToken).ConfigureAwait(false);

        var tokenResponse = await response.Content
            .ReadFromJsonAsync<CognitoTokenEndpointResponse>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var error = tokenResponse?.Error ?? response.StatusCode.ToString();
            var description = tokenResponse?.ErrorDescription;
            throw new InvalidOperationException($"Token endpoint returned error: {error} {description}");
        }

        if (tokenResponse is null)
        {
            throw new InvalidOperationException("Token endpoint response could not be parsed.");
        }

        if (string.IsNullOrWhiteSpace(tokenResponse.IdToken))
        {
            throw new InvalidOperationException("Token endpoint response did not contain id_token.");
        }

        if (string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
        {
            throw new InvalidOperationException("Token endpoint response did not contain access_token.");
        }

        return new OAuthTokenResult
        {
            IdToken = tokenResponse.IdToken,
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            ExpiresIn = tokenResponse.ExpiresIn,
            TokenType = tokenResponse.TokenType,
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn)
        };
    }

    public async Task<OAuthTokenResult> RefreshAsync(
        string tokenEndpoint,
        string clientId,
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["client_id"] = clientId,
            ["refresh_token"] = refreshToken
        });

        using var response = await _httpClient.PostAsync(
            tokenEndpoint,
            content,
            cancellationToken).ConfigureAwait(false);

        var tokenResponse = await response.Content
            .ReadFromJsonAsync<CognitoTokenEndpointResponse>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var error = tokenResponse?.Error ?? response.StatusCode.ToString();
            var description = tokenResponse?.ErrorDescription;
            throw new InvalidOperationException($"Token endpoint returned error: {error} {description}");
        }

        if (tokenResponse is null)
        {
            throw new InvalidOperationException("Token endpoint response could not be parsed.");
        }

        if (string.IsNullOrWhiteSpace(tokenResponse.IdToken))
        {
            throw new InvalidOperationException("Token endpoint response did not contain id_token.");
        }

        if (string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
        {
            throw new InvalidOperationException("Token endpoint response did not contain access_token.");
        }

        return new OAuthTokenResult
        {
            IdToken = tokenResponse.IdToken,
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken ?? refreshToken,
            ExpiresIn = tokenResponse.ExpiresIn,
            TokenType = tokenResponse.TokenType,
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn)
        };
    }
}

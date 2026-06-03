using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using AppSyncCognitoSample.ClientCore.Configuration;
using AppSyncCognitoSample.ClientCore.TokenStorage;
using Microsoft.Extensions.Options;

namespace AppSyncCognitoSample.ClientCore.Authentication;

public sealed class CognitoAuthService : ICognitoAuthService
{
    private readonly IAmazonCognitoIdentityProvider _cognito;
    private readonly ITokenStore _tokenStore;
    private readonly AwsClientOptions _options;

    public CognitoAuthService(
        IAmazonCognitoIdentityProvider cognito,
        ITokenStore tokenStore,
        IOptions<AwsClientOptions> options)
    {
        _cognito = cognito;
        _tokenStore = tokenStore;
        _options = options.Value;
    }

    public async Task<LoginResult> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _cognito.InitiateAuthAsync(
            new InitiateAuthRequest
            {
                ClientId = _options.CognitoClientId,
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                    ["USERNAME"] = request.Username,
                    ["PASSWORD"] = request.Password
                }
            },
            cancellationToken).ConfigureAwait(false);

        if (response.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
        {
            throw new InvalidOperationException(
                "Cognito user requires a new password. Complete the initial password change first.");
        }

        if (response.AuthenticationResult is null)
        {
            throw new InvalidOperationException(
                "Cognito authentication did not return tokens.");
        }

        var result = ToLoginResult(response.AuthenticationResult);

        await _tokenStore.SaveAsync(
            new StoredTokenSet
            {
                IdToken = result.IdToken,
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                ExpiresAt = result.ExpiresAt
            },
            cancellationToken).ConfigureAwait(false);

        return result;
    }

    public async Task<LoginResult> RefreshAsync(
        CancellationToken cancellationToken = default)
    {
        var current = await _tokenStore.LoadAsync(cancellationToken)
            .ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(current?.RefreshToken))
        {
            throw new InvalidOperationException("Refresh token is not available.");
        }

        var response = await _cognito.InitiateAuthAsync(
            new InitiateAuthRequest
            {
                ClientId = _options.CognitoClientId,
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                    ["REFRESH_TOKEN"] = current.RefreshToken
                }
            },
            cancellationToken).ConfigureAwait(false);

        if (response.AuthenticationResult is null)
        {
            throw new InvalidOperationException(
                "Cognito refresh did not return tokens.");
        }

        var result = ToLoginResult(
            response.AuthenticationResult,
            fallbackRefreshToken: current.RefreshToken);

        await _tokenStore.SaveAsync(
            new StoredTokenSet
            {
                IdToken = result.IdToken,
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                ExpiresAt = result.ExpiresAt
            },
            cancellationToken).ConfigureAwait(false);

        return result;
    }

    public async Task LogoutAsync(
        CancellationToken cancellationToken = default)
    {
        await _tokenStore.ClearAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<bool> IsLoggedInAsync(
        CancellationToken cancellationToken = default)
    {
        var tokenSet = await _tokenStore.LoadAsync(cancellationToken)
            .ConfigureAwait(false);

        return tokenSet is not null
            && !string.IsNullOrWhiteSpace(tokenSet.IdToken)
            && !tokenSet.IsExpiredOrNearExpiry(TimeSpan.FromMinutes(1));
    }

    private static LoginResult ToLoginResult(
        AuthenticationResultType auth,
        string? fallbackRefreshToken = null)
    {
        var expiresAt = DateTimeOffset.UtcNow.AddSeconds((double)auth.ExpiresIn!);

        return new LoginResult
        {
            IdToken = auth.IdToken,
            AccessToken = auth.AccessToken,
            RefreshToken = auth.RefreshToken ?? fallbackRefreshToken,
            ExpiresIn = (int)auth.ExpiresIn,
            ExpiresAt = expiresAt,
            TokenType = auth.TokenType
        };
    }
}
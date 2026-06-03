namespace AppSyncCognitoSample.ClientCore.Authentication;

public sealed class LoginResult
{
    public required string IdToken { get; init; }

    public required string AccessToken { get; init; }

    public string? RefreshToken { get; init; }

    public int ExpiresIn { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }

    public string? TokenType { get; init; }
}
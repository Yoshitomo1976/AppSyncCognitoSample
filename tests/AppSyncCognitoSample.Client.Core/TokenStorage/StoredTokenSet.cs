namespace AppSyncCognitoSample.ClientCore.TokenStorage;

public sealed class StoredTokenSet
{
    public required string IdToken { get; init; }

    public required string AccessToken { get; init; }

    public string? RefreshToken { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }

    public bool IsExpiredOrNearExpiry(TimeSpan margin)
    {
        return DateTimeOffset.UtcNow >= ExpiresAt.Subtract(margin);
    }
}
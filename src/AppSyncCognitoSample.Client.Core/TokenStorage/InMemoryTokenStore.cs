namespace AppSyncCognitoSample.ClientCore.TokenStorage;

public sealed class InMemoryTokenStore : ITokenStore
{
    private StoredTokenSet? _tokenSet;

    public Task SaveAsync(
        StoredTokenSet tokenSet,
        CancellationToken cancellationToken = default)
    {
        _tokenSet = tokenSet;
        return Task.CompletedTask;
    }

    public Task<StoredTokenSet?> LoadAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_tokenSet);
    }

    public Task ClearAsync(
        CancellationToken cancellationToken = default)
    {
        _tokenSet = null;
        return Task.CompletedTask;
    }
}
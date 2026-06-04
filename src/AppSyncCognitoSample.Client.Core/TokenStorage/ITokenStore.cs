namespace AppSyncCognitoSample.ClientCore.TokenStorage;

public interface ITokenStore
{
    Task SaveAsync(
        StoredTokenSet tokenSet,
        CancellationToken cancellationToken = default);

    Task<StoredTokenSet?> LoadAsync(
        CancellationToken cancellationToken = default);

    Task ClearAsync(
        CancellationToken cancellationToken = default);
}
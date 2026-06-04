namespace AppSyncCognitoSample.ClientCore.Models;

public sealed class SaveUserInputResult
{
    public required string Id { get; init; }

    public required string UserSub { get; init; }

    public string? Username { get; init; }

    public string? Email { get; init; }

    public required string Title { get; init; }

    public string? Body { get; init; }

    public required string CreatedAt { get; init; }
}
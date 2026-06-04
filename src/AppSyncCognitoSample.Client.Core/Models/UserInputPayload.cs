namespace AppSyncCognitoSample.ClientCore.Models;

public sealed class UserInputPayload
{
    public required string Title { get; init; }

    public string? Body { get; init; }
}
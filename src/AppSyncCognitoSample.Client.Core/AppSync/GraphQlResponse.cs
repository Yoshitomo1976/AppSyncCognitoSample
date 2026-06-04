namespace AppSyncCognitoSample.ClientCore.AppSync;

public sealed class GraphQlResponse<T>
{
    public T? Data { get; init; }

    public GraphQlError[]? Errors { get; init; }

    public bool HasErrors => Errors is { Length: > 0 };
}

public sealed class GraphQlError
{
    public string? Message { get; init; }

    public string? ErrorType { get; init; }
}
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using AppSyncCognitoSample.ClientCore.Authentication;
using AppSyncCognitoSample.ClientCore.Configuration;
using AppSyncCognitoSample.ClientCore.Models;
using AppSyncCognitoSample.ClientCore.TokenStorage;
using Microsoft.Extensions.Options;

namespace AppSyncCognitoSample.ClientCore.AppSync;

public sealed class AppSyncClient : IAppSyncClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly ITokenStore _tokenStore;
    private readonly ICognitoAuthService _authService;
    private readonly AwsClientOptions _options;

    public AppSyncClient(
        HttpClient httpClient,
        ITokenStore tokenStore,
        ICognitoAuthService authService,
        IOptions<AwsClientOptions> options)
    {
        _httpClient = httpClient;
        _tokenStore = tokenStore;
        _authService = authService;
        _options = options.Value;
    }

    public async Task<SaveUserInputResult> SaveUserInputAsync(
        UserInputPayload input,
        CancellationToken cancellationToken = default)
    {
        var tokenSet = await GetValidTokenSetAsync(cancellationToken)
            .ConfigureAwait(false);

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            _options.AppSyncGraphQlUrl);

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenSet.IdToken);

        request.Content = JsonContent.Create(
            new GraphQlRequest
            {
                Query = """
                    mutation Save($input: SaveUserInput!) {
                      saveUserInput(input: $input) {
                        id
                        userSub
                        username
                        email
                        title
                        body
                        createdAt
                      }
                    }
                    """,
                Variables = new
                {
                    input = new
                    {
                        title = input.Title,
                        body = input.Body
                    }
                }
            },
            options: JsonOptions);

        using var response = await _httpClient.SendAsync(
            request,
            cancellationToken).ConfigureAwait(false);

        var responseText = await response.Content.ReadAsStringAsync(
            cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"AppSync request failed. StatusCode={response.StatusCode}, Body={responseText}");
        }

        var graphQlResponse =
            JsonSerializer.Deserialize<GraphQlResponse<SaveUserInputGraphQlData>>(
                responseText,
                JsonOptions);

        if (graphQlResponse is null)
        {
            throw new InvalidOperationException("AppSync response could not be parsed.");
        }

        if (graphQlResponse.HasErrors)
        {
            var messages = string.Join(
                Environment.NewLine,
                graphQlResponse.Errors!.Select(x => x.Message));

            throw new InvalidOperationException($"AppSync returned errors: {messages}");
        }

        if (graphQlResponse.Data?.SaveUserInput is null)
        {
            throw new InvalidOperationException("AppSync response did not contain saveUserInput.");
        }

        return graphQlResponse.Data.SaveUserInput;
    }

    private async Task<StoredTokenSet> GetValidTokenSetAsync(
        CancellationToken cancellationToken)
    {
        var tokenSet = await _tokenStore.LoadAsync(cancellationToken)
            .ConfigureAwait(false);

        if (tokenSet is null)
        {
            throw new InvalidOperationException("User is not logged in.");
        }

        if (!tokenSet.IsExpiredOrNearExpiry(TimeSpan.FromMinutes(2)))
        {
            return tokenSet;
        }

        await _authService.RefreshAsync(cancellationToken)
            .ConfigureAwait(false);

        var refreshed = await _tokenStore.LoadAsync(cancellationToken)
            .ConfigureAwait(false);

        return refreshed
            ?? throw new InvalidOperationException("Token refresh succeeded but token store is empty.");
    }
}
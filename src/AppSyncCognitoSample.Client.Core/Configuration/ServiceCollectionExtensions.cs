using Amazon;
using Amazon.CognitoIdentityProvider;
using AppSyncCognitoSample.Client.Core;
using AppSyncCognitoSample.ClientCore.AppSync;
using AppSyncCognitoSample.ClientCore.Authentication;
using AppSyncCognitoSample.ClientCore.Authentication.HostedUi;
using AppSyncCognitoSample.ClientCore.TokenStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AppSyncCognitoSample.ClientCore.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppSyncCognitoClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AwsClientOptions>(
            configuration.GetSection("Aws"));

        services.Configure<HostedUiOptions>(
            configuration.GetSection("HostedUi"));

        services.AddSingleton<ITokenStore, InMemoryTokenStore>();

        services.AddSingleton<IAmazonCognitoIdentityProvider>(provider =>
        {
            var options = provider
                .GetRequiredService<IOptions<AwsClientOptions>>()
                .Value;

            return new AmazonCognitoIdentityProviderClient(
                RegionEndpoint.GetBySystemName(options.Region));
        });

        services.AddHttpClient<IAppSyncClient, AppSyncClient>();

        services.AddSingleton<ICognitoAuthService, CognitoAuthService>();

        services.AddSingleton<IHostedUiAuthService, HostedUiAuthService>();

        return services;
    }
}
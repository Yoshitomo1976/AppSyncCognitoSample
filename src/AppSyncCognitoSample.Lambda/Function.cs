using Amazon.Lambda.Core;
using AppSyncCognitoSample.Application.Interfaces;
using AppSyncCognitoSample.Shared.AppSync;
using AppSyncCognitoSample.Shared.Models;
using Microsoft.Extensions.DependencyInjection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AppSyncCognitoSample.Lambda
{
    public sealed class Function
    {
        private readonly IServiceProvider _serviceProvider;

        public Function()
            : this(ServiceProviderFactory.Create())
        {
        }

        // Constructor for unit tests.
        public Function(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<SaveUserInputResult> FunctionHandler(AppSyncRequest request, ILambdaContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IUserInputService>();

            context.Logger.LogInformation($"Handling AppSync field: {request.Info?.ParentTypeName}.{request.Info?.FieldName}");

            return await service.SaveAsync(request, CancellationToken.None).ConfigureAwait(false);
        }
    }
}

using AppSyncCognitoSample.Application.Interfaces;
using AppSyncCognitoSample.Application.Services;
using AppSyncCognitoSample.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSyncCognitoSample.Lambda
{
    public static class ServiceProviderFactory
    {
        private static readonly Lazy<IServiceProvider> Provider = new(CreateServiceProvider);

        public static IServiceProvider Create() => Provider.Value;

        private static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddScoped<IUserInputService, UserInputService>();
            services.AddInfrastructure();

            return services.BuildServiceProvider(validateScopes: false);
        }
    }
}

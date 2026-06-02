using Amazon.DynamoDBv2;
using AppSyncCognitoSample.Application.Interfaces;
using AppSyncCognitoSample.Infrastructure.DynamoDb;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSyncCognitoSample.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient());
            services.AddScoped<IUserInputRepository, DynamoDbUserInputRepository>();

            return services;
        }
    }
}

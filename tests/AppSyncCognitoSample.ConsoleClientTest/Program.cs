using AppSyncCognitoSample.ClientCore.AppSync;
using AppSyncCognitoSample.ClientCore.Authentication;
using AppSyncCognitoSample.ClientCore.Configuration;
using AppSyncCognitoSample.ClientCore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .Build();

var services = new ServiceCollection();

services.AddAppSyncCognitoClient(configuration);

using var serviceProvider = services.BuildServiceProvider();

var authService = serviceProvider.GetRequiredService<ICognitoAuthService>();
var appSyncClient = serviceProvider.GetRequiredService<IAppSyncClient>();

Console.Write("Username: ");
var username = Console.ReadLine();

Console.Write("Password: ");
var password = ReadPassword();

await authService.LoginAsync(new LoginRequest
{
    Username = username ?? string.Empty,
    Password = password
});

Console.WriteLine();
Console.WriteLine("Login succeeded.");

Console.Write("Title: ");
var title = Console.ReadLine();

Console.Write("Body: ");
var body = Console.ReadLine();

var result = await appSyncClient.SaveUserInputAsync(
    new UserInputPayload
    {
        Title = title ?? string.Empty,
        Body = body
    });

Console.WriteLine("Saved:");
Console.WriteLine($"  Id        : {result.Id}");
Console.WriteLine($"  UserSub   : {result.UserSub}");
Console.WriteLine($"  Username  : {result.Username}");
Console.WriteLine($"  Email     : {result.Email}");
Console.WriteLine($"  Title     : {result.Title}");
Console.WriteLine($"  Body      : {result.Body}");
Console.WriteLine($"  CreatedAt : {result.CreatedAt}");

static string ReadPassword()
{
    var password = string.Empty;

    while (true)
    {
        var key = Console.ReadKey(intercept: true);

        if (key.Key == ConsoleKey.Enter)
        {
            break;
        }

        if (key.Key == ConsoleKey.Backspace)
        {
            if (password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }

            continue;
        }

        password += key.KeyChar;
        Console.Write("*");
    }

    return password;
}
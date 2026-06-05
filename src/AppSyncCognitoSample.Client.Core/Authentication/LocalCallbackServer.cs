using System.Net;
using System.Text;
using System.Web;

namespace AppSyncCognitoSample.ClientCore.Authentication.HostedUi;

public sealed class LocalCallbackResult
{
    public required string Code { get; init; }

    public string? State { get; init; }

    public string? RawUrl { get; init; }
}

/// <summary>
/// Cognito Hosted UI から localhost callback に返ってきた authorization code を受け取ります。
/// </summary>
public sealed class LocalCallbackServer
{
    public async Task<LocalCallbackResult> WaitForAuthorizationCodeAsync(
        string redirectUri,
        string expectedState,
        TimeSpan timeout,
        string successPageTitle,
        string successPageMessage,
        CancellationToken cancellationToken = default)
    {
        if (!HttpListener.IsSupported)
        {
            throw new NotSupportedException("HttpListener is not supported on this platform.");
        }

        var prefix = NormalizeHttpListenerPrefix(redirectUri);

        using var timeoutCts = new CancellationTokenSource(timeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
            timeoutCts.Token,
            cancellationToken);

        using var listener = new HttpListener();
        listener.Prefixes.Add(prefix);
        listener.Start();

        try
        {
            var contextTask = listener.GetContextAsync();
            var completedTask = await Task.WhenAny(
                contextTask,
                Task.Delay(Timeout.InfiniteTimeSpan, linkedCts.Token)).ConfigureAwait(false);

            if (completedTask != contextTask)
            {
                throw new TimeoutException("Timed out waiting for the Cognito Hosted UI callback.");
            }

            var context = await contextTask.ConfigureAwait(false);
            var request = context.Request;

            var query = HttpUtility.ParseQueryString(request.Url?.Query ?? string.Empty);
            var error = query["error"];
            var errorDescription = query["error_description"];

            if (!string.IsNullOrWhiteSpace(error))
            {
                await WriteHtmlResponseAsync(
                    context.Response,
                    "Login failed",
                    $"Login failed: {WebUtility.HtmlEncode(error)}<br>{WebUtility.HtmlEncode(errorDescription)}",
                    cancellationToken).ConfigureAwait(false);

                throw new InvalidOperationException($"Hosted UI returned error: {error} {errorDescription}");
            }

            var code = query["code"];
            var state = query["state"];

            if (string.IsNullOrWhiteSpace(code))
            {
                await WriteHtmlResponseAsync(
                    context.Response,
                    "Login failed",
                    "Authorization code was not returned.",
                    cancellationToken).ConfigureAwait(false);

                throw new InvalidOperationException("Authorization code was not returned.");
            }

            if (!string.Equals(state, expectedState, StringComparison.Ordinal))
            {
                await WriteHtmlResponseAsync(
                    context.Response,
                    "Login failed",
                    "Invalid state value was returned.",
                    cancellationToken).ConfigureAwait(false);

                throw new InvalidOperationException("Invalid state value was returned.");
            }

            await WriteHtmlResponseAsync(
                context.Response,
                successPageTitle,
                successPageMessage,
                cancellationToken).ConfigureAwait(false);

            return new LocalCallbackResult
            {
                Code = code,
                State = state,
                RawUrl = request.RawUrl
            };
        }
        finally
        {
            if (listener.IsListening)
            {
                listener.Stop();
            }
        }
    }

    public async Task<LocalCallbackResult> WaitForLogoutCallbackAsync()
    {
        throw new NotSupportedException();
    }

    private static string NormalizeHttpListenerPrefix(string redirectUri)
    {
        if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out var uri))
        {
            throw new InvalidOperationException("RedirectUri must be an absolute URI.");
        }

        var builder = new UriBuilder(uri)
        {
            Query = string.Empty,
            Fragment = string.Empty
        };

        var prefix = builder.Uri.ToString();
        return prefix.EndsWith("/", StringComparison.Ordinal) ? prefix : prefix + "/";
    }

    private static async Task WriteHtmlResponseAsync(
        HttpListenerResponse response,
        string title,
        string message,
        CancellationToken cancellationToken)
    {
        var html = $"""
            <!doctype html>
            <html lang=""ja"">
            <head>
              <meta charset=""utf-8"">
              <title>{WebUtility.HtmlEncode(title)}</title>
            </head>
            <body>
              <h1>{WebUtility.HtmlEncode(title)}</h1>
              <p>{message}</p>
            </body>
            </html>
            """;

        var bytes = Encoding.UTF8.GetBytes(html);
        response.StatusCode = 200;
        response.ContentType = "text/html; charset=utf-8";
        response.ContentLength64 = bytes.Length;

        await response.OutputStream.WriteAsync(bytes, cancellationToken).ConfigureAwait(false);
        response.OutputStream.Close();
    }
}

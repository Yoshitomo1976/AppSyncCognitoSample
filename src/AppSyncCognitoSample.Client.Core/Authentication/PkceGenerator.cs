using System.Security.Cryptography;
using System.Text;

namespace AppSyncCognitoSample.ClientCore.Authentication.HostedUi;

public sealed record PkcePair(string CodeVerifier, string CodeChallenge);

/// <summary>
/// OAuth 2.0 PKCE の code_verifier / code_challenge を生成します。
/// </summary>
public static class PkceGenerator
{
    /// <summary>
    /// S256 方式の PKCE ペアを生成します。
    /// </summary>
    public static PkcePair Generate()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(32);
        var codeVerifier = Base64UrlEncode(randomBytes);

        var challengeBytes = SHA256.HashData(Encoding.ASCII.GetBytes(codeVerifier));
        var codeChallenge = Base64UrlEncode(challengeBytes);

        return new PkcePair(codeVerifier, codeChallenge);
    }

    internal static string GenerateState()
    {
        return Base64UrlEncode(RandomNumberGenerator.GetBytes(32));
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}

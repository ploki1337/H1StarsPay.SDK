using System.Security.Cryptography;
using System.Text;

namespace H1StarsPay.SDK.Webhooks;

/// <summary>
/// Метод для проверки подлинности webhook`а.
/// </summary>
public static class WebhookVerifier
{
    /// <summary>
    /// Проверяет подлинность webhook`а.
    /// </summary>
    /// <param name="payload">Необработанное тело запроса (raw JSON string).</param>
    /// <param name="signatureHeader">Значение заголовка X-Signature ("sha256=...").</param>
    /// <param name="secret">Секретный ключ для подписи вебхуков.</param>
    /// <returns>True, если подпись верна, иначе False.</returns>
    public static bool VerifySignature(string payload, string signatureHeader, string secret)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(payload);
        ArgumentException.ThrowIfNullOrWhiteSpace(signatureHeader);
        ArgumentException.ThrowIfNullOrWhiteSpace(secret);

        const string prefix = "sha256=";
        if (!signatureHeader.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var signature = signatureHeader.AsSpan()[prefix.Length..];
        var secretBytes = Encoding.UTF8.GetBytes(secret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);

        var computedHashBytes = HMACSHA256.HashData(secretBytes, payloadBytes);

        var computedHex = Convert.ToHexString(computedHashBytes);

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computedHex),
            Encoding.UTF8.GetBytes(signature.ToString().ToUpperInvariant())
        );
    }
}
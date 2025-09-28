using System.Text.Json.Serialization;

namespace H1StarsPay.SDK.Models;

/// <summary>
/// Информация о созданном платеже.
/// </summary>
public readonly record struct Payment(
    [property: JsonPropertyName("payment_id")] string PaymentId,
    [property: JsonPropertyName("payment_url")] string PaymentUrl,
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("payment_amount")] decimal PaymentAmount,
    [property: JsonPropertyName("status")] PaymentStatus Status,
    [property: JsonPropertyName("expires_at")] DateTimeOffset ExpiresAt,
    [property: JsonPropertyName("phone_number")] string PhoneNumber,
    [property: JsonPropertyName("bank")] string Bank
);
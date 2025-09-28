using System.Text.Json.Serialization;

namespace H1StarsPay.SDK.Models;

/// <summary>
/// Информация о платеже.
/// </summary>
public readonly record struct PaymentDetails(
    [property: JsonPropertyName("payment_id")] string PaymentId,
    [property: JsonPropertyName("status")] PaymentStatus Status,
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("payment_amount")] decimal PaymentAmount,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("completed_at")] DateTimeOffset? CompletedAt,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("user_id")] string UserId
);
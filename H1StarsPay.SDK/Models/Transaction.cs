using System.Text.Json.Serialization;

namespace H1StarsPay.SDK.Models;

/// <summary>
/// Представляет одну транзакцию в истории вашего личного кабинета.
/// </summary>
public readonly record struct Transaction(
    [property: JsonPropertyName("transaction_id")] string TransactionId,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt
);
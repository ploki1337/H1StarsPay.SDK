using System.Text.Json.Serialization;

namespace H1StarsPay.SDK.Models;

/// <summary>
/// Баланс в личном кабинете.
/// </summary>
public readonly record struct Balance(
    [property: JsonPropertyName("balance")] decimal CurrentBalance,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("pending_withdrawals")] decimal PendingWithdrawals,
    [property: JsonPropertyName("available_for_withdrawal")] decimal AvailableForWithdrawal,
    [property: JsonPropertyName("last_updated")] DateTimeOffset LastUpdated
);
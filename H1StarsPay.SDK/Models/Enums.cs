using System.Text.Json.Serialization;

namespace H1StarsPay.SDK.Models;

/// <summary>
/// Определяет статусы платежа.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<PaymentStatus>))]
public enum PaymentStatus
{
    Pending,
    Completed,
    Expired,
    Inactive
}
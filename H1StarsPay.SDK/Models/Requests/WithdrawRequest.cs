using System.Text.Json.Serialization;

namespace H1StarsPay.SDK.Models.Requests;

/// <summary>
/// Модель запроса для вывода средств.
/// </summary>
/// <param name="Amount">Сумма для вывода.</param>
/// <param name="PaymentMethod">Метод вывода (например, "card").</param>
/// <param name="CardNumber">Номер карты.</param>
/// <param name="CardholderName">Имя владельца карты.</param>
public readonly record struct WithdrawRequest(
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("payment_method")] string PaymentMethod,
    [property: JsonPropertyName("card_number")] string CardNumber,
    [property: JsonPropertyName("cardholder_name")] string CardholderName
);
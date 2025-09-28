using System.Text.Json.Serialization;

namespace H1StarsPay.SDK.Models.Requests;

/// <summary>
/// Модель запроса для создания нового платежа.
/// </summary>
/// <param name="Amount">Сумма.</param>
/// <param name="Description">Описание.</param>
/// <param name="UserId">ID пользователя в вашей системе.</param>
/// <param name="CallbackUrl">URL для получения webhook`а.</param>
/// <param name="ReturnUrl">URL для перенаправления пользователя после успешной оплаты.</param>
public readonly record struct CreatePaymentRequest(
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("callback_url")] string CallbackUrl,
    [property: JsonPropertyName("return_url")] string ReturnUrl
);
using H1StarsPay.SDK.Models;
using H1StarsPay.SDK.Models.Requests;
using System.Net.Http.Json;

namespace H1StarsPay.SDK.Clients;

/// <summary>
/// Клиент для взаимодействия с API платежей.
/// </summary>
public class PaymentsClient(HttpClient httpClient)
{
    /// <summary>
    /// Создает новый платеж.
    /// </summary>
    /// <returns>Информация о созданном платеже.</returns>
    public async ValueTask<Payment> CreateAsync(CreatePaymentRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("create-payment", request, ct);
        await H1StarsPayClient.HandleResponseErrorsAsync(response, ct);

        Payment? payment = await response.Content.ReadFromJsonAsync<Payment>(ct);
        if (!payment.HasValue)
        {
            throw new InvalidOperationException("Failed to deserialize payment response.");
        }
        return payment.Value;
    }

    /// <summary>
    /// Получает информацию о платеже по его ID.
    /// </summary>
    /// <returns>Информация о платеже.</returns>
    public async ValueTask<PaymentDetails> GetByIdAsync(string paymentId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(paymentId);

        var response = await httpClient.GetAsync($"payment/{paymentId}", ct);
        await H1StarsPayClient.HandleResponseErrorsAsync(response, ct);

        PaymentDetails? paymentDetails = await response.Content.ReadFromJsonAsync<PaymentDetails>(ct);
        if (!paymentDetails.HasValue)
        {
            throw new InvalidOperationException("Failed to deserialize payment details response.");
        }
        return paymentDetails.Value;
    }
}
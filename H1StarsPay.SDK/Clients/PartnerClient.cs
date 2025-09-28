using H1StarsPay.SDK.Models;
using H1StarsPay.SDK.Models.Requests;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace H1StarsPay.SDK.Clients;

/// <summary>
/// Клиент для взаимодействия с личным кабинетом через API.
/// </summary>
public class PartnerClient(HttpClient httpClient)
{
    /// <summary>
    /// Получает текущий баланс личного кабинета.
    /// </summary>
    public async ValueTask<Balance> GetBalanceAsync(CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync("partner/balance", ct);
        await H1StarsPayClient.HandleResponseErrorsAsync(response, ct);

        try
        {
            Balance? balance = await response.Content.ReadFromJsonAsync<Balance>(ct);
            if (!balance.HasValue)
            {
                throw new InvalidOperationException("API response body is null.");
            }
            return balance.Value;
        }
        catch (JsonException jsonEx)
        {
            var rawBody = await response.Content.ReadAsStringAsync(ct);
            throw new InvalidOperationException($"Failed to deserialize JSON response. Raw response body: {rawBody}", jsonEx);
        }
    }

    /// <summary>
    /// Создает запрос на вывод средств.
    /// </summary>
    public async ValueTask WithdrawAsync(WithdrawRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("partner/withdraw", request, ct);
        await H1StarsPayClient.HandleResponseErrorsAsync(response, ct);
    }

    /// <summary>
    /// Получает список транзакций с личного кабинета с возможностью фильтрации.
    /// </summary>
    /// <param name="limit">Количество записей (по умолчанию: 50).</param>
    /// <param name="offset">Смещение для пагинации.</param>
    /// <param name="dateFrom">Начальная дата (YYYY-MM-DD).</param>
    /// <param name="dateTo">Конечная дата (YYYY-MM-DD).</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>История транзакций.</returns>
    public async ValueTask<IReadOnlyList<Transaction>> GetTransactionsAsync(
        int limit = 50,
        int offset = 0,
        DateOnly? dateFrom = null,
        DateOnly? dateTo = null,
        CancellationToken ct = default)
    {
        var query = new StringBuilder("partner/transactions?");
        query.Append($"limit={limit}&offset={offset}");

        if (dateFrom.HasValue)
        {
            query.Append($"&date_from={dateFrom.Value:yyyy-MM-dd}");
        }
        if (dateTo.HasValue)
        {
            query.Append($"&date_to={dateTo.Value:yyyy-MM-dd}");
        }

        var response = await httpClient.GetAsync(query.ToString(), ct);
        await H1StarsPayClient.HandleResponseErrorsAsync(response, ct);

        var transactions = await response.Content.ReadFromJsonAsync<IReadOnlyList<Transaction>>(ct);
        return transactions ?? Array.Empty<Transaction>();
    }
}
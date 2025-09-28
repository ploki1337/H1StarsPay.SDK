using H1StarsPay.SDK.Clients;
using H1StarsPay.SDK.Exceptions;
using System.Net;
using System.Net.Http.Headers;

namespace H1StarsPay.SDK;

/// <summary>
/// Основной клиент для работы с pay.h1stars.ru/api.
/// </summary>
public class H1StarsPayClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly bool _isHttpClientOwned;

    /// <summary>
    /// Предоставляет доступ к платежам через API.
    /// </summary>
    public PaymentsClient Payments { get; }

    /// <summary>
    /// Предоставляет доступ к личному кабинету через API.
    /// </summary>
    public PartnerClient Partner { get; }

    /// <summary>
    /// Конструктор для использования с Dependency Injection (IHttpClientFactory).
    /// </summary>
    public H1StarsPayClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
        _isHttpClientOwned = false;
        Payments = new PaymentsClient(_httpClient);
        Partner = new PartnerClient(_httpClient);
    }

    /// <summary>
    /// Конструктор для ручного создания клиента без Dependency Injection.
    /// </summary>
    /// <param name="apiKey">Ваш API ключ.</param>
    public H1StarsPayClient(string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://pay.h1stars.ru/api/");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _isHttpClientOwned = true;
        Payments = new PaymentsClient(_httpClient);
        Partner = new PartnerClient(_httpClient);
    }

    /// <summary>
    /// Метод для обработки ошибок HTTP ответов.
    /// </summary>
    internal static async Task HandleResponseErrorsAsync(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var errorContent = await response.Content.ReadAsStringAsync(ct);
        var message = $"API request failed with status code {response.StatusCode}. Response: {errorContent}";

        throw response.StatusCode switch
        {
            HttpStatusCode.BadRequest => new H1StarsPayBadRequestException(message),
            HttpStatusCode.Unauthorized => new H1StarsPayUnauthorizedException(message),
            HttpStatusCode.Forbidden => new H1StarsPayForbiddenException(message),
            HttpStatusCode.NotFound => new H1StarsPayNotFoundException(message),
            HttpStatusCode.TooManyRequests => new H1StarsPayRateLimitException(message),
            HttpStatusCode.InternalServerError => new H1StarsPayServerErrorException(message),
            _ => new H1StarsPayException(message, response.StatusCode)
        };
    }

    public void Dispose()
    {
        if (_isHttpClientOwned)
        {
            _httpClient.Dispose();
        }
    }
}
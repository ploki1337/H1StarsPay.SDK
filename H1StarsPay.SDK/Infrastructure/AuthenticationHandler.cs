using System.Net.Http.Headers;

namespace H1StarsPay.SDK.Infrastructure;

/// <summary>
/// Обработчик для автоматического добавления API-токена ко всем запросам.
/// </summary>
internal class AuthenticationHandler(string apiKey) : DelegatingHandler
{
    private readonly AuthenticationHeaderValue _authHeader = new("Bearer", apiKey);

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = _authHeader;
        return base.SendAsync(request, cancellationToken);
    }
}
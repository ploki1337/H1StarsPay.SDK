using H1StarsPay.SDK.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace H1StarsPay.SDK;

/// <summary>
/// Класс-конфигуратор для регистрации SDK в DI-контейнере.
/// </summary>
public class H1StarsPayOptions
{
    /// <summary>
    /// Ваш API ключ для доступа к pay.h1stars.ru/api.
    /// </summary>
    public string? ApiKey { get; set; }
}

/// <summary>
/// Методы для удобной регистрации H1StarsPayClient в IServiceCollection.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует H1StarsPayClient и связанные службы в DI-контейнере.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configureOptions">Действие для конфигурации опций, включая ApiKey.</param>
    /// <returns>Та же коллекция сервисов для цепочки вызовов.</returns>
    public static IServiceCollection AddH1StarsPayClient(
        this IServiceCollection services,
        Action<H1StarsPayOptions> configureOptions)
    {
        var options = new H1StarsPayOptions();
        configureOptions(options);

        ArgumentException.ThrowIfNullOrWhiteSpace(options.ApiKey, nameof(options.ApiKey));

        services.AddTransient(_ => new AuthenticationHandler(options.ApiKey));

        services.AddHttpClient<H1StarsPayClient>(client =>
        {
            client.BaseAddress = new Uri("https://pay.h1stars.ru/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
        .AddHttpMessageHandler<AuthenticationHandler>();

        return services;
    }
}
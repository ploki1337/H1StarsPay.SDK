# pay.h1stars.ru .NET SDK

SDK для работы с API платежной системы pay.h1stars.ru. Библиотека предоставляет современный, асинхронный и типобезопасный способ интеграции с вашими .NET-приложениями.

## Возможности

- **Полная поддержка API платежей**: создание и получение статуса платежей.
- **Интеграция с партнерским API**: получение баланса, истории транзакций и вывод средств.
- **Безопасность**: встроенный верификатор подписей для Webhook-уведомлений.
- **Современная архитектура**: спроектирован для .NET 8+ с использованием `async/await`, `ValueTask` и `IHttpClientFactory`.
- **Простая интеграция**: легко подключается в ASP.NET Core и другие приложения с помощью Dependency Injection.
- **Надежная обработка ошибок**: предоставляет кастомные исключения для всех кодов ошибок API.

## Начало работы

### 1. Конфигурация (рекомендуемый способ)

Лучший способ использовать SDK — зарегистрировать его в DI-контейнере вашего приложения.

**a) Добавьте ваш API-ключ в `appsettings.json`:**

```json
{
  "H1StarsPay": {
    "ApiKey": "ВАШ_СЕКРЕТНЫЙ_КЛЮЧ_API_СЮДА"
  }
}
```

**b) Зарегистрируйте клиент в `Program.cs` (для ASP.NET Core или Generic Host):**

```csharp
using H1StarsPay.SDK;

var builder = WebApplication.CreateBuilder(args);

// ...

var apiKey = builder.Configuration["H1StarsPay:ApiKey"];

builder.Services.AddH1StarsPayClient(options =>
{
    options.ApiKey = apiKey;
});

// ...

var app = builder.Build();
```

### 2. Использование

После регистрации вы можете получить клиент через конструктор в ваших сервисах или контроллерах.

#### Создание платежа

```csharp
[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly H1StarsPayClient _h1StarsClient;

    public PaymentController(H1StarsPayClient h1StarsClient)
    {
        _h1StarsClient = h1StarsClient;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePayment()
    {
        var paymentRequest = new CreatePaymentRequest(
            Amount: 1000m,
            Description: "500 звезд",
            UserId: "5331086645",
            CallbackUrl: "https://site.com/webhooks/h1stars",
            ReturnUrl: "https://site.com/payment/success"
        );

        var createdPayment = await _h1StarsClient.Payments.CreateAsync(paymentRequest);
        
        return Redirect(createdPayment.PaymentUrl);
    }
}
```

#### Проверка статуса платежа

```csharp
[HttpGet("status/{paymentId}")]
public async Task<IActionResult> GetPaymentStatus(string paymentId)
{
    try
    {
        var paymentDetails = await _h1StarsClient.Payments.GetByIdAsync(paymentId);
        return Ok(new { status = paymentDetails.Status });
    }
    catch (H1StarsPayNotFoundException)
    {
        return NotFound("Платеж не найден.");
    }
}
```

#### Получение баланса партнера

```csharp
var balance = await _h1StarsClient.Partner.GetBalanceAsync();
Console.WriteLine($"Доступно для вывода: {balance.AvailableForWithdrawal}");
```

#### Получение истории транзакций

```csharp
var transactions = await _h1StarsClient.Partner.GetTransactionsAsync(
    limit: 10, 
    dateFrom: new DateOnly(2025, 6, 6)
);
```

### 3. Обработка Webhooks

API отправляет уведомления об изменении статуса платежа на ваш `CallbackUrl`. Крайне важно проверять их подлинность.

```csharp
// Пример ASP.NET Core Minimal API
app.MapPost("/webhooks/h1stars", async (HttpRequest request, IConfiguration config) => 
{
    var signature = request.Headers["X-Signature"].ToString();
    var webhookSecret = config["H1StarsPay:WebhookSecret"]; // храните в конфигурации

    using var reader = new StreamReader(request.Body);
    var payload = await reader.ReadToEndAsync();

    if (!WebhookVerifier.VerifySignature(payload, signature, webhookSecret))
    {
        return Results.Unauthorized();
    }

    // Подпись верна, обрабатываем уведомление
    var webhookPayload = JsonSerializer.Deserialize<WebhookPayload>(payload);
    
    // ...

    return Results.Ok();
});
```

### 4. Ручное создание клиента (без DI)

Если вы не используете Dependency Injection, вы можете создать экземпляр клиента напрямую.

```csharp
using H1StarsPay.SDK;

var apiKey = "ВАШ_СЕКРЕТНЫЙ_КЛЮЧ_API_СЮДА";
using var client = new H1StarsPayClient(apiKey);

var balance = await client.Partner.GetBalanceAsync();
Console.WriteLine($"Баланс: {balance.CurrentBalance}");
```

### Обработка ошибок

SDK выбрасывает кастомные исключения, унаследованные от `H1StarsPayException`, для всех стандартных ошибок API.

```csharp
try
{
    var details = await client.Payments.GetByIdAsync("invalid-id");
}
catch (H1StarsPayNotFoundException ex)
{
    // 404
    Console.WriteLine("Платеж не найден.");
}
catch (H1StarsPayUnauthorizedException ex)
{
    // 401
    Console.WriteLine("Ошибка авторизации. Проверьте ваш API ключ.");
}
catch (H1StarsPayException ex)
{
    // Остальные ошибки API
    Console.WriteLine($"Ошибка API: {ex.StatusCode}, {ex.Message}");
}
```

## Лицензия

Этот проект распространяется под лицензией MIT. Подробности см. в файле `LICENSE`.
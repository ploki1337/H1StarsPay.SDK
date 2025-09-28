using System.Net;

namespace H1StarsPay.SDK.Exceptions;

/// <summary>
/// Базовое исключение для всех ошибок, возвращаемых API.
/// </summary>
public class H1StarsPayException(string message, HttpStatusCode statusCode) : Exception(message)
{
    /// <summary>
    /// Cтатус-код, полученный от API.
    /// </summary>
    public HttpStatusCode StatusCode { get; } = statusCode;
}

/// <summary>
/// 400. Неверные параметры запроса. Проверьте передаваемые данные.
/// </summary>
public class H1StarsPayBadRequestException(string message) : H1StarsPayException(message, HttpStatusCode.BadRequest);

/// <summary>
/// 401. Неверный или отсутствующий API ключ. Проверьте заголовок Authorization.
/// </summary>
public class H1StarsPayUnauthorizedException(string message) : H1StarsPayException(message, HttpStatusCode.Unauthorized);

/// <summary>
/// 403. Доступ запрещен. Аккаунт заблокирован или не активен.
/// </summary>
public class H1StarsPayForbiddenException(string message) : H1StarsPayException(message, HttpStatusCode.Forbidden);

/// <summary>
/// 404. Платеж не найден. Проверьте правильность payment_id.
/// </summary>
public class H1StarsPayNotFoundException(string message) : H1StarsPayException(message, HttpStatusCode.NotFound);

/// <summary>
/// 429. Превышен лимит запросов. Уменьшите частоту запросов.
/// </summary>
public class H1StarsPayRateLimitException(string message) : H1StarsPayException(message, HttpStatusCode.TooManyRequests);

/// <summary>
/// 500. Внутренняя ошибка сервера. Обратитесь в поддержку.
/// </summary>
public class H1StarsPayServerErrorException(string message) : H1StarsPayException(message, HttpStatusCode.InternalServerError);
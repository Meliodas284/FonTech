using FonTech.Domain.Exceptions;
using FonTech.Domain.Result;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using ILogger = Serilog.ILogger;

namespace FonTech.Api.Middlewares;

/// <summary>
/// Middleware для обработки ошибок
/// </summary>
public class ExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    /// <summary>
    /// Конструктор для инициализации зависимостей
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

	/// <summary>
	/// Вызывает следующий middleware и обрабатывает исключения, если они возникают
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

	/// <summary>
	/// Обрабатывает исключение
	/// </summary>
	/// <param name="context"></param>
	/// <param name="exception"></param>
	/// <returns></returns>
	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.Error(exception, exception.Message);

        var response = new BaseResult()
        {
            ErrorMessage = exception.Message,
        };

        response.ErrorCode = exception switch
        {
            ReportsNotFoundException _ => (int)HttpStatusCode.NotFound,
            ReportNotFoundException _ => (int)HttpStatusCode.NotFound,
            ReportAlreadyExistsException _ => (int)HttpStatusCode.BadRequest,
            UserNotFoundException _ => (int)HttpStatusCode.NotFound,
            UserAlreadyExistsException _ => (int)HttpStatusCode.BadRequest,
            WrongPasswordException _ => (int)HttpStatusCode.Unauthorized,
            PasswordNotEqualsException _ => (int)HttpStatusCode.Unauthorized,
            InvalidClientRequestException _ => (int)HttpStatusCode.BadRequest,
            SecurityTokenException _ => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)response.ErrorCode;

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(response);
    }
}

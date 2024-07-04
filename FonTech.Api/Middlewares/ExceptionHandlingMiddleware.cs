using FonTech.Domain.Enum;
using FonTech.Domain.Result;
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
            _logger.Error(ex, ex.Message);

            var result = new BaseResult
            {
                ErrorCode = (int)ErrorCodes.InternalServerError,
                ErrorMessage = "An error occurred while processing your request."
			};

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}

using FonTech.Domain.Dto;
using FonTech.Domain.Result;

namespace FonTech.Domain.Interfaces.Services;

/// <summary>
/// Сервис, отвечающий за работу с доменной частью отчета (Report)
/// </summary>
public interface IReportService
{
	/// <summary>
	/// Получить все отчеты пользователя
	/// </summary>
	/// <param name="userId">ID пользователя</param>
	/// <returns></returns>
	Task<CollectionResult<ReportDto>> GetReportsAsync(long userId);
}

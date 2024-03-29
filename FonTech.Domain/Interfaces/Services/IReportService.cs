using FonTech.Domain.Dto.Report;
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

	/// <summary>
	/// Получить отчет по ID
	/// </summary>
	/// <param name="reportId">ID отчета</param>
	/// <returns></returns>
	Task<BaseResult<ReportDto>> GetReportByIdAsync(long reportId);

	/// <summary>
	/// Создать отчет
	/// </summary>
	/// <param name="dto"></param>
	/// <returns></returns>
	Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto dto);

	/// <summary>
	/// Удалить отчет по ID
	/// </summary>
	/// <param name="id">ID отчета</param>
	/// <returns></returns>
	Task<BaseResult<ReportDto>> DeleteReportAsync(long id);

	/// <summary>
	/// Обновить отчет
	/// </summary>
	/// <param name="dto"></param>
	/// <returns></returns>
	Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto dto);
}

using Asp.Versioning;
using FonTech.Domain.Dto.Report;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FonTech.Api.Controllers;

/// <summary>
/// Контроллер для модели Report
/// </summary>
//[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
	private readonly IReportService _reportService;

    /// <summary>
	/// Конструктор для инициализации зависимостей
	/// </summary>
	/// <param name="reportService"></param>
	public ReportController(IReportService reportService)
    {
        _reportService = reportService;
	}

    /// <summary>
	/// Получить отчет по идентификатору
	/// </summary>
	/// <param name="id">Идентификатор отчета</param>
	/// <remarks>
	/// Sample request:
	/// 
	///		GET
	///		{
	///			"id": 1
	///		}
	/// </remarks>
	[HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<ReportDto>>> GetReport(long id)
    {
        var response = await _reportService.GetReportByIdAsync(id);

        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
	}

    /// <summary>
	/// Получить все отчеты пользователя по его идентификатору
	/// </summary>
	/// <param name="userId">Идентификатор пользователя</param>
	/// <remarks>
	/// Sample request:
	/// 
	///		GET
	///		{
	///			"userId": 1
	///		}
	/// </remarks>
	[HttpGet("reports/{userId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<ReportDto>>> GetUserReports(long userId)
    {
        var response = await _reportService.GetReportsAsync(userId);

        if (response.IsSuccess)
            return Ok(response);

		return BadRequest(response);
	}

	/// <summary>
	/// Удалить отчет по идентификатору
	/// </summary>
	/// <param name="id">Идентификатор отчета</param>
	/// <remarks>
	/// Sample request:
	/// 
	///		DELETE
	///		{
	///			"id": 1
	///		}
	/// </remarks>
	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<ReportDto>>> DeleteReport(long id)
	{
		var response = await _reportService.DeleteReportAsync(id);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}

	/// <summary>
	/// Создать отчет
	/// </summary>
	/// <param name="dto"></param>
	/// <remarks>
	///	Sample request:
	///	
	///		POST
	///		{
	///			"name": "Report 1",
	///			"description": "Description 1",
	///			"user_id": 1
	///		}
	/// </remarks>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<ReportDto>>> CreateReport([FromBody] CreateReportDto dto)
	{
		var response = await _reportService.CreateReportAsync(dto);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}

	/// <summary>
	/// Обновить отчет
	/// </summary>
	/// <param name="dto"></param>
	/// <remarks>
	/// Sample request:
	/// 
	///		PUT
	///		{
	///			"id": 1
	///			"name": "Changed name",
	///			"description": "Changed description"
	///		}
	/// </remarks>
	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<ReportDto>>> UpdateReport([FromBody] UpdateReportDto dto)
	{
		var response = await _reportService.UpdateReportAsync(dto);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}
}

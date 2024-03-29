using FonTech.Domain.Dto.Report;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace FonTech.Api.Controllers;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
	private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
	}

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
}

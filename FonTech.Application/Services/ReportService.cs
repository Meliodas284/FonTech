using AutoMapper;
using FonTech.Application.Resources;
using FonTech.Domain.Dto.Report;
using FonTech.Domain.Entity;
using FonTech.Domain.Enum;
using FonTech.Domain.Interfaces.Repositories;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Interfaces.Validators;
using FonTech.Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace FonTech.Application.Services;

public class ReportService : IReportService
{
    private readonly IReportValidator _reportValidator;
	private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

	public ReportService(
        IReportValidator reportValidator,
		IUnitOfWork unitOfWork,
        IMapper mapper)
    {
		_reportValidator = reportValidator;
		_unitOfWork = unitOfWork;
        _mapper = mapper;
	}

	/// <inheritdoc />
	public async Task<CollectionResult<ReportDto>> GetReportsAsync(long userId)
	{
        Report[] reports;

		reports = await _unitOfWork.Reports.GetAll()
				.Where(x => x.UserId == userId)
				.ToArrayAsync();

		if (!reports.Any())
        {
			return new CollectionResult<ReportDto>
			{
				ErrorCode = (int)ErrorCodes.ReportsNotFound,
				ErrorMessage = ErrorMessage.ReportsNotFound
			};
		}

        return new CollectionResult<ReportDto>
        {
            Data = reports
				.Select(x => _mapper.Map<ReportDto>(x)),
            Count = reports.Length
        };
	}

	/// <inheritdoc />
	public async Task<BaseResult<ReportDto>> GetReportByIdAsync(long reportId)
	{
        Report? report;

		report = await _unitOfWork.Reports.GetAll()
				.FirstOrDefaultAsync(x => x.Id == reportId);

		var result = _reportValidator.ValidateOnNull(report);
		if (!result.IsSuccess)
		{
			return new BaseResult<ReportDto>
			{
				ErrorCode = result.ErrorCode,
				ErrorMessage = result.ErrorMessage
			};
		}

        return new BaseResult<ReportDto>
        {
            Data = _mapper.Map<ReportDto>(report)
		};
	}

	/// <inheritdoc/>
    public async Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto dto)
	{
		var user = await _unitOfWork.Users.GetAll()
				.FirstOrDefaultAsync(x => x.Id == dto.userId);

		var report = await _unitOfWork.Reports.GetAll()
			.FirstOrDefaultAsync(x => x.Name == dto.name);

		var result = _reportValidator.CreateValidator(report, user);
		if (!result.IsSuccess)
		{
			return new BaseResult<ReportDto>
			{
				ErrorCode = result.ErrorCode,
				ErrorMessage = result.ErrorMessage
			};
		}

		report = new Report
		{
			Name = dto.name,
			Description = dto.description,
			UserId = user!.Id
		};

		await _unitOfWork.Reports.CreateAsync(report);
		await _unitOfWork.SaveChangeAsync();

		return new BaseResult<ReportDto>
		{
			Data = _mapper.Map<ReportDto>(report)
		};
	}

	/// <inheritdoc />
    public async Task<BaseResult<ReportDto>> DeleteReportAsync(long id)
	{
		var report = await _unitOfWork.Reports
				.GetAll()
				.FirstOrDefaultAsync(x => x.Id == id);

		var result = _reportValidator.ValidateOnNull(report);
		if (!result.IsSuccess)
		{
			return new BaseResult<ReportDto>
			{
				ErrorCode = result.ErrorCode,
				ErrorMessage = result.ErrorMessage
			};
		}

		await _unitOfWork.Reports.DeleteAsync(report!);
		await _unitOfWork.SaveChangeAsync();

		return new BaseResult<ReportDto>
		{
			Data = _mapper.Map<ReportDto>(report)
		};
	}

	/// <inheritdoc />
	public async Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto dto)
	{
		var report = await _unitOfWork.Reports
				.GetAll()
				.FirstOrDefaultAsync(x => x.Id == dto.id);

		var result = _reportValidator.ValidateOnNull(report);
		if (!result.IsSuccess)
		{
			return new BaseResult<ReportDto>
			{
				ErrorCode = result.ErrorCode,
				ErrorMessage = result.ErrorMessage
			};
		}

		report!.Name = dto.name;
		report.Description = dto.description;

		await _unitOfWork.Reports.UpdateAsync(report);
		await _unitOfWork.SaveChangeAsync();

		return new BaseResult<ReportDto>
		{
			Data = _mapper.Map<ReportDto>(report)
		};
	}
}

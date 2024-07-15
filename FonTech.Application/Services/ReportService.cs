using AutoMapper;
using FluentValidation;
using FonTech.Application.Extensions;
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
	private readonly IBaseRepository<Report> _reportRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IReportValidator _reportValidator;
	private readonly IValidator<CreateReportDto> _createReportValidator;
	private readonly IValidator<UpdateReportDto> _updateReportValidator;
	private readonly IMapper _mapper;

	public ReportService(
        IBaseRepository<Report> reportRepository,
		IBaseRepository<User> userRepository,
        IReportValidator reportValidator,
		IValidator<CreateReportDto> createReportValidator,
		IValidator<UpdateReportDto> updateReportValidator,
		IMapper mapper)
    {
        _reportRepository = reportRepository;
        _userRepository = userRepository;
		_reportValidator = reportValidator;
		_createReportValidator = createReportValidator;
		_updateReportValidator = updateReportValidator;
        _mapper = mapper;
	}

	/// <inheritdoc />
	public async Task<CollectionResult<ReportDto>> GetReportsAsync(long userId)
	{
        Report[] reports;

		reports = await _reportRepository.GetAll()
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

		report = await _reportRepository.GetAll()
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
		var fluentValidation = _createReportValidator.Validate(dto);
		if (!fluentValidation.IsValid)
		{
			return new BaseResult<ReportDto>
			{
				ErrorCode = (int)ErrorCodes.CreateDataIsNotValid,
				ErrorMessage = fluentValidation.ToFormattedString()
			};
		}

		var user = await _userRepository.GetAll()
				.FirstOrDefaultAsync(x => x.Id == dto.userId);

		var report = await _reportRepository.GetAll()
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

		await _reportRepository.CreateAsync(report);

		return new BaseResult<ReportDto>
		{
			Data = _mapper.Map<ReportDto>(report)
		};
	}

	/// <inheritdoc />
    public async Task<BaseResult<ReportDto>> DeleteReportAsync(long id)
	{
		var report = await _reportRepository
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

		await _reportRepository.DeleteAsync(report!);

		return new BaseResult<ReportDto>
		{
			Data = _mapper.Map<ReportDto>(report)
		};
	}

	/// <inheritdoc />
	public async Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto dto)
	{
		var fluentValidation = _updateReportValidator.Validate(dto);
		if (!fluentValidation.IsValid)
		{
			return new BaseResult<ReportDto>
			{
				ErrorCode = (int)ErrorCodes.UpdateDataIsNotValid,
				ErrorMessage = fluentValidation.ToFormattedString()
			};
		}

		var report = await _reportRepository
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

		await _reportRepository.UpdateAsync(report);

		return new BaseResult<ReportDto>
		{
			Data = _mapper.Map<ReportDto>(report)
		};
	}
}

using AutoMapper;
using FonTech.Application.Resources;
using FonTech.Domain.Dto.Report;
using FonTech.Domain.Entity;
using FonTech.Domain.Exceptions;
using FonTech.Domain.Interfaces.Repositories;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Interfaces.Validators;
using FonTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FonTech.Application.Services;

public class ReportService : IReportService
{
	private readonly IBaseRepository<Report> _reportRepository;
    private readonly IBaseRepository<User> _userRepository;
	private readonly ILogger _logger;
    private readonly IReportValidator _reportValidator;
    private readonly IMapper _mapper;

	public ReportService(
        IBaseRepository<Report> reportRepository,
		IBaseRepository<User> userRepository,
		ILogger logger,
        IReportValidator reportValidator,
        IMapper mapper)
    {
        _reportRepository = reportRepository;
        _userRepository = userRepository;
		_logger = logger;
		_reportValidator = reportValidator;
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
			throw new ReportsNotFoundException(ErrorMessage.ReportsNotFound);
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
			throw new ReportNotFoundException(result.ErrorMessage);
		}

        return new BaseResult<ReportDto>
        {
            Data = _mapper.Map<ReportDto>(report)
		};
	}

	/// <inheritdoc/>
    public async Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto dto)
	{
		var user = await _userRepository.GetAll()
				.FirstOrDefaultAsync(x => x.Id == dto.userId);

		if (user == null)
		{
			throw new UserNotFoundException(ErrorMessage.UserNotFound);
		}

		var report = await _reportRepository.GetAll()
			.FirstOrDefaultAsync(x => x.Name == dto.name);

		if (report != null)
		{
			throw new ReportAlreadyExistsException(ErrorMessage.ReportAlreadyExist);
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
			throw new ReportNotFoundException(result.ErrorMessage);
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
		var report = await _reportRepository
				.GetAll()
				.FirstOrDefaultAsync(x => x.Id == dto.id);

		var result = _reportValidator.ValidateOnNull(report);
		if (!result.IsSuccess)
		{
			throw new ReportNotFoundException(result.ErrorMessage);
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

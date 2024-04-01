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
        ReportDto[] reports;

        try
        {
            reports = await _reportRepository.GetAll()
                .Where(x => x.UserId == userId)
                .Select(x => new ReportDto(x.Id, x.Name, x.Description, x.CreatedAt.ToLongDateString()))
				.ToArrayAsync();

		}
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);

            return new CollectionResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
		}

        if (!reports.Any())
        {
            _logger.Warning(ErrorMessage.ReportsNotFound, reports.Length);
			return new CollectionResult<ReportDto>()
			{
				ErrorMessage = ErrorMessage.ReportsNotFound,
				ErrorCode = (int)ErrorCodes.ReportsNotFound
			};
		}

        return new CollectionResult<ReportDto>
        {
            Data = reports,
            Count = reports.Length
        };
	}

	/// <inheritdoc />
	public async Task<BaseResult<ReportDto>> GetReportByIdAsync(long reportId)
	{
        Report? report;

        try
        {
            report = await _reportRepository.GetAll()
				.FirstOrDefaultAsync(x => x.Id == reportId);

		}
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            
            return new BaseResult<ReportDto>
            {
                ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}

		var result = _reportValidator.ValidateOnNull(report);
		if (!result.IsSuccess)
		{
			_logger.Warning("Отчет с ID = {reportId} не найден", reportId);

			return new BaseResult<ReportDto>
			{
				ErrorMessage = result.ErrorMessage,
				ErrorCode = result.ErrorCode
			};
		}


        return new BaseResult<ReportDto>
        {
            Data = new ReportDto(
				report!.Id,
				report.Name, 
				report.Description, 
				report.CreatedAt.ToLongDateString())
		};
	}

	/// <inheritdoc/>
    public async Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto dto)
	{
		try
        {
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == dto.userId);

            var report = await _reportRepository.GetAll()
				.FirstOrDefaultAsync(x => x.Name == dto.name);

            var result = _reportValidator.CreateValidator(report, user);
            if (!result.IsSuccess)
            {
                return new BaseResult<ReportDto>
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode
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
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);

			return new BaseResult<ReportDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	/// <inheritdoc />
    public async Task<BaseResult<ReportDto>> DeleteReportAsync(long id)
	{
        try
        {
            var report = await _reportRepository
                .GetAll()
				.FirstOrDefaultAsync(x => x.Id == id);

            var result = _reportValidator.ValidateOnNull(report);
			if (!result.IsSuccess)
			{
				return new BaseResult<ReportDto>
				{
					ErrorMessage = result.ErrorMessage,
					ErrorCode = result.ErrorCode
				};
			}

			await _reportRepository.DeleteAsync(report!);

            return new BaseResult<ReportDto>
            {
                Data = _mapper.Map<ReportDto>(report)
            };
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);

			return new BaseResult<ReportDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	/// <inheritdoc />
	public async Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto dto)
	{
        try
        {
			var report = await _reportRepository
				.GetAll()
				.FirstOrDefaultAsync(x => x.Id == dto.id);

			var result = _reportValidator.ValidateOnNull(report);
			if (!result.IsSuccess)
			{
				return new BaseResult<ReportDto>
				{
					ErrorMessage = result.ErrorMessage,
					ErrorCode = result.ErrorCode
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
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);

			return new BaseResult<ReportDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}
}

using FonTech.Application.Resources;
using FonTech.Domain.Entity;
using FonTech.Domain.Enum;
using FonTech.Domain.Interfaces.Validators;
using FonTech.Domain.Result;

namespace FonTech.Application.Validations;

public class ReportValidator : IReportValidator
{
	public BaseResult ValidateOnNull(Report model)
	{
		if (model == null)
		{
			return new BaseResult
			{
				ErrorMessage = ErrorMessage.ReportNotFound,
				ErrorCode = (int)ErrorCodes.ReportNotFound
			};
		}

		return new BaseResult();
	}

	/// <inheritdoc />
	public BaseResult CreateValidator(Report report, User user)
	{
		if (report != null)
		{
			return new BaseResult
			{
				ErrorMessage = ErrorMessage.ReportAlreadyExist,
				ErrorCode = (int)ErrorCodes.ReportAlreadyExist
			};
		}

		if (user == null)
		{
			return new BaseResult
			{
				ErrorMessage = ErrorMessage.UserNotFound,
				ErrorCode = (int)ErrorCodes.UserNotFound
			};
		}

		return new BaseResult();
	}
}

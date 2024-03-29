using FluentValidation;
using FonTech.Domain.Dto.Report;

namespace FonTech.Application.Validations.FluentValidations.Report;

public class UpdateReportValidator : AbstractValidator<UpdateReportDto>
{
    public UpdateReportValidator()
    {
        RuleFor(x => x.id).NotEmpty();
		RuleFor(x => x.name).NotEmpty().MaximumLength(200);
		RuleFor(x => x.description).NotEmpty().MaximumLength(1000);
	}
}

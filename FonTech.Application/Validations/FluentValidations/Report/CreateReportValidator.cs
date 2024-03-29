using FluentValidation;
using FonTech.Domain.Dto.Report;

namespace FonTech.Application.Validations.FluentValidations.Report;

public class CreateReportValidator : AbstractValidator<CreateReportDto>
{
    public CreateReportValidator()
    {
        RuleFor(x => x.name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.description).NotEmpty().MaximumLength(1000);
    }
}

using FluentValidation;
using FonTech.Domain.Dto.User;

namespace FonTech.Application.Validations.FluentValidations.User;

public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserValidator()
    {
		RuleFor(x => x.Login)
			.NotEmpty().WithMessage("Login is required.")
			.Length(4, 25).WithMessage("Login must be between 4 and 25 characters.")
			.Matches("^[a-zA-Z0-9]+$").WithMessage("Login can only contain letters and numbers.");

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Password is required.")
			.MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
			.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
			.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
			.Matches("[0-9]").WithMessage("Password must contain at least one number.")
			.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

		RuleFor(x => x.PasswordConfirm)
			.NotEmpty().WithMessage("Password confirmation is required.")
			.Equal(x => x.Password).WithMessage("Passwords do not match.");
	}
}

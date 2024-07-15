using FluentValidation.Results;

namespace FonTech.Application.Extensions;

public static class ValidationExtensions
{
	public static string ToFormattedString(this ValidationResult validationResult)
	{
		return string.Join(Environment.NewLine, validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
	}
}

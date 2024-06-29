namespace FonTech.Domain.Exceptions;

public class PasswordNotEqualsException : Exception
{
	public PasswordNotEqualsException() { }

	public PasswordNotEqualsException(string message) : base(message) { }
}

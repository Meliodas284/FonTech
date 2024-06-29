namespace FonTech.Domain.Exceptions;

public class InvalidClientRequestException : Exception
{
	public InvalidClientRequestException() { }

	public InvalidClientRequestException(string message) : base(message) { }
}

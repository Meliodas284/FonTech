namespace FonTech.Domain.Exceptions;

public class ReportAlreadyExistsException : Exception
{
	public ReportAlreadyExistsException() { }

	public ReportAlreadyExistsException(string message) : base(message) { }
}

namespace FonTech.Domain.Exceptions;

public class ReportsNotFoundException : Exception
{
    public ReportsNotFoundException() { }

    public ReportsNotFoundException(string message) : base(message) { }
}

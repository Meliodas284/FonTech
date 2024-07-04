namespace FonTech.Domain.Exceptions;

public class RolesNotFoundException : Exception
{
	public RolesNotFoundException() { }

	public RolesNotFoundException(string message) : base(message) { }
}

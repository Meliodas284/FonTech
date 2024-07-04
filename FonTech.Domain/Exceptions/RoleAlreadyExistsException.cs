namespace FonTech.Domain.Exceptions;

public class RoleAlreadyExistsException : Exception
{
	public RoleAlreadyExistsException() { }

	public RoleAlreadyExistsException(string message) : base(message) { }
}

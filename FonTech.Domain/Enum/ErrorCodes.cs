namespace FonTech.Domain.Enum;

public enum ErrorCodes
{
	ReportsNotFound = 0,
	ReportNotFound = 1,
	ReportAlreadyExists = 2,

	UserNotFound = 11,
	UserAlreadyExists = 12,


	PasswordNotEquals = 21,
	WrongPassword = 22,
	InvalidClientRequest = 23,


	InternalServerError = 10
}
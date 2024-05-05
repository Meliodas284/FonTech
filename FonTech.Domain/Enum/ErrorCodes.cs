namespace FonTech.Domain.Enum;

public enum ErrorCodes
{
	ReportsNotFound = 0,
	ReportNotFound = 1,
	ReportAlreadyExist = 2,

	UserNotFound = 11,
	UserAlreadyExists = 12,


	PasswordNotEquals = 21,
	WrongPassword = 22,


	InternalServerError = 10
}
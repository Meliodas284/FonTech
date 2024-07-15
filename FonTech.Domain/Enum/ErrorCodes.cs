namespace FonTech.Domain.Enum;

public enum ErrorCodes
{
	ReportsNotFound = 0,
	ReportNotFound = 1,
	ReportAlreadyExists = 2,
	CreateDataIsNotValid = 3,
	UpdateDataIsNotValid = 4,

	UserNotFound = 11,
	UserAlreadyExists = 12,
	UserAlreadyHasThisRole = 13,
	RegisterDataIsNotValid = 14,
	LoginDataIsNotValid = 15,

	WrongPassword = 21,
	InvalidClientRequest = 22,

	RolesNotFound = 31,
	RoleNotFound = 32,
	RoleAlreadyExists = 33,

	InternalServerError = 10
}
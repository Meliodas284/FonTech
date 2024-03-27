using FonTech.Domain.Entity;
using FonTech.Domain.Result;

namespace FonTech.Domain.Interfaces.Validators;

public interface IReportValidator : IBaseValidator<Report>
{
	/// <summary>
	/// Если в базе уже есть отчет с таким именем - вернуть ошибку
	/// Если в базе нет пользователя с таким id - вернуть ошибку
	/// </summary>
	/// <param name="report"></param>
	/// <param name="user"></param>
	/// <returns></returns>
	BaseResult CreateValidator(Report report, User user);
}

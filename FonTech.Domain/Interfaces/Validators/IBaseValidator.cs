using FonTech.Domain.Result;

namespace FonTech.Domain.Interfaces.Validators;

public interface IBaseValidator<T> where T : class
{
	BaseResult ValidateOnNull(T model);
}

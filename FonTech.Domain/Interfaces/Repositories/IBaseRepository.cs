namespace FonTech.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity>
{
	IQueryable<TEntity> GetAll();

	Task<TEntity> CreateAsync(TEntity entity);

	Task UpdateAsync(TEntity entity);

	Task DeleteAsync(TEntity entity);
}

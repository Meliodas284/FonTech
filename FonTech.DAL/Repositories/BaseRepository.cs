using FonTech.Domain.Interfaces.Repositories;

namespace FonTech.DAL.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
	private readonly ApplicationDbContext _dbContext;

	public BaseRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<TEntity> CreateAsync(TEntity entity)
	{
		if (entity == null)
			throw new ArgumentException("Entity is null");

		await _dbContext.AddAsync(entity);

		return entity;
	}

	public Task DeleteAsync(TEntity entity)
	{
		if (entity == null)
			throw new ArgumentException("Entity is null");

		_dbContext.Remove(entity);

		return Task.CompletedTask;
	}

	public IQueryable<TEntity> GetAll()
	{
		return _dbContext.Set<TEntity>();
	}

	public Task UpdateAsync(TEntity entity)
	{
		if (entity == null)
			throw new ArgumentException("Entity is null");

		_dbContext.Update(entity);

		return Task.CompletedTask;
	}
}

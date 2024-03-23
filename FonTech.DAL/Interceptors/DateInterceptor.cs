using FonTech.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FonTech.DAL.Interceptors;

public class DateInterceptor : SaveChangesInterceptor
{
	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		var dbContext = eventData.Context;
		if (dbContext is null)
		{
			return base.SavingChanges(eventData, result);
		}

		var entries = dbContext.ChangeTracker.Entries<IAuditable>();
		foreach (var entry in entries)
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedAt = DateTime.UtcNow;
					break;
				case EntityState.Modified:
					entry.Entity.UpdatedAt = DateTime.UtcNow;
					break;
			}
		}

		return base.SavingChanges(eventData, result);
	}
}

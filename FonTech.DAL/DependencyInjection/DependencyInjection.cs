﻿using FonTech.DAL.Interceptors;
using FonTech.DAL.Repositories;
using FonTech.Domain.Entity;
using FonTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FonTech.DAL.DependencyInjection;

public static class DependencyInjection
{
	public static void AddDataAccessLayer(
		this IServiceCollection services, 
		IConfiguration configuration)
	{
		services.AddSingleton<DateInterceptor>();

		var connectionString = configuration.GetConnectionString("MSSQL");

		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseSqlServer(connectionString);
		});

		services.InitRepositories();
	}

	private static void InitRepositories(this IServiceCollection services)
	{
		services.AddScoped<IBaseRepository<Report>, BaseRepository<Report>>();
		services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
	}
}

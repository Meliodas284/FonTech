using Asp.Versioning;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace FonTech.Api;

public static class Startup
{
	/// <summary>
	/// Подключение Swagger
	/// </summary>
	/// <param name="services"></param>
	public static void AddSwagger(this IServiceCollection services)
	{
		services.AddApiVersioning()
			.AddApiExplorer(options =>
			{
				options.DefaultApiVersion = new ApiVersion(1, 0);
				options.GroupNameFormat = "'v'VVV";
				options.SubstituteApiVersionInUrl = true;
				options.AssumeDefaultVersionWhenUnspecified = true;
			});

		services.AddEndpointsApiExplorer();
		
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo()
			{
				Version = "v1",
				Title = "FonTech.Api",
				Description = "FonTech.Api version 1",
				TermsOfService = new Uri("https://example.com/terms"),
				Contact = new OpenApiContact()
				{
					Name = "Andy",
					Url = new Uri("https://web.telegram.org/k/")
				}
			});

			options.SwaggerDoc("v2", new OpenApiInfo()
			{
				Version = "v2",
				Title = "FonTech.Api",
				Description = "FonTech.Api version 2",
				TermsOfService = new Uri("https://example.com/terms"),
				Contact = new OpenApiContact()
				{
					Name = "Andy",
					Url = new Uri("https://web.telegram.org/k/")
				}
			});

			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
			{
				In = ParameterLocation.Header,
				Description = "Please enter token",
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				BearerFormat = "JWT",
				Scheme = "Bearer"
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement()
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new List<string>()
				}
			});

			var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
		});
	}
}

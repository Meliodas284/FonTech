using Asp.Versioning;
using FonTech.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace FonTech.Api;

public static class Startup
{
	/// <summary>
	/// Подключение аутентификации и авторизации
	/// </summary>
	/// <param name="services"></param>
	/// <param name="builder"></param>
	public static void AddAuthenticationAndAuthorization(this IServiceCollection services, WebApplicationBuilder builder)
	{		
		services.AddAuthorization();
		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options =>
		{
			var jwtSettings = builder.Configuration.GetSection(JwtSettings.DefaultJwtSection).Get<JwtSettings>();
			var issuer = jwtSettings.Issuer;
			var audience = jwtSettings.Audience;
			var key = jwtSettings.JwtKey;

			options.Authority = jwtSettings.Authority;
			options.RequireHttpsMetadata = false;
			options.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidIssuer = issuer,
				ValidAudience = audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true
			};
		});
	}
	
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

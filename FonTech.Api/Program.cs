using FonTech.DAL.DependencyInjection;
using FonTech.Application.DependencyInjection;
using Serilog;
using FonTech.Api;
using FonTech.Domain.Settings;
using FonTech.Api.Middlewares;
using FonTech.Producer.DependencyInjection;
using FonTech.Consumer.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultJwtSection));
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(nameof(RabbitMqSettings)));

builder.Host.UseSerilog((context, configuration) =>
{
	configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddControllers();

builder.Services.AddAuthenticationAndAuthorization(builder);
builder.Services.AddSwagger();

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddProducer();
builder.Services.AddConsumer();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "FonTech Swagger v 1.0");
		c.SwaggerEndpoint("/swagger/v2/swagger.json", "FonTech Swagger v 2.0");
	});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using FonTech.Producer.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FonTech.Producer.DependencyInjection;

public static class DependencyInjection
{
	public static void AddConsumer(this IServiceCollection services)
	{
		services.AddScoped<IMessageProducer, Producer>();
	}
}

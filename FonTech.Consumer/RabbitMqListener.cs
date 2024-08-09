using FonTech.Domain.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace FonTech.Consumer;

public class RabbitMqListener : BackgroundService
{
	private readonly IConnection _connection;
	private readonly IModel _channel;
	private readonly RabbitMqSettings _options;

    public RabbitMqListener(IOptions<RabbitMqSettings> options)
    {
		_options = options.Value;
		var factory = new ConnectionFactory() { HostName = "localhost" };
		_connection = factory.CreateConnection();
		_channel = _connection.CreateModel();
		_channel.QueueDeclare(_options.QueueName, durable: true, exclusive: true, 
			autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		stoppingToken.ThrowIfCancellationRequested();


		
		throw new NotImplementedException();
	}
}

using FonTech.Domain.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FonTech.Consumer;

public class RabbitMqListener : BackgroundService
{
	private readonly ILogger<RabbitMqListener> _logger;
	private readonly IConnection _connection;
	private readonly IModel _channel;
	private readonly RabbitMqSettings _options;

	public RabbitMqListener(IOptions<RabbitMqSettings> options, ILogger<RabbitMqListener> logger)
	{
		_options = options.Value;
		var factory = new ConnectionFactory() { HostName = "localhost" };
		_connection = factory.CreateConnection();
		_channel = _connection.CreateModel();
		_channel.QueueDeclare(_options.QueueName, durable: true, exclusive: true,
			autoDelete: false, arguments: null);
		_logger = logger;
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		stoppingToken.ThrowIfCancellationRequested();

		var consumer = new EventingBasicConsumer(_channel);
		consumer.Received += Consumer_Received;

		_channel.BasicConsume(_options.QueueName, false, consumer);

		return Task.CompletedTask;
	}

	private void Consumer_Received(object? sender, BasicDeliverEventArgs e)
	{
		var content = Encoding.UTF8.GetString(e.Body.ToArray());

		_logger.LogInformation($"Сообщение получено: {content}");

		_channel.BasicAck(e.DeliveryTag, false);
	}

	public override void Dispose()
	{
		_channel.Close();
		_connection.Close();
		base.Dispose();
	}
}

namespace FonTech.Domain.Settings;

public class RabbitMqSetting
{
	public string QueueName { get; set; }

	public string ExchangeName { get; set; }

    public string RoutingKey { get; set; }
}

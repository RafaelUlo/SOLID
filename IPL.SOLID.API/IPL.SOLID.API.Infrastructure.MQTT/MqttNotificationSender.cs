using IPL.SOLID.API.Application.Notifications;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace IPL.SOLID.API.Infrastructure.MQTT;

public sealed class MqttNotificationSender : IMqttNotificationSender
{
    public async Task SendAsync(string endpoint, string topic, string payload, CancellationToken token)
    {
        if (!NotificationEndpoint.IsValid(endpoint))
        {
            throw new ArgumentException("Invalid MQTT broker endpoint.", nameof(endpoint));
        }

        var broker = new Uri(endpoint);

        using var client = new MqttFactory().CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(broker.Host, broker.Port > 0 ? broker.Port : broker.Scheme == "mqtts" ? 8883 : 1883)
            .WithClientId("solid-" + Guid.NewGuid().ToString("N"))
            .WithCleanSession();

        if (broker.Scheme == "mqtts")
        {
            options.WithTlsOptions(tls => tls.UseTls());
        }

        if (!string.IsNullOrEmpty(broker.UserInfo))
        {
            var credentials = broker.UserInfo.Split(':', 2);

            options.WithCredentials(Uri.UnescapeDataString(credentials[0]),
                credentials.Length == 2 ? Uri.UnescapeDataString(credentials[1]) : string.Empty);
        }

        await client.ConnectAsync(options.Build(), token);

        var result = await client.PublishAsync(new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .WithRetainFlag(false)
            .Build(), token);

        if ((int)result.ReasonCode >= 128)
        {
            throw new InvalidOperationException($"MQTT publish was rejected: {result.ReasonCode}.");
        }
    }
}

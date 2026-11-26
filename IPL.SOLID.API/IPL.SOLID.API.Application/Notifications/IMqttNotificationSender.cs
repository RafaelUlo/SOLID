namespace IPL.SOLID.API.Application.Notifications;

public interface IMqttNotificationSender
{
    Task SendAsync(string endpoint, string topic, string payload, CancellationToken token);
}

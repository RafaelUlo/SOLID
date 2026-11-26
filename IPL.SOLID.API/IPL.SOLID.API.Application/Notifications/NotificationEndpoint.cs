namespace IPL.SOLID.API.Application.Notifications;

public static class NotificationEndpoint
{
    public static bool IsValid(string? value)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) || string.IsNullOrWhiteSpace(uri.Host))
        {
            return false;
        }

        return uri.Scheme switch
        {
            "http" or "https" => string.IsNullOrEmpty(uri.Fragment) && string.IsNullOrEmpty(uri.UserInfo),
            "mqtt" or "mqtts" => (uri.AbsolutePath is "" or "/") &&
                string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(uri.Fragment),
            _ => false
        };
    }
}

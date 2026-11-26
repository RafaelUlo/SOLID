namespace IPL.SOLID.API.Application.ErrorMessages;

public static class ApplicationErrorMessage
{
    public const string APPLICATION_NOT_FOUND_ERROR_MESSAGE = "The application with name {0} was not found";
    public const string CHANNEL_WITH_SAME_NAME_ERROR_MESSAGE =
    """
        The channel name provided {0} is already in use in this application {1}
    """;
    public const string CHANNEL_NOT_FOUND_ERROR_MESSAGE = "The channel with name {0} was not found for the application {1}";
}

using IPL.SOLID.SensorSimulator.Options;
using IPL.SOLID.SensorSimulator.SolidMiddlewareContracts;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace IPL.SOLID.SensorSimulator;

public sealed class SolidMiddlewareHttpClient
{
    private readonly HttpClient _httpClient;

    private readonly IOptions<SolidMiddlewareOptions> _options;
    public SolidMiddlewareHttpClient(HttpClient httpClient, IOptions<SolidMiddlewareOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<HttpStatusCode> CreateApplicationAsync(string applicationName, string description)
    {
        var createApplicationCommand = new CreateApplicationCommand
        {
            ResourceType = "application",
            ResourceName = applicationName,
            Description = description
        };

        // We are assuming here that the base API path is the creation of the applications 
        // and that the API follow a restful approach, which cannot be true
        var response = await _httpClient.PostAsJsonAsync(_options.Value.ApiPath, createApplicationCommand);

        response.EnsureSuccessStatusCode();

        return response.StatusCode;
    }

    public async Task<List<string>?> DiscoveryApplicationEndpoints(string applicationName)
    {
        var response = await _httpClient.GetFromJsonAsync<List<string>>($"{_options.Value.ApiPath}\\{applicationName}");

        return response;
    }

    public async Task<HttpStatusCode> CreateChannelAsync(string channelName, string description)
    {
        var createApplicationCommand = new CreateChannelCommand
        {
            ResourceType = "channel",
            ResourceName = channelName,
            Description = description
        };

        // We are assuming here that the base API path is the creation of the applications 
        // and that the API follow a restful approach, which cannot be true
        var response = await _httpClient.PostAsJsonAsync($"{_options.Value.ApiPath}\\{channelName}", createApplicationCommand);

        response.EnsureSuccessStatusCode();

        return response.StatusCode;
    }
}

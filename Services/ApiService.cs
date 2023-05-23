using System.Text;
using System.Text.Json;
using RageshakeWebhookHandler.Dtos;

namespace RageshakeWebhookHandler.Services;

public class ApiService : IApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ApiService> _logger;

    public ApiService(ILogger<ApiService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task Notify(EnvsDto envsDto, string message)
    {
        var client = _httpClientFactory.CreateClient("api");

        var matrixNotifierBody = new MatrixNotifier(envsDto.MatrixHomeserverUrl, envsDto.MatrixHomeserverUser,
            envsDto.MatrixHomeserverPasswd, envsDto.MatrixHomeserverRoom, envsDto.MatrixHomeserverDeviceId, message);

        var jsonString = JsonSerializer.Serialize(matrixNotifierBody);

        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        await client.PostAsync(envsDto.MatrixNotifierUrl, content);
    }
}
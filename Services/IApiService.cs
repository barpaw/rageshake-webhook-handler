using RageshakeWebhookHandler.Dtos;

namespace RageshakeWebhookHandler.Services;

public interface IApiService
{
    Task Notify(EnvsDto envsDto, string message);
}
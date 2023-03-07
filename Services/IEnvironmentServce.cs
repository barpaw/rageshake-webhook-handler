using RageshakeWebhookHandler.Dtos;

namespace RageshakeWebhookHandler.Services;

public interface IEnvironmentService
{
    EnvsDto GetEnvDto();
}
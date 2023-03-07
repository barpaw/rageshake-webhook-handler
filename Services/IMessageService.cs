using RageshakeWebhookHandler.Dtos;

namespace RageshakeWebhookHandler.Services;

public interface IMessageService
{
    string PrepareMessage(RageshakeWebhook rageshakeWebhook, EnvsDto envsDto);
}
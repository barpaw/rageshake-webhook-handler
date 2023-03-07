using MediatR;

namespace RageshakeWebhookHandler.Commands;

public class HandleRageshakeWebhookCommand : IRequest<string>
{
    public HandleRageshakeWebhookCommand(RageshakeWebhook rageshakeWebhook)
    {
        RageshakeWebhook = rageshakeWebhook;
    }

    public RageshakeWebhook RageshakeWebhook { get; set; }
}
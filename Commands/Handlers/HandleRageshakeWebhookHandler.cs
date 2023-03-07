using MediatR;
using RageshakeWebhookHandler.Services;

namespace RageshakeWebhookHandler.Commands.Handlers;

public class HandleRageshakeWebhookHandler : IRequestHandler<HandleRageshakeWebhookCommand, string>
{
    private readonly IApiService _apiService;
    private readonly IEnvironmentService _environmentService;
    private readonly ILogger<HandleRageshakeWebhookHandler> _logger;
    private readonly IMessageService _messageService;

    public HandleRageshakeWebhookHandler(ILogger<HandleRageshakeWebhookHandler> logger,
        IEnvironmentService environmentService, IMessageService messageService, IApiService apiService)
    {
        _logger = logger;
        _environmentService = environmentService;
        _messageService = messageService;
        _apiService = apiService;
    }

    public async Task<string> Handle(HandleRageshakeWebhookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var envsDto = _environmentService.GetEnvDto();
            var message = _messageService.PrepareMessage(request.RageshakeWebhook, envsDto);
            await _apiService.Notify(envsDto, message);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogError(e.StackTrace);
            return "Error";
        }

        return "Ok";
    }
}
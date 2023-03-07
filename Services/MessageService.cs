using System.Text;
using RageshakeWebhookHandler.Dtos;

namespace RageshakeWebhookHandler.Services;

public class MessageService : IMessageService
{
    private readonly ILogger<MessageService> _logger;

    public MessageService(ILogger<MessageService> logger)
    {
        _logger = logger;
    }

    public string PrepareMessage(RageshakeWebhook rageshakeWebhook, EnvsDto envsDto)
    {
        var sb = new StringBuilder();

        //RAGESHAKE WEBHOOK PART I
        sb.Append(envsDto.MatrixNotifierMessageHeader);
        sb.Append(Environment.NewLine);
        sb.Append($"ℹ️ {rageshakeWebhook?.App} {rageshakeWebhook?.Data?.Version}");
        sb.Append(Environment.NewLine);
        sb.Append($"🪪 {rageshakeWebhook?.Data?.UserId}");
        sb.Append(Environment.NewLine);
        sb.Append($"📱 {rageshakeWebhook?.Data?.Device} {rageshakeWebhook?.Data?.Os}");

        sb.Append(Environment.NewLine);
        sb.Append(Environment.NewLine);
        sb.Append(rageshakeWebhook?.UserText);
        sb.Append(Environment.NewLine);
        sb.Append(Environment.NewLine);
        sb.Append("- - -");
        sb.Append(Environment.NewLine);
        sb.Append($"app: {rageshakeWebhook?.App}");
        sb.Append(Environment.NewLine);

        //DATA
        sb.Append($"User-Agent: {rageshakeWebhook?.Data?.UserAgent}");
        sb.Append(Environment.NewLine);
        sb.Append($"Version: {rageshakeWebhook?.Data?.Version}");
        sb.Append(Environment.NewLine);
        sb.Append($"app_language: {rageshakeWebhook?.Data?.AppLanguage}");

        if (rageshakeWebhook is not null && rageshakeWebhook.Data is not null &&
            rageshakeWebhook.Data.AutoUisi.EventId.Length > 0)
        {
            sb.Append(Environment.NewLine);
            sb.Append($"uisi_session_id: {rageshakeWebhook?.Data?.AutoUisi?.SessionId}");

            sb.Append(Environment.NewLine);
            sb.Append($"uisi_sender_key: {rageshakeWebhook?.Data?.AutoUisi?.SenderKey}");

            sb.Append(Environment.NewLine);
            sb.Append($"uisi_device_id: {rageshakeWebhook?.Data?.AutoUisi?.DeviceId}");

            sb.Append(Environment.NewLine);
            sb.Append($"uisi_user_id: {rageshakeWebhook?.Data?.AutoUisi?.UserId}");

            sb.Append(Environment.NewLine);
            sb.Append($"uisi_event_id: {rageshakeWebhook?.Data?.AutoUisi?.EventId}");

            sb.Append(Environment.NewLine);
            sb.Append($"uisi_room_id: {rageshakeWebhook?.Data?.AutoUisi?.RoomId}");
        }

        sb.Append(Environment.NewLine);
        sb.Append($"build: {rageshakeWebhook?.Data?.Build}");
        sb.Append(Environment.NewLine);
        sb.Append($"default_app_language: {rageshakeWebhook?.Data?.DefaultAppLanguage}");
        sb.Append(Environment.NewLine);
        sb.Append($"device: {rageshakeWebhook?.Data?.Device}");
        sb.Append(Environment.NewLine);
        sb.Append($"device_id: {rageshakeWebhook?.Data?.DeviceId}");
        sb.Append(Environment.NewLine);
        sb.Append($"lazy_loading: {rageshakeWebhook?.Data?.LazyLoading}");
        sb.Append(Environment.NewLine);
        sb.Append($"local_time: {rageshakeWebhook?.Data?.LocalTime}");
        sb.Append(Environment.NewLine);
        sb.Append($"locale: {rageshakeWebhook?.Data?.Locale}");
        sb.Append(Environment.NewLine);
        sb.Append($"matrix_sdk_version: {rageshakeWebhook?.Data?.MatrixSdkVersion}");
        sb.Append(Environment.NewLine);
        sb.Append($"olm_kit_version: {rageshakeWebhook?.Data?.OlmKitVersion}");
        sb.Append(Environment.NewLine);
        sb.Append($"os: {rageshakeWebhook?.Data?.Os}");
        sb.Append(Environment.NewLine);
        sb.Append($"user_id: {rageshakeWebhook?.Data?.UserId}");
        sb.Append(Environment.NewLine);
        sb.Append($"utc_time: {rageshakeWebhook?.Data?.UtcTime}");

        //RAGESHAKE WEBHOOK PART II

        if (rageshakeWebhook is not null && rageshakeWebhook.Labels.Any())
            foreach (var label in rageshakeWebhook.Labels)
            {
                sb.Append(Environment.NewLine);
                sb.Append($"label: {label}");
            }

        if (rageshakeWebhook is not null)
        {
            sb.Append(Environment.NewLine);
            sb.Append($"logs_count: {rageshakeWebhook?.Logs.Count()}");
        }

        sb.Append(Environment.NewLine);
        sb.Append($"logErrors: {rageshakeWebhook?.LogErrors}");

        sb.Append(Environment.NewLine);
        sb.Append(
            $"report_url: {rageshakeWebhook?.ReportUrl.Replace(envsDto.RageshakeDomainToBeReplaced, envsDto.RageshakeDomain)}");

        sb.Append(Environment.NewLine);
        sb.Append(
            $"listing_url: {rageshakeWebhook?.ListingUrl.Replace(envsDto.RageshakeDomainToBeReplaced, envsDto.RageshakeDomain)}");

        var message = sb.ToString();

        _logger.LogInformation(message);

        return message;
    }
}
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/", () =>
{
    return Results.Ok("Working");
});

app.MapGet("/health", () =>
{
    return Results.Ok();
});

app.MapPost("/handle-rageshake-webhook", async (RageshakeWebhook rageshakeWebhook, IHttpClientFactory httpClientFactory) =>
{

    var client = httpClientFactory.CreateClient();

    string rageshake_domain_to_be_replaced = Environment.GetEnvironmentVariable("RAGESHAKE_DOMAIN_TO_BE_REPLACED");
    string rageshake_domain = Environment.GetEnvironmentVariable("RAGESHAKE_DOMAIN");

    string matrix_notifier_url = Environment.GetEnvironmentVariable("MATRIX_NOTIFIER_URL");
    string matrix_notifier_message_header = Environment.GetEnvironmentVariable("MATRIX_NOTIFIER_MESSAGE_HEADER");

    string matrix_homeserver_url = Environment.GetEnvironmentVariable("MATRIX_HOMESERVER_URL");
    string matrix_homeserver_user = Environment.GetEnvironmentVariable("MATRIX_HOMESERVER_USER");
    string matrix_homeserver_passwd = Environment.GetEnvironmentVariable("MATRIX_HOMESERVER_PASSWD");
    string matrix_homeserver_room = Environment.GetEnvironmentVariable("MATRIX_HOMESERVER_ROOM");

    StringBuilder sb = new StringBuilder();

    //RAGESHAKE WEBHOOK PART I
    sb.Append(matrix_notifier_message_header);
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
    sb.Append(Environment.NewLine);
    sb.Append($"app_language: {rageshakeWebhook?.Data?.AutoUisi}");
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

    if (rageshakeWebhook is not null && rageshakeWebhook.Labels is not null && rageshakeWebhook.Labels.Any())
    {
        foreach (var label in rageshakeWebhook.Labels)
        {
            sb.Append(Environment.NewLine);
            sb.Append($"label: {label}");
        }
    }

    if (rageshakeWebhook is not null && rageshakeWebhook.Logs is not null)
    {
        sb.Append(Environment.NewLine);
        sb.Append($"logs_count: {rageshakeWebhook?.Logs.Count()}");
    }

    sb.Append(Environment.NewLine);

    sb.Append($"logErrors: {rageshakeWebhook?.LogErrors}");

    sb.Append(Environment.NewLine);
    sb.Append($"report_url: {rageshakeWebhook?.ReportUrl.Replace(rageshake_domain_to_be_replaced, rageshake_domain)}");

    sb.Append(Environment.NewLine);
    sb.Append($"listing_url: {rageshakeWebhook?.ListingUrl.Replace(rageshake_domain_to_be_replaced, rageshake_domain)}");

    string message = sb.ToString();

    app.Logger.LogInformation(message);

    var matrixNotifierBody = new MatrixNotifier(matrix_homeserver_url, matrix_homeserver_user, matrix_homeserver_passwd, matrix_homeserver_room, message);

    string jsonString = JsonSerializer.Serialize(matrixNotifierBody);

    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

    await client.PostAsync(matrix_notifier_url, content);

    return Results.Ok("Handled");
});

app.Run("http://localhost:3412");

public record Data(
    [property: JsonPropertyName("User-Agent")] string UserAgent,
    [property: JsonPropertyName("Version")] string Version,
    [property: JsonPropertyName("app_language")] string AppLanguage,
    [property: JsonPropertyName("auto_uisi")] string AutoUisi,
    [property: JsonPropertyName("build")] string Build,
    [property: JsonPropertyName("default_app_language")] string DefaultAppLanguage,
    [property: JsonPropertyName("device")] string Device,
    [property: JsonPropertyName("device_id")] string DeviceId,
    [property: JsonPropertyName("lazy_loading")] string LazyLoading,
    [property: JsonPropertyName("local_time")] string LocalTime,
    [property: JsonPropertyName("locale")] string Locale,
    [property: JsonPropertyName("matrix_sdk_version")] string MatrixSdkVersion,
    [property: JsonPropertyName("olm_kit_version")] string OlmKitVersion,
    [property: JsonPropertyName("os")] string Os,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("utc_time")] string UtcTime
);

public record RageshakeWebhook(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("user_text")] string UserText,
    [property: JsonPropertyName("app")] string App,
    [property: JsonPropertyName("data")] Data Data,
    [property: JsonPropertyName("labels")] IReadOnlyList<string> Labels,
    [property: JsonPropertyName("logs")] IReadOnlyList<string> Logs,
    [property: JsonPropertyName("logErrors")] object LogErrors,
    [property: JsonPropertyName("files")] object Files,
    [property: JsonPropertyName("fileErrors")] object FileErrors,
    [property: JsonPropertyName("report_url")] string ReportUrl,
    [property: JsonPropertyName("listing_url")] string ListingUrl
);

public record MatrixNotifier(
    [property: JsonPropertyName("matrixHomeserverURL")] string MatrixHomeserverURL,
    [property: JsonPropertyName("matrixHomeserverUser")] string MatrixHomeserverUser,
    [property: JsonPropertyName("matrixHomeserverPasswd")] string MatrixHomeserverPasswd,
    [property: JsonPropertyName("matrixHomeserverRoom")] string MatrixHomeserverRoom,
    [property: JsonPropertyName("message")] string Message
);

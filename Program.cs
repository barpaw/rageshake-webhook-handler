using System.Text.Json.Serialization;
using MediatR;
using Polly;
using RageshakeWebhookHandler.Commands;
using RageshakeWebhookHandler.Services;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    .CreateLogger();

try
{
    Log.Information("Starting web application");


    var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Host.UseSerilog();
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    builder.Services.AddTransient<IEnvironmentService, EnvironmentService>();
    builder.Services.AddTransient<IMessageService, MessageService>();
    builder.Services.AddTransient<IApiService, ApiService>();

    builder.Services.AddHttpClient("api")
        .AddTransientHttpErrorPolicy(policyBuilder =>
            policyBuilder.WaitAndRetryAsync(
                10, retryNumber => TimeSpan.FromSeconds(Math.Pow(2, retryNumber))));

    var app = builder.Build();

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.MapGet("/", () => { return Results.Ok("Working"); });

    app.MapGet("/health", () => { return Results.Ok(); });

    app.MapPost("/handle-rageshake-webhook",
        async (IMediator mediator, RageshakeWebhook rageshakeWebhook) =>
        {
            var response = await mediator.Send(new HandleRageshakeWebhookCommand(rageshakeWebhook));

            return Results.Ok(response);
        });

    app.Run("http://localhost:3412");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public record Data(
    [property: JsonPropertyName("User-Agent")]
    string UserAgent,
    [property: JsonPropertyName("Version")]
    string Version,
    [property: JsonPropertyName("app_language")]
    string AppLanguage,
    [property: JsonPropertyName("auto_uisi")]
    Uisi AutoUisi,
    [property: JsonPropertyName("build")] string Build,
    [property: JsonPropertyName("default_app_language")]
    string DefaultAppLanguage,
    [property: JsonPropertyName("device")] string Device,
    [property: JsonPropertyName("device_id")]
    string DeviceId,
    [property: JsonPropertyName("lazy_loading")]
    string LazyLoading,
    [property: JsonPropertyName("local_time")]
    string LocalTime,
    [property: JsonPropertyName("locale")] string Locale,
    [property: JsonPropertyName("matrix_sdk_version")]
    string MatrixSdkVersion,
    [property: JsonPropertyName("olm_kit_version")]
    string OlmKitVersion,
    [property: JsonPropertyName("crypto_module_version")]
    string CryptoModuleVersion,
    [property: JsonPropertyName("os")] string Os,
    [property: JsonPropertyName("user_id")]
    string UserId,
    [property: JsonPropertyName("utc_time")]
    string UtcTime
);

public record Uisi(
    [property: JsonPropertyName("session_id")]
    string SessionId,
    [property: JsonPropertyName("sender_key")]
    string SenderKey,
    [property: JsonPropertyName("device_id")]
    string DeviceId,
    [property: JsonPropertyName("user_id")]
    string UserId,
    [property: JsonPropertyName("event_id")]
    string EventId,
    [property: JsonPropertyName("room_id")]
    string RoomId
);


public record RageshakeWebhook(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("user_text")]
    string UserText,
    [property: JsonPropertyName("app")] string App,
    [property: JsonPropertyName("data")] Data Data,
    [property: JsonPropertyName("labels")] IReadOnlyList<string> Labels,
    [property: JsonPropertyName("logs")] IReadOnlyList<string> Logs,
    [property: JsonPropertyName("logErrors")]
    object LogErrors,
    [property: JsonPropertyName("files")] object Files,
    [property: JsonPropertyName("fileErrors")]
    object FileErrors,
    [property: JsonPropertyName("report_url")]
    string ReportUrl,
    [property: JsonPropertyName("listing_url")]
    string ListingUrl
);

public record MatrixNotifier(
    [property: JsonPropertyName("matrixHomeserverURL")]
    string MatrixHomeserverURL,
    [property: JsonPropertyName("matrixHomeserverUser")]
    string MatrixHomeserverUser,
    [property: JsonPropertyName("matrixHomeserverPasswd")]
    string MatrixHomeserverPasswd,
    [property: JsonPropertyName("matrixHomeserverRoom")]
    string MatrixHomeserverRoom,
    [property: JsonPropertyName("message")]
    string Message
);
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapPost("/handle-rageshake-webhook", (RageshakeWebhook rageshakeWebhook) =>
{


    return Results.Ok("Handled");
});

app.Run();

record RageshakeWebhook();
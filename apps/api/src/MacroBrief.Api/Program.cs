LocalEnvFile.LoadFromWorkingDirectory();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("WebClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var storageMode = builder.Configuration["MB_STORAGE_MODE"] ?? "memory";
if (storageMode.Equals("local_json", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddSingleton(sp =>
        LocalJsonStorageOptions.FromConfiguration(
            sp.GetRequiredService<IConfiguration>(),
            sp.GetRequiredService<IHostEnvironment>()));
    builder.Services.AddSingleton<IHoldingsService, LocalJsonHoldingsService>();
    builder.Services.AddSingleton<IFeedbackService, LocalJsonFeedbackService>();
    builder.Services.AddSingleton<IKpiEventService, LocalJsonKpiEventService>();
    builder.Services.AddSingleton<IAiExplanationService, LocalJsonAiExplanationService>();
}
else
{
    builder.Services.AddSingleton<IHoldingsService, InMemoryHoldingsService>();
    builder.Services.AddSingleton<IFeedbackService, InMemoryFeedbackService>();
    builder.Services.AddSingleton<IKpiEventService, InMemoryKpiEventService>();
    builder.Services.AddSingleton<IAiExplanationService, InMemoryAiExplanationService>();
}

builder.Services.AddSingleton<IMappingRulesProvider, JsonMappingRulesProvider>();
builder.Services.AddSingleton<IPortfolioInsightsService, InMemoryPortfolioInsightsService>();
builder.Services.AddSingleton<IStorageStatusService, StorageStatusService>();

var app = builder.Build();
app.UseCors("WebClient");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapDashboardEndpoints();
app.MapHoldingEndpoints();
app.MapPortfolioInsightsEndpoints();
app.MapFeedbackEndpoints();
app.MapAiGuardrailsEndpoints();
app.MapKpiEventsEndpoints();
app.MapSystemEndpoints();

app.Run();

public partial class Program;

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
builder.Services.AddSingleton<IHoldingsService, InMemoryHoldingsService>();
builder.Services.AddSingleton<IMappingRulesProvider, JsonMappingRulesProvider>();
builder.Services.AddSingleton<IAiExplanationService, InMemoryAiExplanationService>();
builder.Services.AddSingleton<IPortfolioInsightsService, InMemoryPortfolioInsightsService>();
builder.Services.AddSingleton<IFeedbackService, InMemoryFeedbackService>();

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

app.Run();

public partial class Program;

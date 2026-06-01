var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IHoldingsService, InMemoryHoldingsService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapDashboardEndpoints();
app.MapHoldingEndpoints();
app.MapPortfolioInsightsEndpoints();

app.Run();

public partial class Program;

using OndeALinhaDesligouWeb.Services;
using OndeALinhaDesligouWeb.Middleware;
using Serilog;
using OndeALinhaDesligouWeb.Options;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog from configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register custom services
// Bind LinhasConfiguration to options and register
builder.Services.Configure<OndeALinhaDesligouWeb.Options.LinhasOptions>(builder.Configuration.GetSection("LinhasConfiguration"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<LinhasOptions>>().Value);
builder.Services.AddSingleton<LinhaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Removido o redirecionamento HTTPS para evitar erro de porta

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting web host");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

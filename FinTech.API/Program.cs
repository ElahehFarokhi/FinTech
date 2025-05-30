using FinTech.API.Middleware;
using FinTech.API.SeedData;
using FinTech.Application;
using FinTech.Application.Interfaces;
using FinTech.Infrsdtructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddInfrastructure();
builder.Services.AddControllers();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

// Seed initial data (optional)
using (var scope = app.Services.CreateScope())
{
    var accountRepo = scope.ServiceProvider.GetRequiredService<IAccountRepository>();
    await SeedData.InitializeAsync(accountRepo);
}

app.Run();


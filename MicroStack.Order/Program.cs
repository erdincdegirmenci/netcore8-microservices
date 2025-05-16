using MicroStack.Order.Extensions;
using Order.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MigrateDatabase();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

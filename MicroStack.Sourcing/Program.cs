using EventBusRabbitMQ;
using EventBusRabbitMQ.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MicroStack.Sourcing.Data;
using MicroStack.Sourcing.Data.Interfaces;
using MicroStack.Sourcing.Hubs;
using MicroStack.Sourcing.Repositories;
using MicroStack.Sourcing.Repositories.Interfaces;
using MicroStack.Sourcing.Settings;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MicroStack.Sourcing",
        Version = "v1"

    });
});

builder.Services.AddControllers();
builder.Services.Configure<SourcingDatabaseSettings>(builder.Configuration.GetSection(nameof(SourcingDatabaseSettings)));


#region Dependencies
builder.Services.AddSingleton<ISourcingDatabaseSettings, SourcingDatabaseSettings>(sp => sp.GetRequiredService<IOptions<SourcingDatabaseSettings>>().Value);
builder.Services.AddTransient<ISourcingContext, SourcingContext>();
builder.Services.AddTransient<IAuctionRepository, AuctionRepository>();
builder.Services.AddTransient<IBidRepository, BidRepository>();
builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
{
    var configuration = builder.Configuration;
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
    var factory = new ConnectionFactory()
    {
        HostName = configuration["EventBus:HostName"]
    };

    if (!string.IsNullOrEmpty(configuration["EventBus:UserName"]))
    {
        factory.UserName = configuration["EventBus:UserName"];
    }

    if (!string.IsNullOrEmpty(configuration["EventBus:Password"]))
    {
        factory.Password = configuration["EventBus:Password"];
    }

    var retryCount = 5;
    if (!string.IsNullOrEmpty(configuration["EventBus:RetryCount"]))
    {
        retryCount = int.Parse(configuration["EventBus:RetryCount"]);
    }
    return new DefaultRabbitMQPersistentConnection(factory, retryCount, logger);
});
builder.Services.AddSingleton<EventBusRabbitMQProducer>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .WithOrigins("https://localhost:44393") // UI adresi
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSignalR();
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroStack.Sourcing v1"));
}


app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.MapHub<AuctionHub>("/auctionhub");

app.Run();

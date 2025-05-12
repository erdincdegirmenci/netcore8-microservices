using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MicroStack.Sourcing.Data;
using MicroStack.Sourcing.Data.Interfaces;
using MicroStack.Sourcing.Repositories;
using MicroStack.Sourcing.Repositories.Interfaces;
using MicroStack.Sourcing.Settings;

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
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroStack.Sourcing v1"));
}
app.UseAuthorization();

app.MapControllers();

app.Run();

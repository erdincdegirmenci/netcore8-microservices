using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MicroStack.Products.Data;
using MicroStack.Products.Data.Interfaces;
using MicroStack.Products.Repositories;
using MicroStack.Products.Repositories.Interfaces;
using MicroStack.Products.Settings;
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title ="MicroStack.Products",
            Version = "v1"

        });
    });
    builder.Services.AddControllers();
    builder.Services.Configure<ProductDatabaseSettings>(builder.Configuration.GetSection(nameof(ProductDatabaseSettings)));

    #region Dependencies
    builder.Services.AddSingleton<IProductDatabaseSettings, ProductDatabaseSettings>(sp => sp.GetRequiredService<IOptions<ProductDatabaseSettings>>().Value);
    builder.Services.AddTransient<IProductContext, ProductContext>();
    builder.Services.AddTransient<IProductRepository, ProductRepository>();
    #endregion


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(s=> s.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroStack.Products v1"));
    }

    app.UseHttpsRedirection();
    app.MapControllers();


    app.Run();

}
catch (Exception ex)
{

	throw ex;
}

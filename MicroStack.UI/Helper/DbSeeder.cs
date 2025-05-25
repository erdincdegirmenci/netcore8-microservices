using MicroStack.Infrastructure.Data;

namespace MicroStack.UI.Helper
{
    public static class DbSeeder
    {
        public static void CreateAndSeedDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var webAppContext = services.GetRequiredService<WebAppContext>();
                    WebAppContextSeed.SeedAsync(webAppContext, loggerFactory).Wait();
                }
                catch (Exception exception)
                {
                    var logger = loggerFactory.CreateLogger("SeedLogger");
                    logger.LogError(exception, "An error occurred seeding the DB.");
                }
            }
        }
    }
}

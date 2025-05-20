using Microsoft.Extensions.Hosting;
using MicroStack.Order.Consumers;
using System.Runtime.CompilerServices;

namespace MicroStack.Order.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static EventBusOrderCreateConsumer Listener { get; set; }
        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetRequiredService<EventBusOrderCreateConsumer>();
            var life = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);

            return app;
        }
        private static void OnStarted()
        {
            Listener.Consume();
        }
        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}

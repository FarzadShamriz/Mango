using Mango.Services.EmailAPI.Messaging;
using System.Runtime.CompilerServices;

namespace Mango.Services.RewardAPI.Extension
{
    public static class AppBuilderExtensions
    {
        private static IAzureServiceBusConsumer _azureServiceBusConsumer { get; set; }

        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            _azureServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLife.ApplicationStarted.Register(OnStarted);
            hostApplicationLife.ApplicationStopping.Register(OnStopped);

            return app;
        }

        private static void OnStopped()
        {
            _azureServiceBusConsumer.Stop();
        }

        private static void OnStarted()
        {
            _azureServiceBusConsumer.Start();
        }
    }
}

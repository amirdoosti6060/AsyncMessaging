using CommandHistory.Models;
using CommandHistory.Services;
using Microsoft.Extensions.Options;

namespace CommandHistory.Extensions
{
    public static class Extensions
    {
        public static void ConfigMemoryHistory(this WebApplicationBuilder builder)
        {
            KafkaSettings myConfig = builder.Configuration.GetSection("Kafka").Get<KafkaSettings>();
            var wrapper = new OptionsWrapper<KafkaSettings>(myConfig);
            var kafka = new KafkaSubscriber<string, CommandMessage>(wrapper);
            var history = new MemoryHistory(kafka);
            builder.Services.AddSingleton(history);
        }
    }
}

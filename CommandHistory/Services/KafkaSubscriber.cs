using CommandHistory.Interfaces;
using CommandHistory.Models;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace CommandHistory.Services
{
    public class CommandMessageDeserializer<Q> : IDeserializer<Q>
    {
        public Q Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            var str = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<Q>(str)!;
        }
    }

    public class KafkaSubscriber<K, Q>: IKafkaSubscriber<K, Q> 
        where Q: class 
        where K: notnull
    {
        private readonly KafkaSettings _settings;
        private readonly ConsumerConfig _consumerConfig;
        private readonly IConsumer<K, Q> _consumer;
        private readonly AdminClientConfig _adminClientConfig;
        private readonly IAdminClient _adminClient;

        public KafkaSubscriber(IOptions<KafkaSettings> settings)
        {
            _settings = settings.Value;

            _adminClientConfig = new AdminClientConfig
            {
                BootstrapServers = _settings.BootstrapServer
            };

            _adminClient = new AdminClientBuilder(_adminClientConfig).Build();

            // Just to check if Kafka is up and running otherwise raise an exception
            // Uncomment it if you want to stop application if Kafka is down
            //_adminClient.GetMetadata(TimeSpan.FromSeconds(1));

            // To create the topic if not exist
            _adminClient.CreateTopicsAsync(
                new TopicSpecification[] {
                    new TopicSpecification {
                        Name = "AsyncMessaging",
                        ReplicationFactor = 1,
                        NumPartitions = 1
                    }
                });

            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _settings.BootstrapServer,
                GroupId = "TestGroup",
                EnableAutoCommit = true
            };

            var deserializer = new CommandMessageDeserializer<Q>();
            _consumer = new ConsumerBuilder<K, Q>(_consumerConfig)
                            .SetValueDeserializer(deserializer)
                            .Build();

        }

        public void GetMessage(
            string topic, 
            CancellationToken cancellationToken, 
            Action<K, Q> handler)
        {
            _consumer.Subscribe(topic);
            while(!cancellationToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(cancellationToken);

                handler(result.Message.Key, result.Message.Value);
            }
            _consumer.Close();
        }
    }
}

using CommandAPI.Interfaces;
using CommandAPI.Models;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace CommandAPI.Services
{
    public class CommandMessageSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            string json = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(json);
        }
    }

    public class CommandMessageDeserializer<Q> : IDeserializer<Q>
    {
        public Q Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            var str = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<Q>(str)!;
        }
    }

    public class KafkaPublisher<K, T>: IKafkaPublisher<K, T> 
        where T: class 
        where K: notnull
    {
        private readonly KafkaSettings _settings;
        private readonly ProducerConfig _producerConfig;
        private readonly IProducer<K, T> _producer;
        private readonly AdminClientConfig _adminClientConfig;
        private readonly IAdminClient _adminClient;

        public KafkaPublisher(IOptions<KafkaSettings> settings)
        {
            _settings = settings.Value;

            _adminClientConfig = new AdminClientConfig
            {
                BootstrapServers = _settings.BootstrapServer
            };

            _producerConfig = new ProducerConfig
            {
                BootstrapServers = _settings.BootstrapServer
            };

            _adminClient = new AdminClientBuilder(_adminClientConfig).Build();
            
            // Just to check if Kafka is up and running otherwise raise an exception
            _adminClient.GetMetadata(TimeSpan.FromSeconds(1));

            // To create the topic if not exist
            _adminClient.CreateTopicsAsync(
                new TopicSpecification[] {
                    new TopicSpecification { 
                        Name = "AsyncMessaging", 
                        ReplicationFactor = 1, 
                        NumPartitions = 1 
                    } 
                });

            var serializer = new CommandMessageSerializer<T>();
            _producer = new ProducerBuilder<K, T>(_producerConfig)
                            .SetValueSerializer(serializer)
                            .Build();

        }

        public async Task<DeliveryResult<K, T>> SendMessage(
            string topic,
            K key, 
            T message, 
            CancellationToken cancellationToken)
        {
            var prodMessage = new Message<K, T>()
            {
                Key = key,
                Value = message
            };

            return await _producer.ProduceAsync(topic, prodMessage, cancellationToken);
        }
    }
}

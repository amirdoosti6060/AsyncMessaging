using Confluent.Kafka;

namespace CommandAPI.Interfaces
{
    public interface IKafkaPublisher<K, T> 
        where T: class 
        where K: notnull
    {
        Task<DeliveryResult<K, T>> SendMessage(string topic, K key, T message, CancellationToken cancellationToken);
    }
}

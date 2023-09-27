using Confluent.Kafka;

namespace CommandHistory.Interfaces
{
    public interface IKafkaSubscriber<K, Q> 
        where Q: class 
        where K: notnull
    {
        void GetMessage(string topic, CancellationToken cancellationToken, Action<K, Q> handler);
    }
}

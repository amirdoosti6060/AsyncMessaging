using CommandHistory.Interfaces;
using CommandHistory.Models;
using System.Collections;
using System.Text;

namespace CommandHistory.Services
{
    public class MemoryHistory
    {
        public ArrayList CommandMessages { get; set; }
        private IKafkaSubscriber<string, CommandMessage> _kafkaSubscriber;
        public MemoryHistory(IKafkaSubscriber<string, CommandMessage> kafkaSubscriber)
        {
            _kafkaSubscriber = kafkaSubscriber;

            CommandMessages = ArrayList.Synchronized(new ArrayList());

            Task.Run(() =>
            {
                var cancellationTokenSource = new CancellationTokenSource();

                _kafkaSubscriber.GetMessage("AsyncMessaging", cancellationTokenSource.Token, (key, message) =>
                {
                    CommandMessages.Add(message);
                });
            });
        }
    }
}

using HS.Message.Share.BaseModel;
using HS.Message.Share.HttpClients.Clients;
using HS.Rabbitmq.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Rabbitmq.Core
{
    public class RabbitmqTopicConsumer : RabbitmqTopic
    {
        private readonly IConsumerCallBack _consumerCallBack;

        public RabbitmqTopicConsumer(IConfiguration configuration,
            ILogger<RabbitmqTopic> logger,
            IServiceProvider serviceProvider,
            IConsumerCallBack consumerCallBack)
            : base(configuration, logger, serviceProvider)
        {
            _consumerCallBack = consumerCallBack;
        }
        public void InitMq()
        {
            ConsumerCallBack = _consumerCallBack.RunAsync;

            if (_rabbitmqConfig.Channels != null && _rabbitmqConfig.Channels.Length > 0)
            {
                foreach (var channel in _rabbitmqConfig.Channels)
                {
                    QueueMessage queueMessage = new QueueMessage
                    {
                        MessageId = Guid.Empty.ToString(),
                        MessageContent = "",
                        MessageType = Enum.Parse<QueueMessageType>(channel)
                    };
                    Producer(queueMessage);
                    ProducerDelayed(queueMessage, 3);
                    string queueName = string.Format(_queueName, _rabbitmqConfig.NameSpace, channel.ToLower());
                    InitConsumer(queueName);
                    string queueNameDelayed = string.Format(_queueNameDelayed, _rabbitmqConfig.NameSpace, queueMessage.MessageType.ToString().ToLower());
                    InitConsumer(queueNameDelayed);
                }
            }

        }
    }
}

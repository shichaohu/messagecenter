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
    public class RabbitmqTopicProducer: RabbitmqTopic
    {
        public RabbitmqTopicProducer(IConfiguration configuration, ILogger<RabbitmqTopic> logger, IServiceProvider serviceProvider)
            : base(configuration, logger, serviceProvider)
        {
        }
        

    }
}

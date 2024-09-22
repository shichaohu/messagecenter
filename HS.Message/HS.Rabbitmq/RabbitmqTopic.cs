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

namespace HS.Rabbitmq
{
    public class RabbitmqTopic
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitmqTopic> _logger;
        private readonly IServiceProvider _serviceProvider;

        private readonly string _exchangeName;
        private readonly string _delayedExchangeName;
        private readonly string _queueName;
        private readonly string _queueNameDelayed;

        private readonly string _userName;
        private readonly string _password;
        private readonly string _hostName;
        private readonly int _port;

        private static ConcurrentBag<string> _queueNameList = [];
        private readonly string _checkConsumerLock;

        public RabbitmqTopic(IConfiguration configuration, ILogger<RabbitmqTopic> logger, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _logger = logger;
            _serviceProvider = serviceProvider;

            string prefix = configuration["Prefix"].ToLower();
            _exchangeName = prefix + "{0}.topicexchange";
            _delayedExchangeName = _exchangeName + ".delayed";
            _queueName = prefix + "{0}.queue.{1}";
            _queueNameDelayed = _queueName + ".delayed";

            _userName = configuration["UserName"];
            _password = configuration["Password"];
            _hostName = configuration["HostName"];
            _port = int.Parse(configuration["Port"]);
        }
        /// <summary>
        /// 生产者
        /// </summary>
        /// <param name="message"></param>
        public ProducerResponse Producer(QueueMessage queueMessage)
        {
            var result = new ProducerResponse();
            try
            {
                queueMessage.NumberOfConsumed = 1;

                _logger.LogInformation($"start queuing a message : {queueMessage}");

                //创建连接工厂
                var factory = new ConnectionFactory { UserName = _userName, Password = _password, HostName = _hostName, Port = _port };

                //创建连接
                using var connection = factory.CreateConnection();
                //创建通道
                using var channel = connection.CreateModel();
                //声明一个交换机
                string exchangeName = string.Format(_exchangeName, queueMessage.NameSpace.ToLower());
                channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true, false, null);
                //声明一个队列
                string queueName = string.Format(_queueName, queueMessage.NameSpace.ToLower(), queueMessage.MessageType.ToString().ToLower());
                channel.QueueDeclare(
                    queue: queueName,
                    durable: true,//是否持久化,true持久化,队列会保存磁盘,服务器重启时可以保证不丢失相关信息。
                    exclusive: false,//是否排他,true排他的,如果一个队列声明为排他队列,该队列仅对首次声明它的连接可见,并在连接断开时自动删除.
                    autoDelete: false,//是否自动删除。true是自动删除。自动删除的前提是：致少有一个消费者连接到这个队列，之后所有与这个队列连接的消费者都断开时,才会自动删除.
                    arguments: null
                    );
                //channel.BasicQos(0, 1, false);//每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
                string routeKey = $"{queueMessage.MessageType}.*";//符号“#”匹配一个或多个词，符号“*”匹配不多不少一个词.
                channel.QueueBind(queueName, exchangeName, routeKey, null);

                string message = $"{queueMessage.MessageType}.{JsonConvert.SerializeObject(queueMessage)}";

                var sendBytes = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                channel.BasicPublish(exchangeName, routeKey, null, sendBytes);

                _logger.LogInformation($"queuing a message success : {queueMessage}");
                CheckConsumer(queueName);
                result.Successed = true;
            }
            catch (Exception ex)
            {
                result.Successed = false;
                string msg = $"queuing a message error : {ex.Message}";
                result.Message = ex.Message;
                _logger.LogError(msg);
            }

            return result;
        }
        /// <summary>
        /// 批量生产消息
        /// </summary>
        /// <param name="message"></param>
        public ProducerResponse BatchProducer(List<QueueMessage> queueMessageList)
        {
            var result = new ProducerResponse();
            try
            {
                //创建连接工厂
                var factory = new ConnectionFactory { UserName = _userName, Password = _password, HostName = _hostName, Port = _port };

                //创建连接
                using var connection = factory.CreateConnection();
                //创建通道
                using var channel = connection.CreateModel();
                //声明一个交换机
                string exchangeName = string.Format(_exchangeName, queueMessageList[0].NameSpace.ToLower());
                channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true, false, null);
                //声明一个队列
                string queueName = string.Format(_queueName, queueMessageList[0].NameSpace.ToLower(), queueMessageList[0].MessageType.ToString().ToLower());
                channel.QueueDeclare(
                    queue: queueName,
                    durable: true,//是否持久化。
                    exclusive: false,//是否排他
                    autoDelete: false,//是否自动删除
                    arguments: null
                    );
                //channel.BasicQos(0, 1, false);//每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
                string routeKey = $"{queueMessageList[0].MessageType}.*";
                channel.QueueBind(queueName, exchangeName, routeKey, null);
                foreach (QueueMessage queueMessage in queueMessageList)
                {
                    queueMessage.NumberOfConsumed = 1;

                    _logger.LogInformation($"start queuing a message : {queueMessage}");

                    string message = $"{queueMessage.MessageType}.{JsonConvert.SerializeObject(queueMessage)}";

                    var sendBytes = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    channel.BasicPublish(exchangeName, routeKey, null, sendBytes);

                    _logger.LogInformation($"queuing a message success : {queueMessage}");
                    CheckConsumer(queueName);
                }

                result.Successed = true;
            }
            catch (Exception ex)
            {
                result.Successed = false;
                string msg = $"queuing a message error : {ex.Message}";
                result.Message = ex.Message;
                _logger.LogError(msg);
            }

            return result;
        }
        /// <summary>
        /// 生产者（延迟投递）
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="delaySeconds">延迟时间（秒）</param>
        private void ProducerDelayed(QueueMessage queueMessage, int delaySeconds)
        {
            try
            {
                queueMessage.NumberOfConsumed++;

                _logger.LogInformation($"start queuing a delayed message : {queueMessage}");

                var factory = new ConnectionFactory { UserName = _userName, Password = _password, HostName = _hostName, Port = _port };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                var arguments = new Dictionary<string, object>
                {
                    { "x-delayed-type", "topic" } // 实际的交换机类型
                };

                //声明一个交换机
                string exchangeName = string.Format(_delayedExchangeName, queueMessage.NameSpace.ToLower());
                channel.ExchangeDeclare(exchangeName, "x-delayed-message", true, false, arguments: arguments);
                string queueName = string.Format(_queueNameDelayed, queueMessage.NameSpace.ToLower(), queueMessage.MessageType.ToString().ToLower());
                //声明一个队列
                channel.QueueDeclare(
                    queue: queueName,
                    durable: true,//是否持久化
                    exclusive: false,//是否排他
                    autoDelete: false,//是否自动删除
                    arguments: arguments
                    );
                //将队列绑定到交换机
                string routeKey = $"{queueMessage.MessageType}.*";
                channel.QueueBind(queueName, _delayedExchangeName, routeKey, null);

                string message = $"{queueMessage.MessageType}.{JsonConvert.SerializeObject(queueMessage)}";
                var sendBytes = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.Headers = new Dictionary<string, object>
                {
                    { "x-delay", delaySeconds } // 需要rabbitmq_delayed_message_exchange的延迟插件支持
                };
                //发布消息
                channel.BasicPublish(_delayedExchangeName, routeKey, properties, sendBytes);

                _logger.LogInformation($"queuing a delayed message success : {queueMessage}");
                CheckConsumer(queueName);
            }
            catch (Exception ex)
            {
                string msg = $"queuing a delayed message error : {ex.Message}";
                _logger.LogError(msg);
            }


        }
        public void CheckConsumer(string queueName)
        {
            lock (_checkConsumerLock)
            {
                if (!_queueNameList.Contains(queueName))
                {
                    lock (_checkConsumerLock)
                    {
                        Parallel.For(0, 100, i => Consumer(queueName));
                        _queueNameList.Add(queueName);
                    }
                }
            }
        }
        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="consumerFunc">消费逻辑</param>
        public void Consumer(string queueName)
        {
            try
            {
                _logger.LogInformation("start creating a basic content-class consumer");

                var factory = new ConnectionFactory { UserName = _userName, Password = _password, HostName = _hostName, Port = _port };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                //事件基本消费者
                var consumer = new EventingBasicConsumer(channel);

                //接收到消息事件
                consumer.Received += async (model, ea) =>
                {
                    var messageSource = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var messageString = messageSource[(messageSource.IndexOf(".") + 1)..];
                    _logger.LogInformation($"receive message : {messageString}");
                    var message = JsonConvert.DeserializeObject<QueueMessage>(messageSource);

                    var res = ConsumerRun(message);
                    if (res.Code == ResponseCode.Success && res.Data.Successed)
                    {
                        channel.BasicAck(ea.DeliveryTag, false);//确认该消息已被消费
                        _logger.LogInformation($"message consumption success : {messageSource}");
                    }
                    else
                    {
                        if (res.Data.DelaySeconds >= 0)
                        {
                            ProducerDelayed(message, res.Data.DelaySeconds);
                            _logger.LogInformation($"message consumption failure : {res}");
                        }
                        else
                        {
                            _logger.LogInformation($"message consumption over the maximum number of retries and stop : {res}");
                        }
                    }
                };
                //启动消费者 设置为手动应答消息
                channel.BasicConsume(queueName, false, consumer);
            }
            catch (Exception ex)
            {
                string msg = $"creating a basic content-class consumer ({queueName}) error : {ex.Message}";
                _logger.LogError(msg);
            }


        }
        public Func<QueueMessage, BaseResponse<ConsumerResponse>> ConsumerFunc { get; set; }
        public BaseResponse<ConsumerResponse> ConsumerRun(QueueMessage message)
        {
            try
            {
                var response = ConsumerFunc(message);

                if (response.Code != ResponseCode.Success)
                {
                    response.Data.DelaySeconds = CalculateDelaySeconds(message.NumberOfConsumed);
                }
                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponse<ConsumerResponse>
                {
                    Code = ResponseCode.InternalError,
                    Message = $"execute consumerfunc error : {ex.Message}",
                    Data = new ConsumerResponse
                    {
                        Successed = false,
                        DelaySeconds = CalculateDelaySeconds(message.NumberOfConsumed),
                    }
                };
            }
        }
        /// <summary>
        /// 计算下次消费的延迟时间
        /// </summary>
        /// <param name="numberOfConsumed">当前第几次消费</param>
        /// <returns></returns>
        private int CalculateDelaySeconds(int numberOfConsumed)
        {
            if (numberOfConsumed > DelaySeconds.Length)
            {
                return -1;
            }
            else
            {
                return DelaySeconds[numberOfConsumed];
            }
        }
        /// <summary>
        /// 延迟时间
        /// </summary>
        private int[] DelaySeconds
        {
            get
            {
                return [1, 5, 10, 20, 60, 300, 600];
            }
        }

    }
}

using RabbitMQ.Client;
using System.Text;

namespace HS.Rabbitmq
{
    public class Class1
    {
        public void DirectCreat(string exchangeName, string queueName, string routeKey, string message)
        {
            //创建连接工厂
            var factory = new ConnectionFactory
            {
                UserName = "admin",//用户名
                Password = "admin123456",//密码
                HostName = "192.168.157.130",//rabbitmq ip
                Port = 5672
            };

            //创建连接
            using var connection = factory.CreateConnection();
            //创建通道
            using var channel = connection.CreateModel();
            //声明一个队列
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false, null);
            channel.QueueDeclare(
                queue: "hello",
                durable: true,//是否持久化,true持久化,队列会保存磁盘,服务器重启时可以保证不丢失相关信息。
                exclusive: false,//是否排他,true排他的,如果一个队列声明为排他队列,该队列仅对首次声明它的连接可见,并在连接断开时自动删除.
                autoDelete: false,//是否自动删除。true是自动删除。自动删除的前提是：致少有一个消费者连接到这个队列，之后所有与这个队列连接的消费者都断开时,才会自动删除.
                arguments: null
                );
            channel.BasicQos(0, 1, false);//每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
            //将队列绑定到交换机
            channel.QueueBind(queueName, exchangeName, routeKey, null);
            var sendBytes = Encoding.UTF8.GetBytes(message);
            //发布消息
            channel.BasicPublish(exchangeName, routeKey, null, sendBytes);

        }
    }
}

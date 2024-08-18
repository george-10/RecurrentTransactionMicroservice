using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client; 
public class RabbitMqProducerService
{
    private readonly ConnectionFactory _factory;

    public RabbitMqProducerService()
    {
        _factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
    }

    public void SendMessage<T>(string queueName, T entity)
    {

        string message = JsonConvert.SerializeObject(entity);
        Type factoryType = _factory.GetType();
        MethodInfo createConnectionMethod = factoryType.GetMethod("CreateConnection", new Type[0]);

        if (createConnectionMethod != null)
        {
            using (IConnection connection = (IConnection)createConnectionMethod.Invoke(_factory, null))
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: queueName,
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }
        else
        {
            Console.WriteLine("CreateConnection method not found.");
        }
    }
}

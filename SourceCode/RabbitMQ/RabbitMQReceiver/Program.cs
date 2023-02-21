using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQReceiver;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (IConnection connection = factory.CreateConnection())
        using (IModel channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "coderserdar",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Employee employee = JsonConvert.DeserializeObject<Employee>(message);
                Console.WriteLine($"Adı: {employee.Name} Soyadı:{employee.Surname} [{employee.Message}]");
                Console.WriteLine("İşe Alındınız. Teşekkürler :)");
            };
            channel.BasicConsume(queue: "coderserdar",
                                 autoAck: true,
                                 consumer: consumer);

            Console.ReadLine();
        }
    }
}

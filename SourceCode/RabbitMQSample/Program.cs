using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQSample;
using System.Text;
using Bogus;
using RabbitMQ.Client.Events;

public class Program
{
    public static void Main(string[] args)
    {
        var programKapansinMi = false;
        while (!programKapansinMi) {
            Console.WriteLine("RabbitMQ kuyruğuna kaç tane kayıt göndermek istiyorsunuz? (Programdan çıkmak için E veya e yazınız)");
            var sayi = Console.ReadLine();
            if (int.TryParse(sayi, out int sayac))
            {
                SendToRabbitMQ(sayac);
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("RabbitMQ kuyruğundan alma işlemleri için ekrana bir değer giriniz");
                Console.ReadLine();
                ReceiveFromRabbitMQ();
                Console.WriteLine("------------------------------------------");
            }
            else
            {
                if (sayi == "e" || sayi == "E")
                    programKapansinMi = true;
                else
                    Console.WriteLine($"{sayi} bir sayı veya çıkma isteği değildir");
            }
        }
        Console.WriteLine("Programı kullandığınız için teşekkürler. İyi günler");
        Environment.Exit(0);
    }

    private static void SendToRabbitMQ(int sayac)
    {
        for (int i = 0; i < sayac; i++)
        {
            var testUsers = new Faker<Employee>()
                .CustomInstantiator(f => new Employee())
                .RuleFor(u => u.Name, f => f.Name.FirstName())
                .RuleFor(u => u.Surname, f => f.Name.LastName())
                .RuleFor(u => u.BirthPlace, (f, u) => f.Address.City())
                .RuleFor(u => u.BirthDate, (f, u) => f.Person.DateOfBirth)
                .RuleFor(u => u.ID, f => new Random().Next());

            var employee = testUsers.Generate();

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "coderserdar",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonConvert.SerializeObject(employee);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "coderserdar",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($"Gönderilen kişi: Adı Soyadı: {employee.Name} {employee.Surname} Doğum Tarihi: {employee.BirthDate.ToShortDateString()}");
                Console.WriteLine((i + 1) + ". kişi gönderildi...");
            }
        }
    }

    private static void ReceiveFromRabbitMQ()
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
                Console.WriteLine($"Adı Soyadı: {employee.Name} {employee.Surname} [{employee.BirthPlace}]");
                Console.WriteLine("RabbitMQ ile tanıştınız. İyi günler.");
            };
            channel.BasicConsume(queue: "coderserdar",
                                 autoAck: true,
                                 consumer: consumer);

            Console.ReadLine();
        }
    }
}
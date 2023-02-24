using Bogus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQSample;
using System.Text;

/// <summary>
/// Rabbit MQ uygulamasının konsolda çalışmasını sağlayan metotları içeren temel sınıftır.
/// </summary>
public class Program
{
    /// <summary>
    /// Konsol uygulamasının çalıştığı ana metottur
    /// </summary>
    /// <param name="args">Gönderilen Argüman Dizizi</param>
    public static void Main(string[] args)
    {
        var programKapansinMi = false;
        // Kullanıcı Programdan çıkmak için e veya E yazmadığı sürece çalışmasını sağlayan döngüdür
        while (!programKapansinMi) {
            Console.WriteLine("RabbitMQ kuyruğuna kaç tane kayıt göndermek istiyorsunuz? (Programdan çıkmak için E veya e yazınız)");
            var sayi = Console.ReadLine();
            // eğer geçerli bir sayı girerse o sayı kadar Kişi kaydı oluşturup
            // RabbitMQ kuyruğuna yollayacak, alacak
            // Ve bu bilgileri konsol ekranında yazdıracak kod bloğu burasıdır
            if (int.TryParse(sayi, out int kisiSayisi))
            {
                RabbitMQKuyrugaGonder(kisiSayisi);
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("RabbitMQ kuyruğundan alma işlemleri için ekrana bir değer giriniz");
                Console.ReadLine();
                RabbitMQKuyruktanAl();
                Console.WriteLine("------------------------------------------");
            }
            else
            {
                // Eğer e veya E harfi girerse kullanıcı konsoldan
                // programdan çıkış yapılmasını sağlayacak kod bloğudur
                if (sayi == "e" || sayi == "E")
                    programKapansinMi = true;
                // eğer geçersiz bir sayı veya metin girerse gösterilecek hata mesajıdır
                else
                {
                    Console.WriteLine($"{sayi} geçerli bir sayı veya uygulama kapatma isteği değildir");
                    Console.WriteLine("------------------------------------------");
                }
            }
        }
        Console.WriteLine("Programı kullandığınız için teşekkürler. İyi günler");
        Environment.Exit(0);
    }

    /// <summary>
    /// Ekrandan alınan sayı kadar Kişi oluşturup
    /// Rabbit MQ kuyruğuna göndermeyi ve gönderilen kişiler ile ilgili bilgileri
    /// Konsol ekranında göstermeye yarayan metottur
    /// </summary>
    /// <param name="kisiSayisi">Kişi Sayısı Bilgisi</param>
    private static void RabbitMQKuyrugaGonder(int kisiSayisi)
    {
        for (int i = 0; i < kisiSayisi; i++)
        {
            // Bogus kütüphanesi kullanılarak sahte veri içeren kişi kayıtlarının
            // otomatik olarak oluşturulmasını sağlayan yapıdır
            var kisiOlusturucu = new Faker<Kisi>()
                .CustomInstantiator(f => new Kisi())
                    .RuleFor(u => u.Adi, f => f.Name.FirstName())
                    .RuleFor(u => u.Soyadi, f => f.Name.LastName())
                    .RuleFor(u => u.DogumYeri, (f, u) => f.Address.City())
                    .RuleFor(u => u.DogumTarihi, (f, u) => f.Person.DateOfBirth)
                    .RuleFor(u => u.ID, f => new Random().Next());

            var kisi = kisiOlusturucu.Generate();

            // Bilgisayara kurulan localhost RabbitMQ Sunucusuna bağlantı için gerekli bilgileri içeren nesnedir
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                // burada coderserdar adında bir kanal açarak, onun içerisine ilgili kişi kayıtlarının gönderilmesi için hazırlamaktadır
                // diğer parametreler de kullanılarak kanalın kullanımı özelleştirilebilir
                channel.QueueDeclare(queue: "coderserdar", durable: false, exclusive: false, autoDelete: false, arguments: null);
                
                string message = JsonConvert.SerializeObject(kisi);
                // sınıf serialize edilerek byte dizisi haline dönüştürülür
                var body = Encoding.UTF8.GetBytes(message);

                // burada dönüştürülen byte dizisinin kanala gönderilmesi sağlanır
                channel.BasicPublish(exchange: "", routingKey: "coderserdar", basicProperties: null, body: body);

                Console.WriteLine($"Gönderilen kişi: Adı Soyadı: {kisi.Adi} {kisi.Soyadi} Doğum Tarihi: {kisi.DogumTarihi.ToShortDateString()}");
                Console.WriteLine((i + 1) + ". kişi gönderildi...");
            }
        }
    }

    /// <summary>
    /// Rabbit MQ kuyruğunda biriken bilgilerin FIFO mantığına göre
    /// Kuyruktan alınarak
    /// Kişi bilgilerinin konsol ekranında görüntülenmesini sağlayan metottur
    /// </summary>
    private static void RabbitMQKuyruktanAl()
    {
        // Bilgisayara kurulan localhost RabbitMQ Sunucusuna bağlantı için gerekli bilgileri içeren nesnedir
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (IConnection connection = factory.CreateConnection())
        using (IModel channel = connection.CreateModel())
        {
            // burada coderserdar adında bir kanal açarak, onun içerisine ilgili kişi kayıtlarının gönderilmesi için hazırlamaktadır
            // diğer parametreler de kullanılarak kanalın kullanımı özelleştirilebilir
            channel.QueueDeclare(queue: "coderserdar", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // burada kuyruğu tüketecek bir comsumer nesnesi oluşturulur ve tüketmeye başlar
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Kisi kisi = JsonConvert.DeserializeObject<Kisi>(message);
                Console.WriteLine($"Adı Soyadı: {kisi.Adi} {kisi.Soyadi} [{kisi.DogumYeri}]");
                Console.WriteLine("RabbitMQ ile tanıştınız. İyi günler.");
            };

            // kanalın kuyruğu tüketerek ilgili bilgilerin konsolda gösterilmesi sağlanır
            channel.BasicConsume(queue: "coderserdar", autoAck: true, consumer: consumer);

            Console.ReadLine();
        }
        Console.WriteLine("Kuyruğun tamamı tüketildi ve gösterildi. Devam etmek için bir tuşa basınız");
        Console.ReadLine();
    }
}
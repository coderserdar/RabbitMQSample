# RabbitMQ Sample

![GitHub stars](https://img.shields.io/github/stars/coderserdar/RabbitMQSample?style=social) ![GitHub forks](https://img.shields.io/github/forks/coderserdar/RabbitMQSample?style=social) ![GitHub watchers](https://img.shields.io/github/watchers/coderserdar/RabbitMQSample?style=social) ![GitHub repo size](https://img.shields.io/github/repo-size/coderserdar/RabbitMQSample?style=plastic) ![GitHub language count](https://img.shields.io/github/languages/count/coderserdar/RabbitMQSample?style=plastic) ![GitHub top language](https://img.shields.io/github/languages/top/coderserdar/RabbitMQSample?style=plastic) ![GitHub last commit](https://img.shields.io/github/last-commit/coderserdar/RabbitMQSample?color=red&style=plastic) ![GitHub issues](https://img.shields.io/github/issues/coderserdar/RabbitMQSample)

### Proje Hakkında

Bu uygulama, **RabbitMQ** adı verilen *message broker* teknolojisinin kendi sistemlerimiz içerisinde nasıl kullanılacağına dair en temel düzeyde bir konsol uygulamadır. Burada amaç, **RabbitMQ** ile metin, sınıf vb. çeşitli verilerin nasıl gönderilip alınabileceğini göstermektir. Siz kendi yazacağınız uygulamalarda çok daha karmaşık işlemler gerçekleştireibilirsiniz. Ancak dediğim gibi burada amaç en basit yoluyla bu teknolojinin nasıl kullanılacağını göstermektir.

Ben temel olarak Kişi adını verdiğimiz bir sınıf bilgisi üzerinden *Bogus* adındaki NuGet paket ile, ekrandan kullanıcının girdiği sayı kadar sınıf nesnesi oluşturup içerisini dump data ile doldurduktan sonra *RabbitMQ* üzerinde ilgili kuyruğa gönderme ve alma işlemlerini gerçekleştirecek, bunu kullanıcı sıkılp uygulamadan çıkana kadar yapacak döngüsel bir yapıya sahip bir konsol uygulaması yazdım. Ancak siz farklı bir yöntem vb. kullanabilirsiniz. Temeli öğrendikten sonra yapabilecekleriniz hayal gücünüz ile kısıtlı.

Bu projede aşağıda belirtilen teknolojiler kullanılmıştır.

|  Programlama Dili  |  .NET Versiyonu  | Geliştirme Ortamı |     Kullanılan NuGet Paketler     |
|------------------------|----------------|----------------------|------------------------|
|          *C#*          |      *6.0*     | *Visual Studio 2022 Community Edition* | *Bogus, RabbitMQ Client, NewtonSoft JSON* |


### Proje Çalıştırılmadan Öncesi Yapılması Gerekenler

Bu projeyi makinanızda çalıştırmak isterseniz öncesinde bazı işlemler gerçekleştirmeniz gerekmektedir. Burada yapılması gereken işlemleri sırasıyla belirteceğim.

1. İlk olarak [**Erlang**](http://www.erlang.org/downloads) linkindeki setup dosyasını indirip kurulumu yapmanız gerekmektedir.
2. *Erlang* başarılı bir şekilde kurulduktan sonra [**RabbitMQ**](http://www.rabbitmq.com/install-windows.html) linkinden setup dosyasını bulup kurulumunu yapmanız gerekmektedir.
3. Bu iki kurulum tamamlandıktan sonra *komut istemi (cmd)* açtıktan sonra *Program Files* içerisindeki *RabbitMQ* klasörünün içerisinde *sbin* yazan klasörün içine kadar **cd** komutları ile gitmeniz gerekmektedir.
4. Bu klasörün içerisine komut isteminde geldikten sonra `` rabbitmq-plugins.bat enable rabbitmq_management `` komutunu çalıştırmanız gerekmektedir.
5. Bu komut başarılı bir şekilde çalıştıktan sonra `` services.msc `` ile aktif çalışan servislerin açıldığı ekrandan **RabbitMQ** servisini bulup yeniden başlatmanız gerekmektedir.
6. Bütün bunları yaptıktan sonra web tarayıcınıza `` localhost:15672 `` yazdığınız zaman, lokal RabbitMQ sunucu sayfanız açılacaktır.
7. Burada hem *kullanıcı adı*, hem de *şifre* alanına `` guest `` yazarak sisteme girebilirsiniz. Sonrasında isterseniz kendi kullanıcılarınızı oluşturabilirsiniz.

### Dokümantasyon ve Örnek Ekran Görüntüleri

Kaynak kod üzerinden oluşturulan teknik dokümana [Kaynak Kod Dokümantasyonu](https://github.com/coderserdar/RabbitMQSample/blob/main/Documentation/RabbitMQSample.pdf) adresinden ulaşabilirsiniz. Bu *PDF* dosyası üzerinden kaynak kodları inceleyebilirsiniz. PDF dosyası Hyperlink desteklediği için doküman üzerinden kodlara, fonksiyonlara vb. gidebilirsiniz. Kaynak kod içerisinde olabildiğince detaylı bir şekilde açıklama satırları yazmaya çalıştım.

Programla ilgili örnek ekran görüntüleri aşağıdadır

<table>
   <tr>
      <td><img src="https://github.com/coderserdar/RabbitMQSample/blob/main/Screenshots/App_Screens_01.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/RabbitMQSample/blob/main/Screenshots/App_Screens_02.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/RabbitMQSample/blob/main/Screenshots/App_Screens_03.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/RabbitMQSample/blob/main/Screenshots/App_Screens_04.png?raw=true"></td>
   </tr>
   <tr>
      <td><img src="https://github.com/coderserdar/RabbitMQSample/blob/main/Screenshots/App_Screens_05.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/RabbitMQSample/blob/main/Screenshots/App_Screens_06.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/RabbitMQSample/blob/main/Screenshots/App_Screens_07.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/RabbitMQSample/blob/main/Screenshots/App_Screens_08.png?raw=true"></td>
   </tr>
</table>
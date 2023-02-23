namespace RabbitMQSample
{
    /// <summary>
    /// RabbitMQ teknolojisi ile gönderilecek ve alınacak verilerin içerisinde bulunduğu sınıftır
    /// </summary>
    public class Kisi
    {
        /// <summary>
        /// Kişinin ID Bilgisi (Veri Tabanından geliyor gibi düşünebiliriz)
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Kişinin Adı Bilgisi
        /// </summary>
        public string Adi { get; set; }
        /// <summary>
        /// Kişinin Soyadı Bilgisi
        /// </summary>
        public string Soyadi { get; set; }
        /// <summary>
        /// Kişinin Doğum Tarihi Bilgisi
        /// </summary>
        public DateTime DogumTarihi { get; set; }
        /// <summary>
        /// Kişinin Doğum Yeri Bilgisi
        /// </summary>
        public string DogumYeri { get; set; }
    }
}

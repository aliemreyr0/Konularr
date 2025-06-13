using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;
/*
 Mustafa OF
 Kocaeli Myo
 */
namespace LinqXmlOrnek
{
    public partial class Anaform : Form
    {
        //Xml Dosyasını okuma
        XElement xmldosya = XElement.Load("isciler.xml");//İnternet adresi olabilir

        public Anaform()
        {
            InitializeComponent();
        }

        private void Anaform_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Düğümleri Xml olarak alma
            textBox1.Text = "";
            IEnumerable<XElement> altdugumler = xmldosya.Elements();
            //Listeleyelim
            foreach (var eleman in altdugumler)
                textBox1.Text += eleman;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Listeleme
            textBox1.Text = "";
            XDocument xmldosya = XDocument.Load("isciler.xml");//En üstteki (root) düğüm gösterilir.
            IEnumerable<XElement> altdugumler = xmldosya.Elements();
            //Düğümleri listeler
            foreach (var eleman in altdugumler)
                textBox1.Text += eleman;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Belirli bir element'i elde etme
            textBox1.Text = "";
            IEnumerable<XElement> altdugumler = xmldosya.Elements();
            //Listeleme
            foreach (var eleman in altdugumler)
            {
                textBox1.Text += string.Format("{0} adlı personelin kimliği {1} ve cinsiyeti {2}",
                    eleman.Element("isim").Value,
                    eleman.Element("kimlik").Value,
                    eleman.Element("cinsiyet").Value +
                    eleman.Element("telefon").Value + //Düğüm isimleri xml dosyasındaki gibi olmalıdır.
                    "\r\n");

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Erkek işçiler
            textBox1.Text = "";
            //Linq
            var erkekisciler = from deg1 in xmldosya.Elements("isci")
                               where deg1.Element("cinsiyet").Value == "Erkek"
                               select deg1;
            //Listeleme
            foreach (XElement eleman in erkekisciler)
                textBox1.Text += eleman;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Kadın işçiler
            textBox1.Text = "";
            //Linq
            var kadinisciler = from deg1 in xmldosya.Elements("isci")
                               where deg1.Element("cinsiyet").Value == "Kadın"
                               select deg1;
            //Listeleme
            foreach (XElement eleman in kadinisciler)
                textBox1.Text += eleman.Element("isim").Value + " Telefon:" + eleman.Element("telefon").Value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Ev Telefonu olanlar
            textBox1.Text = "";
            //Linq
            var evtelefonu = from deg1 in xmldosya.Elements("isci")
                             where (string)deg1.Element("telefon").Attribute("tip") == "ev"
                             select deg1;

            //Listeleme
            foreach (XElement eleman in evtelefonu)
                textBox1.Text += eleman.Element("isim").Value + " Telefon:" + eleman.Element("telefon").Value;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //İş Telefonu olanlar
            textBox1.Text = "";
            //Linq
            var istelefonu = from deg1 in xmldosya.Elements("isci")
                             where (string)deg1.Element("telefon").Attribute("tip") == "iş"
                             select deg1;

            //Listeleme
            foreach (XElement eleman in istelefonu)
                textBox1.Text += eleman.Element("isim").Value + " Telefon:" + eleman.Element("telefon").Value + "\r\n";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Kocaeli şehri
            textBox1.Text = "";
            //Linq
            var sehir = from deg1 in xmldosya.Elements("isci")
                        where (string)deg1.Element("adres").Element("sehir").Value == "Kocaeli"
                        select deg1;
            //Listeleme
            foreach (XElement eleman in sehir)
                textBox1.Text += eleman.Element("isim").Value
                    + " Şehir:" + eleman.Element("adres").Element("sehir").Value +
                    " Cadde:" + eleman.Element("adres").Element("cadde").Value +
                    " Posta Kodu:" + eleman.Element("adres").Element("postakodu").Value +
                    " Ülke:" + eleman.Element("adres").Element("ulke").Value +
                    "\r\n";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //İsimleri döndürelim
            textBox1.Text = "";
            //Descendants metodu ile sadece verilen düğüm ismindeki değerler elde edilebilir.
            foreach (XElement eleman in xmldosya.Descendants("isim"))
            {
                textBox1.Text += (string)eleman + "\r\n";
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //Xml dosyası yazdırma            
            //Xml düğümlerinin/elementlerinin oluşturulması
            XDocument xmldosyabilgi = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement("Personel",
                new XComment("Personel bilgileri oluşturuldu"),
                new XElement("kimlik", 5),
                new XElement("isim", "Ahmet Eren"),
                new XElement("birimi", "Muhasebe"),
                new XElement("cinsiyet", "Erkek"))
                );
            //Dosya yazma işlemleri
            StringWriter metinyaz = new StringWriter();
            xmldosyabilgi.Save(metinyaz);
            //Sonucu listele
            textBox1.Text = metinyaz.ToString();
            //Dosyayı oluşturalım
            StreamWriter dosyayaz = new StreamWriter("personelbilgi.xml");
            dosyayaz.Write(metinyaz.ToString());
            dosyayaz.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //XmlReader nesnesi ile xml verisini okuma
            textBox1.Text = "";
            XmlReader xmloku = XmlReader.Create("isciler.xml");
            XElement elementler = XElement.Load(xmloku);
            textBox1.Text = elementler.ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //kitapbilgi.xml dosyasından veri okur.
            string dosyaadi = "kitapbilgi.xml";
            if (File.Exists(dosyaadi))//Eğer dosya varsa oku
            {
                XElement xmldosya2 = XElement.Load(dosyaadi);//İnternet adresi olabilir                                                                 //Belirli bir element'i elde etme
                textBox1.Text = "";
                IEnumerable<XElement> altdugumler = xmldosya2.Elements();
                //Listeleme
                foreach (var eleman in altdugumler)
                {
                    textBox1.Text += string.Format("Kitap adı: {0}, Yayınevi:{1}, Yazarı: {2}, Fiyatı: {3}",
                        eleman.Element("isim").Value,
                        eleman.Element("yayinevi").Value,
                        eleman.Element("yazari").Value,
                        eleman.Element("fiyat").Value + "\r\n");
                }
            }
            else
            {
                MessageBox.Show("kitapbilgi.xml dosyası bulunamadı");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //Öğrencileri oku.
            //ogrenciler.xml dosyasından veri okur.
            string dosyaadi = "ogrenciler.xml";
            textBox1.Text = "";
            if (File.Exists(dosyaadi))//Eğer dosya varsa oku
            {
                XElement xmldosya2 = XElement.Load(dosyaadi);//İnternet adresi olabilir
                IEnumerable<XElement> altdugumler = xmldosya2.Elements();
                //Listeleyelim
                foreach (var eleman in altdugumler)
                {
                    textBox1.Text += string.Format("Öğrencinin numarası: {0}, Adı Soyadı:{1}, Bölümü: {2}, Yaşı: {3}",
                        eleman.Element("numara").Value,
                        eleman.Element("adsoyad").Value,
                        eleman.Element("bolum").Value,
                        eleman.Element("yas").Value + "\r\n");
                }
            }

        }

        private void button14_Click(object sender, EventArgs e)
        {
            //Yaşı 20-25 arasında olan öğrenciler
            textBox1.Text = "";
            // XML verisini yükleyelim
            XElement xmldosya = XElement.Load("ogrenciler.xml"); // XML dosyasının yolu

            // Yaşı 20-25 arasında olan öğrencileri sorgulamak
            var sonuc = from ogrenci in xmldosya.Elements("ogrenci")
                        let yas = (int)ogrenci.Element("yas") // Yaş bilgisini alıyoruz bir değişkene atıyoruz
                        where yas >= 20 && yas <= 25 // Yaş aralığına göre filtreliyoruz
                        select new
                        {
                            Numara = ogrenci.Element("numara").Value,
                            AdSoyad = ogrenci.Element("adsoyad").Value,
                            Bolum = ogrenci.Element("bolum").Value,
                            Yas = yas
                        };
            int sayac = 0;
            // Sonuçları ekrana yazdırıyoruz
            foreach (var ogrenci in sonuc)
            {
                textBox1.Text += ($"Numara: {ogrenci.Numara}, Ad Soyad: {ogrenci.AdSoyad}, Bölüm: {ogrenci.Bolum}, Yaş: {ogrenci.Yas}" + "\r\n");
                sayac ++;
            }
            textBox1.Text += "Toplam:" + sayac;
        }
    }
}

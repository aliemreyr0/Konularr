using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*Mustafa OF - Kocaeli Üniversitesi*/
namespace RehberLinqtoSql
{
    public partial class Form1 : Form
    {
        /*
        * LINQ to SQL, .NET framework'ünde kullanılan bir ORM (Object-Relational Mapping) teknolojisidir. Veritabanındaki tablolara karşılık gelen 
        * sınıflar oluşturmanıza olanak tanır ve bu sayede veritabanı ile etkileşimde bulunmanızı kolaylaştırır. LINQ to SQL, veritabanı ile 
        * nesne tabanlı bir yaklaşım kullanarak çalışmanıza imkan verir, yani SQL sorguları yazmak yerine C# gibi .NET dillerinde LINQ 
        * (Language Integrated Query) kullanarak veritabanı işlemlerini gerçekleştirebilirsiniz.
        * LINQ to SQL'deki temel yapı, "DataContext" ve "Entity" sınıflarıdır. Bu sınıflar veritabanındaki tablo ve ilişkileri temsil eder. 
        * Avantajları: Veritabanı Bağımsızlığı, Kodun Temizliği ve Bakımı, Veri Doğrulama
        * Küçük ve orta çaplı uygulamalar için uygundur.
        * Büyük çaplı projeler için Linq to Sql Classes yerine Entity Framework tercih edilir.
        * DataContext Sınıfı: DataContext sınıfı, veritabanı ile etkileşimi sağlayan ana sınıftır. Veritabanındaki tablolarla doğrudan 
        * bağlantı kurar ve veritabanı işlemleri (sorgulama, ekleme, silme, güncelleme) yapılabilir. Herhangi bir LINQ to SQL işlemine 
        * başlamak için bir DataContext nesnesi oluşturulur.
        * Entity Sınıfı:
        * Veritabanındaki her tablo için bir sınıf (entity) oluşturulur. Bu sınıflar, tablodaki sütunlarla eşleşir. Bu sınıflar,
        * veritabanı tablosundaki her bir kaydı bir nesne olarak temsil eder.
        */


        //Linq To Sql Classes eklenmelidir. Projede sağ tuşa tıklanır. Ekle>Yeni Öğe>Veri sekmesi seçilir. "Linq To Sql Classes" seçilir. 
        //Dbml dosyası oluşturulur.
        //Eğer şablonda LinqToSqlClasses gözükmüyorsa "Araçlar>Araçlar ve Özellikleri Edinin.." tıklanır. Kurulum ekranı açılır.
        //Değiştir düğmesi seçilir. Gelen pencerede "Bağımsız bileşenler" seçilir. Arama kısmına "Linq To Sql Classes" yazılır işaretlenir ve yüklenir.
        //Localdb (Mssql'in mini sürümü) veri tabanı için Görünüm>Sql Server Object Explorer seçilir. Veritabanı sunucusunun adı buradan alınır.
        //Localdb kurulu değilse yukarıdaki gibi "Bağımsız Bileşenler" listesinden "localdb" araması yapılır ve yüklenir.
        //Veri tabanı bağlantı cümlesini App.config dosyasına girerek değiştirilebilir.
        //Örnek bağlantı cümlesi şöyle olabilir: Server=(localdb)\MSSQLLocalDB;Database=Veriler1
        //Mssql class'ları için Nuget paket yöneticisi (Proje>Nuget Paketleri) ile "Microsoft.Data.SqlClient" paketi kurulmalıdır.

        /*
         * Tablo oluşturmak için sql kodu:
        CREATE TABLE rehber (
            Id INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
            Adsoyad  NVARCHAR (25)  NULL,
            Eposta   NVARCHAR (50)  NULL,
            Telno NVARCHAR (15) NULL,
            Sehir    NVARCHAR (30)  NULL,
            Adres    NVARCHAR (50)  NULL,
            Aciklama NVARCHAR (400) NULL,
            Ktarihi DATETIME DEFAULT getdate() NULL);
         */
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Tabloyuolustur();//Tablo yoksa veritabanında olşturur.
            textBox1.Focus();
            toolStripStatusLabel1.Text = Kayitsayisi().ToString() + " adet kayıt var.";
        }

        private void Tabloyuolustur()
        {
            //Veri tabanı tablosu yoksa oluşturur
            //Veri tabanı bağlantı cümlesini alma. Bağlantı cümlesinin adı (name), App.config dosyasındadır.
            string bcumle = ConfigurationManager.ConnectionStrings["RehberLinqtoSql.Properties.Settings.veriler1ConnectionString"].ConnectionString;
            SqlConnection bag = new SqlConnection(bcumle);
            bag.Open();
            string sql = @"CREATE TABLE rehber (
            Id INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
            Adsoyad  NVARCHAR (25)  NULL,
            Eposta   NVARCHAR (50)  NULL,
            Telno NVARCHAR (15) NULL,
            Sehir    NVARCHAR (30)  NULL,
            Adres    NVARCHAR (50)  NULL,
            Aciklama NVARCHAR (400) NULL,
            Ktarihi DATETIME DEFAULT getdate() NULL);";
            SqlCommand komut = new SqlCommand(sql,bag);
            try
            {
                komut.ExecuteNonQuery();
            }
            catch (Exception)
            {
            }
            bag.Close();
        }

        private int Kayitsayisi()
        {
            //Kayıt sayısını döndürür
            //Linq to Sql nesnelerini oluştur. RehberClasses1DataContext 
            RehberClasses1DataContext vtab = new RehberClasses1DataContext();
            var kayitsayisi = vtab.rehber.Count();//Kayıt sayısı
            return (int) kayitsayisi;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Ekle Düğmesi
            if(textBox1.Text =="" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == ""
                || textBox5.Text == "")
            {
                return;//Herhangi biri boşluksa çık
            }
            DialogResult cevap = MessageBox.Show("Bu kayıt eklensin mi?", "Yeni Kayıt", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if(cevap== DialogResult.Yes)
            {
                //Yeni kayıt ekle metodunu çağır.
                Yenikayitekle();
            }
        }

        private void Yenikayitekle()
        {
            //Yeni kayıt ekleme işlemleri
            //Linq to sql nesnelerini oluştur.
            RehberClasses1DataContext vtab = new RehberClasses1DataContext();//Datacontext nesnesini oluştur.
            rehber kayitlar = new rehber();//Tabloyu temsil eden Entity sınıfını oluştur.
            //Girilenleri, alanlara aktar
            kayitlar.Adsoyad = textBox1.Text.Trim();
            kayitlar.Eposta = textBox2.Text.Trim();
            kayitlar.Telno = textBox3.Text.Trim();
            kayitlar.Sehir = textBox4.Text.Trim();
            kayitlar.Adres = textBox5.Text.Trim();
            kayitlar.Ktarihi = DateTime.Now;
            //Kayıtları veritabanına aktar
            vtab.rehber.InsertOnSubmit(kayitlar);//Yeni kayıt ekledi
            try
            {
                vtab.SubmitChanges();//Değişiklikleri kaydet
            }
            catch (Exception hata)
            {
                toolStripStatusLabel1.Text = "Hata oluştu. Hata:" + hata.Message;
                return;
            }
            toolStripStatusLabel1.Text = kayitlar.Id.ToString() + " numaralı kayıt eklendi.";
            Metinalanlaribosalt();
        }

        private void Metinalanlaribosalt()
        {
            textBox1.Text = "";textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = "";
            textBox5.Text = "";
            textBox1.Focus();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //Listele formunu açar
            Listele lformu = new Listele();
            lformu.StartPosition = FormStartPosition.CenterScreen;
            lformu.ShowDialog();
        }

    }
}

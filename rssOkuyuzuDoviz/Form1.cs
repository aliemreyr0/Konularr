using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data.SQLite;//Nuget paket yöneticisi ile arama kısmına "System.Data.SQlite" yazılır ve yüklenir.
using System.IO;


namespace Rssokuyucu
{
	public partial class Form1 : Form
	{
		enum listsecimtur { dovizal, haberal }
		public static SQLiteConnection bag;

		listsecimtur listsecim;//Hangi hizmetin istendiğini belli eder.
		XDocument xmlverisi = null;
		XDocument xmlrssverisi = null;
		string dovizrss = "https://www.tcmb.gov.tr/kurlar/today.xml";
		string rsskaynakadresi = "";
		Dictionary<string, string> kaynaklistesi = new Dictionary<string, string>();

		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//Döviz bilgileri
			listsecim = listsecimtur.dovizal;
			Dovizlerigetir();
			toolStripStatusLabel1.Text = "Toplam Döviz Sayısı:" + listBox1.Items.Count.ToString();
		}

		private void Dovizlerigetir()
		{
			//Döviz bilgileri
			try
			{
				xmlverisi = XDocument.Load(dovizrss);
			}
			catch (Exception hata)
			{
				toolStripStatusLabel1.Text = "Veri alma işleminde hata oluştu. Hata:" + hata.Message;
				return;
			}
			listBox1.Items.Clear();//Temizle
								   //Linq satırı
			var dovizisimleri = from d1 in xmlverisi.Descendants("Currency")
								select new
								{
									trismi = d1.Element("Isim").Value,
									ingismi = d1.Element("CurrencyName").Value
								};
			//Listele
			foreach (var eleman in dovizisimleri)
			{
				listBox1.Items.Add(eleman.trismi.ToString());
			}

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			textBox1.Text = "";
			linkLabel1.Text = "";
			listBox1.Sorted = true;//Sıralar
			toolStripStatusLabel1.Text = "";
			toolStripStatusLabel2.Text = "";
			Sisteminsaatiniyaz();
			//Timer ayarları
			timer1.Interval = 1000;//1 saniye
			timer1.Enabled = true;//Aktif
			button1_Click(sender, e);//Döviz bilgisi getir düğmesinin Click olayı
			Baglan();
			Tablodanrsskaynagiekle();//Veri tabanından Rss kaynaklarını Açılan kutuya ekler
		}

		private void Tablodanrsskaynagiekle()
		{
			//Tablodan Rss kaynaklarını getir
			comboBox1.Items.Clear();
			kaynaklistesi.Clear();
			string sql = "Select * from xmlkaynaklari Order by kaynakadi;";
			SQLiteDataReader oku;
			SQLiteCommand okukomut = new SQLiteCommand(sql, bag);
			oku = okukomut.ExecuteReader();
			while (oku.Read())
			{
				kaynaklistesi.Add(oku["kaynakadi"].ToString(), oku["kaynagi"].ToString());
				comboBox1.Items.Add(oku["kaynakadi"].ToString());
			}
		}

		private void Baglan()
		{
			//Sqlite bağlantısını yapar
			if (!File.Exists("veriler.sqlite")) //Eğer veriler.sqlite adlı veri tabanı mevcut değilse db ve tabloyu oluştur
			{
				bag = new SQLiteConnection("Data Source=veriler.sqlite");
				bag.Open();
				//Tabloyu oluştur
				string yenisql = "CREATE TABLE xmlkaynaklari (id INTEGER PRIMARY KEY AUTOINCREMENT," +
				" kaynakadi TEXT NOT NULL, kaynagi TEXT NOT NULL);";
				SQLiteCommand komut = new SQLiteCommand(yenisql, bag);
				komut.ExecuteNonQuery();
				//İlk varsayılan kayıtları ekle
				string sql = "INSERT INTO xmlkaynaklari (kaynakadi,kaynagi) " +
				"VALUES ('Trt Haber Son Dakika','https://www.trthaber.com/sondakika_articles.rss')," +
				"('Trt Haber Gündem','https://www.trthaber.com/gundem_articles.rss')," +
				"('Trt Haber Türkiye','https://www.trthaber.com/turkiye_articles.rss')," +
				"('Trt Haber Dünya','https://www.trthaber.com/dunya_articles.rss')," +
				"('Trt Haber Ekonomi','https://www.trthaber.com/ekonomi_articles.rss');";
				SQLiteCommand komut2 = new SQLiteCommand(sql, bag);
				komut2.ExecuteNonQuery();
			}
			else
			{
				//Dosya mevcutsa db bağlantısını oluştur.
				bag = new SQLiteConnection("Data Source=veriler.sqlite");
				bag.Open();
			}
		}

		private void Sisteminsaatiniyaz()
		{
			//Sistemin saatini durum çubuğuna yaz.
			toolStripStatusLabel1.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			Sisteminsaatiniyaz();
		}



		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			//Web adresini açar
			Webadresiniac(linkLabel1.Tag.ToString());
		}

		private void Haberkaynaginidegis(string rssaadresi)
		{
			listsecim = listsecimtur.haberal;
			rsskaynakadresi = rssaadresi;
			Haberbilgilerinigetir();
			toolStripStatusLabel2.Text = "Toplam Haber Sayısı: " + listBox1.Items.Count.ToString();
		}

		private void Haberbilgilerinigetir()
		{
			//Rss kaynağından gelen habererli süz
			try
			{
				xmlrssverisi = XDocument.Load(rsskaynakadresi);
			}
			catch (Exception hata)
			{
				toolStripStatusLabel1.Text = "Veri alma işleminde hata oluştu. Hata:" + hata.Message;
				return;
			}
			listBox1.Items.Clear();//Temizle
								   //Linq satırı
			var haberbilgileri = from haberler in xmlrssverisi.Descendants("item")
								 select new
								 {
									 haberadi = haberler.Element("title").Value
								 };
			//Listele
			foreach (var eleman in haberbilgileri)
			{
				listBox1.Items.Add(eleman.haberadi.ToString());
			}
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Hangi düğme kullanıldı?
			if (listsecim == listsecimtur.dovizal)
			{
				//Döviz ayrıntıları
				//Linq satırı
				var alissatis = from doviz in xmlverisi.Descendants("Currency")
								where doviz.Element("Isim").Value == listBox1.SelectedItem.ToString()
								select new
								{
									alis = doviz.Element("ForexBuying").Value,
									satis = doviz.Element("ForexSelling").Value
								};
				//Listele
				foreach (var eleman in alissatis)
				{
					textBox1.Text = "Alış :" + eleman.alis + " Tl\r\n" + "Satış :" + eleman.satis + " Tl";
				}

			}
			else if (listsecim == listsecimtur.haberal)
			{
				//Son dakika haber ayrıntıları
				//Linq satırı
				var haberbilgileri = from haberler in xmlrssverisi.Descendants("item")
									 where haberler.Element("title").Value == listBox1.SelectedItem.ToString()
									 select new
									 {
										 baslik = haberler.Element("title").Value,
										 baglanti = haberler.Element("link").Value,
										 yayintarihi = haberler.Element("pubDate").Value,
										 //kategori = haberler.Element("category").Value,
										 aciklama = haberler.Element("description").Value
									 };
				//Listele
				foreach (var eleman in haberbilgileri)
				{
                    textBox1.Text = "Yayın Tarihi :" + eleman.yayintarihi
						//+ "\n" + "Kategori :" + eleman.kategori.ToString()
						+ "\n" + "Açıklama :" + eleman.aciklama.ToString()
						+ "\n" + "Web Adresi :" + eleman.baglanti.ToString();
					linkLabel1.Text = eleman.baslik;
					linkLabel1.Tag = eleman.baglanti;//Tag adlı ek özelliğe bağlantı atılır.
				}
			}
		}

		private void Webadresiniac(string webadresi)
		{
			ProcessStartInfo calismabilgi = new ProcessStartInfo();
			calismabilgi.UseShellExecute = true;//Web adresini, uygun Windows programıyla açmayı sağlar.
			calismabilgi.FileName = webadresi;//Açılacak web adresini alır
			Process.Start(calismabilgi);//İşlemi başlatır
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			bag.Close();
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			Haberkaynaginidegis(kaynaklistesi[comboBox1.Text]);
		}

		private void button6_Click(object sender, EventArgs e)
		{
			//Kaynaklar formunu aç
			Rsskaynaklari rformu = new Rsskaynaklari();
			rformu.StartPosition = FormStartPosition.CenterScreen;
			rformu.ShowDialog();
		}

		private void Form1_Activated(object sender, EventArgs e)
		{
			Tablodanrsskaynagiekle();
		}
	}
}

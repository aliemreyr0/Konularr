using System.Data.SQLite; //Nuget paket yöneticisi ile "System.Data.SQLite" aranmalý ve yüklenmelidir.
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Linq;
/*Öðr.Gör.Dr. Mustafa OF*/
namespace Rssokuma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Genel nesne/deðiþken
        public static SQLiteConnection bag;//Tüm class'larda paylaþýldý
        Dictionary<string, string> kaynaklistesi = new Dictionary<string, string>();//Anahtar: Rss kaynaðýnýn adý, deðer: Xml/Rss adresi
        XDocument xmlrssverisi = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            linkLabel1.Text = "";
            listBox1.Items.Clear();
            listBox1.Sorted = true;//Sýralar
            toolStripStatusLabel1.Text = "";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            Baglan();//Veri tabanýna baðlanma iþlemi
            Tablodanrsskaynagiekle();//Vt tablosundan Rss kaynaklarýný ComboBox içerisine ekler.
            comboBox1.SelectedIndex = 0;//Ýlk kaynak seçili olsun.
            //Haberlerigetir(comboBox1.Text);
            ShowInTaskbar = false; //Görev çubuðunda gözükmemesi saðlanýr. Bu sayede sað taraftaki simge alanýna küçülür.
        }

        private void Tablodanrsskaynagiekle()
        {
            //Vt tablosundan Rss kaynaklarýný Combobox'a ekler
            comboBox1.Items.Clear();
            kaynaklistesi.Clear();//Anahtar: Kaynak adý, Deðer:Kaynak adresi
            string sql = "Select * from rsskaynak Order By kaynakadi;";
            SQLiteDataReader oku = null;
            SQLiteCommand komut = new SQLiteCommand(sql, bag);
            oku = komut.ExecuteReader();//Okumayý çalýþtýr
            while (oku.Read())
            {
                kaynaklistesi.Add(oku["kaynakadi"].ToString(), oku["kaynagi"].ToString());
                comboBox1.Items.Add(oku["kaynakadi"].ToString());
            }//while
        }

        private void Baglan()
        {
            //Vt baðlantý iþlemini yapar
            if (!File.Exists("veriler.sqlite")) //Eðer veriler.sqlite dosyasý mevcut deðilse
            {
                bag = new SQLiteConnection("Data Source=veriler.sqlite");//Baðlantý cümlesi (Connection string)
                bag.Open();//Baðlantýyý aç
                //Tabloyu oluþtur
                string sql = "CREATE TABLE rsskaynak (id INTEGER PRIMARY KEY AUTOINCREMENT,";
                sql += " kaynakadi TEXT NOT NULL, kaynagi TEXT NOT NULL);";
                SQLiteCommand komut = new SQLiteCommand(sql, bag);
                komut.ExecuteNonQuery();//Komutu çalýþtýr
                //Bir kayýt ekle
                string sql2 = "INSERT INTO rsskaynak (kaynakadi,kaynagi) Values ('Trt Haber Manþet','https://www.trthaber.com/manset_articles.rss')";
                SQLiteCommand komut2 = new SQLiteCommand(sql2, bag);
                komut2.ExecuteNonQuery();
            }
            else
            {
                //veriler.sqlite mevcut ise
                bag = new SQLiteConnection("Data Source=veriler.sqlite");//Baðlantý cümlesi ile baðlantýyý hazýrla
                bag.Open();//Baðlantýyý aç
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Rss kaynaklarýnýn olduðu formu aç
            Rsskaynaklari rformu = new Rsskaynaklari();
            rformu.StartPosition = FormStartPosition.CenterScreen;
            rformu.ShowDialog();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //Form, aktif olduðunda çalýþýr
            Tablodanrsskaynagiekle();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Haber kaynaðý deðiþtiðinde Rss kaynaklarýnýn içeriðini listbox'a aktar
            Haberlerigetir(comboBox1.Text);
        }

        private void Haberlerigetir(string kaynakadi)
        {
            listBox1.Items.Clear();
            xmlrssverisi = XDocument.Load(kaynaklistesi[kaynakadi]);//Xml kaynaðýný yükle
            //Linq satýrý ile sorgula
            var haberbilgisi = from haberler in xmlrssverisi.Descendants("item")
                               select new
                               {
                                   haberadi = haberler.Element("title").Value
                               };
            //Listbox içerisine atalým
            foreach (var eleman in haberbilgisi)
            {
                listBox1.Items.Add(eleman.haberadi.ToString());//ListBox'a ekler
            }
            toolStripStatusLabel1.Text = "Toplam Haber Sayýsý: " + haberbilgisi.Count();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Lixtbox elemaný deðiþtiðinde meydana gelir.
            //Linq sorgusu
            var haberbilgileri = from haberler in xmlrssverisi.Descendants("item")
                                 where haberler.Element("title").Value == listBox1.SelectedItem.ToString()
                                 select new
                                 {
                                     baslik = haberler.Element("title").Value,
                                     yayintarihi = haberler.Element("pubDate").Value,
                                     baglanti = haberler.Element("link").Value,
                                     aciklama = haberler.Element("description").Value
                                 };
            textBox1.Text = "";
            //Verileri yaz
            foreach (var eleman in haberbilgileri)
            {
                textBox1.Text += "Baþlýðý:" + eleman.baslik + "\r\n" + "Açýklamasý:" + eleman.aciklama + "\r\n"
                    + "Yayýn Tarihi:" + eleman.yayintarihi;
                linkLabel1.Text = eleman.baslik;//Baþlýðý text'e yazýyoruz
                linkLabel1.Tag = eleman.baglanti;//Haberin hedef adresini Tag özelliðine yazýyoruz
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Týklandýðýnda çalýþýr
            Webadresiniac(linkLabel1.Tag.ToString());//Rss adresi Tag özelliðinden gelir.
        }

        private void Webadresiniac(string? webadresi)
        {
            ProcessStartInfo surecbilgi = new ProcessStartInfo();
            surecbilgi.UseShellExecute = true;//Windows'ta Ýliþkili programý açar
            surecbilgi.FileName = webadresi;//Web adresi için web tarayýcýsý uygulamasý açýlýr.
            Process.Start(surecbilgi);
        }

        private void çýkýþToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();//Programdan çýk
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            //Simge çift týklandýðýnda çalýþýr
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //Form, yeniden boyutlandýðýnda çalýþýr.
            if (this.WindowState == FormWindowState.Minimized) //Eðer form, simgeye küçültülürse
            {
                notifyIcon1.ShowBalloonTip(1, "Rss Okuyucu Simge Durumunda", "Rss Okuyucu", ToolTipIcon.Info);//Görev çubuðunda bilgilendirme yap
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Baðlantýyý kapat
            bag.Close();
        }

        private void hakkýndaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Hakkýnda formunu çaðýr
            Hakkinda hformu = new Hakkinda();
            hformu.TopMost = true;//En üstte görünen pencere yap
            hformu.StartPosition = FormStartPosition.CenterScreen;
            hformu.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();//Çýkýþ
        }

        private void hakkýndaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Hakkýnda formu click olayýný çalýþtýr
            hakkýndaToolStripMenuItem_Click(sender, e);
        }
    }
}

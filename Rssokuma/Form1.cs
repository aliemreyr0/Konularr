using System.Data.SQLite; //Nuget paket y�neticisi ile "System.Data.SQLite" aranmal� ve y�klenmelidir.
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Linq;
/*��r.G�r.Dr. Mustafa OF*/
namespace Rssokuma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Genel nesne/de�i�ken
        public static SQLiteConnection bag;//T�m class'larda payla��ld�
        Dictionary<string, string> kaynaklistesi = new Dictionary<string, string>();//Anahtar: Rss kayna��n�n ad�, de�er: Xml/Rss adresi
        XDocument xmlrssverisi = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            linkLabel1.Text = "";
            listBox1.Items.Clear();
            listBox1.Sorted = true;//S�ralar
            toolStripStatusLabel1.Text = "";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            Baglan();//Veri taban�na ba�lanma i�lemi
            Tablodanrsskaynagiekle();//Vt tablosundan Rss kaynaklar�n� ComboBox i�erisine ekler.
            comboBox1.SelectedIndex = 0;//�lk kaynak se�ili olsun.
            //Haberlerigetir(comboBox1.Text);
            ShowInTaskbar = false; //G�rev �ubu�unda g�z�kmemesi sa�lan�r. Bu sayede sa� taraftaki simge alan�na k���l�r.
        }

        private void Tablodanrsskaynagiekle()
        {
            //Vt tablosundan Rss kaynaklar�n� Combobox'a ekler
            comboBox1.Items.Clear();
            kaynaklistesi.Clear();//Anahtar: Kaynak ad�, De�er:Kaynak adresi
            string sql = "Select * from rsskaynak Order By kaynakadi;";
            SQLiteDataReader oku = null;
            SQLiteCommand komut = new SQLiteCommand(sql, bag);
            oku = komut.ExecuteReader();//Okumay� �al��t�r
            while (oku.Read())
            {
                kaynaklistesi.Add(oku["kaynakadi"].ToString(), oku["kaynagi"].ToString());
                comboBox1.Items.Add(oku["kaynakadi"].ToString());
            }//while
        }

        private void Baglan()
        {
            //Vt ba�lant� i�lemini yapar
            if (!File.Exists("veriler.sqlite")) //E�er veriler.sqlite dosyas� mevcut de�ilse
            {
                bag = new SQLiteConnection("Data Source=veriler.sqlite");//Ba�lant� c�mlesi (Connection string)
                bag.Open();//Ba�lant�y� a�
                //Tabloyu olu�tur
                string sql = "CREATE TABLE rsskaynak (id INTEGER PRIMARY KEY AUTOINCREMENT,";
                sql += " kaynakadi TEXT NOT NULL, kaynagi TEXT NOT NULL);";
                SQLiteCommand komut = new SQLiteCommand(sql, bag);
                komut.ExecuteNonQuery();//Komutu �al��t�r
                //Bir kay�t ekle
                string sql2 = "INSERT INTO rsskaynak (kaynakadi,kaynagi) Values ('Trt Haber Man�et','https://www.trthaber.com/manset_articles.rss')";
                SQLiteCommand komut2 = new SQLiteCommand(sql2, bag);
                komut2.ExecuteNonQuery();
            }
            else
            {
                //veriler.sqlite mevcut ise
                bag = new SQLiteConnection("Data Source=veriler.sqlite");//Ba�lant� c�mlesi ile ba�lant�y� haz�rla
                bag.Open();//Ba�lant�y� a�
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Rss kaynaklar�n�n oldu�u formu a�
            Rsskaynaklari rformu = new Rsskaynaklari();
            rformu.StartPosition = FormStartPosition.CenterScreen;
            rformu.ShowDialog();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //Form, aktif oldu�unda �al���r
            Tablodanrsskaynagiekle();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Haber kayna�� de�i�ti�inde Rss kaynaklar�n�n i�eri�ini listbox'a aktar
            Haberlerigetir(comboBox1.Text);
        }

        private void Haberlerigetir(string kaynakadi)
        {
            listBox1.Items.Clear();
            xmlrssverisi = XDocument.Load(kaynaklistesi[kaynakadi]);//Xml kayna��n� y�kle
            //Linq sat�r� ile sorgula
            var haberbilgisi = from haberler in xmlrssverisi.Descendants("item")
                               select new
                               {
                                   haberadi = haberler.Element("title").Value
                               };
            //Listbox i�erisine atal�m
            foreach (var eleman in haberbilgisi)
            {
                listBox1.Items.Add(eleman.haberadi.ToString());//ListBox'a ekler
            }
            toolStripStatusLabel1.Text = "Toplam Haber Say�s�: " + haberbilgisi.Count();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Lixtbox eleman� de�i�ti�inde meydana gelir.
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
                textBox1.Text += "Ba�l���:" + eleman.baslik + "\r\n" + "A��klamas�:" + eleman.aciklama + "\r\n"
                    + "Yay�n Tarihi:" + eleman.yayintarihi;
                linkLabel1.Text = eleman.baslik;//Ba�l��� text'e yaz�yoruz
                linkLabel1.Tag = eleman.baglanti;//Haberin hedef adresini Tag �zelli�ine yaz�yoruz
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //T�kland���nda �al���r
            Webadresiniac(linkLabel1.Tag.ToString());//Rss adresi Tag �zelli�inden gelir.
        }

        private void Webadresiniac(string? webadresi)
        {
            ProcessStartInfo surecbilgi = new ProcessStartInfo();
            surecbilgi.UseShellExecute = true;//Windows'ta �li�kili program� a�ar
            surecbilgi.FileName = webadresi;//Web adresi i�in web taray�c�s� uygulamas� a��l�r.
            Process.Start(surecbilgi);
        }

        private void ��k��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();//Programdan ��k
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            //Simge �ift t�kland���nda �al���r
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //Form, yeniden boyutland���nda �al���r.
            if (this.WindowState == FormWindowState.Minimized) //E�er form, simgeye k���lt�l�rse
            {
                notifyIcon1.ShowBalloonTip(1, "Rss Okuyucu Simge Durumunda", "Rss Okuyucu", ToolTipIcon.Info);//G�rev �ubu�unda bilgilendirme yap
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Ba�lant�y� kapat
            bag.Close();
        }

        private void hakk�ndaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Hakk�nda formunu �a��r
            Hakkinda hformu = new Hakkinda();
            hformu.TopMost = true;//En �stte g�r�nen pencere yap
            hformu.StartPosition = FormStartPosition.CenterScreen;
            hformu.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();//��k��
        }

        private void hakk�ndaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Hakk�nda formu click olay�n� �al��t�r
            hakk�ndaToolStripMenuItem_Click(sender, e);
        }
    }
}

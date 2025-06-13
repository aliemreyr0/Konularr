using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;//MSSQL veri tabanları için
using System.IO;

namespace RehberSql
{
    public partial class Form1 : Form
    {
        //Localdb (Mssql'in mini sürümü) veri tabanı için Görünüm>Sql Server Object Explorer seçilir. Veritabanı sunucusunun adı buradan alınır.
        //Localdb kurulu değilse yukarıdaki gibi "Bağımsız Bileşenler" listesinden "localdb" araması yapılır ve yüklenir.
        //Veri tabanı bağlantı cümlesini App.config dosyasına girerek değiştirilebilir.
        //Örnek bağlantı cümlesi şöyle olabilir: Server=(localdb)\MSSQLLocalDB;Database=Veriler1
        //Vt bağlantısı, uygulamanın olduğu klasörde bagbilgi.txt içerisinde bulunmaktadır.
        //Mssql class'ları için Nuget paket yöneticisi (Proje>Nuget Paketleri) ile "System.Data.SqlClient" paketi kurulmalıdır.
        /*
         * Tablo için sql kodu:
         CREATE TABLE [dbo].[rehber] (
        [Id]       INT            IDENTITY (1, 1) NOT NULL,
        [Adsoyad]  NVARCHAR (25)  NULL,
        [Eposta]   NVARCHAR (50)  NULL,
        [Sehir]    NVARCHAR (30)  NULL,
        [Adres]    NVARCHAR (50)  NULL,
        [Aciklama] NVARCHAR (400) NULL,
        [Ktarihi]  DATETIME       CONSTRAINT [DF_rehber_KTARIHI] DEFAULT (getdate()) NULL,
        [Telno]    NVARCHAR (15)  NULL,
        CONSTRAINT [PK_rehber1] PRIMARY KEY CLUSTERED ([Id] ASC))
         */

        //Genel tanımlar
        public static SqlConnection baglanti;
        string sirayonu = "ASC";//A-Z'ye doğru sıra yönü
        int idno = 0;//Kayıtları düzeltmek, silmek için
        //Bağlantı cümlesi
        string bcumle;
        List<string> secilenkayit = new List<string>();//Seçilen kaydın bilgilerini tutar

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Veri tabanında Tablo mevcut değilse tabloyu oluştur
            ToolTip toolTip1 = new ToolTip();
            toolTip1.IsBalloon = true;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(listView1, "İşlemler için bir kayıt seçilmeli");
            bcumle = Bcumleyial();//Dosyadan bağlantı cümlesini al
            if (!Tablomevcutmu("rehber", bcumle))
            {
                //Eğer tablo mevcut değilse tabloyu oluşturacak
                SqlConnection baglanti2 = new SqlConnection(bcumle);
                string sqlolustur = @"CREATE TABLE rehber (
                Id INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
                Adsoyad  NVARCHAR (25)  NULL,
                Eposta   NVARCHAR (50)  NULL,
                Telno NVARCHAR (15) NULL,
                Sehir    NVARCHAR (30)  NULL,
                Adres    NVARCHAR (50)  NULL,
                Aciklama NVARCHAR (400) NULL,
                Ktarihi DATETIME DEFAULT getdate() NULL);";
                SqlCommand komut = new SqlCommand(sqlolustur, baglanti2);
                baglanti2.Open();
                int sonuc = komut.ExecuteNonQuery();
                baglanti2.Close();
                komut.Dispose();
            }//if
            Baglan();
            toolStripStatusLabel1.Text = Kayitsayisi("rehber");
            Listele("Select * from rehber Order By Adsoyad");
        }

        private string Bcumleyial()
        {
            //Bağlantı cümlesini bir dosyadan alır
            string dosyaadi = "bagbilgi.txt";//Bağlantı cümlesinin olduğu dosya. Çalışan uygulama dosyasının olduğu yerde arar.
            if (!File.Exists(dosyaadi))
            {
                MessageBox.Show("Veri tabanı bağlantı cümlesi dosyası bulunamadı.");
                this.Close();
            }
            StreamReader dosya = new StreamReader(dosyaadi);
            string donen = dosya.ReadLine();
            dosya.Close();
            return donen;
        }

        private void Listele(string sql)
        {
            listView1.Items.Clear();
            SqlCommand komut = new SqlCommand(sql, baglanti);
            //Kayıtları okuma
            SqlDataReader oku = null;
            oku = komut.ExecuteReader();
            //Döngü yap
            while (oku.Read())
            {
                ListViewItem lw1 = new ListViewItem(oku["Id"].ToString());
                lw1.SubItems.Add(oku["Adsoyad"].ToString());
                lw1.SubItems.Add(oku["Eposta"].ToString());
                lw1.SubItems.Add(oku["Sehir"].ToString());
                lw1.SubItems.Add(oku["Adres"].ToString());
                lw1.SubItems.Add(oku["Ktarihi"].ToString());
                listView1.Items.Add(lw1);
            }
            oku.Close();
        }

        private string Kayitsayisi(string tabloadi)
        {
            string donen = "";
            SqlCommand komut = new SqlCommand("Select Count(*) from " + tabloadi, baglanti);
            int toplamkayit = (int)komut.ExecuteScalar();
            donen = toplamkayit + " adet kayıt mevcut";
            return donen;
        }

        private void Baglan()
        {
            try
            {
                //Bağlantı yapar
                baglanti = new SqlConnection(bcumle);
                baglanti.Open();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Veri Tabanı sunucusuna bağlanılamadı.\nBagbilgi.txt dosyasına bakınız. \nHata:" + hata.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            baglanti.Close();//Bağlantıyı kapat
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Değiştiğinde...
            Listele("Select * from rehber Where Adsoyad like '%" + textBox1.Text + "%' Order By Adsoyad");
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //Kolon başlıklarına tıklandığında çalışır.
            //Alan isimleri
            string[] alanlar = { "Id", "Adsoyad", "Eposta", "Sehir", "Adres", "Ktarihi" };
            if (sirayonu == "ASC")
            {
                Listele("Select * from rehber Order By " + alanlar[e.Column] + " " + sirayonu);
                sirayonu = "DESC";
            }
            else
            {
                Listele("Select * from rehber Order By " + alanlar[e.Column] + " " + sirayonu);
                sirayonu = "ASC";
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Yenikayit formunu aç
            Yenikayit yformu = new Yenikayit();
            yformu.StartPosition = FormStartPosition.CenterScreen;
            yformu.ShowDialog();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //Form her etkinleştirildiğinde çalışan olaydır.
            idno = 0;
            if (textBox1.Text == "")
            {
                Listele("Select * from rehber Order by Adsoyad");
            }
            else
            {
                Listele("Select * from rehber Where Adsoyad like '%" + textBox1.Text + "%' Order By Adsoyad");
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //Kayıt düzeltme
            if (idno == 0)
            {
                toolStripStatusLabel1.Text = "Düzeltme işlemi için bir kayıt seçmelisiniz.";
                return;
            }
            Duzeltme dformu = new Duzeltme(idno);
            dformu.StartPosition = FormStartPosition.CenterScreen;
            dformu.ShowDialog();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = "Kimlik: " + listView1.SelectedItems[0].Text + " - " + Kayitsayisi("rehber");
                idno = Int32.Parse(listView1.SelectedItems[0].Text);//Seçilen kaydın Id nosu
                //Seçilen kaydın diğer bilgilerini al
                secilenkayit.Add(listView1.SelectedItems[0].Text);//Kimlik
                secilenkayit.Add(listView1.SelectedItems[0].SubItems[1].Text);//Adsoyad
                secilenkayit.Add(listView1.SelectedItems[0].SubItems[2].Text);//Eposta
                secilenkayit.Add(listView1.SelectedItems[0].SubItems[3].Text);//Sehir
                secilenkayit.Add(listView1.SelectedItems[0].SubItems[4].Text);//Adres
                secilenkayit.Add(listView1.SelectedItems[0].SubItems[5].Text);//Ktarihi
            }
            catch
            {

            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //Kayıt silme
            if (idno == 0)
            {
                toolStripStatusLabel1.Text = "Silme işlemi için bir kayıt seçmelisiniz.";
                return;
            }
            int idno2 = idno;//Genel değişkeni yerel değişkene aktar
            DialogResult cevap;
            cevap = MessageBox.Show(idno2 + " numaralı kayıt silinsin mi?", "Kayıt silme", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (cevap == DialogResult.Yes)
            {
                string sql = "Delete from rehber Where Id=@Idno";
                int silinen = 0;
                try
                {
                    SqlCommand komut = new SqlCommand(sql, baglanti);
                    komut.Parameters.AddWithValue("@Idno", idno2);
                    silinen = komut.ExecuteNonQuery();
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Silme işleminde hata oluştu. Hata:" + hata.Message);
                    return;
                }
                toolStripStatusLabel1.Text = silinen.ToString() + " adet kayıt silindi.";
                idno = 0;
                Listele("Select * from rehber Order by Adsoyad");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //Seçilen Kaydı yazdır
            if (idno != 0)
            {
                try
                {
                    printDialog1.Document = printDocument1;//Yazdırma işlemi bağlantısını ayarlar
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        printDocument1.Print();//Yazdırmayı başlatır
                    }
                }
                catch (Exception hata)
                {
                    toolStripStatusLabel1.Text = "Yazdırma işlemi sırasında bir hata oluştu. Hata:" + hata.Message;
                    return;
                }
                idno = 0;

            }
            else
            {
                toolStripStatusLabel1.Text = "Yazdırma işlemi için bir kayıt seçmelisiniz.";
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Yazdırma işleminin yapıldığı metot.
            string yazilan = "";
            try
            {
                yazilan = "Adres Defteri Bilgileri\r\n" +
                    "Kimlik No :" + secilenkayit[0] +
                    "\r\nAdı Soyadı :" + secilenkayit[1] +
                    "\r\nE-Posta :" + secilenkayit[2] +
                    "\r\nŞehir :" + secilenkayit[3] +
                    "\r\nAdres :" + secilenkayit[4] +
                    "\r\nKayıt Tarihi :" + secilenkayit[5] +
                    "\r\nYazdırma Tarihi :" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                Font yazicifontu = new Font("Times", 18);
                SolidBrush firca = new SolidBrush(Color.Blue);
                e.Graphics.DrawLine(new Pen(Color.Black), 10, 50, 250, 50);//Yatay çizgi çizer
                e.Graphics.DrawString(yazilan, yazicifontu, firca, 10, 20, new StringFormat());
            }
            catch (Exception hata)
            {
                toolStripStatusLabel1.Text = "Yazdırma işlemi sırasında bir hata oluştu. Hata:" + hata.Message;
            }//try
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //Yazıcı önizleme penceresini açar
            if (idno != 0)
            {
                PrintPreviewDialog ppdialog1 = new PrintPreviewDialog(); // Yazıcı önizleme nesnesini oluştur.  
                ppdialog1.Document = printDocument1;//Yazdırma bağlantısını seç
                ppdialog1.ShowDialog(); // Pencereyi göster
                idno = 0;
            }
            else
            {
                toolStripStatusLabel1.Text = "Yazdırma önizleme işlemi için bir kayıt seçmelisiniz.";
            }
        }

        private bool Tablomevcutmu(string tabloadi, string baglanticumlesi)
        {
            //Verilen bağlantıda tablo mevcut olup olmadığını döndürür
            string komut = $"select * from sys.tables";
            SqlConnection bag = new SqlConnection(baglanticumlesi);
            bag.Open();
            SqlCommand com = new SqlCommand(komut, bag);
            SqlDataReader okuyucu = com.ExecuteReader();
            while (okuyucu.Read())
            {
                if (okuyucu.GetString(0).ToLower() == tabloadi.ToLower())
                    return true;
            }
            okuyucu.Close();
            return false;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            //Csv (Virgülle ayrılmış değerler) listesi olarak Excel çıktısı ver
            string dosyaadi = @"Rehberkayitlar.csv"; // Dosyanın kaydedileceği yol, çalıştığı klasördür.
            StreamWriter dosyayaz = new StreamWriter(dosyaadi, false, System.Text.Encoding.UTF8);
            dosyayaz.WriteLine("Kimlik;Adı Soyadı;E-Posta;Şehir;Adres;Kayıt Tarihi");
            string sql = "Select * from rehber";
            SqlCommand komut = new SqlCommand(sql, baglanti);
            //Kayıtları okuma
            SqlDataReader oku = null;
            oku = komut.ExecuteReader();
            //Döngü yap
            while (oku.Read())
            {
                dosyayaz.WriteLine(oku["Id"].ToString() + ";" + oku["Adsoyad"].ToString() + ";" +
                    oku["Eposta"].ToString() + ";" + oku["Sehir"].ToString() + ";" +
                    oku["Adres"].ToString() + ";" + oku["Ktarihi"].ToString());
            }
            oku.Close();
            dosyayaz.Close();
            MessageBox.Show("Tüm Kayıtlar, Excel Csv olarak klasöre kaydedildi." + "\nDosya:" + Application.StartupPath + "\\" + dosyaadi, "Excel Çıktısı", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }
    }
}

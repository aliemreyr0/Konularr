using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Policy;

/*
Mustafa OF / Kocaeli
-MSSSQL Localdb veri tabanı kullanıldı. Veri tabanı adı : Veriler1 (Localdb veri tabanını görmek için Görünüm>Sql Server Object Explorer menüsü seçilmelidir.)
Eğer Localdb vt sunucusu gözükmüyorsa Araçlar>Araçlar ve özellikleri edinin... menüsü kullanılır. Gelen ekrandan "Değiştir" seçilir. 
"Bağımsız bileşenler" den arama alanına "Localdb" yazılır. Bulunursa işaretlenir ve yüklemek için "Yükle" düğmesi seçilir.
-Nuget paket yöneticisinden (Proje>Nuget paketlerini yönet menüsü) "System.Data.SqlClient" aranarak yüklenmelidir.
-Veri tabanına ait bağlantı cümlesi aşağıdaki "Baglan" metodu içerisinde güncellenmelidir. Örnek veri tabanı adı olarak "Veriler1" kullanılmıştır.
-Kitaplar (Ana tablo) ve Kitaplar_hareket (Ayrıntı tablosu) tabloları uygulama çalıştığında oluşturulacaktır

Verileri listelerken iki tablo arasında inner join ile veri döndürmek için şu Sql satırı kullanılabilir
select * from Kitaplar inner join Kitaplar_hareket on Kitaplar.id=Kitaplar_hareket.Kitapid;
*/

namespace Kitaplar_adap
{
    public partial class Anaform : Form
    {
        public Anaform()
        {
            InitializeComponent();
        }

        //Genel tanımlar
        public static SqlConnection baglanti;
        SqlDataAdapter adap1;
        DataSet ds;//Veri kümesi
        SqlCommandBuilder komut;//Komut hazırlayıcı
        int idno = 0;//Ana kitaplara hareket bilgisini bağlamak için kitaplar tablosundaki id alanı
        int idnohareket;//kitaplar_hareket tablosu için id numarası
        string sirayonu = "ASC";//Listeleme işleminde kolonlara göre sıralama yaparken kullanılacak sıra yönü
        Kitapbilgisi kitapblg = new Kitapbilgisi();//Class tip
        string idnoyazici = "";

        private void Anaform_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            textBox1.Focus();
            //İpucu metni yazılır
            toolTip1.SetToolTip(textBox1, "Kayıt silmek için satırı işaretleyip Delete tuşuna basınız. Sonra Kaydet düğmesini tıklayınız.");
            Baglan();
            Tablolariolustur();
            Gridlistele("Select * from kitaplar Order By Kitapadi");
        }

        private void Tablolariolustur()
        {
            //Veri tabanında tabloları oluştur
            //Tablo yoksa yeniden oluştur
            //Kitaplar tablosu
            string sql = "IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Kitaplar' and xtype='U')" +
                " CREATE TABLE Kitaplar (Id int IDENTITY(1, 1) NOT NULL primary key, Kitapadi nvarchar(50) null," +
                " Yazari nvarchar(25) null, Yayinevi nvarchar(50) null, Tarihsaat datetime default CURRENT_TIMESTAMP);";
            SqlCommand komut = new SqlCommand(sql, baglanti);
            komut.ExecuteNonQuery();
            //Kitaplar_hareket tablosunu oluştur
            string sql2 = "IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Kitaplar_hareket' and xtype='U')" +
                " CREATE TABLE Kitaplar_hareket (Id int IDENTITY(1, 1) NOT NULL primary key, Kitapid int null, Islemturu nvarchar(10) null," +
                " Fiyati Numeric(18) null, Adet int null, Aciklama nvarchar(255) null, Kayittarihi datetime default CURRENT_TIMESTAMP);";
            SqlCommand komut2 = new SqlCommand(sql2, baglanti);
            komut2.ExecuteNonQuery();
        }

        private void Gridlistele(string sql)
        {
            //Grid'e verileri listeler
            adap1 = new SqlDataAdapter(sql, baglanti);//Adaptörü hazırla
            ds = new DataSet();//Dataset'i oluştur.
            adap1.Fill(ds, "Kitaplar");//Kitaplar tablosu ile ds'yi doldur
            dataGridView1.DataSource = ds;//Veri kaynağını aktar
            dataGridView1.DataMember = "Kitaplar";//Tablo ismini aktar
            //Kolon başlıklarının ayarlanması
            dataGridView1.Columns[0].HeaderText = "Kimlik";
            dataGridView1.Columns[0].ReadOnly = true;//Id numarasının değiştirilmesi engellenir.
            dataGridView1.Columns[1].HeaderText = "Kitap Adı";
            dataGridView1.Columns[2].HeaderText = "Yazarı";
            dataGridView1.Columns[3].HeaderText = "Yayın Evi";
            dataGridView1.Columns[4].HeaderText = "Kayıt Tarihi";
            //Kolonların genişlikleri
            dataGridView1.Columns[0].Width = 40;//Id kolonunun genişliği
            dataGridView1.Columns[1].Width = 140;//Kitapadi kolonunun genişliği
            dataGridView1.Columns[2].Width = 90;//Yazari kolonunun genişliği
            dataGridView1.Columns[3].Width = 140;//Yayinevi kolonunun genişliği
            dataGridView1.Columns[4].Width = 140;//Kayittarihi kolonunun genişliği
        }

        private void Baglan()
        {
            //Bağlantı yap
            string bcumle = @"Server=(localdb)\MSSQLLocalDB;Database=Veriler1";
            baglanti = new SqlConnection(bcumle);
            baglanti.Open();//Bağlantıyı aç
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Kitap adına göre ara
            Gridlistele("Select * from kitaplar Where Kitapadi Like '%" +
                textBox1.Text.Trim() + "%'");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Kaydet düğmesi
            Kaydet();
        }

        private void Kaydet()
        {
            //Kaydet metodu           
            DialogResult cevap;
            cevap = MessageBox.Show("Kayıt edilsin mi ?", "Kayıt", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            //Eğer cevap evet ise
            if (cevap == DialogResult.Yes)
            {
                try
                {
                    komut = new SqlCommandBuilder(adap1);//Adapter'dan bilgiler alınarak Sql komutlarını oluştur.
                    adap1.Update(ds, "Kitaplar");//Güncelle
                }//try
                catch (Exception hata)
                {
                    toolStripStatusLabel1.Text = "Kayıt edilemedi. Hata: " + hata.Message;
                    return;
                }
                toolStripStatusLabel1.Text = "Veriler Kayıt Edildi";
                Gridlistele("Select * from Kitaplar Order By Kitapadi");//Güncell bilgileri listele
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                //Geçerli kaydın bilgisi elde edilir
                int satirno = dataGridView1.CurrentCell.RowIndex;
                //int kolonno = dataGridView1.CurrentCell.ColumnIndex;//Hangi alana ait olduğu?
                //İlk kolondaki seçilen satırın değeri
                idno = Int32.Parse(dataGridView1.Rows[satirno].Cells[0].Value.ToString());//Seçilen kitabın id numarasını al
                idnoyazici = dataGridView1.Rows[satirno].Cells[0].Value.ToString();
                //Kitap bilgilerini nesneye aktar
                kitapblg.Id = idno;
                kitapblg.Kitapadi = dataGridView1.Rows[satirno].Cells[1].Value.ToString();
                kitapblg.Yazari = dataGridView1.Rows[satirno].Cells[2].Value.ToString();
                kitapblg.Yayinevi = dataGridView1.Rows[satirno].Cells[3].Value.ToString();
                kitapblg.Tarihsaat = dataGridView1.Rows[satirno].Cells[4].Value.ToString();
                if (idno != 0)
                {
                    //Hareket tablosunu listele
                    Listele("select * from Kitaplar_hareket where kitapid=" + idno);
                }
            }
            catch
            {

            }
        }

        private void Listele(string sql)
        {
            listView1.Items.Clear();//İçeriği temizle
            SqlCommand komut = new SqlCommand(sql, baglanti);
            //Kayıtları okuma
            SqlDataReader oku = null;
            oku = komut.ExecuteReader();
            //Döngü yap
            while (oku.Read())
            {
                ListViewItem lw1 = new ListViewItem(oku["Kitapid"].ToString());//kitaplar_hareket isimli tablonun alanları
                lw1.Tag = oku["Id"].ToString();//Hareket tablosunda ilave olarak Tag alanına Id eklenir. Düzeltme ve silme için gereklidir.
                lw1.SubItems.Add(oku["Islemturu"].ToString());
                lw1.SubItems.Add(oku["Fiyati"].ToString());
                lw1.SubItems.Add(oku["Adet"].ToString());
                lw1.SubItems.Add(oku["Aciklama"].ToString());
                lw1.SubItems.Add(oku["Kayittarihi"].ToString());
                listView1.Items.Add(lw1);
            }
            oku.Close();
        }

        private void Anaform_FormClosing(object sender, FormClosingEventArgs e)
        {
            baglanti.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Yeni kayıt formunu aç
            if (idno != 0)
            {
                Hareketformu hformu = new Hareketformu(idno);//idno değişkeni Hareketformu isimli Class'a aktarılır.
                hformu.StartPosition = FormStartPosition.CenterScreen;//Ekranın ortasında gözükür.
                hformu.ShowDialog();
            } else
            {
                toolStripStatusLabel1.Text = "Lütfen bir kayıt seçiniz";
            }
        }

        private void Anaform_Activated(object sender, EventArgs e)
        {
            //Form her etkinleştirildiğinde çalışan olaydır.
            Listele("select * from Kitaplar_hareket where kitapid=" + idno);//kitaplar_hareket tablosunu listele
            idno = 0;
            idnohareket = 0;
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //Kolon başlıklarına tıklandığında çalışır.
            //Alan isimleri
            string[] alanlar = { "Kitapid", "Islemturu", "Fiyati", "Adet", "Aciklama", "Kayittarihi" };
            if (sirayonu == "ASC")
            {
                Listele("select * from Kitaplar_hareket where kitapid=" + idno + " Order By " + alanlar[e.Column] + " " + sirayonu);
                sirayonu = "DESC";
            }
            else
            {
                Listele("select * from Kitaplar_hareket where kitapid=" + idno + " Order By " + alanlar[e.Column] + " " + sirayonu);
                sirayonu = "ASC";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Hareket tablosu kayıtlarını düzeltme
            if (idnohareket != 0)
            {
                toolStripStatusLabel1.Text = "";
                Duzeltmeformu dformu = new Duzeltmeformu(idnohareket);
                dformu.StartPosition = FormStartPosition.CenterScreen;
                dformu.ShowDialog();
            } else
            {
                toolStripStatusLabel1.Text = "Düzeltmek için bir hareket kaydı seçmelisiniz.";
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                idnohareket = Int32.Parse(listView1.SelectedItems[0].Tag.ToString());//Seçilen kaydın Id nosu
            }
            catch
            {

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Hareket tablosu kayıtlarını silme
            if (idnohareket != 0)
            {
                int idnosilinen = idnohareket;
                toolStripStatusLabel1.Text = "";
                DialogResult cevap;
                cevap = MessageBox.Show(idnohareket + " nolu hareket kaydı silinsin mi?", "Kayıt silme", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (cevap == DialogResult.Yes)
                {
                    string sql = "Delete from Kitaplar_hareket Where Id=@Idno";
                    int silinen = 0;
                    try
                    {
                        SqlCommand komut = new SqlCommand(sql, baglanti);
                        komut.Parameters.AddWithValue("@Idno", idnosilinen);
                        silinen = komut.ExecuteNonQuery();
                    }
                    catch (Exception hata)
                    {
                        MessageBox.Show("Silme işleminde hata oluştu. Hata:" + hata.Message);
                        return;
                    }
                    toolStripStatusLabel1.Text = silinen.ToString() + " adet hareket kaydı silindi.";
                    Listele("select * from Kitaplar_hareket where kitapid=" + idno);
                    idnohareket = 0;
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Silmek için bir hareket kaydı seçmelisiniz.";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Kitap ve hareket bilgileri yazdırır.
            if (idno != 0)
            {
                DialogResult cevap;
                cevap = MessageBox.Show(idno + " nolu kitabın bilgilerini yazdırmak istiyor musunuz?", "Yazdırma", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (cevap == DialogResult.Yes)
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
                }//if
            }
            else
            {
                toolStripStatusLabel1.Text = "Yazdırma işlemi için bir kitap seçmelisiniz.";
            }

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Yazdırma işleminin yapıldığı metot.            
            string kitap_yazilan = "";
            string hareket_yazilan = "";            
            try
            {
                kitap_yazilan = "Kitap Bilgileri\r\n" +
                    "Kimlik No :" + kitapblg.Id +
                    "\r\nKitap Adı :" + kitapblg.Kitapadi +
                    "\r\nYazarı :" + kitapblg.Yazari +
                    "\r\nYayınevi :" + kitapblg.Yayinevi +
                    "\r\nKayıt Tarihi :" + kitapblg.Tarihsaat +                    
                    "\r\nYazdırma Tarihi :" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                //Hareket bilgileri
                string ayrac = new string(' ', 10);
                hareket_yazilan = "\r\n\r\nKitap Hareketleri\r\n";
                hareket_yazilan += $"İşlem Türü{ayrac}Fiyat{ayrac}Adet{ayrac}\r\n";
                string sql = $"Select * from Kitaplar_hareket where Kitapid={idnoyazici} order by Islemturu;";
                SqlCommand komut = new SqlCommand(sql, baglanti);
                SqlDataReader oku = null;
                oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    hareket_yazilan += string.Format("{0,-24} {1,-13}   {1,-5}\r\n", oku["Islemturu"], oku["Fiyati"], oku["Adet"]);                    
                }//while
                oku.Close();
                var yazdirmabicimi = new StringFormat();//Parantez içerisinde StringFormatFlags ile yazdırma yönü vb. bilgiler ayarlanabilir
                Font yazicifontu = new Font("Times", 18);
                SolidBrush firca = new SolidBrush(Color.Black);
                e.Graphics.DrawLine(new Pen(Color.Black), 10, 50, 250, 50);//Yatay çizgi çizer
                e.Graphics.DrawLine(new Pen(Color.Black), 10, 305, 550, 305);//Yatay çizgi çizer
                e.Graphics.DrawString(kitap_yazilan + hareket_yazilan, yazicifontu, firca, 10, 20, yazdirmabicimi);
            }
            catch (Exception hata)
            {
                toolStripStatusLabel1.Text = "Yazdırma işlemi sırasında bir hata oluştu. Hata:" + hata.Message;
            }//try
        }
    }

    public class Kitapbilgisi
    {
        //Kitap tablosundaki verileri tutan class tipidir
        public int Id { get; set; }
        public string Kitapadi { get; set; }
        public string Yazari { get; set; }
        public string Yayinevi { get; set; }
        public string Tarihsaat { get; set; }
    }
}

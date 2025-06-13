using KisilerEf.Models;
using Microsoft.Office.Interop.Excel;//Projede Ekle>Com Baþvurusu>Excel eklenmelidir.
using System.Text;

namespace KisilerEf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /*
Entity Framework nedir ?
ORM teknolojisidir.
Object Relational Mapping:
Nyp programlama dilleri iliþkisel veri tabaný yönetim sistemleri arasýnda köprü kuran bir tekniktir.
Orm sayesinde veri tabanýndaki tablolarla direk uðraþmadan Sql kodu yazmadan etkileþim kurulabilir. Bunun yerine veri tabaný iþlemleri nesneler ve sýnýflar aracýlýðýyla yapýlýrlar.
ORm ne iþe yarar?
•	Sql sorgusu yazmadan vt iþlemleri yapýlabilir. (Veri ekleme, güncelleme, silme, sorgulama vs.)
•	Vt baðýmlýlýðýný azaltýr. 
•	Veri modellemesi nesneler üzerinden yapýlýr. NYP prensiplerine uygun bir durumdur.
Ef’in Core Temel özellikler:
1.	CodeFirst: Önce C# sýnýflarýný yazýlýr sonra veri tabaný otomatik olarak oluþturulur.
2.	Database First: Mevcut veritabanýndan sýnýflarý otomatik oluþturma
3.	Linq Desteði: Vt sorgularýný C# Linq kodlarý ile yazýlabilir.
4.	Migration: Vt þemasýný kod ile yönetme
Basit bir Ef Core için neler gerekli?
•	.Net 6/7/8 Sdk
•	Visual Studio veya Vs Code
•	Sql Server (LocalDb kafidir) veri tabaný
•	Nuget Paketleri
    o	Microsoft.EntityFrameworkCore
    o	Microsoft.EntityFrameworkCoreSqlServer
    o	Microsoft.EntityFrameworkCoreTools
Ef Core için gerekli olan kavramlar:
1.	DbContext
    a.	Veri tabaný baðlantýlarýný yönetir
    b.	SaveChanges() ile deðiþiklikleri veri tabanýna aktarýr.
2.	DbSet<T> 
    a.	Veri tabanýndaki tablolarý temsil eder
    b.	Linq için bir giriþ noktasýdýr.
3.	Migration
    a.	Veritabaný þemasýndaki deðiþiklikleri kod ile yürütür.
4.	Linq to Entities
    a.	C# kodu ile Sql sorgularý yazabilme
    b.	Tip güvenli sorgular yapabilme  
        
Bu proje için .Net 8 kullanýldý. Nuget paketleri olarak 
        Microsoft.EntityFrameworkCoreSqlServer ve Microsoft.EntityFrameworkCoreTools paketleri yüklendi       
        1. Kisi Class'ýný oluþturun. Kodlar ilgili sýnýftadýr. (Ekle>Sýnýf)
        2. VeritabaniContext sýnýfýný oluþturun. Kodlar ilgili sýnýftadýr.(Ekle>Sýnýf)
        Veri tabaný baðlantý cümlesi doðru olarak verilir.
        3. Araçlar>Nuget Paket Yöneticisi>Paket Yöneticisi Konsolü ne geçilir.
        Komut satýrýnda iken þu komutlar çalýþtýrýlýr.
        Add-Migration IlkOlusturma (Eðer hata verirse Remove-Migration ile kaldýrýlýr)
        Update-Database
        */

        private void button1_Click(object sender, EventArgs e)
        {
            //Yeni Kayýt ekler
            //Boþluk kontrolü
            if (Metinlerbosmu())
            {
                MessageBox.Show("Tüm alanlar dolu olmalýdýr.");
                textBox1.Focus();
                return;
            }
            int girilenyas = 0;
            using (var veritabani = new VeritabaniContext())
            {
                //Eðer tüm alanlar dolu ise yeni bir kiþi ekleyelim.
                try
                {
                    girilenyas = Convert.ToInt32(textBox3.Text.Trim());
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Yaþ alanýna sayý girilmelidir. Hata:" + hata.Message);
                    return;
                }
                var yenikisi = new Kisi
                {
                    Adsoyad = textBox1.Text.Trim(),
                    Eposta = textBox2.Text.Trim(),
                    Yas = girilenyas
                };
                veritabani.Kisiler.Add(yenikisi);//Yeni bir kayýt ekler
                try
                {
                    veritabani.SaveChanges();//Deðiþiklikleri veri tabanýna kaydeder.
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Hata oluþtu. Hata:" + hata.Message);
                    return;
                }
                MessageBox.Show("Kayýt Eklendi");
                Metinleribosalt();
                Listele("");
            }
        }

        private void Metinleribosalt()
        {
            //Metin alanlarýný boþalt
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox1.Focus();
        }

        private bool Metinlerbosmu()
        {
            //Metin alanlarýný boþ olup olmadýðýný kontrol eder.
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                //Eðer boþluða eþitse
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ListView ayarlarý
            listView1.View = View.Details;
            listView1.FullRowSelect = true;//Tüm satýr seçilebilsin
            listView1.GridLines = true;//Satýrlar arasý çizgiler görünsün
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Columns.Clear(); // Eðer varsa eski kolonlarý temizle
            // Kolon baþlýklarýný ekleme
            listView1.Columns.Add("Kimlik", 50);   // Kolon geniþlið: 50 piksel
            listView1.Columns.Add("Adý Soyadý", 120);
            listView1.Columns.Add("Eposta", 120);
            listView1.Columns.Add("Yaþ", 50);

            //Tüm kayýtlarý listeler
            Listele("");
        }

        private void Listele(string arananAdSoyad)
        {
            //Kayýtlarý listeler ve adsoyad alanýnda arama yapar.
            listView1.Items.Clear(); // Eski verileri temizle

            using (var db = new VeritabaniContext())
            {
                // Adsoyad içeren kayýtlarý getir (Linq kullanarak)
                var kisiler = db.Kisiler
                                .Where(k => k.Adsoyad.Contains(arananAdSoyad))
                                .ToList();

                // ListView'e her bir kiþiyi ekle
                foreach (var kisi in kisiler)
                {
                    var satir = new ListViewItem(kisi.Id.ToString()); // Id'yi ekle
                    satir.SubItems.Add(kisi.Adsoyad);                // Adsoyad'ý ekle
                    satir.SubItems.Add(kisi.Eposta);                 // Eposta'yý ekle
                    satir.SubItems.Add(kisi.Yas.ToString());         // Yaþ'ý ekle
                    listView1.Items.Add(satir); // Satýrý ListView'e ekle
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //Adý soyadýný tabloda arar
            Listele(textBox4.Text.Trim());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Seçilen kaydý siler
            // Seçilen satýrý kontrol et
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bir kayýt seçiniz", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Eðer hiç seçim yapýlmadýysa, iþlemi durdur
            }
            // Seçilen satýrdaki Id'yi al
            int seciliKisiId = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            // Onay penceresini göster
            var sonuc = MessageBox.Show("Bu kayýt silinecek. Onaylýyor musunuz?", "Silme Onayý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc == DialogResult.Yes)
            {
                // Veritabanýndan kaydý sil
                using (var db = new VeritabaniContext())
                {
                    var kisiSil = db.Kisiler.FirstOrDefault(k => k.Id == seciliKisiId);
                    if (kisiSil != null)
                    {
                        db.Kisiler.Remove(kisiSil); // Kaydý sil
                        db.SaveChanges(); // Deðiþiklikleri kaydet

                        // ListView'den kaydý sil
                        listView1.Items.Remove(listView1.SelectedItems[0]);
                        MessageBox.Show("Kayýt baþarýyla silindi.", "Baþarýlý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }//silme
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Satýrlara týklandýðýnda veriler textBox'lara aktarýlýr.
            // Eðer Seçili satýr varsa
            if (listView1.SelectedItems.Count > 0)
            {
                // Seçilen satýrdaki Id'yi al
                var seciliSatir = listView1.SelectedItems[0];
                // Id'yi al (ilk kolon)
                int seciliKisiId = int.Parse(seciliSatir.SubItems[0].Text);
                using (var db = new VeritabaniContext())
                {
                    var kisi = db.Kisiler.FirstOrDefault(k => k.Id == seciliKisiId);
                    if (kisi != null)
                    {
                        // Seçilen kiþinin verilerini TextBox'lara aktar
                        textBox1.Text = kisi.Adsoyad;
                        textBox2.Text = kisi.Eposta;
                        textBox3.Text = kisi.Yas.ToString();
                    }
                }
            }
            else
            {
                // Hiçbir þey seçilmediyse TextBox'larý temizle
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // ListView'den herhangi bir satýr seçilip seçilmediðini kontrol et
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bir kayýt seçiniz", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Eðer seçim yapýlmamýþsa iþlemi durdur
            }
            // Seçilen satýrdaki Id'yi al
            int seciliKisiId = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            // Onay penceresini göster
            var sonuc = MessageBox.Show("Bu kaydý güncellemek istediðinizden emin misiniz?", "Güncelleme Onayý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc == DialogResult.Yes)
            {
                // Veritabanýný güncelle
                using (var db = new VeritabaniContext())
                {
                    var kisi = db.Kisiler.FirstOrDefault(k => k.Id == seciliKisiId);
                    if (kisi != null)
                    {
                        // TextBox'lardaki verileri veritabanýna aktar
                        kisi.Adsoyad = textBox1.Text;
                        kisi.Eposta = textBox2.Text;
                        kisi.Yas = int.Parse(textBox3.Text);
                        db.SaveChanges(); // Deðiþiklikleri kaydet
                        // ListView'i güncelle
                        Listele(""); // Ya da belirli bir filtreyle güncelleyebilirsin
                        MessageBox.Show("Kayýt baþarýyla güncellendi.", "Baþarýlý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Metinleribosalt();
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // SaveFileDialog ile dosya kaydetme penceresini aç
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Dosyasý (*.xls)|*.xls"; // Dosya formatý XLS olacak
            saveFileDialog.DefaultExt = "xls"; // Varsayýlan uzantý xls
            saveFileDialog.Title = "Excel Olarak Kaydet";
            // Kullanýcý eðer bir dosya seçtiyse
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {                
                string dosyaYolu = saveFileDialog.FileName;// Dosya yolunu al
                // Excel Uygulamasýný baþlat
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = true;
                // Yeni bir çalýþma kitabý (workbook) oluþtur
                Workbook calismakitabi = excelApp.Workbooks.Add();
                Worksheet worksheet = (Worksheet) calismakitabi.Sheets[1]; // Çalýþma sayfasýný al
                //Kolon Baþlýklarýný yaz
                worksheet.Cells[1, 1] = "Kimlik";
                worksheet.Cells[1, 2] = "Adý Soyadý";
                worksheet.Cells[1, 3] = "Eposta";
                worksheet.Cells[1, 4] = "Yaþ";
                // Baþlýk hücrelerini koyu (bold) yap
                Microsoft.Office.Interop.Excel.Range baslikAraligi = worksheet.Range["A1", "D1"];  // A1'den D1'e kadar olan hücre aralýðýný al
                baslikAraligi.Font.Bold = true;
                // ListView'deki verileri Excel dosyasýna aktar
                int satirIndex = 2; // Baþlýk satýrýndan sonra baþlamalýyýz (1. satýr baþlýktýr)
                foreach (ListViewItem item in listView1.Items)
                {
                    worksheet.Cells[satirIndex, 1] = item.SubItems[0].Text;  // Kimlik
                    worksheet.Cells[satirIndex, 2] = item.SubItems[1].Text;  // Adý Soyadý
                    worksheet.Cells[satirIndex, 3] = item.SubItems[2].Text;  // Eposta
                    worksheet.Cells[satirIndex, 4] = item.SubItems[3].Text;  // Yaþ
                    satirIndex++; // Bir sonraki satýra geç
                }
                // Dosyayý kaydet
                calismakitabi.SaveAs(dosyaYolu, XlFileFormat.xlWorkbookNormal); // .xls formatýnda kaydet
                //calismakitabi.Close(); // Ýstenirse çalýþma kitabý kapatýlabilir.
                MessageBox.Show("Veriler baþarýyla Excel dosyasýna aktarýldý.", "Ýþlem Baþarýlý", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

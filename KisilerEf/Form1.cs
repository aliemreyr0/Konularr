using KisilerEf.Models;
using Microsoft.Office.Interop.Excel;//Projede Ekle>Com Ba�vurusu>Excel eklenmelidir.
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
Nyp programlama dilleri ili�kisel veri taban� y�netim sistemleri aras�nda k�pr� kuran bir tekniktir.
Orm sayesinde veri taban�ndaki tablolarla direk u�ra�madan Sql kodu yazmadan etkile�im kurulabilir. Bunun yerine veri taban� i�lemleri nesneler ve s�n�flar arac�l���yla yap�l�rlar.
ORm ne i�e yarar?
�	Sql sorgusu yazmadan vt i�lemleri yap�labilir. (Veri ekleme, g�ncelleme, silme, sorgulama vs.)
�	Vt ba��ml�l���n� azalt�r. 
�	Veri modellemesi nesneler �zerinden yap�l�r. NYP prensiplerine uygun bir durumdur.
Ef�in Core Temel �zellikler:
1.	CodeFirst: �nce C# s�n�flar�n� yaz�l�r sonra veri taban� otomatik olarak olu�turulur.
2.	Database First: Mevcut veritaban�ndan s�n�flar� otomatik olu�turma
3.	Linq Deste�i: Vt sorgular�n� C# Linq kodlar� ile yaz�labilir.
4.	Migration: Vt �emas�n� kod ile y�netme
Basit bir Ef Core i�in neler gerekli?
�	.Net 6/7/8 Sdk
�	Visual Studio veya Vs Code
�	Sql Server (LocalDb kafidir) veri taban�
�	Nuget Paketleri
    o	Microsoft.EntityFrameworkCore
    o	Microsoft.EntityFrameworkCoreSqlServer
    o	Microsoft.EntityFrameworkCoreTools
Ef Core i�in gerekli olan kavramlar:
1.	DbContext
    a.	Veri taban� ba�lant�lar�n� y�netir
    b.	SaveChanges() ile de�i�iklikleri veri taban�na aktar�r.
2.	DbSet<T> 
    a.	Veri taban�ndaki tablolar� temsil eder
    b.	Linq i�in bir giri� noktas�d�r.
3.	Migration
    a.	Veritaban� �emas�ndaki de�i�iklikleri kod ile y�r�t�r.
4.	Linq to Entities
    a.	C# kodu ile Sql sorgular� yazabilme
    b.	Tip g�venli sorgular yapabilme  
        
Bu proje i�in .Net 8 kullan�ld�. Nuget paketleri olarak 
        Microsoft.EntityFrameworkCoreSqlServer ve Microsoft.EntityFrameworkCoreTools paketleri y�klendi       
        1. Kisi Class'�n� olu�turun. Kodlar ilgili s�n�ftad�r. (Ekle>S�n�f)
        2. VeritabaniContext s�n�f�n� olu�turun. Kodlar ilgili s�n�ftad�r.(Ekle>S�n�f)
        Veri taban� ba�lant� c�mlesi do�ru olarak verilir.
        3. Ara�lar>Nuget Paket Y�neticisi>Paket Y�neticisi Konsol� ne ge�ilir.
        Komut sat�r�nda iken �u komutlar �al��t�r�l�r.
        Add-Migration IlkOlusturma (E�er hata verirse Remove-Migration ile kald�r�l�r)
        Update-Database
        */

        private void button1_Click(object sender, EventArgs e)
        {
            //Yeni Kay�t ekler
            //Bo�luk kontrol�
            if (Metinlerbosmu())
            {
                MessageBox.Show("T�m alanlar dolu olmal�d�r.");
                textBox1.Focus();
                return;
            }
            int girilenyas = 0;
            using (var veritabani = new VeritabaniContext())
            {
                //E�er t�m alanlar dolu ise yeni bir ki�i ekleyelim.
                try
                {
                    girilenyas = Convert.ToInt32(textBox3.Text.Trim());
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Ya� alan�na say� girilmelidir. Hata:" + hata.Message);
                    return;
                }
                var yenikisi = new Kisi
                {
                    Adsoyad = textBox1.Text.Trim(),
                    Eposta = textBox2.Text.Trim(),
                    Yas = girilenyas
                };
                veritabani.Kisiler.Add(yenikisi);//Yeni bir kay�t ekler
                try
                {
                    veritabani.SaveChanges();//De�i�iklikleri veri taban�na kaydeder.
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Hata olu�tu. Hata:" + hata.Message);
                    return;
                }
                MessageBox.Show("Kay�t Eklendi");
                Metinleribosalt();
                Listele("");
            }
        }

        private void Metinleribosalt()
        {
            //Metin alanlar�n� bo�alt
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox1.Focus();
        }

        private bool Metinlerbosmu()
        {
            //Metin alanlar�n� bo� olup olmad���n� kontrol eder.
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                //E�er bo�lu�a e�itse
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ListView ayarlar�
            listView1.View = View.Details;
            listView1.FullRowSelect = true;//T�m sat�r se�ilebilsin
            listView1.GridLines = true;//Sat�rlar aras� �izgiler g�r�ns�n
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Columns.Clear(); // E�er varsa eski kolonlar� temizle
            // Kolon ba�l�klar�n� ekleme
            listView1.Columns.Add("Kimlik", 50);   // Kolon geni�li�: 50 piksel
            listView1.Columns.Add("Ad� Soyad�", 120);
            listView1.Columns.Add("Eposta", 120);
            listView1.Columns.Add("Ya�", 50);

            //T�m kay�tlar� listeler
            Listele("");
        }

        private void Listele(string arananAdSoyad)
        {
            //Kay�tlar� listeler ve adsoyad alan�nda arama yapar.
            listView1.Items.Clear(); // Eski verileri temizle

            using (var db = new VeritabaniContext())
            {
                // Adsoyad i�eren kay�tlar� getir (Linq kullanarak)
                var kisiler = db.Kisiler
                                .Where(k => k.Adsoyad.Contains(arananAdSoyad))
                                .ToList();

                // ListView'e her bir ki�iyi ekle
                foreach (var kisi in kisiler)
                {
                    var satir = new ListViewItem(kisi.Id.ToString()); // Id'yi ekle
                    satir.SubItems.Add(kisi.Adsoyad);                // Adsoyad'� ekle
                    satir.SubItems.Add(kisi.Eposta);                 // Eposta'y� ekle
                    satir.SubItems.Add(kisi.Yas.ToString());         // Ya�'� ekle
                    listView1.Items.Add(satir); // Sat�r� ListView'e ekle
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //Ad� soyad�n� tabloda arar
            Listele(textBox4.Text.Trim());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Se�ilen kayd� siler
            // Se�ilen sat�r� kontrol et
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bir kay�t se�iniz", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // E�er hi� se�im yap�lmad�ysa, i�lemi durdur
            }
            // Se�ilen sat�rdaki Id'yi al
            int seciliKisiId = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            // Onay penceresini g�ster
            var sonuc = MessageBox.Show("Bu kay�t silinecek. Onayl�yor musunuz?", "Silme Onay�", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc == DialogResult.Yes)
            {
                // Veritaban�ndan kayd� sil
                using (var db = new VeritabaniContext())
                {
                    var kisiSil = db.Kisiler.FirstOrDefault(k => k.Id == seciliKisiId);
                    if (kisiSil != null)
                    {
                        db.Kisiler.Remove(kisiSil); // Kayd� sil
                        db.SaveChanges(); // De�i�iklikleri kaydet

                        // ListView'den kayd� sil
                        listView1.Items.Remove(listView1.SelectedItems[0]);
                        MessageBox.Show("Kay�t ba�ar�yla silindi.", "Ba�ar�l�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }//silme
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Sat�rlara t�kland���nda veriler textBox'lara aktar�l�r.
            // E�er Se�ili sat�r varsa
            if (listView1.SelectedItems.Count > 0)
            {
                // Se�ilen sat�rdaki Id'yi al
                var seciliSatir = listView1.SelectedItems[0];
                // Id'yi al (ilk kolon)
                int seciliKisiId = int.Parse(seciliSatir.SubItems[0].Text);
                using (var db = new VeritabaniContext())
                {
                    var kisi = db.Kisiler.FirstOrDefault(k => k.Id == seciliKisiId);
                    if (kisi != null)
                    {
                        // Se�ilen ki�inin verilerini TextBox'lara aktar
                        textBox1.Text = kisi.Adsoyad;
                        textBox2.Text = kisi.Eposta;
                        textBox3.Text = kisi.Yas.ToString();
                    }
                }
            }
            else
            {
                // Hi�bir �ey se�ilmediyse TextBox'lar� temizle
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // ListView'den herhangi bir sat�r se�ilip se�ilmedi�ini kontrol et
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bir kay�t se�iniz", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // E�er se�im yap�lmam��sa i�lemi durdur
            }
            // Se�ilen sat�rdaki Id'yi al
            int seciliKisiId = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            // Onay penceresini g�ster
            var sonuc = MessageBox.Show("Bu kayd� g�ncellemek istedi�inizden emin misiniz?", "G�ncelleme Onay�", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc == DialogResult.Yes)
            {
                // Veritaban�n� g�ncelle
                using (var db = new VeritabaniContext())
                {
                    var kisi = db.Kisiler.FirstOrDefault(k => k.Id == seciliKisiId);
                    if (kisi != null)
                    {
                        // TextBox'lardaki verileri veritaban�na aktar
                        kisi.Adsoyad = textBox1.Text;
                        kisi.Eposta = textBox2.Text;
                        kisi.Yas = int.Parse(textBox3.Text);
                        db.SaveChanges(); // De�i�iklikleri kaydet
                        // ListView'i g�ncelle
                        Listele(""); // Ya da belirli bir filtreyle g�ncelleyebilirsin
                        MessageBox.Show("Kay�t ba�ar�yla g�ncellendi.", "Ba�ar�l�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Metinleribosalt();
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // SaveFileDialog ile dosya kaydetme penceresini a�
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Dosyas� (*.xls)|*.xls"; // Dosya format� XLS olacak
            saveFileDialog.DefaultExt = "xls"; // Varsay�lan uzant� xls
            saveFileDialog.Title = "Excel Olarak Kaydet";
            // Kullan�c� e�er bir dosya se�tiyse
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {                
                string dosyaYolu = saveFileDialog.FileName;// Dosya yolunu al
                // Excel Uygulamas�n� ba�lat
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = true;
                // Yeni bir �al��ma kitab� (workbook) olu�tur
                Workbook calismakitabi = excelApp.Workbooks.Add();
                Worksheet worksheet = (Worksheet) calismakitabi.Sheets[1]; // �al��ma sayfas�n� al
                //Kolon Ba�l�klar�n� yaz
                worksheet.Cells[1, 1] = "Kimlik";
                worksheet.Cells[1, 2] = "Ad� Soyad�";
                worksheet.Cells[1, 3] = "Eposta";
                worksheet.Cells[1, 4] = "Ya�";
                // Ba�l�k h�crelerini koyu (bold) yap
                Microsoft.Office.Interop.Excel.Range baslikAraligi = worksheet.Range["A1", "D1"];  // A1'den D1'e kadar olan h�cre aral���n� al
                baslikAraligi.Font.Bold = true;
                // ListView'deki verileri Excel dosyas�na aktar
                int satirIndex = 2; // Ba�l�k sat�r�ndan sonra ba�lamal�y�z (1. sat�r ba�l�kt�r)
                foreach (ListViewItem item in listView1.Items)
                {
                    worksheet.Cells[satirIndex, 1] = item.SubItems[0].Text;  // Kimlik
                    worksheet.Cells[satirIndex, 2] = item.SubItems[1].Text;  // Ad� Soyad�
                    worksheet.Cells[satirIndex, 3] = item.SubItems[2].Text;  // Eposta
                    worksheet.Cells[satirIndex, 4] = item.SubItems[3].Text;  // Ya�
                    satirIndex++; // Bir sonraki sat�ra ge�
                }
                // Dosyay� kaydet
                calismakitabi.SaveAs(dosyaYolu, XlFileFormat.xlWorkbookNormal); // .xls format�nda kaydet
                //calismakitabi.Close(); // �stenirse �al��ma kitab� kapat�labilir.
                MessageBox.Show("Veriler ba�ar�yla Excel dosyas�na aktar�ld�.", "��lem Ba�ar�l�", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RehberLinqtoSql
{
    public partial class Listele : Form
    {
        int kontrolid = 0;//Silme/Düzeltme işlemlerinde kullanılacak kayıt id numarası
        string kontrolisim = "";//İşlemlerinde kullanılacak kayıt ismi
        string siraalanigenel = "Id";

        public Listele()
        {
            InitializeComponent();
        }

        private void Listele_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            toolTip1.SetToolTip(listView1, "Sağ tuş menüsü ile diğer işlemleri yapabilirsiniz");//İpucu nesnesini bir nesneye ata.
            Kayitlarilistele("", listView1,"Id");
        }

        private void Kayitlarilistele(string arananadsoyad, ListView listView1, string siraalani)
        {
            //Listeleme yapar
            //siraalani, şunları alabilir: "Id","Adsoyad","Eposta","Telno","Sehir","Adres","Ktarihi";
            listView1.Items.Clear();            
            //Linq to Sql nesnelerini oluştur
            RehberClasses1DataContext vtab = new RehberClasses1DataContext();
            //Linq satırı
            var kayitlar = from kayit in vtab.GetTable<rehber>() orderby kayit.Id where kayit.Adsoyad.Contains(arananadsoyad) select kayit;
            if (siraalani == "Id")//siraalani değişkenine göre kontrol et
            {
                kayitlar = from kayit in vtab.GetTable<rehber>() orderby kayit.Id where kayit.Adsoyad.Contains(arananadsoyad) select kayit;
            } else if (siraalani == "Adsoyad")
            {
                kayitlar = from kayit in vtab.GetTable<rehber>() orderby kayit.Adsoyad where kayit.Adsoyad.Contains(arananadsoyad) select kayit;
            } else if (siraalani == "Eposta")
            {
                kayitlar = from kayit in vtab.GetTable<rehber>() orderby kayit.Eposta where kayit.Adsoyad.Contains(arananadsoyad) select kayit;
            } else if (siraalani == "Telno")
            {
                kayitlar = from kayit in vtab.GetTable<rehber>() orderby kayit.Telno where kayit.Adsoyad.Contains(arananadsoyad) select kayit;
            } else if (siraalani == "Sehir")
            {
                kayitlar = from kayit in vtab.GetTable<rehber>() orderby kayit.Sehir where kayit.Adsoyad.Contains(arananadsoyad) select kayit;
            } else if (siraalani == "Adres")
            {
                kayitlar = from kayit in vtab.GetTable<rehber>() orderby kayit.Adres where kayit.Adsoyad.Contains(arananadsoyad) select kayit;
            } else if (siraalani == "Ktarihi")
            {
                kayitlar = from kayit in vtab.GetTable<rehber>() orderby kayit.Ktarihi where kayit.Adsoyad.Contains(arananadsoyad) select kayit;
            }
            int satir = 0;//Kaç adet listelenmiş
            foreach(var eleman in kayitlar) //Döngü yap
            {
                try
                { 
                ListViewItem lw1 = new ListViewItem(eleman.Id.ToString());
                lw1.SubItems.Add(eleman.Adsoyad.ToString());//Alanlar                
                lw1.SubItems.Add(eleman.Eposta.ToString());//Alanlar
                lw1.SubItems.Add(eleman.Telno.ToString());//Alanlar
                lw1.SubItems.Add(eleman.Sehir.ToString());//Alanlar
                lw1.SubItems.Add(eleman.Adres.ToString());//Alanlar
                lw1.SubItems.Add(eleman.Ktarihi.ToString());//Alanlar
                //Formdaki listview'e ekle
                listView1.Items.Add(lw1);
                }catch
                {

                }
                satir++;
            }//Döngü sonu
            toolStripStatusLabel1.Text = satir + " adet kayıt listelendi";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Adı soyadına göre arama
            Kayitlarilistele(textBox1.Text.Trim(), listView1, siraalanigenel);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Listview seçilirse işlem yapılacak Id bilgisine ulaşılmalıdır.
                kontrolid = Int32.Parse(listView1.SelectedItems[0].Text);//Seçilenin Id nosunu al
                kontrolisim = listView1.SelectedItems[0].SubItems[1].Text;//Seçilenin Adının Soyadını al
            }
            catch
            {
                //Hata durumu kontrol edilebilir
            }
        }

        private void kaydıSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Kayıt silme
            if (kontrolid == 0) return;//Kayıt seçili değilse çık.
            DialogResult cevap = MessageBox.Show("Bu kayıt silinsin mi? (" +kontrolisim+ ")", "Yeni Kayıt", MessageBoxButtons.YesNo,
            MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
            if(cevap==DialogResult.Yes)
            {
                try
                {
                    //Linq to Sql nesnelerini oluştur
                    RehberClasses1DataContext vtab = new RehberClasses1DataContext();
                    rehber idyegorebul = new rehber();
                    //Linq Satırı
                    idyegorebul = vtab.rehber.Single(idno => idno.Id == kontrolid);
                    vtab.rehber.DeleteOnSubmit(idyegorebul);//Kaydı sil
                    vtab.SubmitChanges();
                }
                catch (Exception hata)
                {
                    toolStripStatusLabel1.Text = "Hata oluştu. Hata:" + hata.Message;
                    return;
                }
                toolStripStatusLabel1.Text = "1 adet kayıt silindi";
                kontrolid = 0;//İlk değerine döndür
                Kayitlarilistele("", listView1, "Id");//Tüm kayıtları tekrar listele
            }//if
        }

        private void kayıdDüzeltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Kayıt düzeltme formunu aç
            if (kontrolid == 0) return;//Seçili bir kayıt yoksa çık
            this.Hide();//Aktif formu gizle
            //Düzeltme formunu aç
            Duzeltme dformu = new Duzeltme(kontrolid);//Id noyu forma gönder
            dformu.TopMost = true;//Tüm pencerelerin üstünde gösterir.
            dformu.StartPosition = FormStartPosition.CenterScreen;
            dformu.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Yazdırma işleminin yapıldığı metot.
            //Bu eylemi, Print metodunu harekete geçirir.
            string yazilan = "";
            try
            {
                yazilan = "Adres Defteri Bilgileri\r\n" +
                    "Kimlik No :" + listView1.SelectedItems[0].Text +
                    "\r\nAdı Soyadı :" + listView1.SelectedItems[0].SubItems[1].Text +
                    "\r\nE-Posta :" + listView1.SelectedItems[0].SubItems[2].Text +
                    "\r\nTelefon :" + listView1.SelectedItems[0].SubItems[3].Text +
                    "\r\nŞehir :" + listView1.SelectedItems[0].SubItems[4].Text +
                    "\r\nAdres :" + listView1.SelectedItems[0].SubItems[5].Text +
                    "\r\nKayıt Tarihi :" + listView1.SelectedItems[0].SubItems[6].Text +
                    "\r\nYazdırma Tarihi :" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                Font yazicifontu = new Font("Times", 18);//Yazı fontu
                SolidBrush firca = new SolidBrush(Color.Black);//Renk
                e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Blue), 3), 9, 9, 536, 258);//Bir dikdörtgen çizer
                e.Graphics.DrawString(yazilan, yazicifontu, firca, 10, 10, new StringFormat());//Bilgileri yazdırma
                yazicifontu.Dispose();//Bellekten at.
                firca.Dispose();
            }
            catch
            {
                //Hata oluşursa yapılacak işlemler
                MessageBox.Show("Yazdırma Sırasında Hata Oluştu", "Yazdırma Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//try
        }

        private void kaydıYazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Aktif kaydı yazdırır.
            if (kontrolid == 0) return;
            //Yazıcılar penceresini açıp bir yazıcı seçilirse yazdıracaktır.
            //Yazıcı diyalog penceresinde gözükecek ayarlar için printdialog nesnesinin özelliklerine bakılmalıdır.
            printDialog1.Document = printDocument1;//Yazdırma işlemini yapan nesneye bağlama
            if(printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();//Yazdır
            }            
            kontrolid = 0;
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //Listview kolon başlığına tıklandığında çalışır.
            string[] alanlar = { "Id", "Adsoyad", "Eposta", "Telefon", "Sehir", "Adres", "Ktarihi" };
            Kayitlarilistele("", listView1, alanlar[e.Column]);//Tıklanan kolon başlığına denk gelen alan ismini döndür.
            siraalanigenel = alanlar[e.Column];//Textbox içerisinde arama yaparken aktif sıraya göre sıralama yap
            toolStripStatusLabel1.Text = alanlar[e.Column] + " alanına göre sıralandı";
        }



        private void kapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Formu kapat
            this.Close();
        }

        private void csvExcelÇıktısıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Csv (Virgülle ayrılmış değerler) listesi olarak Excel çıktısı ver
            //App.config'den veritabanı bağlantı cümlesini alıyoruz.
            string bcumle = ConfigurationManager.ConnectionStrings["RehberLinqtoSql.Properties.Settings.veriler1ConnectionString"].ConnectionString;
            string dosyaadi = @"Rehberkayitlar.csv"; // Dosyanın kaydedileceği yol, çalıştığı klasördür.
            using (var context = new RehberClasses1DataContext(bcumle))
            {
                // rehber tablosundaki tüm verileri çekiyoruz
                var rehberkayitlari = context.rehber.ToList();
                // CSV dosyasını oluşturma                
                using (StreamWriter dosyayaz = new StreamWriter(dosyaadi,false,System.Text.Encoding.UTF8))
                {
                    // Başlık satırını yazıyoruz
                    dosyayaz.WriteLine("Kimlik;Adı Soyadı;E-Posta;TelefonŞehir;Adres;Açıklama;Kayıt Tarihi");

                    // Verileri yazıyoruz
                    foreach (var eleman in rehberkayitlari)
                    {
                        try
                        {
                            //Yaz ve alt satıra in
                            dosyayaz.WriteLine($"{eleman.Id};{eleman.Adsoyad};{eleman.Eposta};{eleman.Telno};{eleman.Sehir};{eleman.Adres};{eleman.Aciklama};{eleman.Ktarihi}");
                        }
                        catch
                        {
                            return;
                        }                        
                    }
                }//using
            }//using
            MessageBox.Show("Tüm Kayıtları Excel Csv olarak klasöre kaydedildi."+ "\nDosya:" + Application.StartupPath+"\\"+ dosyaadi, "Excel Çıktısı", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }
    }
}

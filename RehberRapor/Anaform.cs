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

namespace Rehberrapor
{
    public partial class Anaform : Form
    {
        //Genel değişkenler
        public static SqlConnection baglanti;
        public Anaform()
        {
            InitializeComponent();
        }
        /*
         * Reportivewer bileşeni için öncelikle Uzantılar>Uzantıları yönet menüsünden arama kısmına "report viewer" yazılarak "Microsoft RDLC Report Designer" yüklenmelidir.
         * Yükleme işleminde sonra Nuget açılarak (Proje sağtuş>Nuget Paketlerini Yönet) arama kısmına "ReportviewerControl" yazılmalı ve yüklenmelidir.
         * Uygulama F5 tuşu bir kez çalışmalı. 
         * Proje>Sağ tuş>Ekle ile "Report" şablonu seçilir. Rapora bir isim verilir. Örnek: kayitlistesiraporu.rdlc
         * Rapor tasarım sekmesi açılır. (RDLC Design)
         * Öncelikle, Dataset>Sağ tuş ile "Add Dataset" seçilerek var olan veya yeni bir veri kaynağı seçilir.
         * Rapor tasarımında iken Araçlar seçilir. TextBox kontrolü rapora sürüklenir. Alan başlığı yazılır.
         * Araçlardan Table kontrolü Rapora sürüklenir. Dataset içerisindeki alanlar Table kontrolünün kolonlarına sürüklenir. 
         * Gerekli ayarlamalar yapılır ve kaydedilir.
         * Rdlc dosyasını uygulamaya eklemek için yeni bir Form eklenir. 
         * Form tasarımında iken Araçlarda arama yapılarak "ReportViewer" kontrolü seçilir ve yeni Form'a sürüklenir.
         * ReportViewer nesninin sağ üstündeki ok işareti ile "Choose Report" ile oluşturulan Rdlc rapor dosyası seçilir. Örnek: kayitlistesiraporu.rdlc
         * Choose Data Sources ile gerekli Dataset seçilir. F4 tuşu özellikleri açılır. Dock özelliği "Fill" yapılarak Formu kaplaması sağlanır. Gerek duyulan özellikler değiştirilir.
         * ReportViewer bileşeninin olduğu Formun Load olayında aşağıdaki gibi bir kodun oluşması gereklidir.
         * // Bu kod satırı 'veriler1DataSet.rehber' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.rehberTableAdapter.Fill(this.veriler1DataSet.rehber);
            this.reportViewer1.RefreshReport();

            rehberTableAdapter vb. nesnesinin veri kaynağı Sql kodları ile değiştirilerek rapora gelen kayıtlar değiştirilebilir.
         */

        private void button1_Click(object sender, EventArgs e)
        {
            //Rapor formunu göster (Adsoyada göre sıralı)
            TumkayitlarRaporformu raporformu = new TumkayitlarRaporformu("Select * from rehber Order by Adsoyad");//
            raporformu.ShowDialog();//Diyalog formunda göster. Hep üstte kalacak bir pencere olarak açılır.
        }

        private void Anaform_Load(object sender, EventArgs e)
        {
            Baglan();//Veri tabanı bağlantısını yap
        }

        private void Baglan()
        {
            //Bağlantıyı yap
            //Bağlantını doğru olması gereklidir. 
            //Sunucu: (localdb)\MSSQLLocalDB
            //Veri tabanı: veriler1
            string bcumle = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=veriler1;Integrated Security=True";
            baglanti = new SqlConnection(bcumle);
            baglanti.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Rapor formunu göster (Adsoyada göre sıralı)
            TumkayitlarRaporformu raporformu = new TumkayitlarRaporformu("Select * from rehber Order by Eposta");
            raporformu.ShowDialog();//Diyalog formunda göster. Hep üstte kalacak bir pencere olarak açılır.
        }

        private void Anaform_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Bağlantıyı kapat
            baglanti.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Ada göre arama yapar. Çıkan sonucu raporlar.
            TumkayitlarRaporformu raporformu = new TumkayitlarRaporformu("Select * from rehber Where Adsoyad Like '%"+textBox1.Text.Trim()+"%' Order By Adsoyad");
            raporformu.ShowDialog();//Diyalog formunda göster. Hep üstte kalacak bir pencere olarak açılır.
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Epostaya göre arama yapar. Çıkan sonucu raporlar.
            TumkayitlarRaporformu raporformu = new TumkayitlarRaporformu("Select * from rehber Where Eposta Like '%" + textBox2.Text.Trim() + "%' Order By Eposta");
            raporformu.ShowDialog();//Diyalog formunda göster. Hep üstte kalacak bir pencere olarak açılır.
        }
    }
}

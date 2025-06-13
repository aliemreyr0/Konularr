using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Raporornek1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /*
         * Yeni proje oluşturulurken Windows Form .Net Framework seçilir.
         * Reportivewer bileşeni için öncelikle Uzantılar>Uzantıları yönet menüsünden arama kısmına "report viewer" yazılarak "Microsoft RDLC Report Designer" yüklenmelidir.
         * Yükleme işleminde sonra Nuget açılarak (Proje sağtuş>Nuget Paketlerini Yönet) arama kısmına "ReportviewerControl" yazılmalı ve yüklenmelidir.
         * Uygulama F5 tuşu bir kez çalışmalı. 
         * Proje>Sağ tuş>Ekle ile "Report" şablonu seçilir. Rapora bir isim verilir. Örnek: kayitlistesiraporu.rdlc
         * Rapor tasarım sekmesi açılır. (RDLC Design)
         * Öncelikle, Dataset>Sağ tuş ile "Add Dataset" seçilerek var olan veya yeni bir veri kaynağı seçilir.
         * Rapor tasarımında iken Araçlar seçilir. TextBox kontrolü rapora sürüklenir. Alan başlığı yazılır.
         * Araçlardan Table kontrolü Rapora sürüklenir. Dataset içerisindeki alanlar Table kontrolünün kolonlarına sürüklenir.          
         * Rdlc dosyasını uygulamaya eklemek için yeni bir Form eklenir. Rdlc dosyası Exe dosyasının olduğu klasöre kopyalanır.
         * Form tasarımında iken Araçlarda arama yapılarak "ReportViewer" kontrolü seçilir ve yeni Form'a sürüklenir.
         * ReportViewer nesninin sağ üstündeki ok işareti ile "Choose Report" ile oluşturulan Rdlc rapor dosyası seçilir. Örnek: kayitlistesiraporu.rdlc
         * Choose Data Sources ile gerekli Dataset seçilir. F4 tuşu özellikleri açılır. Dock özelliği "Fill" yapılarak Formu kaplaması sağlanır. Gerek duyulan özellikler değiştirilir.         
         */
        string bcumle = "";
        private string veritabaniAdi = "Veriler1";
        private string tabloAdi = "musteriler";
        public static SqlConnection bag = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            VeritabaniVeTabloHazirla(); // Veritabanı ve tablo yoksa oluşturacak
            // 1. SQL bağlantısı ve veri çekelim
            bcumle = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog="+ veritabaniAdi + ";Integrated Security=True;"; // SQL Server ayarına göre düzenle
            bag = new SqlConnection(bcumle);
            SqlDataAdapter adap1 = new SqlDataAdapter("SELECT * FROM "+tabloAdi+"", bag);
            DataTable veritablosu = new DataTable();
            adap1.Fill(veritablosu);

            // 2. ReportViewer için veri kaynağını hazırlayalım
            ReportDataSource verikaynagi = new ReportDataSource("DataSet1", veritablosu);
            // 3. ReportViewer ayarları
            reportViewer1.LocalReport.DataSources.Clear();//Temizle
            reportViewer1.LocalReport.DataSources.Add(verikaynagi);            
            reportViewer1.LocalReport.ReportPath = Application.StartupPath + "\\musterilerraporu.rdlc"; // RDLC dosyasının yolunu kontrol et
            //Rdlc dosyası, Exe dosyasının olduğu klasörde olmalıdır.
            reportViewer1.RefreshReport();//Raporu göster      
        }

        private void VeritabaniVeTabloHazirla()
        {
            //Veri tabanı ve tablo yoksa oluşturur ve örnek kayıt ekler.
            // 1. Veritabanı kontrol ve oluşturma (master bağlantısı ile)
            string masterBaglantiCumlesi = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
            using (SqlConnection masterBag = new SqlConnection(masterBaglantiCumlesi))
            {
                masterBag.Open();
                string veritabaniSorgu = $"IF DB_ID('{veritabaniAdi}') IS NULL CREATE DATABASE [{veritabaniAdi}];";
                SqlCommand veritabaniKomut = new SqlCommand(veritabaniSorgu, masterBag);
                veritabaniKomut.ExecuteNonQuery();
            }

            // 2. Tabloyu oluştur ve veri ekle
            if (bag == null)
            {
                bcumle = $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={veritabaniAdi};Integrated Security=True;";
                bag = new SqlConnection(bcumle);
            }

            if (bag.State != ConnectionState.Open)
                bag.Open();

            // Tablo oluşturma sorgusu
            string tabloSorgu = $@"
            IF OBJECT_ID('{tabloAdi}', 'U') IS NULL
            CREATE TABLE {tabloAdi} (
            Id INT PRIMARY KEY IDENTITY(1,1),
            Adsoyad NVARCHAR(100) Null,
            Eposta NVARCHAR(100) Null,
            Ktarihi DATETIME DEFAULT GETDATE()
            );";
            SqlCommand tabloKomut = new SqlCommand(tabloSorgu, bag);
            tabloKomut.ExecuteNonQuery();

            // Örnek veri ekleme (Eğer tablo boşsa)
            string veriKontrol = $@"
            IF NOT EXISTS (SELECT * FROM {tabloAdi})
            INSERT INTO {tabloAdi} (Adsoyad, Eposta) VALUES
            ('Ali Ak', 'ali@ali.com'),
            ('Ahmet Deniz', 'ahmet@ahmet.com');";
        
            SqlCommand veriKomut = new SqlCommand(veriKontrol, bag);
            veriKomut.ExecuteNonQuery();
        }

    }
}


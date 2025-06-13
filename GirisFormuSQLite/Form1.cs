using System.Data.SQLite;
using System.Drawing.Printing;
using System.Text;
namespace GirisFormuSQLite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Sýralama iþlemi için
        int siralamakolonu = -1;
        bool artansirami = true;

        PrintDocument belge = new PrintDocument();
        string yazdirilacakMetin = "";

        // Veritabaný baðlantýsý için SQLite baðlantý cümlesi
        string bcumlesi = "Data Source=veriler.db;Version=3;";
        int secilenId = -1;//Seçilen kaydýn Id deðeri

        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Text = "";
            listView1.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            listView1.FullRowSelect = true;//Tüm satýr seçili olsun
            // Veritabaný dosyasýnýn var olup olmadýðýný kontrol et
            if (!System.IO.File.Exists("veriler.db"))
            {
                // Veritabaný dosyasýný oluþtur
                SQLiteConnection.CreateFile("veriler.db");
                // Veritabanýna baðlan ve tabloyu oluþtur
                using (SQLiteConnection conn = new SQLiteConnection(bcumlesi))
                {
                    conn.Open();
                    string createTableQuery = @"
                    CREATE TABLE kisiler (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        AdSoyad TEXT,
                        Eposta TEXT,
                        TcNo TEXT,
                        Ktarihi DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";
                    SQLiteCommand command = new SQLiteCommand(createTableQuery, conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }
            // ListView için kolon baþlýklarýný belirle
            listView1.Columns.Add("Kimlik", 60);
            listView1.Columns.Add("Adý Soyadý", 150);
            listView1.Columns.Add("Eposta", 150);
            listView1.Columns.Add("TC No", 150);
            listView1.Columns.Add("K. Tarihi", 160);

            // Veritabanýndaki tüm kayýtlarý listede göster
            VerileriListele("SELECT * FROM kisiler");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Yeni kayýt
            string adSoyad = textBox1.Text;
            string eposta = textBox2.Text;
            string tcNo = textBox3.Text;
            if (string.IsNullOrEmpty(adSoyad) || string.IsNullOrEmpty(eposta) || string.IsNullOrEmpty(tcNo))
            {
                label4.Text = "Tüm alanlar zorunludur.";
                return;
            }
            if (!eposta.Contains("@") || !eposta.Contains("."))
            {
                label4.Text = "Geçersiz e-posta formatý.";
                return;
            }
            if (tcNo.Length != 11 || !long.TryParse(tcNo, out _))
            {
                label4.Text = "TC No 11 haneli bir sayý olmalýdýr.";
                return;
            }
            try
            {
                using (SQLiteConnection baglanti = new SQLiteConnection(bcumlesi))
                {
                    baglanti.Open();

                    if (secilenId == -1)
                    {
                        // Yeni kayýt öncesi TC kontrolü
                        if (TcNoVarmi(tcNo))
                        {
                            label4.Text = "Bu TC numarasýyla daha önce kayýt yapýlmýþ.";
                            return;
                        }

                        // Yeni kayýt ekle
                        string insertQuery = "INSERT INTO kisiler (AdSoyad, Eposta, TcNo) VALUES (@adSoyad, @eposta, @tcNo)";
                        SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, baglanti);
                        insertCommand.Parameters.AddWithValue("@adSoyad", adSoyad);
                        insertCommand.Parameters.AddWithValue("@eposta", eposta);
                        insertCommand.Parameters.AddWithValue("@tcNo", tcNo);
                        insertCommand.ExecuteNonQuery();
                        label4.Text = "Yeni kayýt oluþturuldu";
                        System.Media.SystemSounds.Exclamation.Play();//Windows seslerinden çal
                    }
                    baglanti.Close();
                }
                MetinleriBosalt();
                secilenId = -1;
                VerileriListele("SELECT * FROM kisiler");
            }
            catch (Exception hata)
            {
                label4.Text = "Hata: " + hata.Message;
            }
        }

        private void MetinleriBosalt()
        {
            textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = "";
            secilenId = -1;
            textBox1.Focus();
        }

        private void VerileriListele(string sql)
        {
            listView1.Items.Clear();
            using (SQLiteConnection baglanti = new SQLiteConnection(bcumlesi))
            {
                baglanti.Open();
                string selectQuery = sql;
                SQLiteCommand komut = new SQLiteCommand(selectQuery, baglanti);
                SQLiteDataReader okuyucu = komut.ExecuteReader();

                while (okuyucu.Read())
                {
                    ListViewItem item = new ListViewItem(okuyucu["Id"].ToString());
                    item.SubItems.Add(okuyucu["AdSoyad"].ToString());
                    item.SubItems.Add(okuyucu["Eposta"].ToString());
                    item.SubItems.Add(okuyucu["TcNo"].ToString());
                    item.SubItems.Add(okuyucu["Ktarihi"].ToString());
                    listView1.Items.Add(item);
                }
                baglanti.Close();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //Arama
            VerileriListele("SELECT * FROM kisiler Where Adsoyad Like '%" + textBox4.Text + "%'");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                secilenId = Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text);
                textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text; // AdSoyad
                textBox2.Text = listView1.SelectedItems[0].SubItems[2].Text; // Eposta
                textBox3.Text = listView1.SelectedItems[0].SubItems[3].Text; // TC No
                label4.Text = "Düzenlemek için alanlardaki bilgileri güncelleyin, ardýndan DÜZELT'e týklayýn.";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Kayýt silme
            if (secilenId == -1)
            {
                label4.Text = "Lütfen silmek için bir kayýt seçin.";
                return;
            }

            DialogResult cevap = MessageBox.Show("Seçili kaydý silmek istediðinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo);

            if (cevap == DialogResult.Yes)
            {
                using (SQLiteConnection baglanti = new SQLiteConnection(bcumlesi))
                {
                    baglanti.Open();
                    string deleteQuery = "DELETE FROM kisiler WHERE Id = @id";
                    SQLiteCommand komut = new SQLiteCommand(deleteQuery, baglanti);
                    komut.Parameters.AddWithValue("@id", secilenId);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                }

                label4.Text = "Kayýt silindi.";
                secilenId = -1;
                MetinleriBosalt();
                VerileriListele("SELECT * FROM kisiler");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (secilenId == -1)
            {
                label4.Text = "Lütfen düzeltmek için bir kayýt seçin.";
                return;
            }
            string adSoyad = textBox1.Text;
            string eposta = textBox2.Text;
            string tcNo = textBox3.Text;

            if (string.IsNullOrEmpty(adSoyad) || string.IsNullOrEmpty(eposta) || string.IsNullOrEmpty(tcNo))
            {
                label4.Text = "Tüm alanlar zorunludur.";
                return;
            }

            if (!eposta.Contains("@") || !eposta.Contains("."))
            {
                label4.Text = "Geçersiz e-posta formatý.";
                return;
            }

            if (tcNo.Length != 11 || !long.TryParse(tcNo, out _))
            {
                label4.Text = "TC No 11 haneli bir sayý olmalýdýr.";
                return;
            }
            // Güncelleme iþleminde TC baþka bir kiþide varsa engelle
            if (TcNoVarmi(tcNo, secilenId))
            {
                label4.Text = "Bu TC numarasý baþka bir kiþiye ait. Lütfen farklý bir TC giriniz.";
                return;
            }

            try
            {
                using (SQLiteConnection baglanti = new SQLiteConnection(bcumlesi))
                {
                    baglanti.Open();
                    string updateQuery = "UPDATE kisiler SET AdSoyad = @adSoyad, Eposta = @eposta, TcNo = @tcNo WHERE Id = @id";
                    SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, baglanti);
                    updateCommand.Parameters.AddWithValue("@adSoyad", adSoyad);
                    updateCommand.Parameters.AddWithValue("@eposta", eposta);
                    updateCommand.Parameters.AddWithValue("@tcNo", tcNo);
                    updateCommand.Parameters.AddWithValue("@id", secilenId);
                    updateCommand.ExecuteNonQuery();
                    baglanti.Close();
                }

                label4.Text = "Kayýt baþarýyla güncellendi.";
                System.Media.SystemSounds.Asterisk.Play();
                secilenId = -1;
                MetinleriBosalt();
                VerileriListele("SELECT * FROM kisiler");
            }
            catch (Exception ex)
            {
                label4.Text = "Hata: " + ex.Message;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Listede dýþa aktarýlacak veri bulunamadý.");
                return;
            }

            SaveFileDialog kaydet = new SaveFileDialog();
            kaydet.Filter = "CSV Dosyasý|*.csv";
            kaydet.Title = "CSV Olarak Kaydet";
            kaydet.FileName = "veriler.csv";

            if (kaydet.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(kaydet.FileName, false, Encoding.UTF8))
                {
                    // Baþlýklar
                    string[] basliklar = listView1.Columns
                                            .Cast<ColumnHeader>()
                                            .Select(col => col.Text)
                                            .ToArray();
                    sw.WriteLine(string.Join(";", basliklar));

                    // Satýrlar
                    foreach (ListViewItem item in listView1.Items)
                    {
                        string[] alanlar = item.SubItems
                                            .Cast<ListViewItem.ListViewSubItem>()
                                            .Select(sub => sub.Text.Replace(";", ","))
                                            .ToArray();
                        sw.WriteLine(string.Join(";", alanlar));
                    }
                }
                MessageBox.Show("CSV dosyasý baþarýyla kaydedildi.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Yazdýrmayý baþlat
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Yazdýrýlacak veri bulunamadý.");
                return;
            }


            // Yazdýrýlacak metni hazýrla
            yazdirilacakMetin = "Kayýt Listesi\n\n";

            foreach (ListViewItem item in listView1.Items)
            {
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    yazdirilacakMetin += item.SubItems[i].Text + "\t";
                }
                yazdirilacakMetin += "\n";
            }
            // Yazdýrma iþlerini baþlatan olayý çaðýr.
            belge.PrintPage += Belge_PrintPage;

            PrintDialog yazdirDialog = new PrintDialog();//Yazýcý seçimi diyalog kutusunu aç
            yazdirDialog.Document = belge;

            if (yazdirDialog.ShowDialog() == DialogResult.OK)
            {
                belge.Print();
            }
        }

        private void Belge_PrintPage(object sender, PrintPageEventArgs e)
        {
            //Yazdýrma iþlerinin yapýldýðý olay
            Font yazifontu = new Font("Times", 10);
            e.Graphics.DrawString(yazdirilacakMetin, yazifontu, Brushes.Black, new PointF(50, 50));
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Ayný kolona týklanýrsa artan/azalan yönü deðiþtir
            if (e.Column == siralamakolonu)
                artansirami = !artansirami;
            else
            {
                siralamakolonu = e.Column;
                artansirami = true;
            }
            // Kolon adýna göre sýralama için SQL ORDER BY ifadesini kullanýyoruz
            string[] kolonlar = { "Id", "Adsoyad", "Eposta", "Tcno","Ktarihi" };
            string secilenKolon = kolonlar[siralamakolonu];
            string yon = artansirami ? "ASC" : "DESC";
            VerileriListele($"SELECT * FROM kisiler ORDER BY {secilenKolon} {yon}");
        }

        private bool TcNoVarmi(string tcNo, int? haricId = null)
        {
            //Yeni kayýt ve düzeltme iþleminde Tc numarasý yinelemesini önler
            using (SQLiteConnection baglanti = new SQLiteConnection(bcumlesi))
            {
                baglanti.Open();
                string sql = "SELECT COUNT(*) FROM kisiler WHERE TcNo = @tcNo";
                if (haricId != null)
                {
                    sql += " AND Id <> @id";
                }
                SQLiteCommand komut = new SQLiteCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@tcNo", tcNo);
                if (haricId != null)//Eðer düzeltme iþleminde kullanýldýysa
                {
                    komut.Parameters.AddWithValue("@id", haricId.Value);
                }
                int adet = Convert.ToInt32(komut.ExecuteScalar());
                baglanti.Close();
                return adet > 0;//adet, 0'dan büyükse True dönecektir.
            }
        }

    }
}

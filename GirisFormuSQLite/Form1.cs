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
        //S�ralama i�lemi i�in
        int siralamakolonu = -1;
        bool artansirami = true;

        PrintDocument belge = new PrintDocument();
        string yazdirilacakMetin = "";

        // Veritaban� ba�lant�s� i�in SQLite ba�lant� c�mlesi
        string bcumlesi = "Data Source=veriler.db;Version=3;";
        int secilenId = -1;//Se�ilen kayd�n Id de�eri

        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Text = "";
            listView1.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            listView1.FullRowSelect = true;//T�m sat�r se�ili olsun
            // Veritaban� dosyas�n�n var olup olmad���n� kontrol et
            if (!System.IO.File.Exists("veriler.db"))
            {
                // Veritaban� dosyas�n� olu�tur
                SQLiteConnection.CreateFile("veriler.db");
                // Veritaban�na ba�lan ve tabloyu olu�tur
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
            // ListView i�in kolon ba�l�klar�n� belirle
            listView1.Columns.Add("Kimlik", 60);
            listView1.Columns.Add("Ad� Soyad�", 150);
            listView1.Columns.Add("Eposta", 150);
            listView1.Columns.Add("TC No", 150);
            listView1.Columns.Add("K. Tarihi", 160);

            // Veritaban�ndaki t�m kay�tlar� listede g�ster
            VerileriListele("SELECT * FROM kisiler");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Yeni kay�t
            string adSoyad = textBox1.Text;
            string eposta = textBox2.Text;
            string tcNo = textBox3.Text;
            if (string.IsNullOrEmpty(adSoyad) || string.IsNullOrEmpty(eposta) || string.IsNullOrEmpty(tcNo))
            {
                label4.Text = "T�m alanlar zorunludur.";
                return;
            }
            if (!eposta.Contains("@") || !eposta.Contains("."))
            {
                label4.Text = "Ge�ersiz e-posta format�.";
                return;
            }
            if (tcNo.Length != 11 || !long.TryParse(tcNo, out _))
            {
                label4.Text = "TC No 11 haneli bir say� olmal�d�r.";
                return;
            }
            try
            {
                using (SQLiteConnection baglanti = new SQLiteConnection(bcumlesi))
                {
                    baglanti.Open();

                    if (secilenId == -1)
                    {
                        // Yeni kay�t �ncesi TC kontrol�
                        if (TcNoVarmi(tcNo))
                        {
                            label4.Text = "Bu TC numaras�yla daha �nce kay�t yap�lm��.";
                            return;
                        }

                        // Yeni kay�t ekle
                        string insertQuery = "INSERT INTO kisiler (AdSoyad, Eposta, TcNo) VALUES (@adSoyad, @eposta, @tcNo)";
                        SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, baglanti);
                        insertCommand.Parameters.AddWithValue("@adSoyad", adSoyad);
                        insertCommand.Parameters.AddWithValue("@eposta", eposta);
                        insertCommand.Parameters.AddWithValue("@tcNo", tcNo);
                        insertCommand.ExecuteNonQuery();
                        label4.Text = "Yeni kay�t olu�turuldu";
                        System.Media.SystemSounds.Exclamation.Play();//Windows seslerinden �al
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
                label4.Text = "D�zenlemek i�in alanlardaki bilgileri g�ncelleyin, ard�ndan D�ZELT'e t�klay�n.";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Kay�t silme
            if (secilenId == -1)
            {
                label4.Text = "L�tfen silmek i�in bir kay�t se�in.";
                return;
            }

            DialogResult cevap = MessageBox.Show("Se�ili kayd� silmek istedi�inizden emin misiniz?", "Onay", MessageBoxButtons.YesNo);

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

                label4.Text = "Kay�t silindi.";
                secilenId = -1;
                MetinleriBosalt();
                VerileriListele("SELECT * FROM kisiler");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (secilenId == -1)
            {
                label4.Text = "L�tfen d�zeltmek i�in bir kay�t se�in.";
                return;
            }
            string adSoyad = textBox1.Text;
            string eposta = textBox2.Text;
            string tcNo = textBox3.Text;

            if (string.IsNullOrEmpty(adSoyad) || string.IsNullOrEmpty(eposta) || string.IsNullOrEmpty(tcNo))
            {
                label4.Text = "T�m alanlar zorunludur.";
                return;
            }

            if (!eposta.Contains("@") || !eposta.Contains("."))
            {
                label4.Text = "Ge�ersiz e-posta format�.";
                return;
            }

            if (tcNo.Length != 11 || !long.TryParse(tcNo, out _))
            {
                label4.Text = "TC No 11 haneli bir say� olmal�d�r.";
                return;
            }
            // G�ncelleme i�leminde TC ba�ka bir ki�ide varsa engelle
            if (TcNoVarmi(tcNo, secilenId))
            {
                label4.Text = "Bu TC numaras� ba�ka bir ki�iye ait. L�tfen farkl� bir TC giriniz.";
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

                label4.Text = "Kay�t ba�ar�yla g�ncellendi.";
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
                MessageBox.Show("Listede d��a aktar�lacak veri bulunamad�.");
                return;
            }

            SaveFileDialog kaydet = new SaveFileDialog();
            kaydet.Filter = "CSV Dosyas�|*.csv";
            kaydet.Title = "CSV Olarak Kaydet";
            kaydet.FileName = "veriler.csv";

            if (kaydet.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(kaydet.FileName, false, Encoding.UTF8))
                {
                    // Ba�l�klar
                    string[] basliklar = listView1.Columns
                                            .Cast<ColumnHeader>()
                                            .Select(col => col.Text)
                                            .ToArray();
                    sw.WriteLine(string.Join(";", basliklar));

                    // Sat�rlar
                    foreach (ListViewItem item in listView1.Items)
                    {
                        string[] alanlar = item.SubItems
                                            .Cast<ListViewItem.ListViewSubItem>()
                                            .Select(sub => sub.Text.Replace(";", ","))
                                            .ToArray();
                        sw.WriteLine(string.Join(";", alanlar));
                    }
                }
                MessageBox.Show("CSV dosyas� ba�ar�yla kaydedildi.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Yazd�rmay� ba�lat
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Yazd�r�lacak veri bulunamad�.");
                return;
            }


            // Yazd�r�lacak metni haz�rla
            yazdirilacakMetin = "Kay�t Listesi\n\n";

            foreach (ListViewItem item in listView1.Items)
            {
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    yazdirilacakMetin += item.SubItems[i].Text + "\t";
                }
                yazdirilacakMetin += "\n";
            }
            // Yazd�rma i�lerini ba�latan olay� �a��r.
            belge.PrintPage += Belge_PrintPage;

            PrintDialog yazdirDialog = new PrintDialog();//Yaz�c� se�imi diyalog kutusunu a�
            yazdirDialog.Document = belge;

            if (yazdirDialog.ShowDialog() == DialogResult.OK)
            {
                belge.Print();
            }
        }

        private void Belge_PrintPage(object sender, PrintPageEventArgs e)
        {
            //Yazd�rma i�lerinin yap�ld��� olay
            Font yazifontu = new Font("Times", 10);
            e.Graphics.DrawString(yazdirilacakMetin, yazifontu, Brushes.Black, new PointF(50, 50));
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Ayn� kolona t�klan�rsa artan/azalan y�n� de�i�tir
            if (e.Column == siralamakolonu)
                artansirami = !artansirami;
            else
            {
                siralamakolonu = e.Column;
                artansirami = true;
            }
            // Kolon ad�na g�re s�ralama i�in SQL ORDER BY ifadesini kullan�yoruz
            string[] kolonlar = { "Id", "Adsoyad", "Eposta", "Tcno","Ktarihi" };
            string secilenKolon = kolonlar[siralamakolonu];
            string yon = artansirami ? "ASC" : "DESC";
            VerileriListele($"SELECT * FROM kisiler ORDER BY {secilenKolon} {yon}");
        }

        private bool TcNoVarmi(string tcNo, int? haricId = null)
        {
            //Yeni kay�t ve d�zeltme i�leminde Tc numaras� yinelemesini �nler
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
                if (haricId != null)//E�er d�zeltme i�leminde kullan�ld�ysa
                {
                    komut.Parameters.AddWithValue("@id", haricId.Value);
                }
                int adet = Convert.ToInt32(komut.ExecuteScalar());
                baglanti.Close();
                return adet > 0;//adet, 0'dan b�y�kse True d�necektir.
            }
        }

    }
}

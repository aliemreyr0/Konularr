using System.Data.SQLite;
using System.Windows.Forms;
namespace OrnekSqlite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Genel tanýmlar
        public static SQLiteConnection baglanti;
        int idno = 0;//Kayýtlarý düzeltmek, silmek için
        string sirayonu = "ASC";//A-Z'ye doðru sýra yönü

        private void Form1_Load(object sender, EventArgs e)
        {
            Baglan();
            Listele("Select * from rehber Order By Adsoyad");
        }

        private void Listele(string sql)
        {
            listView1.Items.Clear();
            SQLiteCommand komut = new SQLiteCommand(sql, baglanti);
            SQLiteDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                ListViewItem lw1 = new ListViewItem(oku["Id"].ToString());
                lw1.SubItems.Add(oku["Adsoyad"].ToString());
                lw1.SubItems.Add(oku["Eposta"].ToString());
                lw1.SubItems.Add(oku["Ktarihi"].ToString());
                listView1.Items.Add(lw1);
            }
            oku.Close();
        }

        private void Baglan()
        {
            if (!File.Exists("veriler.sqlite"))
            {
                //Veritabaný mevcut deðilse
                baglanti = new SQLiteConnection("Data Source=veriler.sqlite");
                baglanti.Open();
                string sql = "Create Table rehber (Id INTEGER PRIMARY KEY AUTOINCREMENT,";
                sql += " Adsoyad TEXT NULL, Eposta TEXT NULL, Ktarihi DATETIME DEFAULT CURRENT_TIMESTAMP);";
                SQLiteCommand komut = new SQLiteCommand(sql, Form1.baglanti);
                komut.ExecuteNonQuery();
            }
            else
            {
                baglanti = new SQLiteConnection("Data Source=veriler.sqlite");
                baglanti.Open();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Alanlar boþ olmamalý");
                return;
            }
            string sql = "Insert into rehber (Adsoyad,Eposta) Values (@Adsoyad,@Eposta);";
            SQLiteCommand komut = new SQLiteCommand(sql, baglanti);
            komut.Parameters.AddWithValue("@Adsoyad", textBox1.Text.Trim());
            komut.Parameters.AddWithValue("@Eposta", textBox2.Text.Trim());
            int eklenen = 0;
            try
            {
                eklenen = komut.ExecuteNonQuery();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Hata oluþtu. Hata:" + hata.Message);
                return;
            }
            MessageBox.Show(eklenen + " adet kayýt eklendi");
            Listele("Select * from rehber Order By Adsoyad");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            baglanti.Close();//Baðlantýyý kapat
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Listele("Select * from rehber Where Adsoyad Like '%" + textBox3.Text + "%' Order By Adsoyad");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                idno = Int32.Parse(listView1.SelectedItems[0].Text);//Seçilen kaydýn Id nosu                
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Kayýt silme
            if (idno == 0)
            {
                MessageBox.Show("Silme iþlemi için bir kayýt seçmelisiniz.");
                return;
            }
            int idno2 = idno;//Genel deðiþkeni yerel deðiþkene aktar
            DialogResult cevap;
            cevap = MessageBox.Show(idno2 + " numaralý kayýt silinsin mi?", "Kayýt silme", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (cevap == DialogResult.Yes)
            {
                string sql = "Delete from rehber Where Id=@Idno";
                int silinen = 0;
                try
                {
                    SQLiteCommand komut = new SQLiteCommand(sql, baglanti);
                    komut.Parameters.AddWithValue("@Idno", idno2);
                    silinen = komut.ExecuteNonQuery();
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Silme iþleminde hata oluþtu. Hata:" + hata.Message);
                    return;
                }
                MessageBox.Show(silinen.ToString() + " adet kayýt silindi.");
                idno = 0;
                Listele("Select * from rehber Order by Adsoyad");
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //Kolon baþlýklarýna týklandýðýnda çalýþýr.
            //Alan isimleri
            string[] alanlar = { "Id", "Adsoyad", "Eposta", "Ktarihi" };
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

        private void button3_Click(object sender, EventArgs e)
        {
            //Kayýt düzeltme
            if (idno == 0)
            {
                MessageBox.Show("Düzeltme iþlemi için bir kayýt seçmelisiniz.");
                return;
            }
            Duzeltme dformu = new Duzeltme(idno);
            dformu.StartPosition = FormStartPosition.CenterScreen;
            dformu.ShowDialog();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //Form her etkinleþtirildiðinde çalýþan olaydýr.
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
    }
}

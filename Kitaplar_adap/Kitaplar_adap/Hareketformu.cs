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

namespace Kitaplar_adap
{
    public partial class Hareketformu : Form
    {
        //Genel tanımlar
        int idno = 0;
        public Hareketformu()
        {
            InitializeComponent();
        }

        public Hareketformu(int idnogelen)
        {
            idno = idnogelen;//Diğer formdan gelen idno bu class'taki idno değişkenine aktarılır.
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //kitaplar_hareket tablosuna kayıt ekler
            //Boşluk kontrolü
            if (comboBox1.SelectedIndex<0 || textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                toolStripStatusLabel1.Text = "Tüm alanlara veri giriş zorunludur.";
                textBox1.Focus();
                return;
            }
            //Ekleme
            string sql = "Insert into Kitaplar_hareket (Kitapid,Islemturu,Fiyati,Adet,Aciklama) ";
            sql += " Values (@Kitapid,@Islemturu,@Fiyati,@Adet,@Aciklama)";
            int eklenen = 0;
            try
            {
                SqlCommand komut = new SqlCommand(sql, Anaform.baglanti);
                komut.Parameters.AddWithValue("@Kitapid", idno);
                komut.Parameters.AddWithValue("@Islemturu", comboBox1.Text.Trim());
                komut.Parameters.AddWithValue("@Fiyati", textBox1.Text.Trim());
                komut.Parameters.AddWithValue("@Adet", textBox2.Text.Trim());
                komut.Parameters.AddWithValue("@Aciklama", textBox3.Text.Trim());
                eklenen = komut.ExecuteNonQuery();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Kayıt eklenemedi. Hata:" + hata.Message);
                return;
            }
            toolStripStatusLabel1.Text = eklenen.ToString() + " Adet kayıt eklendi.";
            Alanlaribosalt();
        }

        public void Alanlaribosalt()
        {
            comboBox1.SelectedIndex = -1;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void Hareketformu_Load(object sender, EventArgs e)
        {
            groupBox1.Text = idno + " numaralı kitap için bilgileri giriniz";
            toolStripStatusLabel1.Text = "";
            //Açılan kutuya eleman ekleme
            comboBox1.Items.Add("Alış");
            comboBox1.Items.Add("Satış");
        }
    }
}

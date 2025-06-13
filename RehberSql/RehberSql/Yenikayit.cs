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

namespace RehberSql
{
    public partial class Yenikayit : Form
    {
        public Yenikayit()
        {
            InitializeComponent();
        }

        private void Yenikayit_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Kayıt ekler
            //Boşluk kontrolü
            if(textBox1.Text =="" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                toolStripStatusLabel1.Text = "Tüm alanlara veri giriş zorunludur.";
                textBox1.Focus();
                return;
            }
            string sql = "Insert into rehber (Adsoyad,Eposta,Sehir,Adres) ";
            sql += " Values (@Adsoyad,@Eposta,@Sehir,@Adres)";
            int eklenen = 0;
            try
            {
                SqlCommand komut = new SqlCommand(sql, Form1.baglanti);
                komut.Parameters.AddWithValue("@Adsoyad", textBox1.Text.Trim());
                komut.Parameters.AddWithValue("@Eposta", textBox2.Text.Trim());
                komut.Parameters.AddWithValue("@Sehir", textBox3.Text.Trim());
                komut.Parameters.AddWithValue("@Adres", textBox4.Text.Trim());
                eklenen = komut.ExecuteNonQuery();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Kayıt eklenemedi. Hata:" + hata.Message);
                return;               
            }
            toolStripStatusLabel1.Text = eklenen.ToString() + " Adet kayıt eklendi.";
            Metinleribosalt();
        }

        private void Metinleribosalt()
        {
            textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = "";
            textBox1.Focus();
        }
    }
}

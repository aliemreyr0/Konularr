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
    public partial class Duzeltme : Form
    {
        //Genel tanımlar
        int idno = 0;
        public Duzeltme()
        {
            InitializeComponent();
        }

        public Duzeltme(int idnogelen)
        {
            idno = idnogelen;
            InitializeComponent();
        }

        private void Duzeltme_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            //Kaydı oku
            if(idno != 0)
            {
                string sql = "Select * from rehber Where Id=" + idno;
                SqlDataReader oku = null;
                SqlCommand komut = new SqlCommand(sql, Form1.baglanti);
                oku = komut.ExecuteReader();
                oku.Read();//Tek bir kayıt oku
                textBox1.Text = oku["Adsoyad"].ToString();
                textBox2.Text = oku["Eposta"].ToString();
                textBox3.Text = oku["Sehir"].ToString();
                textBox4.Text = oku["Adres"].ToString();
                oku.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Güncelleme
            if(idno != 0)
            {
                //Boşluk kontrolü
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                {
                    toolStripStatusLabel1.Text = "Tüm alanlara veri giriş zorunludur.";
                    textBox1.Focus();
                    return;
                }
                DialogResult cevap;
                cevap = MessageBox.Show(idno + " nolu kayıt güncellensin mi?", "Kayıt güncelleme", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if(cevap==DialogResult.Yes)
                {
                    string sql = "Update rehber Set Adsoyad=@Adsoyad,Eposta=@Eposta,Sehir=@Sehir,Adres=@Adres";
                    sql += " Where Id=" + idno;
                    int duzeltilen = 0;
                    try
                    {
                        SqlCommand komut = new SqlCommand(sql, Form1.baglanti);
                        komut.Parameters.AddWithValue("@Adsoyad", textBox1.Text.Trim());
                        komut.Parameters.AddWithValue("@Eposta", textBox2.Text.Trim());
                        komut.Parameters.AddWithValue("@Sehir", textBox3.Text.Trim());
                        komut.Parameters.AddWithValue("@Adres", textBox4.Text.Trim());
                        duzeltilen = komut.ExecuteNonQuery();
                    }
                    catch (Exception hata)
                    {
                        MessageBox.Show("Kayıt güncellenemedi. Hata:" + hata.Message);
                        return;                       
                    }
                    toolStripStatusLabel1.Text = duzeltilen.ToString() + " adet kayıt güncellendi";
                    idno = 0;
                }//if cevap
            }
        }
    }
}

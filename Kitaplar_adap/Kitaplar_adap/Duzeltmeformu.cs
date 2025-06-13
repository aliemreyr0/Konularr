using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Kitaplar_adap
{
    public partial class Duzeltmeformu : Form
    {
        //Genel tanımlar
        int idnohareket = 0;
        public Duzeltmeformu()
        {
            InitializeComponent();
        }

        public Duzeltmeformu(int idnogelen)
        {
            idnohareket = idnogelen;//Diğer formdan gelen idno bu class'taki idno değişkenine aktarılır.
            InitializeComponent();
        }
        private void Duzeltmeformu_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            //Açılan kutuya eleman ekleme
            comboBox1.Items.Add("Alış");
            comboBox1.Items.Add("Satış");
            //Kaydı oku
            if (idnohareket != 0)
            {
                string sql = "Select * from kitaplar_hareket Where Id=" + idnohareket;
                SqlDataReader oku = null;
                SqlCommand komut = new SqlCommand(sql, Anaform.baglanti);
                oku = komut.ExecuteReader();
                oku.Read();//Tek bir kayıt oku
                //İşlem  türünü belirle
                if (oku["Islemturu"].ToString() == "Alış")
                    comboBox1.SelectedIndex = 0;
                else if (oku["Islemturu"].ToString() == "Satış")
                    comboBox1.SelectedIndex = 1;
                textBox1.Text = oku["Fiyati"].ToString();
                textBox2.Text = oku["Adet"].ToString();
                textBox3.Text = oku["Aciklama"].ToString();                
                oku.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Düzeltmeyi kaydetme            
            if (idnohareket != 0)
            {
                //Boşluk kontrolü
                if (comboBox1.SelectedIndex < 0 || textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
                {
                    toolStripStatusLabel1.Text = "Tüm alanlara veri giriş zorunludur.";
                    textBox1.Focus();
                    return;
                }
                DialogResult cevap;
                cevap = MessageBox.Show("Seçili olan hareket güncellensin mi?", "Kayıt güncelleme", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (cevap == DialogResult.Yes)
                {
                    string sql = "Update kitaplar_hareket Set Islemturu=@Islemturu,Fiyati=@Fiyati,Adet=@Adet,Aciklama=@Aciklama,Kayittarihi=@Kayittarihi";
                    sql += " Where Id=" + idnohareket;
                    int duzeltilen = 0;
                    try
                    {
                        SqlCommand komut = new SqlCommand(sql, Anaform.baglanti);                        
                        komut.Parameters.AddWithValue("@Islemturu", comboBox1.Text.Trim());
                        komut.Parameters.AddWithValue("@Fiyati", textBox1.Text.Trim());
                        komut.Parameters.AddWithValue("@Adet", textBox2.Text.Trim());
                        komut.Parameters.AddWithValue("@Aciklama", textBox3.Text.Trim());
                        komut.Parameters.AddWithValue("@Kayittarihi", DateTime.Now);
                        duzeltilen = komut.ExecuteNonQuery();
                    }
                    catch (Exception hata)
                    {
                        MessageBox.Show("Kayıt güncellenemedi. Hata:" + hata.Message);
                        return;
                    }
                    toolStripStatusLabel1.Text = duzeltilen.ToString() + " adet kayıt güncellendi";
                    idnohareket = 0;
                }//if cevap
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RehberLinqtoSql
{
    public partial class Duzeltme : Form
    {
        int kontrolid = 0;
        public Duzeltme(int gecilenid)
        {
            InitializeComponent();
            kontrolid = gecilenid;//Listele formundan gelen id
        }

        private void Duzeltme_Load(object sender, EventArgs e)
        {
            //Linq to Sql nesnelerini oluştur
            RehberClasses1DataContext vtab = new RehberClasses1DataContext();
            rehber duzeltilecek = new rehber();
            //Linq satırı
            duzeltilecek = vtab.rehber.Single(idno => idno.Id == kontrolid);//Bir satır dönecek
            textBox1.Text = duzeltilecek.Adsoyad;
            textBox2.Text = duzeltilecek.Eposta;
            textBox3.Text = duzeltilecek.Telno;
            textBox4.Text = duzeltilecek.Sehir;
            textBox5.Text = duzeltilecek.Adres;
            toolStripStatusLabel1.Text = "Id Numarası:" + kontrolid;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Kaydetme
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == ""
                || textBox5.Text == "")
            {
                return;
            }
            DialogResult cevap = MessageBox.Show("Bu kayıt düzeltilsin mi? ", "Düzeltme", MessageBoxButtons.YesNo,
            MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
            if (cevap == DialogResult.Yes)
            {
                //Linq to Sql nesneleri
                RehberClasses1DataContext vtab = new RehberClasses1DataContext();
                rehber duzeltilecek = new rehber();
                //Linq satırı
                duzeltilecek = vtab.rehber.Single(idno => idno.Id == kontrolid);
                duzeltilecek.Adsoyad = textBox1.Text.Trim();
                duzeltilecek.Eposta = textBox2.Text.Trim();
                duzeltilecek.Telno = textBox3.Text.Trim();
                duzeltilecek.Sehir = textBox4.Text.Trim();
                duzeltilecek.Adres = textBox5.Text.Trim();
                try
                {
                    vtab.SubmitChanges();//Fiziksel olarak veri tabanı yaz.
                }
                catch (Exception hata)
                {
                    toolStripStatusLabel1.Text = "Hata oluştu. Hata:" + hata.Message;
                    return;
                }
                MessageBox.Show(kontrolid + " Kimlik numaralı kayıt güncellendi");
                this.Hide();//Aktif formu gizle                
            }
        }
    }
}

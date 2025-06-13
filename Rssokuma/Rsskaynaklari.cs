using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Rssokuma
{
    public partial class Rsskaynaklari : Form
    {
        public Rsskaynaklari()
        {
            InitializeComponent();
        }
        //Genel nesne/değişken
        SQLiteDataAdapter veriadaptoru;
        DataSet verikumesi = new DataSet();
        DataSet verikumesi2;//Güncelleme için kullanılacak
        SQLiteCommandBuilder komutolusturucu;//Sql komutlarını oluşturur.

        private void Rsskaynaklari_Load(object sender, EventArgs e)
        {
            Datagrideveriyukle("Select * from rsskaynak Order By kaynakadi");
        }

        private void Datagrideveriyukle(string sql)
        {
            //veriadaptoru hazırla
            veriadaptoru = new SQLiteDataAdapter(sql, Form1.bag);
            //Gridview'e yükle
            veriadaptoru.Fill(verikumesi, "rsskaynak");//Verileri doldur
            dataGridView1.DataSource = verikumesi.Tables[0];
            //Sütunları ayarla
            dataGridView1.Columns[0].HeaderText = "Kimlik";
            dataGridView1.Columns[0].Width = 35;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].HeaderText = "Kaynağın Adı";
            dataGridView1.Columns[1].Width = 250;
            dataGridView1.Columns[2].HeaderText = "Kaynak Adresi (Rss)";
            dataGridView1.Columns[2].Width = 350;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Datagridview içerisindeki değişiklikleri kaydeder
            if (verikumesi.HasChanges())//Eğer verikumesi'nde değişiklik varsa
            {
                DialogResult cevap = MessageBox.Show("Kayıt Edilsin mi?", "Kayıt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (cevap == DialogResult.Yes)
                {
                    komutolusturucu = new SQLiteCommandBuilder(veriadaptoru);
                    verikumesi2 = verikumesi.GetChanges();//Aktif değişiklikleri al
                    try
                    {
                        veriadaptoru.Update(verikumesi2, "rsskaynak");//Veritabanına yazma
                    }
                    catch (Exception hata)
                    {
                        MessageBox.Show("Kayıt esnasında hata oluştu. Hata:" + hata.Message);
                        return;
                    }
                    MessageBox.Show("Değişiklikler Kaydedildi", "Kayıt Onayı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }//if
            }//if
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Metin kutusundan girilen bilgileri tabloda arama
            // Öncelikle dataset içindeki eski verileri temizle
            verikumesi.Tables["rsskaynak"].Clear();
            Datagrideveriyukle("SELECT * FROM rsskaynak WHERE kaynakadi LIKE '%" + textBox1.Text + "%' ORDER BY kaynakadi;");
        }
    }
}

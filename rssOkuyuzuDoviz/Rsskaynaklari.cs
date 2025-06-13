using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rssokuyucu
{
	public partial class Rsskaynaklari : Form
	{
		public Rsskaynaklari()
		{
			InitializeComponent();
		}
		SQLiteDataAdapter veriadaptoru;
		DataSet verikumesi = new DataSet();
		DataSet verikumesi2;
		SQLiteCommandBuilder komutolusturucu;

		private void Rsskaynaklari_Load(object sender, EventArgs e)
		{
			veriadaptoru = new SQLiteDataAdapter("Select * From xmlkaynaklari", Form1.bag);						
			//Grid'e veri yükle			
			veriadaptoru.Fill(verikumesi, "xmlkaynaklari");
			dataGridView1.DataSource = verikumesi.Tables[0];
			//Sütun başlıkları
			dataGridView1.Columns[0].HeaderText = "No";
			dataGridView1.Columns[0].Width = 35;
			dataGridView1.Columns[0].ReadOnly = true;
			dataGridView1.Columns[1].HeaderText = "Kaynağın Adı";
			dataGridView1.Columns[1].Width = 200;
			dataGridView1.Columns[2].HeaderText = "Adresi";
			dataGridView1.Columns[2].Width = 320;
			dataGridView1.ShowCellToolTips = true;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//Değişiklikleri veri tabanına aktarır
			if (verikumesi.HasChanges()) //Veri kümesinde değişiklik olursa
			{
				DialogResult cevap = MessageBox.Show("Kayıt Edilsin mi?", "Kayıt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if(cevap == DialogResult.Yes)
				{
					komutolusturucu = new SQLiteCommandBuilder(veriadaptoru);
					verikumesi2 = verikumesi.GetChanges();
					if (verikumesi2 != null)
					{
						try
						{
							veriadaptoru.Update(verikumesi2, "xmlkaynaklari");
						}
						catch (Exception hata)
						{
							MessageBox.Show("Kayıt işleminde hata oluştu. Hata:" + hata.Message);
							return;
						}
						MessageBox.Show("Değişiklikler Kaydedildi", "Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}//if
				}//if cevap
			}//if verikumesi		

		}
	}
}

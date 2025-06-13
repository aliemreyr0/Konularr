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

namespace Rehberrapor
{
    public partial class TumkayitlarRaporformu : Form
    {
        public TumkayitlarRaporformu()
        {
            InitializeComponent();
        }
        //Genel değişken
        private string sql = "";

        public TumkayitlarRaporformu(string sqlsatiri)
        {
            sql = sqlsatiri;//sqlsatiri değişkeni, Anaform'dan bilgiyi almaktadır.
            InitializeComponent();
        }

        private void TumkayitlarRaporformu_Load(object sender, EventArgs e)
        {
            SqlDataAdapter adap1 = new SqlDataAdapter(sql, Anaform.baglanti);
            adap1.Fill(veriler1DataSet, "rehber");          
            
            //this.rehberTableAdapter.Fill(this.veriler1DataSet.rehber);
            
            this.reportViewer1.RefreshReport();//Raporu göster
        }
    }
}

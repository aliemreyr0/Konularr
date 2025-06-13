namespace Rehberrapor
{
    partial class TumkayitlarRaporformu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.veriler1DataSet = new Rehberrapor.veriler1DataSet();
            this.rehberBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rehberTableAdapter = new Rehberrapor.veriler1DataSetTableAdapters.rehberTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.veriler1DataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rehberBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.rehberBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Rehberrapor.Rehberlistesiraporu.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(800, 450);
            this.reportViewer1.TabIndex = 0;
            // 
            // veriler1DataSet
            // 
            this.veriler1DataSet.DataSetName = "veriler1DataSet";
            this.veriler1DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // rehberBindingSource
            // 
            this.rehberBindingSource.DataMember = "rehber";
            this.rehberBindingSource.DataSource = this.veriler1DataSet;
            // 
            // rehberTableAdapter
            // 
            this.rehberTableAdapter.ClearBeforeFill = true;
            // 
            // TumkayitlarRaporformu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.reportViewer1);
            this.Name = "TumkayitlarRaporformu";
            this.Text = "Tüm Kayıtlar";
            this.Load += new System.EventHandler(this.TumkayitlarRaporformu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.veriler1DataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rehberBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource rehberBindingSource;
        private veriler1DataSet veriler1DataSet;
        private veriler1DataSetTableAdapters.rehberTableAdapter rehberTableAdapter;
    }
}
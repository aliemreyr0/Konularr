namespace Kitaplar_adap
{
    partial class Anaform
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer üretilen kod

		/// <summary>
		/// Tasarımcı desteği için gerekli metot - bu metodun 
		///içeriğini kod düzenleyici ile değiştirmeyin.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			statusStrip1 = new System.Windows.Forms.StatusStrip();
			toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			button1 = new System.Windows.Forms.Button();
			dataGridView1 = new System.Windows.Forms.DataGridView();
			textBox1 = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			toolTip1 = new System.Windows.Forms.ToolTip(components);
			listView1 = new System.Windows.Forms.ListView();
			columnHeader2 = new System.Windows.Forms.ColumnHeader();
			columnHeader3 = new System.Windows.Forms.ColumnHeader();
			columnHeader4 = new System.Windows.Forms.ColumnHeader();
			columnHeader5 = new System.Windows.Forms.ColumnHeader();
			columnHeader6 = new System.Windows.Forms.ColumnHeader();
			columnHeader12 = new System.Windows.Forms.ColumnHeader();
			button2 = new System.Windows.Forms.Button();
			button3 = new System.Windows.Forms.Button();
			button4 = new System.Windows.Forms.Button();
			button5 = new System.Windows.Forms.Button();
			printDocument1 = new System.Drawing.Printing.PrintDocument();
			printDialog1 = new System.Windows.Forms.PrintDialog();
			statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
			SuspendLayout();
			// 
			// statusStrip1
			// 
			statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1 });
			statusStrip1.Location = new System.Drawing.Point(0, 478);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			statusStrip1.Size = new System.Drawing.Size(765, 22);
			statusStrip1.TabIndex = 14;
			statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
			toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// button1
			// 
			button1.Location = new System.Drawing.Point(609, 5);
			button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(146, 27);
			button1.TabIndex = 13;
			button1.Text = "Kitabı &Kaydet";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// dataGridView1
			// 
			dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.Location = new System.Drawing.Point(14, 37);
			dataGridView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			dataGridView1.Name = "dataGridView1";
			dataGridView1.Size = new System.Drawing.Size(740, 197);
			dataGridView1.TabIndex = 12;
			dataGridView1.CellContentClick += dataGridView1_CellContentClick;
			dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
			// 
			// textBox1
			// 
			textBox1.Location = new System.Drawing.Point(91, 7);
			textBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(116, 23);
			textBox1.TabIndex = 11;
			textBox1.TextChanged += textBox1_TextChanged;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 162);
			label1.Location = new System.Drawing.Point(14, 10);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(66, 13);
			label1.TabIndex = 10;
			label1.Text = "Kitap Adı :";
			// 
			// toolTip1
			// 
			toolTip1.AutoPopDelay = 5000;
			toolTip1.InitialDelay = 5;
			toolTip1.IsBalloon = true;
			toolTip1.ReshowDelay = 100;
			toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			toolTip1.ToolTipTitle = "Bilgi";
			// 
			// listView1
			// 
			listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6, columnHeader12 });
			listView1.FullRowSelect = true;
			listView1.Location = new System.Drawing.Point(17, 274);
			listView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			listView1.Name = "listView1";
			listView1.Size = new System.Drawing.Size(737, 164);
			listView1.TabIndex = 15;
			listView1.UseCompatibleStateImageBehavior = false;
			listView1.View = System.Windows.Forms.View.Details;
			listView1.ColumnClick += listView1_ColumnClick;
			listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
			// 
			// columnHeader2
			// 
			columnHeader2.Text = "Kitap No";
			columnHeader2.Width = 61;
			// 
			// columnHeader3
			// 
			columnHeader3.Text = "İşlem Türü";
			columnHeader3.Width = 74;
			// 
			// columnHeader4
			// 
			columnHeader4.Text = "Fiyat";
			columnHeader4.Width = 67;
			// 
			// columnHeader5
			// 
			columnHeader5.Text = "Adet";
			columnHeader5.Width = 68;
			// 
			// columnHeader6
			// 
			columnHeader6.Text = "Açıklama";
			columnHeader6.Width = 143;
			// 
			// columnHeader12
			// 
			columnHeader12.Text = "Kayıt Tarihi";
			columnHeader12.Width = 159;
			// 
			// button2
			// 
			button2.Location = new System.Drawing.Point(14, 444);
			button2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(146, 27);
			button2.TabIndex = 16;
			button2.Text = "&Hareket Ekle";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// button3
			// 
			button3.Location = new System.Drawing.Point(167, 444);
			button3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			button3.Name = "button3";
			button3.Size = new System.Drawing.Size(142, 27);
			button3.TabIndex = 17;
			button3.Text = "Düzelt";
			button3.UseVisualStyleBackColor = true;
			button3.Click += button3_Click;
			// 
			// button4
			// 
			button4.Location = new System.Drawing.Point(316, 444);
			button4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			button4.Name = "button4";
			button4.Size = new System.Drawing.Size(142, 27);
			button4.TabIndex = 18;
			button4.Text = "Kaydi Sil";
			button4.UseVisualStyleBackColor = true;
			button4.Click += button4_Click;
			// 
			// button5
			// 
			button5.Location = new System.Drawing.Point(514, 3);
			button5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			button5.Name = "button5";
			button5.Size = new System.Drawing.Size(88, 27);
			button5.TabIndex = 19;
			button5.Text = "Yazdır";
			button5.UseVisualStyleBackColor = true;
			button5.Click += button5_Click;
			// 
			// printDocument1
			// 
			printDocument1.PrintPage += printDocument1_PrintPage;
			// 
			// printDialog1
			// 
			printDialog1.UseEXDialog = true;
			// 
			// Anaform
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(765, 500);
			Controls.Add(button5);
			Controls.Add(button4);
			Controls.Add(button3);
			Controls.Add(button2);
			Controls.Add(listView1);
			Controls.Add(statusStrip1);
			Controls.Add(button1);
			Controls.Add(dataGridView1);
			Controls.Add(textBox1);
			Controls.Add(label1);
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "Anaform";
			Text = "Kitaplar";
			Activated += Anaform_Activated;
			FormClosing += Anaform_FormClosing;
			Load += Anaform_Load;
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintDialog printDialog1;
    }
}


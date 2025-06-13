namespace Rssokuyucu
{
	partial class Rsskaynaklari
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
			dataGridView1 = new System.Windows.Forms.DataGridView();
			button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
			SuspendLayout();
			// 
			// dataGridView1
			// 
			dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.Location = new System.Drawing.Point(0, 35);
			dataGridView1.Name = "dataGridView1";
			dataGridView1.RowHeadersWidth = 51;
			dataGridView1.RowTemplate.Height = 29;
			dataGridView1.Size = new System.Drawing.Size(613, 290);
			dataGridView1.TabIndex = 0;
			// 
			// button1
			// 
			button1.Location = new System.Drawing.Point(3, 2);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(180, 29);
			button1.TabIndex = 1;
			button1.Text = "Değişiklikleri Kaydet";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// Rsskaynaklari
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(612, 326);
			Controls.Add(button1);
			Controls.Add(dataGridView1);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "Rsskaynaklari";
			Text = "Rss Kaynakları";
			Load += Rsskaynaklari_Load;
			((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Button button1;
	}
}
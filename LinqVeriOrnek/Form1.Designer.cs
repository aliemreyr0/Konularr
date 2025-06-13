namespace LinqVeriOrnek
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            listBox1 = new ListBox();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            button11 = new Button();
            button12 = new Button();
            button13 = new Button();
            button14 = new Button();
            button15 = new Button();
            button16 = new Button();
            button17 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 20);
            button1.Name = "button1";
            button1.Size = new Size(115, 23);
            button1.TabIndex = 0;
            button1.Text = "Öğrenciler (Sorgu)";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 165);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(664, 259);
            listBox1.TabIndex = 1;
            // 
            // button2
            // 
            button2.Location = new Point(12, 49);
            button2.Name = "button2";
            button2.Size = new Size(115, 23);
            button2.TabIndex = 2;
            button2.Text = "Ürünler (Metot)";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(12, 78);
            button3.Name = "button3";
            button3.Size = new Size(115, 23);
            button3.TabIndex = 3;
            button3.Text = "Kitaplar (Sorgu)";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(133, 20);
            button4.Name = "button4";
            button4.Size = new Size(122, 23);
            button4.TabIndex = 4;
            button4.Text = "Gruplama (Öğrenci)";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(133, 78);
            button5.Name = "button5";
            button5.Size = new Size(122, 23);
            button5.TabIndex = 5;
            button5.Text = "SelectMany";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(261, 49);
            button6.Name = "button6";
            button6.Size = new Size(144, 23);
            button6.TabIndex = 6;
            button6.Text = "Join";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new Point(261, 20);
            button7.Name = "button7";
            button7.Size = new Size(144, 23);
            button7.TabIndex = 7;
            button7.Text = "Join (Öğrenci/Dersler)";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Location = new Point(133, 49);
            button8.Name = "button8";
            button8.Size = new Size(122, 23);
            button8.TabIndex = 8;
            button8.Text = "Gruplama (Ürün)";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Location = new Point(261, 78);
            button9.Name = "button9";
            button9.Size = new Size(144, 23);
            button9.TabIndex = 9;
            button9.Text = "Count (Öğrenci)";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button10
            // 
            button10.Location = new Point(12, 107);
            button10.Name = "button10";
            button10.Size = new Size(115, 23);
            button10.TabIndex = 10;
            button10.Text = "Kişiler (Sorgu)";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // button11
            // 
            button11.Location = new Point(133, 107);
            button11.Name = "button11";
            button11.Size = new Size(122, 23);
            button11.TabIndex = 11;
            button11.Text = "SelectMany";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
            // 
            // button12
            // 
            button12.Location = new Point(261, 107);
            button12.Name = "button12";
            button12.Size = new Size(144, 23);
            button12.TabIndex = 12;
            button12.Text = "SelectMany";
            button12.UseVisualStyleBackColor = true;
            button12.Click += button12_Click;
            // 
            // button13
            // 
            button13.Location = new Point(12, 136);
            button13.Name = "button13";
            button13.Size = new Size(115, 23);
            button13.TabIndex = 13;
            button13.Text = "Personeller";
            button13.UseVisualStyleBackColor = true;
            button13.Click += button13_Click;
            // 
            // button14
            // 
            button14.Location = new Point(133, 136);
            button14.Name = "button14";
            button14.Size = new Size(122, 23);
            button14.TabIndex = 14;
            button14.Text = "FirstOrDefault";
            button14.UseVisualStyleBackColor = true;
            button14.Click += button14_Click;
            // 
            // button15
            // 
            button15.Location = new Point(411, 49);
            button15.Name = "button15";
            button15.Size = new Size(144, 23);
            button15.TabIndex = 15;
            button15.Text = "ElementAt";
            button15.UseVisualStyleBackColor = true;
            button15.Click += button15_Click;
            // 
            // button16
            // 
            button16.Location = new Point(261, 136);
            button16.Name = "button16";
            button16.Size = new Size(144, 23);
            button16.TabIndex = 16;
            button16.Text = "LastOrDefault";
            button16.UseVisualStyleBackColor = true;
            button16.Click += button16_Click;
            // 
            // button17
            // 
            button17.Location = new Point(411, 20);
            button17.Name = "button17";
            button17.Size = new Size(144, 23);
            button17.TabIndex = 17;
            button17.Text = "SingleOrDefault";
            button17.UseVisualStyleBackColor = true;
            button17.Click += button17_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(690, 439);
            Controls.Add(button17);
            Controls.Add(button16);
            Controls.Add(button15);
            Controls.Add(button14);
            Controls.Add(button13);
            Controls.Add(button12);
            Controls.Add(button11);
            Controls.Add(button10);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(listBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Linq";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private ListBox listBox1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
        private Button button11;
        private Button button12;
        private Button button13;
        private Button button14;
        private Button button15;
        private Button button16;
        private Button button17;
    }
}

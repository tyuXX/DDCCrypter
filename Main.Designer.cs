namespace DDCCrypter
{
    partial class Main
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose( bool disposing )
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.pastOperationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notepadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cryptLibraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBox4 = new DDCCrypter.SizedComboBox();
            this.comboBox3 = new DDCCrypter.SizedComboBox();
            this.button6 = new DDCCrypter.SizedButton();
            this.richTextBox3 = new DDCCrypter.SizedRichTextBox();
            this.textBox1 = new DDCCrypter.SizedTextBox();
            this.button5 = new DDCCrypter.SizedButton();
            this.comboBox2 = new DDCCrypter.SizedComboBox();
            this.button4 = new DDCCrypter.SizedButton();
            this.button3 = new DDCCrypter.SizedButton();
            this.button2 = new DDCCrypter.SizedButton();
            this.listBox1 = new DDCCrypter.SizedListBox();
            this.button1 = new DDCCrypter.SizedButton();
            this.comboBox1 = new DDCCrypter.SizedComboBox();
            this.richTextBox2 = new DDCCrypter.SizedRichTextBox();
            this.richTextBox1 = new DDCCrypter.SizedRichTextBox();
            this.textBox2 = new DDCCrypter.SizedTextBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.AllowDrop = true;
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.toolStripSeparator1,
            this.pastOperationsToolStripMenuItem,
            this.notepadToolStripMenuItem,
            this.cryptLibraryToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 134);
            this.contextMenuStrip1.Text = "Extras";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(207, 6);
            // 
            // pastOperationsToolStripMenuItem
            // 
            this.pastOperationsToolStripMenuItem.Name = "pastOperationsToolStripMenuItem";
            this.pastOperationsToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.pastOperationsToolStripMenuItem.Text = "Past Operations";
            this.pastOperationsToolStripMenuItem.Click += new System.EventHandler(this.pastOperationsToolStripMenuItem_Click);
            // 
            // notepadToolStripMenuItem
            // 
            this.notepadToolStripMenuItem.Name = "notepadToolStripMenuItem";
            this.notepadToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.notepadToolStripMenuItem.Text = "Notepad";
            this.notepadToolStripMenuItem.Click += new System.EventHandler(this.notepadToolStripMenuItem_Click);
            // 
            // cryptLibraryToolStripMenuItem
            // 
            this.cryptLibraryToolStripMenuItem.Name = "cryptLibraryToolStripMenuItem";
            this.cryptLibraryToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.cryptLibraryToolStripMenuItem.Text = "Crypt Library";
            this.cryptLibraryToolStripMenuItem.Click += new System.EventHandler(this.cryptLibraryToolStripMenuItem_Click);
            // 
            // comboBox4
            // 
            this.comboBox4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.comboBox4.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.comboBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "trim"});
            this.comboBox4.Location = new System.Drawing.Point(93, 160);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.comboBox4.Size = new System.Drawing.Size(82, 24);
            this.comboBox4.TabIndex = 18;
            this.comboBox4.Text = "Destination Encoding";
            // 
            // comboBox3
            // 
            this.comboBox3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.comboBox3.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.comboBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "trim"});
            this.comboBox3.Location = new System.Drawing.Point(12, 160);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.comboBox3.Size = new System.Drawing.Size(75, 24);
            this.comboBox3.TabIndex = 17;
            this.comboBox3.Text = "Source Encoding";
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.SystemColors.GrayText;
            this.button6.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button6.Location = new System.Drawing.Point(181, 70);
            this.button6.Name = "button6";
            this.button6.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button6.Size = new System.Drawing.Size(120, 28);
            this.button6.TabIndex = 14;
            this.button6.Text = "Extras";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // richTextBox3
            // 
            this.richTextBox3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.richTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox3.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.richTextBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.richTextBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.richTextBox3.Location = new System.Drawing.Point(307, 12);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.Size = new System.Drawing.Size(109, 253);
            this.richTextBox3.TabIndex = 13;
            this.richTextBox3.Text = "Status:";
            this.richTextBox3.WordWrap = false;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.textBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox1.Location = new System.Drawing.Point(12, 160);
            this.textBox1.Name = "textBox1";
            this.textBox1.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.textBox1.Size = new System.Drawing.Size(163, 22);
            this.textBox1.TabIndex = 16;
            this.textBox1.Text = "Hash";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.GrayText;
            this.button5.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button5.Location = new System.Drawing.Point(181, 41);
            this.button5.Name = "button5";
            this.button5.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button5.Size = new System.Drawing.Size(120, 28);
            this.button5.TabIndex = 10;
            this.button5.Text = "Decode";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.comboBox2.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "trim"});
            this.comboBox2.Location = new System.Drawing.Point(12, 130);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.comboBox2.Size = new System.Drawing.Size(163, 24);
            this.comboBox2.TabIndex = 9;
            this.comboBox2.Text = "Arg";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.button4.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button4.Location = new System.Drawing.Point(181, 237);
            this.button4.Name = "button4";
            this.button4.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button4.Size = new System.Drawing.Size(120, 28);
            this.button4.TabIndex = 8;
            this.button4.Text = "Clear (Arg)";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.button3.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button3.Location = new System.Drawing.Point(181, 208);
            this.button3.Name = "button3";
            this.button3.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button3.Size = new System.Drawing.Size(120, 28);
            this.button3.TabIndex = 7;
            this.button3.Text = "Remove (Arg)";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.button2.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.Location = new System.Drawing.Point(181, 179);
            this.button2.Name = "button2";
            this.button2.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button2.Size = new System.Drawing.Size(120, 28);
            this.button2.TabIndex = 6;
            this.button2.Text = "Add (Arg)";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.listBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(181, 108);
            this.listBox1.Name = "listBox1";
            this.listBox1.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.listBox1.Size = new System.Drawing.Size(120, 66);
            this.listBox1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.GrayText;
            this.button1.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(181, 12);
            this.button1.Name = "button1";
            this.button1.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button1.Size = new System.Drawing.Size(120, 28);
            this.button1.TabIndex = 3;
            this.button1.Text = "Encode";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.comboBox1.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 100);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.comboBox1.Size = new System.Drawing.Size(163, 24);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.Text = "Type";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox2.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.richTextBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.richTextBox2.Location = new System.Drawing.Point(12, 188);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.richTextBox2.Size = new System.Drawing.Size(163, 77);
            this.richTextBox2.TabIndex = 1;
            this.richTextBox2.Text = "Text to Decode";
            this.richTextBox2.TextChanged += new System.EventHandler(this.richTextBox2_TextChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.richTextBox1.Size = new System.Drawing.Size(163, 82);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "Text to Encode";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.ContainerOrigin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.textBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox2.Location = new System.Drawing.Point(93, 160);
            this.textBox2.Name = "textBox2";
            this.textBox2.Origin = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.textBox2.Size = new System.Drawing.Size(82, 22);
            this.textBox2.TabIndex = 12;
            this.textBox2.Text = "Hash2";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(428, 281);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.SizeChanged += new System.EventHandler(this.Main_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SizedRichTextBox richTextBox1;
        private SizedComboBox comboBox1;
        private SizedButton button1;
        private SizedListBox listBox1;
        private SizedButton button2;
        private SizedButton button3;
        private SizedButton button4;
        private SizedComboBox comboBox2;
        public SizedRichTextBox richTextBox2;
        private SizedButton button5;
        private SizedTextBox textBox1;
        private SizedRichTextBox richTextBox3;
        private SizedButton button6;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem pastOperationsToolStripMenuItem;
        private SizedTextBox textBox2;
        private SizedComboBox comboBox3;
        private SizedComboBox comboBox4;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem notepadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cryptLibraryToolStripMenuItem;
    }
}


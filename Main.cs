﻿namespace DDCCrypter
{
    public partial class Main : Form
    {
        bool init = false;
        public Main()
        {
            InitializeComponent();
            foreach (IResizeable control in Controls)
            {
                control.SetSize();
            }
            init = true;
        }

        private void button2_Click( object sender, EventArgs e )
        {
            if ((!listBox1.Items.Contains( comboBox2.Text )) && string.IsNullOrEmpty( comboBox2.Text ))
            {
                listBox1.Items.Add( comboBox2.Text );
            }
            comboBox2.Text = "";
        }

        private void button4_Click( object sender, EventArgs e ) => listBox1.Items.Clear();

        private void button3_Click( object sender, EventArgs e )
        {
            List<string> _ = new() { };
            foreach (string str in listBox1.SelectedItems)
            {
                _.Add( str );
            }
            foreach (string str in _)
            {
                listBox1.Items.Remove( str );
            }
        }

        private void Main_Load( object sender, EventArgs e )
        {
            comboBox3.Hide();
            comboBox4.Hide();
            foreach (Crypter crypter in Engine.crypters)
            {
                comboBox1.Items.Add( crypter.Description.ID );
            }
            foreach (EncodingInfo encoding in Encoding.GetEncodings())
            {
                comboBox3.Items.Add( encoding.Name );
                comboBox4.Items.Add( encoding.Name );
            }
        }

        private void richTextBox2_TextChanged( object sender, EventArgs e )
        {

        }

        private void button1_Click( object sender, EventArgs e ) => Send( true );

        private void button5_Click( object sender, EventArgs e ) => Send( false );
        private void Send( bool od )
        {
            string _hash = "";
            List<string> _ = new() { };
            foreach (string str in listBox1.Items)
            {
                _.Add( str );
            }
            _.Add( "type=" + comboBox1.Text.Replace( " ", string.Empty ).Replace( "\n", string.Empty ).ToUpper() );
            _.Add( "do=" + od );
            if (Engine.OrCompare( textBox1.Text, "Hash", string.Empty ))
            {
                textBox1.Text = Engine.GenerateHash();
            }
            _.Add( "hash=" + textBox1.Text );
            _hash = textBox1.Text;
            if (Engine.OrCompare( textBox2.Text, "Hash2", string.Empty ))
            {
                textBox2.Text = Engine.GenerateHash();
            }
            _.Add( "hash2=" + textBox2.Text );
            _hash = textBox2.Text;
            if (od)
            {
                _.Add( "estring=" + richTextBox1.Text );
                _.Add( "encoding1=" + comboBox3.Text );
                _.Add( "encoding2=" + comboBox4.Text );
                (string, TimeSpan, bool, ArgStore) output = Engine.Process( _ );
                Engine.Ops.Add( new Operation( output ) );
                richTextBox2.Text = output.Item1;
                richTextBox3.Text = $"Status:\nLenght:{richTextBox2.TextLength:N0}\nHash:{_hash}\nTime:{output.Item2.Milliseconds}ms\nSucksess:{output.Item3}";
            }
            else
            {
                _.Add( "estring=" + richTextBox2.Text );
                _.Add( "encoding1=" + comboBox4.Text );
                _.Add( "encoding2=" + comboBox3.Text );
                (string, TimeSpan, bool, ArgStore) output = Engine.Process( _ );
                Engine.Ops.Add( new Operation( output ) );
                richTextBox1.Text = output.Item1;
                richTextBox3.Text = $"Status:\nLenght:{richTextBox1.TextLength:N0}\nHash:{_hash}\nTime:{output.Item2.Milliseconds}ms\nSucksess:{output.Item3}";
            }
        }

        private void button6_Click( object sender, EventArgs e ) => contextMenuStrip1.Show( this, button6.Location );

        private void pastOperationsToolStripMenuItem_Click( object sender, EventArgs e ) => Engine.OpenForm<PastOperations>();

        private void comboBox1_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (Engine.GetCrypter( comboBox1.Text ).Type == CrypterType.DualInput)
            {
                textBox1.Width = 62;
            }
            else
            {
                textBox1.Width = 123;
            }
            if (Engine.GetCrypter( comboBox1.Text ).Type == CrypterType.Encoding)
            {
                comboBox3.Show();
                comboBox4.Show();
            }
            else
            {
                comboBox3.Hide();
                comboBox4.Hide();
            }
        }

        private void Main_SizeChanged( object sender, EventArgs e )
        {
            if (init)
            {
                foreach (IResizeable control in Controls)
                {
                    Engine.Resize( this, control );
                }
            }
        }

        private void notepadToolStripMenuItem_Click( object sender, EventArgs e ) => Engine.OpenForm<TextEditor>();

        private void cryptLibraryToolStripMenuItem_Click( object sender, EventArgs e ) => Engine.OpenForm<CryptLibrary>();

        private void aboutToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Engine.OpenForm<About>();
        }
    }
}

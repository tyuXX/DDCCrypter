﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DDCCrypter
{
    public partial class Main : Form
    {
        const string defaulthash = "DDCCrypter$PID";
        public Main()
        {
            InitializeComponent();
        }

        private void button2_Click( object sender, EventArgs e )
        {
            if ((!listBox1.Items.Contains( comboBox2.Text )) && string.IsNullOrEmpty( comboBox2.Text ))
            {
                listBox1.Items.Add( comboBox2.Text );
            }
            comboBox2.Text = "";
        }

        private void button4_Click( object sender, EventArgs e )
        {
            listBox1.Items.Clear();
        }

        private void button3_Click( object sender, EventArgs e )
        {
            List<string> _ = new List<string> { };
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

        }

        private void richTextBox2_TextChanged( object sender, EventArgs e )
        {

        }

        private void button1_Click( object sender, EventArgs e )
        {
            Send( true );
        }

        private void button5_Click( object sender, EventArgs e )
        {
            Send( false );
        }
        private void Send( bool od )
        {
            string _hash = "";
            List<string> _ = new List<string> { };
            foreach (string str in listBox1.Items)
            {
                _.Add( str );
            }
            _.Add( "type=" + comboBox1.Text.Replace( " ", string.Empty ).Replace( "\n", string.Empty ).ToUpper() );
            _.Add( "do=" + od );
            if (textBox1.Text == "Hash")
            {
                _.Add( "hash=" + defaulthash );
                _hash = defaulthash;
            }
            else
            {
                _.Add( "hash=" + textBox1.Text );
                _hash = textBox1.Text;
            }
            if (od)
            {
                _.Add( "estring=" + richTextBox1.Text );
                (string, TimeSpan) output = Engine.Process( _ );
                richTextBox2.Text = output.Item1;
                richTextBox3.Text = $"Status:\nLenght:{richTextBox2.TextLength}\nHash:{_hash}\nTime:{output.Item2.Milliseconds}ms";
            }
            else
            {
                _.Add( "estring=" + richTextBox2.Text );
                (string, TimeSpan) output = Engine.Process( _ );
                richTextBox1.Text = output.Item1;
                richTextBox3.Text = $"Status:\nLenght:{richTextBox1.TextLength}\nHash:{_hash}\nTime:{output.Item2.Milliseconds}ms";
            }
        }
    }
}

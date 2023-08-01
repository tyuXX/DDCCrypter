using System;
using System.Windows.Forms;

namespace DDCCrypter
{
    public partial class TextEditor : Form
    {
        public string file;
        public string passcode;
        public TextEditor()
        {
            InitializeComponent();
        }
        public TextEditor( string sfile, string passcode )
        {
            file = sfile;
            this.passcode = passcode;
            InitializeComponent();
        }
        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Application.Exit();
        }
    }
}

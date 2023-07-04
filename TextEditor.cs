using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public TextEditor(string sfile, string passcode )
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

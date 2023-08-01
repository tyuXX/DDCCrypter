using System;
using System.Windows.Forms;

namespace DDCCrypter
{
    public partial class Opener : Form
    {
        public Opener()
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            Engine.OpenForm<PastOperations>();
        }
    }
}

using System;
using System.Linq;
using System.Windows.Forms;

namespace DDCCrypter
{
    public partial class PastOperations : Form
    {
        int index = 0;
        public PastOperations()
        {
            index = Engine.Ops.Count - 1;
            InitializeComponent();
        }
        private void Refresh()
        {
            Text = $"{nameof( PastOperations )}:{index + 1}/{Engine.Ops.Count}";
            richTextBox1.Text = Engine.Ops.ElementAt( index ).ToString();
        }

        private void button3_Click( object sender, EventArgs e )
        {
            Refresh();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            if (index != 0)
            {
                index--;
            }
            Refresh();
        }

        private void button2_Click( object sender, EventArgs e )
        {
            if (index != Engine.Ops.Count - 1)
            {
                index++;
            }
            Refresh();
        }
    }
}

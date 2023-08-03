using System.Linq;

namespace DDCCrypter
{
    public partial class PastOperations : Form
    {
        bool init = false;
        int index = 0;
        public PastOperations()
        {
            index = Engine.Ops.Count - 1;
            InitializeComponent();
            foreach (IResizeable control in Controls)
            {
                control.SetSize();
            }
            init = true;
        }
        private void ERefresh()
        {
            Text = $"{nameof( PastOperations )}:{index + 1}/{Engine.Ops.Count}";
            try { richTextBox1.Text = Engine.Ops.ElementAt( index ).ToString(); } catch (Exception e) { richTextBox1.Text = e.Message; }
        }

        private void button3_Click( object sender, EventArgs e ) => Refresh();

        private void button1_Click( object sender, EventArgs e )
        {
            if (index > 0)
            {
                index--;
            }
            ERefresh();
        }

        private void button2_Click( object sender, EventArgs e )
        {
            if (index != Engine.Ops.Count - 1)
            {
                index++;
            }
            ERefresh();
        }

        private void PastOperations_SizeChanged( object sender, EventArgs e )
        {
            if (init)
            {
                foreach (IResizeable control in Controls)
                {
                    Engine.Resize( this, control );
                }
            }
        }
    }
}

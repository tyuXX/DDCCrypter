using System.Linq;

namespace DDCCrypter
{
    public partial class CryptLibrary : Form
    {
        bool init = false;
        int index = 0;
        public CryptLibrary()
        {
            InitializeComponent();
            foreach (IResizeable control in Controls)
            {
                control.SetSize();
            }
            init = true;
        }
        private void ERefresh()
        {
            Text = $"{nameof( CryptLibrary )}:{index + 1}/{Engine.crypters.Count}";
            try { sizedRichTextBox1.Text = "Description:\n" + Engine.crypters.ElementAt( index ).Description.Description; sizedTextBox2.Text = "Name:" + Engine.crypters.ElementAt( index ).Description.Name; sizedTextBox1.Text = "ID:" + Engine.crypters.ElementAt( index ).Description.ID; } catch (Exception e) { sizedRichTextBox1.Text = e.Message; }
        }
        private void sizedButton3_Click( object sender, EventArgs e ) => ERefresh();

        private void CryptLibrary_SizeChanged( object sender, EventArgs e )
        {
            if (init)
            {
                foreach (IResizeable control in Controls)
                {
                    Engine.Resize( this, control );
                }
            }
        }

        private void sizedButton2_Click( object sender, EventArgs e )
        {
            if (index != Engine.crypters.Count - 1)
            {
                index++;
            }
            ERefresh();
        }

        private void sizedButton1_Click( object sender, EventArgs e )
        {
            if (index > 0)
            {
                index--;
            }
            ERefresh();
        }

        private void CryptLibrary_Load( object sender, EventArgs e ) => ERefresh();
    }
}

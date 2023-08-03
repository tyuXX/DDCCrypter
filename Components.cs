using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDCCrypter
{
    public interface IResizeable
    {
        Rectangle Origin { get; set; }
        Rectangle ContainerOrigin { get; set; }
        Size Size { get; set; }
        Point Location { get; set; }
        void SetSize();
    }
    public class SizedButton : Button, IResizeable
    {
        public Rectangle Origin { get; set; }
        public Rectangle ContainerOrigin { get; set; }
        public void SetSize()
        {
            Origin = new Rectangle( Location, Size );
            ContainerOrigin = new Rectangle( Parent.Location, Parent.Size );
        }
    }
    public class SizedRichTextBox : RichTextBox, IResizeable
    {
        public Rectangle Origin { get; set; }
        public Rectangle ContainerOrigin { get; set; }
        public void SetSize()
        {
            Origin = new Rectangle( Location, Size );
            ContainerOrigin = new Rectangle( Parent.Location, Parent.Size );
        }
    }
    public class SizedTextBox : TextBox, IResizeable
    {
        public Rectangle Origin { get; set; }
        public Rectangle ContainerOrigin { get; set; }
        public void SetSize()
        {
            Origin = new Rectangle( Location, Size );
            ContainerOrigin = new Rectangle( Parent.Location, Parent.Size );
        }
    }
    public class SizedComboBox : ComboBox, IResizeable
    {
        public Rectangle Origin { get; set; }
        public Rectangle ContainerOrigin { get; set; }
        public void SetSize()
        {
            Origin = new Rectangle( Location, Size );
            ContainerOrigin = new Rectangle( Parent.Location, Parent.Size );
        }
    }
    public class SizedListBox : ListBox, IResizeable
    {
        public Rectangle Origin { get; set; }
        public Rectangle ContainerOrigin { get; set; }
        public void SetSize()
        {
            Origin = new Rectangle( Location, Size );
            ContainerOrigin = new Rectangle( Parent.Location, Parent.Size );
        }
    }
}

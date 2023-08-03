global using System;
global using System.Collections.Generic;
global using System.Text;
global using System.Windows.Forms;

namespace DDCCrypter
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new Main() );
        }
    }
}

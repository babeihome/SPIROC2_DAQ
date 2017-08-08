using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.IO;

namespace SPIROC_DAQ
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main_Form());

        
            //test code
            
            SC_model slowConfig = new SC_model();
            byte[] bit_block = new byte[117];
            slowConfig.set_property(settings.TRIG_EXT, 1);
            slowConfig.bit_transform(ref bit_block);
            
        }
    }
}

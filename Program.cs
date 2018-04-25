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
            /*
            AFG3252 signalSource = new AFG3252();
            signalSource.initial(settings.AFG_DESCR);
            signalSource.Write("*RCL 4");
            signalSource.openOutput();
            */
        }
    }
}

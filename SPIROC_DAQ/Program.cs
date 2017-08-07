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
            SC_model slowConfig = new SC_model();
            String filePath = Directory.GetCurrentDirectory() + "\\test.txt";
            FileStream fw = new FileStream(filePath, FileMode.Create);
            byte[] tmp = new byte[1000];
            slowConfig.bit_transform(ref tmp);
            byte[] data = System.Text.Encoding.Default.GetBytes(slowConfig.bit_string);
            fw.Write(data, 0, data.Length);
            
            fw.Flush();
            fw.Close();
            */
        }
    }
}

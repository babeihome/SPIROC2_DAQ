using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;

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
            // test code

            SC_model test_config_data1 = new SC_model();
            SC_model test_config_data2 = new SC_model();
            byte[] byte_config = new byte[SC_model.bit_length / 8 + 1];

            test_config_data1.set_property(1,100);
            test_config_data1.save_settings(2);
            test_config_data2.recall_settings(2);
            test_config_data2.bit_transform(ref byte_config);
            Console.WriteLine(test_config_data2.get_property(1).ToString());
            //test_config_data1.test();


            // test code2
            /*
            ushort[] test_int_array = new ushort[3] { 1, 16, 1023 };
            StringBuilder test_StringBuilder = new StringBuilder(30);
            test_StringBuilder.Append(Convert.ToString(1,2).PadLeft(8,'0'));
            //test_StringBuilder.Append(test_int_array[2]);

            Console.WriteLine(test_StringBuilder);
            */

            // test code3
            
        }
    }
}

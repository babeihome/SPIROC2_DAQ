using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SPIROC_DAQ
{
    class SC_model
    {
        // member variables
        private uint[] config_data;
        private static int properties_num = 175;
        public const int bit_length = 929;


        private const string cache_loc = ".\\cache\\";

        // const variable describe length of each config properties.
        // notice that Discriminator Mask config (36 bits) is divided to two group whose length is 18 bits 
        // It can be check or get from "Spiroc2abcd_chip.xls" file.
        private static readonly ushort[] property_length = new ushort[175] {1,1,1,1,12,8,1,1,1,1,1,2,1,1,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,
            9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,4,6,1,1,4,1,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,
            8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,1,3,1,1,3,1,1,1,1,1,1,1,1,1,10,10,1,6,1,1,1,1,1,1,1,
            1,18,18,1,1,6,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,1,1,
            1,6,1,6,1,1,1,1,1,1,1,1,1};


        // Initial method
        public SC_model()
        {
            // default settings
            config_data = new uint[175];
        }

        // test method redundant for any test function
        public void test()
        {
            int sum = 0;
            foreach (ushort len in property_length)
            {
                Console.WriteLine(sum);
                sum += len;
            }
            //Console.WriteLine(sum);
        }

        // need cooperate with PROPERTIES-ID table in settings.cs
        public void set_property(int id, uint value)
        {
            uint max = uint.MaxValue;
            max = max >> (32 - property_length[id]);
            if (value > max )
            {
                value = value & max;
            }
            config_data[id] = value;   
        }

        public uint get_property(int id)
        {
            return config_data[id];

        }

        public void bit_transform(ref byte[] bit_block)
        {
            // to record how many bit has been transformed
            int bit_count = 0;
            int byte_count = 0; //byte_count shold be bit_count/8;

            StringBuilder bit_As_Char = new StringBuilder(1000);
            String bit_string;
            for(int i = 0; i < 175; i++)
            {
                // 将配置的bit串用字符串的形式保存
                bit_As_Char.Append(Convert.ToString(config_data[i], 2).PadLeft(property_length[i], '0'));
            }
            bit_string = bit_As_Char.ToString();


            // transform 'bit in char form' into real bit stream
            while(bit_count + 8 < bit_length)
            {
                bit_block[byte_count] = Convert.ToByte(bit_string.Substring(bit_count, 8),2);
                byte_count++;
                bit_count += 8;
            }
            bit_block[byte_count] = Convert.ToByte(bit_string.Substring(bit_count, bit_length - bit_count).PadRight(8,'0'));

        }

        public void save_settings(int settings_id)
        {   
            // save SlowContorl
            // Serialize
            String cache_path = cache_loc + settings_id.ToString() + ".cache";

            if (!Directory.Exists(cache_loc))
                Directory.CreateDirectory(cache_loc);

            FileStream fileStream = new FileStream(cache_path, FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(fileStream, this.config_data);
            fileStream.Close();

        }
            
        public void recall_settings(int settings_id)
        {
            // load SlowControl saving config
            // Deserialize
            String cache_path = cache_loc + settings_id.ToString() + ".cache";

            if (!File.Exists(cache_path))
                throw new InvalidOperationException("Settings doesn't exist");


            FileStream fileStream = new FileStream(cache_path, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter b = new BinaryFormatter();
            this.config_data = b.Deserialize(fileStream) as uint[];
            fileStream.Close();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace SPIROC_DAQ
{
    
    interface Iversion
    {
        void test();
        void set_property(int id, uint value);
        uint get_property(int id);
        int bit_transform(ref byte[] bit_block);
        void save_settings(int settings_id);
        void recall_settings(int settings_id);
        string getTag();
        string settingName { get; set; }

    }

    [Serializable]
    class SC_model: Iversion 
    {
        // member variables
        private uint[] config_data;
        private static int properties_num = 175;
        public string settingName
        {
            get;
            set;
        }
        public const int bit_length = 929;
        public String bit_string;
        // hello
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
            settingName = "default";
            this.set_property(settings.TRIG_EXT, 0);
            this.set_property(settings.FLAG_TDC_EXT, 0);
            this.set_property(settings.START_RAMP_ADC_EXT, 0);
            this.set_property(settings.START_RAMP_TDC_EXT, 0);
            this.set_property(settings.ADC_GRAY, 1);
            this.set_property(settings.CHIPID, 0x80);
            this.set_property(settings.PROBE_OTA, 0);
            this.set_property(settings.ENABLE_ANALOGUE_OUTPUT, 1);
            this.set_property(settings.DISABLE_ANALOGUE_OUTPUT_PP, 1);
            this.set_property(settings.NC, 0);
            this.set_property(settings.EN_OR36, 1);
            this.set_property(settings.ADC_RAMP_SLOPE, 0);
            this.set_property(settings.ADC_RAMP_CURRENT_SOURCE, 0); // PP 
            this.set_property(settings.ADC_RAMP_INTEGRATOR, 0);     // PP
            for (int i = 0; i < 36; i++)
            {
                this.set_property(settings.INDAC[i], 0x1ff);
            }
            this.set_property(settings.CAP_HG_PA_COMPENSATION, 0x0f);
            this.set_property(settings.NC2, 0);
            this.set_property(settings.FS, 1);
            this.set_property(settings.NC3, 0);
            this.set_property(settings.CAP_LG_PA_COMPENSATION, 0x0e);
            this.set_property(settings.ENABLE_PREAMP_PP, 0);
            for (int i = 0; i < 36; i++)
            {
                this.set_property(settings.PREAMP_GAIN[i], 0xd8);
            }
            //disable channel 0
            //this.set_property(settings.PREAMP_GAIN[0], 0xec);
            this.set_property(settings.ENABLE_LG_SS_FOLLOWER_PP, 0);
            this.set_property(settings.LG_SS_TIME_CONSTANT, 0x04);
            this.set_property(settings.ENABLE_LG_SS_PP, 0);
            this.set_property(settings.ENABLE_HG_SS_FOLLOWER_PP, 0);
            this.set_property(settings.HG_SS_TIME_CONSTANT, 0x04);
            this.set_property(settings.ENABLE_HG_SS_PP, 0);
            this.set_property(settings.FS_FOLLOWER_PP, 0);
            this.set_property(settings.FS_PP, 0);
            this.set_property(settings.BACKUP_SCA, 0);
            this.set_property(settings.SCA_PP, 0);
            this.set_property(settings.EN_BANDGAP, 1);
            this.set_property(settings.BANDGAP_PP, 1);
            this.set_property(settings.EN_DAC, 1);
            this.set_property(settings.DAC_PP, 1);
            this.set_property(settings.TRIG_DAC, 0x0fa);
            this.set_property(settings.GAIN_DAC, 0x1f4);
            this.set_property(settings.DELAY_START_RAMP_TDC_PP, 0);
            this.set_property(settings.DELAY_START_RAMP_TDC, 1);
            this.set_property(settings.TDC_RAMP_SLOPE_GC, 0);
            this.set_property(settings.TDC_RAMP_PP, 0);
            this.set_property(settings.ADC_DISCRI_PP, 0);
            this.set_property(settings.GAIN_SELECT_DISCRI_PP, 0);
            this.set_property(settings.AUTO_GAIN, 0);
            this.set_property(settings.GAIN_SELECT, 0);
            this.set_property(settings.ADC_EXT_INPUT, 0);
            this.set_property(settings.SWITCH_TDC_ON, 1);
            this.set_property(settings.DISCRIMINATOR_MASK1, 0);
            this.set_property(settings.DISCRIMINATOR_MASK2, 0);
            this.set_property(settings.NC4, 0);
            this.set_property(settings.DISCRI_DELAY_PP, 1);
            this.set_property(settings.DELAY_TRIGGER, 0x02);

            for (int i = 0; i < 36; i++)
            {
                this.set_property(settings.DISCRI_4BIT_ADJUST[i], 0);
            }

            this.set_property(settings.ADJUST_4BIT_DAC, 0);
            this.set_property(settings.DAC_4BIT_PP, 0);
            this.set_property(settings.TRIG_DISCRI_PP, 0);
            this.set_property(settings.DELAY_VALIDHOLD_PP, 0);
            this.set_property(settings.DELAY_VALIDHOLD, 0x14);
            this.set_property(settings.DELAY_RSTCOL_PP, 0);
            this.set_property(settings.DELAY_RSTCOL, 0x14);
            this.set_property(settings.CLOCK_LVDS_RECEIVE, 0);
            this.set_property(settings.POD, 0);
            this.set_property(settings.END_READOUT, 1);
            this.set_property(settings.START_READOUT, 1);
            this.set_property(settings.CHIPSAT, 1);
            this.set_property(settings.TRANSMITON2, 1);
            this.set_property(settings.TRANSMITON1, 1);
            this.set_property(settings.DOUT2, 1);
            this.set_property(settings.DOUT1, 1);

        }


        // test method redundant for any test function
        public   void test()
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
        public  void set_property(int id, uint value)
        {
            uint max = uint.MaxValue;
            max = max >> (32 - property_length[id]);
            if (value > max )
            {
                value = value & max;
            }
            config_data[id] = value;   
        }

        public   uint get_property(int id)
        {
            return config_data[id];

        }

        public  int bit_transform(ref byte[] bit_block)
        {
            // to record how many bit has been transformed
            int bit_count = 0;
            int byte_count = 0; //byte_count shold be bit_count/8;

            StringBuilder bit_As_Char = new StringBuilder(1000);
            
            // now bit is as this
            // location 0   1   2   3   4   ... 13  14  15
            // bit      1   1   1   1   1   ... 1   x   x
            for(int i = 0; i < 175; i++)
            {
                // 将配置的bit串用字符串的形式保存
                bit_As_Char.Append(Convert.ToString(config_data[i], 2).PadLeft(property_length[i], '0'));
            }

            // reverse sequence of chars in bit_As_Char
            // now bit is as this
            // location 13  12  11  10  9   ... 3   2   1
            // bit      1   1   1   1   1   ... 1   1   1
            StringBuilder bitAsChar_MsbFirst = new StringBuilder(bit_length);
            for( int i = bit_length - 1; i>=0; i--)
            {
                bitAsChar_MsbFirst.Append(bit_As_Char[i]);
            }


            bit_string = bitAsChar_MsbFirst.ToString();


            // transform 'bit in char form' into real bit stream
            // MSB in byte is bigger conig bit
            while(bit_count + 8 < bit_length)
            {
                bit_block[byte_count] = Convert.ToByte(bit_string.Substring(bit_count, 8),2);
                byte_count++;
                bit_count += 8;
            }

            
            bit_block[byte_count] = Convert.ToByte(bit_string.Substring(bit_count, bit_length - bit_count).PadRight(8,'0'),2);

            // for example if congfig data is 1101 0100 10
            // so now the bit block is 0100 1010 1100
            // bit_block[0]: 0x4
            // bit_block[1]: 0xA
            // bit_block[2]: 0xC
            return byte_count+1;

        }

        public  void save_settings(int settings_id)
        {   
            // save SlowContorl
            // Serialize
            String cache_path = cache_loc + settings_id.ToString() + ".cache";

            if (!Directory.Exists(cache_loc))
                Directory.CreateDirectory(cache_loc);

            FileStream fileStream = new FileStream(cache_path, FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(fileStream, this);
            fileStream.Close();

        }
            
        public  void recall_settings(int settings_id)
        {
            // load SlowControl saving config
            // Deserialize
            String cache_path = cache_loc + settings_id.ToString() + ".cache";

            if (!File.Exists(cache_path))
            {
                throw new InvalidOperationException("Settings doesn't exist");
            }


            FileStream fileStream = new FileStream(cache_path, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter b = new BinaryFormatter();

            // restore config_data and settingName property
            var tmp = b.Deserialize(fileStream) as SC_model;
            tmp.config_data.CopyTo(this.config_data, 0);
            this.settingName = tmp.settingName;

            fileStream.Close();

        }

        public  string getTag()
        {
            string result;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("\tTrigger_Ext:\t" + this.get_property(settings.TRIG_EXT).ToString() );
            builder.AppendLine("\tPreamp 1:\t" + this.get_property(settings.PREAMP_GAIN[0]).ToString());
            builder.AppendLine("\tSwitch TDC On:\t" + this.get_property(settings.SWITCH_TDC_ON).ToString());
            builder.AppendLine("\tAuto Gain:\t" + this.get_property(settings.AUTO_GAIN).ToString());
            builder.AppendLine("\tGain Select:\t" + this.get_property(settings.GAIN_SELECT).ToString());
            builder.AppendLine("\tHigh Gain Shaper\t" + this.get_property(settings.HG_SS_TIME_CONSTANT).ToString());
            builder.AppendLine("\tTrigger Delay\t" + this.get_property(settings.DELAY_TRIGGER).ToString());
            builder.AppendLine("");
            result = builder.ToString();
            return result;
        }
    }

    class SC_model_2E : SC_model, Iversion 
    {
        private static readonly ushort[] property_length = new ushort[191] {1,1,1,1,1,1,12,8,1,1,1,1,1,2,1,1,1,1,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,
            9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,1,1,1,1,1,1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,1,1,3,1,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,10,10,1,1,1,1,1,1,1,1,1,18,18,1,1,8,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,1,1,4,1,6,1,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
        private static int properties_num = 191;
        new public const int bit_length = 1186;
        private uint[] config_data;
        private const string cache_loc = ".\\cache\\";

        // const variable describe length of each config properties.
        // notice that Discriminator Mask config (36 bits) is divided to two group whose length is 18 bits 
        // It can be check or get from "Spiroc2abcd_chip.xls" file.

        public SC_model_2E()
        {
            // default settings_2E
            config_data = new uint[191];
            settingName = "default";
            this.set_property(settings_2E.Sel_Temp_sensor_to_ADC_GC, 0);
            this.set_property(settings_2E.TRIG_EXT, 0);
            this.set_property(settings_2E.FLAG_TDC_EXT, 0);
            this.set_property(settings_2E.START_RAMP_ADC_EXT, 0);
            this.set_property(settings_2E.START_RAMP_TDC_EXT, 0);
            this.set_property(settings_2E.ADC_GRAY, 1);
            this.set_property(settings_2E.CHIPID, 0x80);
            this.set_property(settings_2E.PROBE_OTA, 0);
            this.set_property(settings_2E.ENABLE_ANALOGUE_OUTPUT, 1);
            this.set_property(settings_2E.DISABLE_ANALOGUE_OUTPUT_PP, 1);
            this.set_property(settings_2E.NC, 0);
            this.set_property(settings_2E.EN_OR36, 1);
            this.set_property(settings_2E.ADC_RAMP_SLOPE, 0);
            this.set_property(settings_2E.ADC_RAMP_CURRENT_SOURCE, 0); // PP 
            this.set_property(settings_2E.ADC_RAMP_INTEGRATOR, 0);     // PP
            this.set_property(settings_2E.EN_input_dac, 1);//?
            this.set_property(settings_2E.GC_8_bit_DAC_reference, 0);//?
            for (int i = 0; i < 36; i++)
            {
                this.set_property(settings_2E.INDAC[i], 0x1ff);
            }
            this.set_property(settings_2E.LG_PA_bias, 0);//?
            this.set_property(settings_2E.High_Gain_PreAmplifier, 0);//?
            this.set_property(settings_2E.Low_Gain_PreAmplifier, 0);//?
            this.set_property(settings_2E.EN_High_Gain_PA, 0);//?
            this.set_property(settings_2E.EN_Low_Gain_PA, 0);//?
            this.set_property(settings_2E.Fast_Shaper_on_LG, 0);//?
            this.set_property(settings_2E.NC2, 0);
            this.set_property(settings_2E.NC3, 0);

            for (int i = 0; i < 36; i++)
            {
                this.set_property(settings_2E.Channel_0_to_35_PA[i], 0xd8);
            }
            //disable channel 0
            //this.set_property(settings_2E.PREAMP_GAIN[0], 0xec);
            this.set_property(settings_2E.EN_Low_Gain_Slow_Shaper, 0);//?
            this.set_property(settings_2E.ENABLE_HG_SS, 0);//?
            this.set_property(settings_2E.EN_FS, 0);//?
            this.set_property(settings_2E.GC_Temp_sensor_high_current, 0);//?
            this.set_property(settings_2E.PP_Temp, 0);//?
            this.set_property(settings_2E.EN_Temp, 0);//?
            this.set_property(settings_2E.EN_DAC1, 0);//?
            this.set_property(settings_2E.DAC1_PP, 0);//?
            this.set_property(settings_2E.EN_DAC2, 0);//?
            this.set_property(settings_2E.PP_DAC2, 0);//?
            this.set_property(settings_2E.TDC_RAMP_EN, 0);//?
            this.set_property(settings_2E.Discri_Delay_Vref_I_source_EN, 0);//?
            this.set_property(settings_2E.Discri_Delay_Vref_I_source_PP, 0);//?
            this.set_property(settings_2E.EN_LVDS_receiver_NoTrig, 0);//?
            this.set_property(settings_2E.PP_LVDS_receiver_NoTrig, 0);//?
            this.set_property(settings_2E.EN_LVDS_receiver_TrigExt, 0);//?
            this.set_property(settings_2E.PP_LVDS_receiver_TrigExt, 0);//?
            this.set_property(settings_2E.EN_LVDS_receiver_ValEvt, 0);//?
            this.set_property(settings_2E.PP_LVDS_receiver_ValEvt, 0); //?
            this.set_property(settings_2E.LG_SS_TIME_CONSTANT, 0x04);
            this.set_property(settings_2E.ENABLE_LG_SS_PP, 0);

            this.set_property(settings_2E.HG_SS_TIME_CONSTANT, 0x04);
            this.set_property(settings_2E.ENABLE_HG_SS_PP, 0);
            this.set_property(settings_2E.FS_FOLLOWER_PP, 0);
            this.set_property(settings_2E.FS_PP, 0);
            this.set_property(settings_2E.BACKUP_SCA, 0);
            this.set_property(settings_2E.SCA_PP, 0);
            this.set_property(settings_2E.EN_BANDGAP, 1);
            this.set_property(settings_2E.BANDGAP_PP, 1);
            this.set_property(settings_2E.TRIG_DAC, 0x0fa);
            this.set_property(settings_2E.GAIN_DAC, 0x1f4);

            this.set_property(settings_2E.TDC_RAMP_SLOPE_GC, 0);
            this.set_property(settings_2E.TDC_RAMP_PP, 0);
            this.set_property(settings_2E.ADC_DISCRI_PP, 0);
            this.set_property(settings_2E.GAIN_SELECT_DISCRI_PP, 0);
            this.set_property(settings_2E.AUTO_GAIN, 0);
            this.set_property(settings_2E.GAIN_SELECT, 0);
            this.set_property(settings_2E.ADC_EXT_INPUT, 0);
            this.set_property(settings_2E.SWITCH_TDC_ON, 1);
            this.set_property(settings_2E.DISCRIMINATOR_MASK1, 0);
            this.set_property(settings_2E.DISCRIMINATOR_MASK2, 0);
            this.set_property(settings_2E.NC3, 0);
            this.set_property(settings_2E.DISCRI_DELAY_PP, 1);
            this.set_property(settings_2E.DELAY_TRIGGER, 0x02);

            for (int i = 0; i < 36; i++)
            {
                this.set_property(settings_2E.DISCRI_4BIT_ADJUST[i], 0);
            }

            this.set_property(settings_2E.DAC_4BIT_PP, 0);
            this.set_property(settings_2E.TRIG_DISCRI_PP, 0);
            this.set_property(settings_2E.DELAY_VALIDHOLD_PP, 0);
            this.set_property(settings_2E.DELAY_VALIDHOLD, 0x14);
            this.set_property(settings_2E.DELAY_RSTCOL_PP, 0);
            this.set_property(settings_2E.DELAY_RSTCOL, 0x14);
            this.set_property(settings_2E.CLOCK_LVDS_RECEIVE, 0);
            this.set_property(settings_2E.POD, 0);
            this.set_property(settings_2E.END_READOUT, 1);
            this.set_property(settings_2E.START_READOUT, 1);
            this.set_property(settings_2E.CHIPSAT, 1);
            this.set_property(settings_2E.TRANSMITON2, 1);
            this.set_property(settings_2E.TRANSMITON1, 1);
            this.set_property(settings_2E.DOUT2, 1);
            this.set_property(settings_2E.DOUT1, 1);

        }
    }
}



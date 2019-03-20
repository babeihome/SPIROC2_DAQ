// this file save function that user defined and thread function
using CyUSB;
using NationalInstruments.VisaNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace SPIROC_DAQ
{
    partial class Main_Form
    {
        
        // private function
        

    
        private void bindEventHandle(GroupBox gbox, EventHandler handle)
        {

            foreach (Control item in gbox.Controls)
            {
                if(item is TextBox)
                {
                    item.Validated += handle;
                }
                else if(item is CheckBox)
                {
                    (item as CheckBox).Validated += handle;
                }
                else if(item is TableLayoutPanel)
                {
                    foreach(Control sub_item in (item as TableLayoutPanel).Controls)
                    {
                        if(sub_item is TextBox)
                        {
                            sub_item.Validated += handle;
                        }
                        else if(sub_item is CheckBox)
                        {
                            (sub_item as CheckBox).Validated += handle;
                        }
                    }
                }
            }
        }

        private void loadsettings()
        {   // unfinished
            const string cache_loc = ".\\cache\\";

        }
        private void refreshParamPanel_2B()
        {
            // text box
            trig_dac_value.Text = slowConfig.get_property(slowConfig.settings["TRIG_DAC"]).ToString();
            gain_sel_value.Text = slowConfig.get_property(slowConfig.settings["GAIN_DAC"]).ToString();
            hgShapeValue.Text = slowConfig.get_property(slowConfig.settings["HG_SS_TIME_CONSTANT"]).ToString();
            lgShapeValue.Text = slowConfig.get_property(slowConfig.settings["LG_SS_TIME_CONSTANT"]).ToString();
            hgAmpComp.Text = slowConfig.get_property(slowConfig.settings["CAP_HG_PA_COMPENSATION"]).ToString();
            lgAmpComp.Text = slowConfig.get_property(slowConfig.settings["CAP_LG_PA_COMPENSATION"]).ToString();
            startrampDelay.Text = slowConfig.get_property(slowConfig.settings["DELAY_START_RAMP_TDC"]).ToString();
            triggerDelay.Text = slowConfig.get_property(slowConfig.settings["DELAY_TRIGGER"]).ToString();
            validholdDelay.Text = slowConfig.get_property(slowConfig.settings["DELAY_VALIDHOLD"]).ToString();
            rstcolDelay.Text = slowConfig.get_property(slowConfig.settings["DELAY_RSTCOL"]).ToString();
            adcResolution.Text = slowConfig.get_property(slowConfig.settings["ADC_GRAY"]).ToString();
            chipID.Text = slowConfig.get_property(slowConfig.settings["CHIPID"]).ToString();


            // combo box
            Dictionary<uint, string> adcramp_dic = new Dictionary<uint, string>();
            adcramp_dic.Add(0, "12bit");
            adcramp_dic.Add(2, "10bit");
            adcramp_dic.Add(3, "8bit");
            adcRampSlope_combo.Text = adcramp_dic[slowConfig.get_property(slowConfig.settings["ADC_RAMP_SLOPE"])];

            Dictionary<uint, string> tdcramp_dic = new Dictionary<uint, string>();
            tdcramp_dic.Add(0, "fast");
            tdcramp_dic.Add(1, "slow");
            tdcRampSlope_combo.Text = tdcramp_dic[slowConfig.get_property(slowConfig.settings["TDC_RAMP_SLOPE_GC"])];

            Dictionary<uint, string> fastshape_dic = new Dictionary<uint, string>();
            fastshape_dic.Add(0, "Low Gain");
            fastshape_dic.Add(1, "High Gain");
            fastShaperFrom_combo.Text = fastshape_dic[slowConfig.get_property(slowConfig.settings["FS"])];

            Dictionary<uint, string> adjust4bit_dic = new Dictionary<uint, string>();
            adjust4bit_dic.Add(0, "fine");
            adjust4bit_dic.Add(1, "coaese");
            adjust4BitDAC_combo.Text = adjust4bit_dic[slowConfig.get_property(slowConfig.settings["ADJUST_4BIT_DAC"])];


            triggerExt_enable.Checked = (slowConfig.get_property(slowConfig.settings["TRIG_EXT"]) == 1);  
            flagTdcExt_enable.Checked = (slowConfig.get_property(slowConfig.settings["FLAG_TDC_EXT"]) == 1);
            startRampAdcExt_enable.Checked = (slowConfig.get_property(slowConfig.settings["START_RAMP_ADC_EXT"]) == 1);
            startRampTdcExt_enable.Checked = (slowConfig.get_property(slowConfig.settings["START_RAMP_TDC_EXT"]) == 1);
            probe_enable.Checked = (slowConfig.get_property(slowConfig.settings["PROBE_OTA"]) == 1);
            analogOutput_enable.Checked = (slowConfig.get_property(slowConfig.settings["ENABLE_ANALOGUE_OUTPUT"]) == 1);
            or36_enable.Checked = (slowConfig.get_property(slowConfig.settings["EN_OR36"]) == 0);
            backSCA_enable.Checked = (slowConfig.get_property(slowConfig.settings["BACKUP_SCA"]) == 1);


            // refresh input DAC table
            foreach(Control table in inputDAC_group.Controls)
            {
                uint value;
                uint chnNum;
                Regex rx_inputdac_value = new Regex(@"inputdac(\d+)_value");
                Regex rx_inputdac_check = new Regex(@"inputdac(\d+)_enable");
                if(table is TableLayoutPanel)
                {
                    foreach(Control c in table.Controls)
                    {
                        if (c is TextBox)
                        {
                            
                            var result = rx_inputdac_value.Match(c.Name);
                            chnNum = uint.Parse(result.Groups[1].Value);
                            string Key = "INDAC" + chnNum.ToString();
                            c.Text = (slowConfig.get_property(slowConfig.settings[Key.ToString ()]) >> 1).ToString();
                        }
                        else if (c is CheckBox)
                        {
                            var result = rx_inputdac_check.Match(c.Name);
                            chnNum = uint.Parse(result.Groups[1].Value);
                            string Key = "INDAC" + chnNum.ToString();
                            (c as CheckBox).Checked = (slowConfig.get_property(slowConfig.settings[Key.ToString()]) & 0x01) == 1;
                        }
                        else
                            continue;
                    }
                }
            }
            foreach(Control table in discri_groupbox.Controls)
            {
                int chnNum;
                if(table is TableLayoutPanel)
                {
                    foreach(Control checkBox in table.Controls)
                    {
                        if (checkBox is CheckBox)
                        {
                            chnNum = int.Parse(checkBox.Text);
                            if (chnNum > 17)
                            {
                                (checkBox as CheckBox).Checked = ((slowConfig.get_property(slowConfig.settings["DISCRIMINATOR_MASK1"]) & (1 << (chnNum - 18))) == 1);
                            }
                            else
                            {
                                (checkBox as CheckBox).Checked = ((slowConfig.get_property(slowConfig.settings["DISCRIMINATOR_MASK2"]) & (1 << chnNum)) == 1);
                            }

                        }
                        else
                            continue;

                    }
                }
            }
            foreach (Control table in preamp_group.Controls)
            {
                uint value;
                uint chnNum;
                Regex rx_preamp_value = new Regex(@"preampValue_(\d+)");
                Regex rx_preamp_check = new Regex(@"preampCheck_(\d+)");
                if (table is TableLayoutPanel)
                {
                    foreach (Control c in table.Controls)
                    {
                        if (c is TextBox)
                        {
                            var result = rx_preamp_value.Match(c.Name);
                            chnNum = uint.Parse(result.Groups[1].Value);
                            string Key = "PREAMP_GAIN" + chnNum.ToString();
                            c.Text = reverse_bit(slowConfig.get_property(slowConfig.settings[Key.ToString ()]) >> 2, 6).ToString();
                        }
                        else if (c is CheckBox)
                        {
                            var result = rx_preamp_check.Match(c.Name);
                            chnNum = uint.Parse(result.Groups[1].Value);
                            string Key = "PREAMP_GAIN" + chnNum.ToString();
                            (c as CheckBox).Checked = !((slowConfig.get_property(slowConfig.settings[Key.ToString()]) & 0x02) == 0x02);
                        }
                        else
                            continue;
                    }

                }
            }

            autoGain_Check.Checked = slowConfig.get_property(slowConfig.settings["AUTO_GAIN"]) == 1;
            gainSelect_Check.Checked = slowConfig.get_property(slowConfig.settings["GAIN_SELECT"]) == 1;
            adcExtInput_Check.Checked = slowConfig.get_property(slowConfig.settings["AUTO_GAIN"]) == 1;
            switchTDCon_Check.Checked = slowConfig.get_property(slowConfig.settings["SWITCH_TDC_ON"]) == 1;
            bandGap_Check.Checked = slowConfig.get_property(slowConfig.settings["EN_BANDGAP"]) == 1;
            dacEnable_Check.Checked = slowConfig.get_property(slowConfig.settings["EN_DAC"]) == 1;
            hgAmpComp.Enabled = true;
            lgAmpComp.Enabled = true;
            startrampDelay.Enabled = true;
            fastShaperFrom_combo.Enabled = true;
            adjust4BitDAC_combo.Enabled = true;
            dacEnable_Check.Enabled = true;

        }
        private void refreshParamPanel_2E()
        {
            // disable some controller
            hgAmpComp.Enabled = false;
            lgAmpComp.Enabled = false;
            startrampDelay.Enabled = false;
            fastShaperFrom_combo.Enabled = false;
            adjust4BitDAC_combo.Enabled = false;
            dacEnable_Check.Enabled = false;
            // text box
            trig_dac_value.Text = slowConfig.get_property(slowConfig.settings["TRIG_DAC"]).ToString();
            gain_sel_value.Text = slowConfig.get_property(slowConfig.settings["GAIN_DAC"]).ToString();
            hgShapeValue.Text = slowConfig.get_property(slowConfig.settings["HG_SS_TIME_CONSTANT"]).ToString();
            lgShapeValue.Text = slowConfig.get_property(slowConfig.settings["LG_SS_TIME_CONSTANT"]).ToString();
            
            
            
            triggerDelay.Text = slowConfig.get_property(slowConfig.settings["DELAY_TRIGGER"]).ToString();
            validholdDelay.Text = slowConfig.get_property(slowConfig.settings["DELAY_VALIDHOLD"]).ToString();
            rstcolDelay.Text = slowConfig.get_property(slowConfig.settings["DELAY_RSTCOL"]).ToString();
            adcResolution.Text = slowConfig.get_property(slowConfig.settings["ADC_GRAY"]).ToString();
            chipID.Text = slowConfig.get_property(slowConfig.settings["CHIPID"]).ToString();


            // combo box
            Dictionary<uint, string> adcramp_dic = new Dictionary<uint, string>();
            adcramp_dic.Add(0, "12bit");
            adcramp_dic.Add(2, "10bit");
            adcramp_dic.Add(3, "8bit");
            adcRampSlope_combo.Text = adcramp_dic[slowConfig.get_property(slowConfig.settings["ADC_RAMP_SLOPE"])];

            Dictionary<uint, string> tdcramp_dic = new Dictionary<uint, string>();
            tdcramp_dic.Add(0, "fast");
            tdcramp_dic.Add(1, "slow");
            tdcRampSlope_combo.Text = tdcramp_dic[slowConfig.get_property(slowConfig.settings["TDC_RAMP_SLOPE_GC"])];

            

            


            triggerExt_enable.Checked = (slowConfig.get_property(slowConfig.settings["TRIG_EXT"]) == 1);
            flagTdcExt_enable.Checked = (slowConfig.get_property(slowConfig.settings["FLAG_TDC_EXT"]) == 1);
            startRampAdcExt_enable.Checked = (slowConfig.get_property(slowConfig.settings["START_RAMP_ADC_EXT"]) == 1);
            startRampTdcExt_enable.Checked = (slowConfig.get_property(slowConfig.settings["START_RAMP_TDC_EXT"]) == 1);
            probe_enable.Checked = (slowConfig.get_property(slowConfig.settings["PROBE_OTA"]) == 1);
            analogOutput_enable.Checked = (slowConfig.get_property(slowConfig.settings["ENABLE_ANALOGUE_OUTPUT"]) == 1);
            or36_enable.Checked = (slowConfig.get_property(slowConfig.settings["EN_OR36"]) == 0);
            backSCA_enable.Checked = (slowConfig.get_property(slowConfig.settings["BACKUP_SCA"]) == 1);


            // refresh input DAC table
            foreach (Control table in inputDAC_group.Controls)
            {
                uint value;
                uint chnNum;
                Regex rx_inputdac_value = new Regex(@"inputdac(\d+)_value");
                Regex rx_inputdac_check = new Regex(@"inputdac(\d+)_enable");
                if (table is TableLayoutPanel)
                {
                    foreach (Control c in table.Controls)
                    {
                        if (c is TextBox)
                        {

                            var result = rx_inputdac_value.Match(c.Name);
                            chnNum = uint.Parse(result.Groups[1].Value);
                            string Key = "INDAC" + chnNum.ToString();
                            c.Text = (slowConfig.get_property(slowConfig.settings[Key.ToString()]) >> 1).ToString();
                        }
                        else if (c is CheckBox)
                        {
                            var result = rx_inputdac_check.Match(c.Name);
                            chnNum = uint.Parse(result.Groups[1].Value);
                            string Key = "INDAC" + chnNum.ToString();
                            (c as CheckBox).Checked = (slowConfig.get_property(slowConfig.settings[Key.ToString()]) & 0x01) == 1;
                        }
                        else
                            continue;
                    }
                }
            }
            foreach (Control table in discri_groupbox.Controls)
            {
                int chnNum;
                if (table is TableLayoutPanel)
                {
                    foreach (Control checkBox in table.Controls)
                    {
                        if (checkBox is CheckBox)
                        {
                            chnNum = int.Parse(checkBox.Text);
                            if (chnNum > 17)
                            {
                                (checkBox as CheckBox).Checked = ((slowConfig.get_property(slowConfig.settings["DISCRIMINATOR_MASK1"]) & (1 << (chnNum - 18))) == 1);
                            }
                            else
                            {
                                (checkBox as CheckBox).Checked = ((slowConfig.get_property(slowConfig.settings["DISCRIMINATOR_MASK2"]) & (1 << chnNum)) == 1);
                            }

                        }
                        else
                            continue;

                    }
                }
            }
            foreach (Control table in preamp_group.Controls)
            {
                uint value;
                uint chnNum;
                Regex rx_preamp_value = new Regex(@"preampValue_(\d+)");
                Regex rx_preamp_check = new Regex(@"preampCheck_(\d+)");
                if (table is TableLayoutPanel)
                {
                    foreach (Control c in table.Controls)
                    {
                        if (c is TextBox)
                        {
                            var result = rx_preamp_value.Match(c.Name);
                            chnNum = uint.Parse(result.Groups[1].Value);
                            string Key = "PREAMP_GAIN" + chnNum.ToString();
                            if(HLGain_Select.Text == "High Gain")
                            {
                                c.Text = reverse_bit((slowConfig.get_property(slowConfig.settings[Key.ToString()]) >> 9) & 0x3f, 6).ToString();
                            }
                            else if(HLGain_Select.Text == "Low Gain")
                            {
                                c.Text = reverse_bit((slowConfig.get_property(slowConfig.settings[Key.ToString()]) >> 3) & 0x3f, 6).ToString();
                            }
                            
                        }
                        else if (c is CheckBox)
                        {
                            var result = rx_preamp_check.Match(c.Name);
                            chnNum = uint.Parse(result.Groups[1].Value);
                            string Key = "PREAMP_GAIN" + chnNum.ToString();
                            (c as CheckBox).Checked = !((slowConfig.get_property(slowConfig.settings[Key.ToString()]) & 0x01) == 0x01);
                        }
                        else
                            continue;
                    }

                }
            }

            autoGain_Check.Checked = slowConfig.get_property(slowConfig.settings["AUTO_GAIN"]) == 1;
            gainSelect_Check.Checked = slowConfig.get_property(slowConfig.settings["GAIN_SELECT"]) == 1;
            adcExtInput_Check.Checked = slowConfig.get_property(slowConfig.settings["AUTO_GAIN"]) == 1;
            switchTDCon_Check.Checked = slowConfig.get_property(slowConfig.settings["SWITCH_TDC_ON"]) == 1;
            bandGap_Check.Checked = slowConfig.get_property(slowConfig.settings["EN_BANDGAP"]) == 1;
            ENDac1.Checked = slowConfig.get_property(slowConfig.settings["EN_DAC1"]) == 1;
            ENDac2.Checked = slowConfig.get_property(slowConfig.settings["EN_DAC2"]) == 1;
            ENHighGain.Checked = slowConfig.get_property(slowConfig.settings["EN_High_Gain_PA"]) == 1;
            ENLowGain.Checked = slowConfig.get_property(slowConfig.settings["EN_Low_Gain_PA"]) == 1;



        }
        #region USB interface operation
        // @@@   basic interface operation  -------------------

        //usb parameters
        // USB command send
        private bool check_USB()
        {
            bool result = true;
            myDevice = usbDevices[VID, PID] as CyUSBDevice;
            //AFG3252 = usbDevices[0x0699, 0x0354] as CyUSBDevice;
            if (myDevice != null)
            {
                Usb_status_label.Text = "USB device connected";
                Usb_status_label.ForeColor = Color.Green;
                usbStatus = true;

                normal_config_button.Enabled = true;

                bulkOutEndPt = myDevice.EndPointOf(0x08) as CyBulkEndPoint; // EP8
                bulkInEndPt = myDevice.EndPointOf(0x82) as CyBulkEndPoint; //EP2

                bulkInEndPt.XferSize = bulkInEndPt.MaxPktSize * 8;  // transfer size means the max limits of data in USB driver
                bulkInEndPt.TimeOut = 100;

            }
            else
            {
                Usb_status_label.Text = "USB not connected";
                Usb_status_label.ForeColor = Color.Red;
                usbStatus = false;
                normal_config_button.Enabled = false;
                normal_acq_button.Enabled = false;
                normal_stop_button.Enabled = false;

                bulkOutEndPt = null;
                bulkInEndPt = null;

                result = false;

            }

            try
            {
                SignalSource.initial();
                //AFG_Session = (MessageBasedSession)ResourceManager.GetLocalManager().Open("USB0::0x0699::0x0345::C022722::INSTR");         
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Resource selected must be a message-based session");
            }
            catch (Exception exp)
            {
                //MessageBox.Show(exp.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            if (SignalSource.isConnected())
            {
                afg3252_label.Text = "Connected";
                afg3252_label.ForeColor = Color.Green;
                //usbStatus = true;

                //normal_config_button.Enabled = true;

                //AFG3252_command_point = AFG3252.BulkOutEndPt; // EP0               


            }
            else
            {
                afg3252_label.Text = "USB not connected";
                afg3252_label.ForeColor = Color.Red;
                //AFG3252_command_point = null;

            }
            return result;
        }
        private bool CommandSend(byte[] OutData, int xferLen)
        {
            bool bResult = false;
            if (bulkOutEndPt == null)
            {
                bResult = false;
            }
            else
            {
                bResult = bulkOutEndPt.XferData(ref OutData, ref xferLen);
            }
            return bResult;
        }
        private bool CommandSend(int Outdata, int xferLen)
        {
            bool bResult = false;
            byte[] cmdbytes = new byte[2];
            cmdbytes[0] = (byte)Outdata;
            cmdbytes[1] = (byte)(Outdata >> 8);
            if(bulkOutEndPt == null)
            {
                bResult = false;
            }
            else
            {
                bResult = bulkOutEndPt.XferData(ref cmdbytes, ref xferLen);
            }
            return bResult;
        }
        //data recieve method
        private bool DataRecieve(byte[] InData, ref int xferLen)
        {
            bool bResult;
            bResult = bulkInEndPt.XferData(ref InData, ref xferLen, true);
            return bResult;
        }

        #endregion

        #region Thread-used function
        private void ext_trig_sweep_threadFunc(CancellationToken token, Form2 parawindows)
        {
            uint trig_delay_start = uint.Parse(parawindows.start1.Text);
            uint trig_delay_step = uint.Parse(parawindows.step1.Text);
            uint trig_delay_stop = uint.Parse(parawindows.stop1.Text);
            byte[] data_buffer = new byte[512];

            BinaryWriter bw = null;
            DateTime dayStamp = DateTime.Now;
            string subDic = string.Format("{0:yyyyMMdd}_{0:HHmm}_TDC_sweep", dayStamp);
            string fullPath = folderBrowserDialog1.SelectedPath + '\\' + subDic;
            string fileName;
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            for (uint delay_value = trig_delay_start; delay_value <= trig_delay_stop; delay_value += trig_delay_step)
            {
                dataAcqTks.Dispose();       //clean up old token source
                dataAcqTks = new CancellationTokenSource(); // generate a new token
                fileName = string.Format("{0}ns_trig_ext.dat", delay_value);
                bw = new BinaryWriter(File.Open(fullPath + '\\' + fileName, FileMode.Create, FileAccess.Write, FileShare.Read));

                // Source1 is signal channel and Source2 is pulse channel

                SignalSource.delaySet(1, (int)delay_value);
                SignalSource.delaySet(2, (int)delay_value);

                SignalSource.openOutput();
                Thread.Sleep(1000);
                CommandSend(0x0100, 2);
                if (usbStatus == false)
                {
                    MessageBox.Show("USB is not connected");
                }

                try
                {
                    Task dataAcqTsk = Task.Factory.StartNew(() => this.dataAcq_threadFunc(dataAcqTks.Token, bw), dataAcqTks.Token);
                    Thread.Sleep(5 * 1000);
                    // time up!
                    // stop asic first
                    CommandSend(0x0200, 2);
                    // stop data receiving
                    dataAcqTks.Cancel();
                    while (!dataAcqTsk.IsCompleted) ;
                }
                catch (AggregateException excption)
                {
                    foreach (var value in excption.InnerExceptions)
                    {
                        exceptionReport.AppendLine(excption.Message + " " + value.Message);
                    }
                }

                // stop signal
                SignalSource.closeOutput();
                if (token.IsCancellationRequested == true)
                {
                    break;
                }
            }
        }
        private void s_curve_sweep_threadFunc(CancellationToken token,Form2 paraWindows, BinaryWriter bw)
        { 
            uint dac_start = uint.Parse(paraWindows.start1.Text);
            uint dac_step = uint.Parse(paraWindows.step1.Text);
            uint dac_stop = uint.Parse(paraWindows.stop1.Text);
            byte[] data_buffer = new byte[512];
            int len;
            bool bResult;

            len = 512;

            for (uint dac_value = dac_start; dac_value <= dac_stop; dac_value += dac_step)
            {
                if(token.IsCancellationRequested == true)
                {
                    bResult = DataRecieve(data_buffer, ref len); // len could be changed for transmit actually num of byte that received
                    bw.Write(data_buffer, 0, len);   // data source, start_index, data_length
                    bw.Flush();
                    break;
                }
                slowConfig.set_property(slowConfig.settings["TRIG_DAC"], dac_value);
                normal_config_button_Click(null, null);
                CommandSend(0x1101, 2);
                Thread.Sleep(10000);  // 10 seconds
                CommandSend(0x1100, 2);

                bResult = DataRecieve(data_buffer, ref len); // len could be changed for transmit actually num of byte that received
                bw.Write(data_buffer, 0, len);   // data source, start_index, data_length
                bw.Flush();
            }

            bw.Close();

        }
        private void HV_set_smooth_threadFunc(CancellationToken token, bool turnOff, bool turnOn)
        {
            decimal target_voltage;
            target_voltage = HV_value.Value;
            decimal tmp_v = current_hv;
            if (turnOn)
            {
                hv_switch(true);
            }
            while (Math.Abs(tmp_v - target_voltage) > (decimal)0.2)
            {
                if(token.IsCancellationRequested != true)
                {
                    if (tmp_v < 68)
                    {
                        if (target_voltage > tmp_v)
                            tmp_v += 1;
                        else
                            tmp_v -= 1;
                    }
                    else
                    {
                        if (target_voltage > tmp_v)
                            tmp_v += (decimal)0.1;
                        else
                            tmp_v -= (decimal)0.1;
                    }
                    hv_set(tmp_v);
                    current_hv = tmp_v;
                    temp_HV.Text = tmp_v.ToString();
                    temp_HV.ForeColor = Color.Black;
                    Thread.Sleep(500);

                }
                else
                {
                    break;
                }

            }
            if(token.IsCancellationRequested != true)
            {
                hv_set(target_voltage);
                temp_HV.Text = target_voltage.ToString();
                temp_HV.ForeColor = Color.Green;
            }
            if (turnOff)
            {
                hv_switch(false);
            }

        }
        private void dataAcq_threadFunc(CancellationToken token, BinaryWriter bw)
        {
            byte[] data_buffer = new byte[512];
            int len;

            bool bResult;
            while (true)
            {
                if (token.IsCancellationRequested == true)
                {
                    Thread.Sleep(100);
                    len = 512;
                    bResult = DataRecieve(data_buffer, ref len); // len could be changed for transmit actually num of byte that received
                    bw.Write(data_buffer, 0, len);   // data source, start_index, data_length

                    if (bResult == false)
                    {
                        bw.Flush();
                        //bw.Close();
                        //bw.Dispose();
                        break;
                    }
                }
                else
                {
                    len = 512;
                    bResult = DataRecieve(data_buffer, ref len); // len could be changed for transmit actually num of byte that received
                    bw.Write(data_buffer, 0, len);
                    bw.Flush();
                }

            }
            bw.Flush();
            bw.Close();
            bw.Dispose();

        }
        private void electronicSweep_threadFunc(CancellationToken token)
        {
            byte[] cmdbytes = new byte[2];

            int startGroup = 1;
            int stepGroup = 1;
            int stopGroup = 8;

            uint startDAC = 0;
            uint stepDAC = 50;
            uint stopDAC = 4095;
            byte value_h;
            byte value_l;

            CommandSend(0x0f01,2);  //enable calibration selection
            BinaryWriter bw;
            DateTime dayStamp = DateTime.Now;
            string subDic = string.Format("{0:yyyyMMdd}_{0:HHmm}_Electronic_Calib", dayStamp);
            string fullPath = folderBrowserDialog1.SelectedPath + '\\' + subDic;
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            for (int groupID = startGroup; groupID <= stopGroup; groupID += stepGroup)
            {
                //CommandSend(0x1200, 2); //close auto calib
                
                


                cmdbytes[1] = 0x0b;
                cmdbytes[0] = (byte)(groupID-1);
                CommandSend(cmdbytes, 2);
                if (token.IsCancellationRequested == true)
                {
                    break;
                }
                else
                {
                    for(uint valueDAC = startDAC; valueDAC <= stopDAC; valueDAC += stepDAC)
                    {
                        if (token.IsCancellationRequested == true)
                        {
                            break;
                        }
                        value_h = (byte)(valueDAC >> 8);
                        value_l = (byte)(valueDAC);
                        cmdbytes[1] = 0x07;
                        cmdbytes[0] = value_h;
                        CommandSend(cmdbytes, 2);
                        cmdbytes[0] = value_l;
                        CommandSend(cmdbytes, 2);

                        byte chn1_mask = 0x80;
                        value_h = (byte)(chn1_mask | value_h);

                        cmdbytes[0] = value_h;
                        CommandSend(cmdbytes, 2);
                        cmdbytes[0] = value_l;
                        CommandSend(cmdbytes, 2);
                        sendMessage("GROUP " + groupID.ToString() + "is being calibrated with DAC: "+valueDAC+" now\n");
                        string fileName = string.Format("{0}group_{1}DAC.dat", groupID, valueDAC);
                        //create file writer
                        bw = new BinaryWriter(File.Open(fullPath + '\\' + fileName, FileMode.Create, FileAccess.Write, FileShare.Read));

                        dataAcqTks.Dispose();       //clean up old token source
                        dataAcqTks = new CancellationTokenSource(); // generate a new token
                        //CommandSend(0x1201, 2); //open auto calib

                        CommandSend(0x0100, 2); //start acq cycle

                        // check USB status
                        if (usbStatus == false)
                        {
                            MessageBox.Show("USB is not connected");
                        }
                        
                        try
                        {
                            Task dataAcqTsk = Task.Factory.StartNew(() => this.dataAcq_threadFunc(dataAcqTks.Token, bw), dataAcqTks.Token);
                            Thread.Sleep(3 * 1000);
                            // time up!
                            // stop asic first
                            CommandSend(0x0200, 2);
                            // stop data receiving
                            dataAcqTks.Cancel();
                            while (!dataAcqTsk.IsCompleted) ;
                        }
                        catch (AggregateException excption)
                        {
                            foreach (var value in excption.InnerExceptions)
                            {
                                exceptionReport.AppendLine(excption.Message + " " + value.Message);
                            }
                        }
                        CommandSend(0x1200, 2);
                    }
                }
            }
            CommandSend(0x0f00, 2); // close calibration selection
            Acq_status_label.Text = "IDLE";
            Acq_status_label.ForeColor = Color.Black;
        }
        private void voltageSweep_threadFunc(CancellationToken taskToken)
        {
            int startVoltage = int.Parse(startVol_textbox.Text);
            int stepVoltage = int.Parse(stepVol_textbox.Text);
            int stopVoltage = int.Parse(stopVol_textbox.Text);

            BinaryWriter bw;

            DateTime dayStamp = DateTime.Now;
            string subDic = string.Format("{0:yyyyMMdd}_{0:HHmm}_VoltageSweep", dayStamp);
            string fullPath = folderBrowserDialog1.SelectedPath + '\\' + subDic;
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            // use default settings
            //SignalSource.Write("*RCL 4");
            SignalSource.closeOutput();
            for (int v = startVoltage; v <= stopVoltage; v += stepVoltage)
            {
                sendMessage("Start acq at " + v.ToString() + " mV of Signal Source\n");
                if(taskToken.IsCancellationRequested == true)
                {
                    break;
                }
                else
                {
                    //file name include voltage value;
                    double real_v = (double)v / double.Parse(atten_textbox.Text);
                    string fileName = string.Format("{0:#0.#}mV.dat",real_v);
                    //create file writer
                    bw = new BinaryWriter(File.Open(fullPath + '\\' + fileName, FileMode.Create,FileAccess.Write,FileShare.Read));




                    // For each voltage, acq DURATION_SWEEP ms. Now set the timer
                    /*
                    System.Timers.Timer taskTimer = new System.Timers.Timer(settings.DURATION_SWEEP);
                    taskTimer.AutoReset = false;
                    taskTimer.
                    */

                    dataAcqTks.Dispose();       //clean up old token source
                    dataAcqTks = new CancellationTokenSource(); // generate a new token


                    byte[] cmdBytes = new byte[2];
                    // start acq cmd is 0x0100;
                    cmdBytes[1] = 0x01;
                    cmdBytes[0] = 0x00;
                    SignalSource.setVoltage(1, v);
                    SignalSource.openOutput();
                    Thread.Sleep(1000); //wait 1 seconds
                    CommandSend(cmdBytes, 2);

                    // tune voltage of channel 1
                    //SignalSource.setOffset(1, v);

                    // check USB status
                    if (usbStatus == false)
                    {
                        MessageBox.Show("USB is not connected");
                    }
                    

                    // Start data acquision thread
                    try
                    {
                        Task dataAcqTsk = Task.Factory.StartNew(() => this.dataAcq_threadFunc(dataAcqTks.Token, bw), dataAcqTks.Token);
                        Thread.Sleep(int.Parse(duration_sweep.Text) * 1000);
                        // time up!

                        // stop signal first
                        SignalSource.closeOutput();
                        Thread.Sleep(500);

                        // stop asic
                        cmdBytes[1] = 0x02;
                        cmdBytes[0] = 0x00;
                        CommandSend(cmdBytes, 2);

                        // stop data receiving
                        dataAcqTks.Cancel();
                        while (!dataAcqTsk.IsCompleted) ;
                    }
                    catch (AggregateException excption)
                    {

                        foreach (var value in excption.InnerExceptions)
                        {

                            exceptionReport.AppendLine(excption.Message + " " + value.Message);
                        }

                    }
                   



                }


            }

            Acq_status_label.Text = "IDLE";
            Acq_status_label.ForeColor = Color.Black;
        }

        private void scSweep_threadFunc(CancellationToken taskToken, string selectedPara)
        {
            uint startValue = uint.Parse(scSweepStart_value.Text);
            uint stepValue = uint.Parse(scSweepStep_value.Text);
            uint stopValue = uint.Parse(scSweepStop_value.Text);
            Dictionary<string, int> propertyTable = new Dictionary<string, int>();

            // property Table information
            propertyTable.Add("trig delay", slowConfig.settings["DELAY_TRIGGER"]);
            propertyTable.Add("trig dac", slowConfig.settings["TRIG_DAC"]);
            propertyTable.Add("gain dac", slowConfig.settings["GAIN_DAC"]);

            BinaryWriter bw;

            DateTime dayStamp = DateTime.Now;
            string subDic = string.Format("{0:yyyyMMdd}_{0:HHmm}_scSweep", dayStamp);
            string fullPath = folderBrowserDialog1.SelectedPath + '\\' + subDic;
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            // use default settings
            //SignalSource.Write("*RCL 4");
            if (SignalSource.isConnected())
            {
                SignalSource.closeOutput();
            }
            else
            {
                sendMessage("Be careful that Signal Generator is not connected");
            }

            for (uint v = startValue; v <= stopValue; v += stepValue)
            {
                sendMessage("Start acq at " + v.ToString() + selectedPara + "\n");
                if (taskToken.IsCancellationRequested == true)
                {
                    break;
                }
                else
                {
                    //file name include voltage value;                  
                    string fileName = string.Format("{1}_{0:#0}.dat", v, selectedPara.Replace(' ', '_'));

                    //create file writer
                    bw = new BinaryWriter(File.Open(fullPath + '\\' + fileName, FileMode.Create, FileAccess.Write, FileShare.Read));

                    // tune voltage of channel 1
                    //SignalSource.setVoltage(1, v);
                    if (SignalSource.isConnected())
                    {
                        SignalSource.openOutput();
                    }
                    
                    Thread.Sleep(100); //wait 1 seconds


                    // get ready for start acq thread
                    dataAcqTks.Dispose();       //clean up old token source
                    dataAcqTks = new CancellationTokenSource(); // generate a new token

                    slowConfig.set_property(propertyTable[selectedPara], v);
                    if(selectedPara == "trig delay" && (version_num == 2))
                    {
                        slowConfig.set_property(slowConfig.settings["DELAY_VALIDHOLD"], (uint)(v / 4));
                    }
                    normal_config_button_Click(null, null);
                    Thread.Sleep(100);
                    byte[] cmdBytes = new byte[2];
                    // start acq cmd is 0x0100;
                    cmdBytes[1] = 0x01;
                    cmdBytes[0] = 0x00;

                    CommandSend(cmdBytes, 2);
                    // check USB status
                    if (usbStatus == false)
                    {
                        MessageBox.Show("USB is not connected");
                    }


                    // Start data acquision thread
                    try
                    {
                        Task dataAcqTsk = Task.Factory.StartNew(() => this.dataAcq_threadFunc(dataAcqTks.Token, bw), dataAcqTks.Token);
                        Thread.Sleep(int.Parse(scSweepTime_value.Text) * 1000);
                        // time up!
                        // stop asic first
                        cmdBytes[1] = 0x02;
                        cmdBytes[0] = 0x00;
                        CommandSend(cmdBytes, 2);

                        // stop data receiving
                        dataAcqTks.Cancel();
                        while (!dataAcqTsk.IsCompleted) ;
                    }
                    catch (AggregateException excption)
                    {

                        foreach (var value in excption.InnerExceptions)
                        {

                            exceptionReport.AppendLine(excption.Message + " " + value.Message);
                        }

                    }

                    // stop signal
                    if(SignalSource.isConnected())
                    {
                        SignalSource.closeOutput();
                    }


                }
            }
            Acq_status_label.Text = "IDLE";
            Acq_status_label.ForeColor = Color.Black;

        }

        private void preampSweep_threadFunc(CancellationToken taskToken)
        {
            uint startValue = uint.Parse(scSweepStart_value.Text);
            uint stepValue = uint.Parse(scSweepStep_value.Text);
            uint stopValue = uint.Parse(scSweepStop_value.Text);


            BinaryWriter bw;

            DateTime dayStamp = DateTime.Now;
            string subDic = string.Format("{0:yyyyMMdd}_{0:HHmm}_preampSweep", dayStamp);
            string fullPath = folderBrowserDialog1.SelectedPath + '\\' + subDic;
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            // use default settings
            //SignalSource.Write("*RCL 4");
            if (SignalSource.isConnected())
            {
                SignalSource.closeOutput();
            }
            for (uint v = startValue; v <= stopValue; v += stepValue)
            {
                sendMessage("Start acq at " + v.ToString() + " preamp\n");
                if (taskToken.IsCancellationRequested == true)
                {
                    break;
                }
                else
                {
                    //file name include voltage value;                  
                    string fileName = string.Format("preamp_{0:#0}.dat", v);

                    //create file writer
                    bw = new BinaryWriter(File.Open(fullPath + '\\' + fileName, FileMode.Create, FileAccess.Write, FileShare.Read));




                    // get ready for start acq thread
                    dataAcqTks.Dispose();       //clean up old token source
                    dataAcqTks = new CancellationTokenSource(); // generate a new token
                    

                    // SC change
                    if(version_num == 1)
                    {
                        for (int chn = 0; chn < 36; chn++)
                        {
                            string Key = "PREAMP_GAIN" + chn.ToString();
                            uint old_value = slowConfig.get_property(slowConfig.settings[Key.ToString()]);
                            uint new_value = (reverse_bit(v, 6) << 2) + (old_value & 0x03);
                            slowConfig.set_property(slowConfig.settings[Key.ToString()], new_value);
                        }
                    }
                    else if (version_num == 2)
                    {
                        for (int chn = 0; chn < 36; chn++)
                        {
                            string Key = "PREAMP_GAIN" + chn.ToString();
                            uint old_value = slowConfig.get_property(slowConfig.settings[Key.ToString()]);
                            uint new_value = (reverse_bit(v, 6) << 3)+ (reverse_bit(v, 6) << 3) + (old_value & 0x07);
                            slowConfig.set_property(slowConfig.settings[Key.ToString()], new_value);
                        }
                    }

                    
                    normal_config_button_Click(null, null);
                    Thread.Sleep(100);
                    if (SignalSource.isConnected())
                    {
                        SignalSource.openOutput();
                    }
                    Thread.Sleep(100); //wait 1 seconds

                    byte[] cmdBytes = new byte[2];
                    // start acq cmd is 0x0100;
                    cmdBytes[1] = 0x01;
                    cmdBytes[0] = 0x00;

                    CommandSend(cmdBytes, 2);
                    // check USB status
                    if (usbStatus == false)
                    {
                        MessageBox.Show("USB is not connected");
                    }


                    // Start data acquision thread
                    try
                    {
                        Task dataAcqTsk = Task.Factory.StartNew(() => this.dataAcq_threadFunc(dataAcqTks.Token, bw), dataAcqTks.Token);
                        Thread.Sleep(int.Parse(scSweepTime_value.Text) * 1000);

                        // time up!
                        // stop asic first
                        cmdBytes[1] = 0x02;
                        cmdBytes[0] = 0x00;
                        CommandSend(cmdBytes, 2);

                        // stop data receiving
                        dataAcqTks.Cancel();
                        while (!dataAcqTsk.IsCompleted) ;
                    }
                    catch (AggregateException excption)
                    {

                        foreach (var value in excption.InnerExceptions)
                        {

                            exceptionReport.AppendLine(excption.Message + " " + value.Message);
                        }

                    }


                    // stop signal
                    if (SignalSource.isConnected())
                    {
                        SignalSource.closeOutput();
                    }
                    


                }
            }
            Acq_status_label.Text = "IDLE";
            Acq_status_label.ForeColor = Color.Black;
        }

        private void delayMatrix_threadFunc(CancellationToken taskToken, Form2 paraWindows)
        {

            
            // extDelay is the duration that signal minus trigger
            // unit is nano-second
            int extDelay_start = int.Parse(paraWindows.start2.Text);
            int extDelay_step = int.Parse(paraWindows.step2.Text);
            int extDelay_stop = int.Parse(paraWindows.stop2.Text);

            uint asicDelay_start = uint.Parse(paraWindows.start1.Text);
            uint asicDelay_step = uint.Parse(paraWindows.step1.Text);
            uint asicDelay_stop = uint.Parse(paraWindows.stop1.Text);


            // initial file writer and file
            BinaryWriter bw;

            DateTime dayStamp = DateTime.Now;
            string subDic = string.Format("{0:yyyyMMdd}_{0:HHmm}_DelayMatrix", dayStamp);
            string fullPath = folderBrowserDialog1.SelectedPath + '\\' + subDic;
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);


            // use default settings
            //SignalSource.Write("*RCL 4");
            SignalSource.closeOutput();
            
            // outside loop is by sc
            for (uint delay_asic = asicDelay_start; delay_asic <= asicDelay_stop; delay_asic += asicDelay_step)
            {
                if(taskToken.IsCancellationRequested == true)
                {
                    break;
                }
                slowConfig.set_property(slowConfig.settings["DELAY_TRIGGER"], delay_asic);
                normal_config_button_Click(null, null);
                Thread.Sleep(100);

                // inside loop is by AFG3252
                for (int delay_afg = extDelay_start; delay_afg <= extDelay_stop; delay_afg += extDelay_step)
                {
                    sendMessage("Sweep point: \n\t delay of asic:\t" + delay_asic.ToString() + "\n\t delay of AFG:\t" + delay_afg.ToString() + "\n");
                    if (taskToken.IsCancellationRequested == true)
                    {
                        break;
                    }
                    // Source1 is signal channel and Source2 is pulse channel
                    if(delay_afg < 0)
                    {
                        SignalSource.delaySet(1, 0);
                        SignalSource.delaySet(2, -delay_afg);
                    }
                    else
                    {
                        SignalSource.delaySet(1, delay_afg);
                        SignalSource.delaySet(2, 0);
                    }

                    SignalSource.openOutput();
                    Thread.Sleep(1000); //wait 1s
                    //file name include voltage value;                  
                    string fileName = string.Format("delay_{0:#0}x{1:#0}.dat", delay_asic, delay_afg);
                    //create file writer
                    bw = new BinaryWriter(File.Open(fullPath + '\\' + fileName, FileMode.Create, FileAccess.Write, FileShare.Read));


                    // get ready for start acq thread
                    dataAcqTks.Dispose();       //clean up old token source
                    dataAcqTks = new CancellationTokenSource(); // generate a new token
                    byte[] cmdBytes = new byte[2];
                    // start acq cmd is 0x0100;
                    cmdBytes[1] = 0x01;
                    cmdBytes[0] = 0x00;

                    CommandSend(cmdBytes, 2);


                    


                    // check USB status

                    // Start data acquision thread
                    try
                    {
                        Task dataAcqTsk = Task.Factory.StartNew(() => this.dataAcq_threadFunc(dataAcqTks.Token, bw), dataAcqTks.Token);
                        Thread.Sleep(5 * 1000);  //acq 5 seconds
                                                 // time up!
                                                 // stop asic first
                        cmdBytes[1] = 0x02;
                        cmdBytes[0] = 0x00;
                        CommandSend(cmdBytes, 2);

                        // stop data receiving
                        dataAcqTks.Cancel();
                        Thread.Sleep(500);
                        while (!dataAcqTsk.IsCompleted) ;
                    }
                    catch (AggregateException excption)
                    {

                        foreach (var value in excption.InnerExceptions)
                        {

                            exceptionReport.AppendLine(excption.Message + " " + value.Message);
                        }

                    }


                    // stop signal
                    SignalSource.closeOutput();
                }
            }

            Acq_status_label.Text = "IDLE";
            Acq_status_label.ForeColor = Color.Black;
        }

        private void preampDelay_sweep_threadFunc(CancellationToken taskToken, Form2 paraWindows)
        {
            // extDelay is the duration that signal minus trigger
            // unit is nano-second


            uint preamp_start = uint.Parse(paraWindows.start1.Text);
            uint preamp_step = uint.Parse(paraWindows.step1.Text);
            uint preamp_stop = uint.Parse(paraWindows.stop1.Text);

            uint delay_start = uint.Parse(paraWindows.start2.Text);
            uint delay_step = uint.Parse(paraWindows.step2.Text);
            uint delay_stop = uint.Parse(paraWindows.stop2.Text);


            // initial file writer and file
            BinaryWriter bw;

            DateTime dayStamp = DateTime.Now;
            string subDic = string.Format("{0:yyyyMMdd}_{0:HHmm}_PreampDelaySweep", dayStamp);
            string fullPath = folderBrowserDialog1.SelectedPath + '\\' + subDic;
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);


            // use default settings
            //SignalSource.Write("*RCL 4");
            SignalSource.closeOutput();

            // outside loop is for preamp gain
            for (uint preamp = preamp_start; preamp <= preamp_stop; preamp += preamp_step)
            {
                if (taskToken.IsCancellationRequested == true)
                {
                    break;
                }
                for(int chn = 0; chn<36;chn ++)
                {
                    string Key = "PREAMP_GAIN" + chn.ToString();
                    uint old_value = slowConfig.get_property(slowConfig.settings[Key.ToString()]);
                    uint new_value = (reverse_bit(preamp,6) << 2) + (old_value & 0x03);
                    slowConfig.set_property(slowConfig.settings[Key.ToString()], new_value);

                }
                // inside loop is for trig_delay
                for (uint delay = delay_start; delay <= delay_stop; delay += delay_step)
                {
                    slowConfig.set_property(slowConfig.settings["DELAY_TRIGGER"], delay);
                    sendMessage("Sweep point: \n\t delay of asic:\t" + delay.ToString() + "\n\t preamp:\t" + preamp.ToString() + "\n");

                    if (taskToken.IsCancellationRequested == true)
                    {
                        break;
                    }
                    // Source1 is signal channel and Source2 is pulse channel
                    normal_config_button_Click(null, null);
                    Thread.Sleep(500);
                    //file name include voltage value;                  
                    string fileName = string.Format("preamp_{0:#0}xdelay_{1:#0}.dat", preamp, delay);
                    //create file writer
                    bw = new BinaryWriter(File.Open(fullPath + '\\' + fileName, FileMode.Create, FileAccess.Write, FileShare.Read));


                    // get ready for start acq thread
                    dataAcqTks.Dispose();       //clean up old token source
                    dataAcqTks = new CancellationTokenSource(); // generate a new token

                    SignalSource.openOutput();
                    Thread.Sleep(100); //wait 100ms

                    byte[] cmdBytes = new byte[2];
                    // start acq cmd is 0x0100;
                    cmdBytes[1] = 0x01;
                    cmdBytes[0] = 0x00;

                    CommandSend(cmdBytes, 2);
                    // check USB status

                    // Start data acquision thread
                    try
                    {
                        Task dataAcqTsk = Task.Factory.StartNew(() => this.dataAcq_threadFunc(dataAcqTks.Token, bw), dataAcqTks.Token);
                        Thread.Sleep(5 * 1000);  //acq 5 seconds
                                                 // time up!
                                                 // stop asic first
                        cmdBytes[1] = 0x02;
                        cmdBytes[0] = 0x00;
                        CommandSend(cmdBytes, 2);

                        // stop data receiving
                        dataAcqTks.Cancel();
                        Thread.Sleep(300);
                        while (!dataAcqTsk.IsCompleted) ;
                    }
                    catch (AggregateException excption)
                    {

                        foreach (var value in excption.InnerExceptions)
                        {

                            exceptionReport.AppendLine(excption.Message + " " + value.Message);
                        }

                    }

                    // stop signal
                    SignalSource.closeOutput();    
                                    
                }
            }

            Acq_status_label.Text = "IDLE";
            Acq_status_label.ForeColor = Color.Black;
        }

        private void volDelay_sweep_threadFunc(CancellationToken taskToken, Form2 paraWindows)
        {
            //TODO: decide not to finished
            // extDelay is the duration that signal minus trigger
            // unit is nano-second


            int voltage_start = int.Parse(paraWindows.start1.Text);
            int voltage_step = int.Parse(paraWindows.step1.Text);
            int voltage_stop = int.Parse(paraWindows.stop1.Text);

            uint delay_start = uint.Parse(paraWindows.start2.Text);
            uint delay_step = uint.Parse(paraWindows.step2.Text);
            uint delay_stop = uint.Parse(paraWindows.stop2.Text);


            // initial file writer and file
            BinaryWriter bw;

            DateTime dayStamp = DateTime.Now;
            string subDic = string.Format("{0:yyyyMMdd}_{0:HHmm}_VoltageDelaySweep", dayStamp);
            string fullPath = folderBrowserDialog1.SelectedPath + '\\' + subDic;
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);


            // use default settings
            //SignalSource.Write("*RCL 4");
            SignalSource.closeOutput();

            // outside loop is for preamp gain
            for (int voltage = voltage_start; voltage <= voltage_stop; voltage += voltage_step)
            {
                if (taskToken.IsCancellationRequested == true)
                {
                    break;
                }
                SignalSource.setVoltage(1, voltage);

                // inside loop is for trig_delay
                for (uint delay = delay_start; delay <= delay_stop; delay += delay_step)
                {
                    slowConfig.set_property(slowConfig.settings["DELAY_TRIGGER"], delay);
                    sendMessage("Sweep point: \n\t delay of asic:\t" + delay.ToString() + "\n\t voltage:\t" + voltage.ToString() + "\n");

                    if (taskToken.IsCancellationRequested == true)
                    {
                        break;
                    }
                    SignalSource.openOutput();
                    Thread.Sleep(100);

                    // Source1 is signal channel and Source2 is pulse channel
                    normal_config_button_Click(null, null);
                    Thread.Sleep(500);
                    //file name include voltage value;                  
                    string fileName = string.Format("voltage_{0:#0}mVxdelay_{1:#0}.dat", voltage, delay);
                    //create file writer
                    bw = new BinaryWriter(File.Open(fullPath + '\\' + fileName, FileMode.Create, FileAccess.Write, FileShare.Read));


                    // get ready for start acq thread
                    dataAcqTks.Dispose();       //clean up old token source
                    dataAcqTks = new CancellationTokenSource(); // generate a new token



                    byte[] cmdBytes = new byte[2];
                    // start acq cmd is 0x0100;
                    cmdBytes[1] = 0x01;
                    cmdBytes[0] = 0x00;

                    CommandSend(cmdBytes, 2);
                    // check USB status

                    // Start data acquision thread
                    try
                    {
                        Task dataAcqTsk = Task.Factory.StartNew(() => this.dataAcq_threadFunc(dataAcqTks.Token, bw), dataAcqTks.Token);
                        Thread.Sleep(5 * 1000);  //acq 5 seconds
                                                 // time up!
                                                 // stop asic first
                        cmdBytes[1] = 0x02;
                        cmdBytes[0] = 0x00;
                        CommandSend(cmdBytes, 2);

                        // stop data receiving
                        dataAcqTks.Cancel();
                        Thread.Sleep(300);
                        while (!dataAcqTsk.IsCompleted) ;
                    }
                    catch (AggregateException excption)
                    {

                        foreach (var value in excption.InnerExceptions)
                        {

                            exceptionReport.AppendLine(excption.Message + " " + value.Message);
                        }

                    }

                    // stop signal
                    SignalSource.closeOutput();

                }
            }

            Acq_status_label.Text = "IDLE";
            Acq_status_label.ForeColor = Color.Black;
        }

        private void ledCalib_threadFunc(CancellationToken taskToken, Form2 paraWindows)
        {
            int voltage_start = int.Parse(paraWindows.start1.Text);
            int voltage_step = int.Parse(paraWindows.step1.Text);
            int voltage_stop = int.Parse(paraWindows.stop1.Text);

            BinaryWriter bw;

            DateTime dayStamp = DateTime.Now;
            string subDic = string.Format("{0:yyyyMMdd}_{0:HHmm}_ledCalib", dayStamp);
            string fullPath = folderBrowserDialog1.SelectedPath + '\\' + subDic;
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            // use default settings
            //SignalSource.Write("*RCL 4");
            bw = new BinaryWriter(File.Open(fullPath + "\\led_calibration.dat"  , FileMode.Create, FileAccess.Write, FileShare.Read));
            SignalSource.setVoltage(1, voltage_start);
            SignalSource.openOutput();

            byte[] cmdBytes = new byte[2];
            // start acq cmd is 0x0100;
            cmdBytes[1] = 0x01;
            cmdBytes[0] = 0x00;

            CommandSend(cmdBytes, 2);

            dataAcqTks.Dispose();       //clean up old token source
            dataAcqTks = new CancellationTokenSource(); // generate a new token
            // check USB status
            if (usbStatus == false)
            {
                MessageBox.Show("USB is not connected");
            }


            // Start data acquision thread
            try
            {
                Task dataAcqTsk = Task.Factory.StartNew(() => this.dataAcq_threadFunc(dataAcqTks.Token, bw), dataAcqTks.Token);

            }
            catch (AggregateException excption)
            {

                foreach (var value in excption.InnerExceptions)
                {

                    exceptionReport.AppendLine(excption.Message + " " + value.Message);
                }

            }
            for (int v = voltage_start + voltage_step ; v <= voltage_step; v += voltage_stop)
            {
                sendMessage("Start acq at " + v.ToString() + " mV of Signal Source\n");
                if (taskToken.IsCancellationRequested == true)
                {
                    break;
                }
                else
                {

                    // tune voltage of channel 1
                    SignalSource.setVoltage(1, v);
                    //SignalSource.openOutput();
                    Thread.Sleep(1000); //wait 5 seconds

                }
            }



            // time up!
            // stop asic first
            cmdBytes[1] = 0x02;
            cmdBytes[0] = 0x00;
            CommandSend(cmdBytes, 2);

            // stop data receiving
            dataAcqTks.Cancel();

            // stop signal
            SignalSource.closeOutput();
            Thread.Sleep(500);
            Acq_status_label.Text = "IDLE";
            Acq_status_label.ForeColor = Color.Black;
        }
        // change textbox1.Text from sub-thread
        private void sendMessage(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(sendMessage);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox1.AppendText(text);
            }
        }



        #endregion

        #region other function
        //要复制的实例必须可序列化，包括实例引用的其它实例都必须在类定义时加[Serializable]特性。  
        public static T Copy<T>(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制     
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }

        byte toascii(byte origin)
        {
            byte ascii_out = 0;
            byte baseline1 = 0x30;
            byte baseline2 = 0x37;
            if (origin < 0x0a)
            {
                ascii_out = (byte)(baseline1 + origin);
            }
            else if (origin < 0x10)
            {
                ascii_out = (byte)(baseline2 + origin);
            }
            else
            {
                return 0;
            }
            return ascii_out;
        }

        int uart_send(byte[] cmd, int n)
        {
            //send cmd in c11024_01 form.
            //for example, computer want to Set the temperature correction factor,
            //what I need transport is [ 0x02, 'H' , 'S' , 'T' , '0' , '0', 'A', '1', '0', '0', '0', '1', '0','0','0','0','0','0','0','0', 'C', '8', 'B' ,'E', '9', '7', '2', 'B', 0x03, 'E', '8', 0x0D]
            // n is  27
            // cmd is "HST00A1000100000000C8BE972B", care of case sensitive

            byte[] TempCommand = new byte[2];
            byte checksum = 0;
            byte letter_high = 0;
            byte letter_low = 0;
            //ViStatus status;
            //int return_count = 0;
            //STX signal 0x02.
            TempCommand[1] = 0x03;      //0x05 stand for this is uart-form data
            TempCommand[0] = 0x02;

            CommandSend(TempCommand, 2);

            //if (status != VI_SUCCESS)
            //    return (-1);

            //Sleep(10);


            //what tranmit here is alraedy ascii code, all we need is send it to USB with 0x05
            for (int i = 0; i < n; i++)
            {
                checksum += cmd[i];
                TempCommand[0] = cmd[i];
                CommandSend(TempCommand, 2);

            }


            //ETX signal 0x03
            TempCommand[0] = 0x03;
            CommandSend(TempCommand, 2);



            // sum check
            checksum += 0x05;
            letter_high = toascii((byte)(checksum >> 4));
            letter_low = toascii((byte)(checksum & 0x0F));

            TempCommand[0] = letter_high;
            CommandSend(TempCommand, 2);

            TempCommand[0] = letter_low;
            CommandSend(TempCommand, 2);

            //CR send

            TempCommand[0] = 0x0D;
            CommandSend(TempCommand, 2);
            /*
            status = viWrite(instr, TempCommand, 2, &return_count);

            if (status != VI_SUCCESS)
                return (-1);
                */
            Thread.Sleep(10);

            TempCommand[1] = 0x05;
            TempCommand[0] = 0x02;
            CommandSend(TempCommand, 2);
            /*
            status = viWrite(instr, TempCommand, 2, &return_count);

            if (status != VI_SUCCESS)
                return (-1);
                */
            return (1);

        }

        void hv_set(decimal voltage)
        {
            byte[] voltage_hex = new byte[4];
            uint temp;
            string cmd;
            byte[] cmd_hex = new byte[100];
            int cmd_length = 0;

            temp = (uint)(voltage / (decimal)1.812 * 1000);
            voltage_hex[0] = toascii((byte)(temp >> 12));
            voltage_hex[1] = toascii((byte)((temp >> 8) & 0x0f));
            voltage_hex[2] = toascii((byte)((temp >> 4) & 0x0f));
            voltage_hex[3] = toascii((byte)(temp & 0x0f));

            cmd = "HST0000000000000000" + System.Text.Encoding.ASCII.GetString(voltage_hex) + "C8BE";
            cmd_hex = System.Text.Encoding.ASCII.GetBytes(cmd);
            cmd_length = 27;
            CommandSend(0x0503, 2); //选通HV control config sending channel

            uart_send(cmd_hex, cmd_length);

            CommandSend(0x0502, 2); //停止选通HV control config sending channel
        }

        void hv_switch(bool turnOn)
        {
            string cmd = "";
            byte[] cmd_hex = new byte[100];
            int cmd_length = cmd.Length;
            if (turnOn)
            {
                cmd = "HON";
            }
            else
            {
                cmd = "HOF";
            }
            cmd_hex = System.Text.Encoding.ASCII.GetBytes(cmd);
            cmd_length = 3;
            CommandSend(0x0503, 2); //选通HV control config sending channel
            uart_send(cmd_hex, cmd_length);
            CommandSend(0x0502, 2); //停止选通HV control config sending channel
        }

        uint bin2gray(uint x)
        {
            return x ^ (x >> 1);
        }

        uint gray2bin(uint x)
        {
            x ^= x >> 16;
            x ^= x >> 8;
            x ^= x >> 4;
            x ^= x >> 2;
            x ^= x ^ 1;
             return x;
        }
        uint reverse_bit(uint c, uint width)
        {
            // reverse a 6bit width value in bit-wise
            if(width == 6)
            {
                c = (c & 0x24) >> 2 | (c & 0x09) << 2 | (c & 0x12);
                c = (c & 0x38) >> 3 | (c & 0x07) << 3;
            }
            else if(width == 8){
                c = (c & 0xf0) >> 4 | (c & 0x0f) << 4;
                c = (c & 0xcc) >> 2 | (c & 0x33) << 2;
                c = (c & 0xaa) >> 1 | (c & 0x55) << 1;
            }
            return c;
        }
        #endregion

        #region Calib function
        private bool calib_turnOn()
        {
            byte[] cmdbytes = new byte[2];

            try
            {
                cmdbytes[1] = 0x0f;
                cmdbytes[0] = 0x01;
                if(CommandSend(cmdbytes, 2))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
            }
            return false;
        }
        private bool calib_turnOff()
        {
            byte[] cmdbytes = new byte[2];
            try
            {
                cmdbytes[1] = 0x0f;
                cmdbytes[0] = 0x00;
                if(CommandSend(cmdbytes, 2))
                {
                    return true;
                }
            }
            catch (Exception e)
            {

            }
            return false;
        }

        private bool calib_chooseGroup(int groupId)
        {
            byte[] cmdbytes = new byte[2];
            if(groupId > 8 || groupId < 1)
            {
                MessageBox.Show("Group ID is between 1-8");
                return false;
            }
            cmdbytes[1] = 0x0b;
            cmdbytes[0] = (byte)groupId;
            if(CommandSend(cmdbytes, 2))
            {
                return true;
            }
            return false;
        }

        private bool calib_S_pulse()
        {
            byte[] cmdbytes = new byte[2];
            cmdbytes[1] = 0x10;
            cmdbytes[0] = 0x01;
            if(CommandSend(cmdbytes, 2))
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}

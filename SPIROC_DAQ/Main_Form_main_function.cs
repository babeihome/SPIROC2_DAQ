// this file save function that user defined and thread function
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using CyUSB;

namespace SPIROC_DAQ
{
    partial class Main_Form
    {
        
        // private function
        private bool check_USB()
        {
            bool result = true;
            myDevice = usbDevices[VID, PID] as CyUSBDevice;
            AFG3252 = usbDevices[0x0699, 0x0345] as CyUSBDevice;
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

            if (AFG3252 != null)
            {
                afg3252_label.Text = "Connected";
                afg3252_label.ForeColor = Color.Green;
                //usbStatus = true;

                //normal_config_button.Enabled = true;

                AFG3252_command_point = AFG3252.EndPointOf(0x01) as CyBulkEndPoint; // EP0               


            }
            else
            {
                afg3252_label.Text = "USB not connected";
                afg3252_label.ForeColor = Color.Red;

                AFG3252_command_point = null;


                result = false;

            }
            return result;
        }

    
        private void bindEventHandle(GroupBox gbox, EventHandler handle)
        {

            foreach (Control item in gbox.Controls)
            {
                if(item is TextBox)
                {
                    item.TextChanged += handle;
                }
                else if(item is CheckBox)
                {
                    (item as CheckBox).CheckedChanged += handle;
                }
                else if(item is TableLayoutPanel)
                {
                    foreach(Control sub_item in (item as TableLayoutPanel).Controls)
                    {
                        if(sub_item is TextBox)
                        {
                            sub_item.TextChanged += handle;
                        }
                        else if(sub_item is CheckBox)
                        {
                            (sub_item as CheckBox).CheckedChanged += handle;
                        }
                    }
                }
            }
        }

        private void loadsettings()
        {
            const string cache_loc = ".\\cache\\";

        }
        private void refreshParamPanel()
        {
            // text box
            trig_dac_value.Text = slowConfig.get_property(settings.TRIG_DAC).ToString();
            gain_sel_value.Text = slowConfig.get_property(settings.GAIN_DAC).ToString();
            hgShapeValue.Text = slowConfig.get_property(settings.HG_SS_TIME_CONSTANT).ToString();
            lgShapeValue.Text = slowConfig.get_property(settings.LG_SS_TIME_CONSTANT).ToString();
            hgAmpComp.Text = slowConfig.get_property(settings.CAP_HG_PA_COMPENSATION).ToString();
            lgAmpComp.Text = slowConfig.get_property(settings.CAP_LG_PA_COMPENSATION).ToString();
            startrampDelay.Text = slowConfig.get_property(settings.DELAY_START_RAMP_TDC).ToString();
            triggerDelay.Text = slowConfig.get_property(settings.DELAY_TRIGGER).ToString();
            validholdDelay.Text = slowConfig.get_property(settings.DELAY_VALIDHOLD).ToString();
            rstcolDelay.Text = slowConfig.get_property(settings.DELAY_RSTCOL).ToString();
            adcResolution.Text = slowConfig.get_property(settings.ADC_GRAY).ToString();
            chipID.Text = slowConfig.get_property(settings.CHIPID).ToString();


            // combo box
            Dictionary<uint, string> adcramp_dic = new Dictionary<uint, string>();
            adcramp_dic.Add(0, "12bit");
            adcramp_dic.Add(2, "10bit");
            adcramp_dic.Add(3, "8bit");
            adcRampSlope_combo.Text = adcramp_dic[slowConfig.get_property(settings.ADC_RAMP_SLOPE)];

            Dictionary<uint, string> tdcramp_dic = new Dictionary<uint, string>();
            tdcramp_dic.Add(0, "fast");
            tdcramp_dic.Add(1, "slow");
            tdcRampSlope_combo.Text = tdcramp_dic[slowConfig.get_property(settings.TDC_RAMP_SLOPE_GC)];

            Dictionary<uint, string> fastshape_dic = new Dictionary<uint, string>();
            fastshape_dic.Add(0, "Low Gain");
            fastshape_dic.Add(1, "High Gain");
            fastShaperFrom_combo.Text = fastshape_dic[slowConfig.get_property(settings.FS)];

            Dictionary<uint, string> adjust4bit_dic = new Dictionary<uint, string>();
            adjust4bit_dic.Add(0, "fine");
            adjust4bit_dic.Add(1, "coaese");
            adjust4BitDAC_combo.Text = adjust4bit_dic[slowConfig.get_property(settings.ADJUST_4BIT_DAC)];


            triggerExt_enable.Checked = (slowConfig.get_property(settings.TRIG_EXT) == 1);  
            flagTdcExt_enable.Checked = (slowConfig.get_property(settings.FLAG_TDC_EXT) == 1);
            startRampAdcExt_enable.Checked = (slowConfig.get_property(settings.START_RAMP_ADC_EXT) == 1);
            startRampTdcExt_enable.Checked = (slowConfig.get_property(settings.START_RAMP_TDC_EXT) == 1);
            probe_enable.Checked = (slowConfig.get_property(settings.PROBE_OTA) == 1);
            analogOutput_enable.Checked = (slowConfig.get_property(settings.ENABLE_ANALOGUE_OUTPUT) == 1);
            or36_enable.Checked = (slowConfig.get_property(settings.EN_OR36) == 0);
            backSCA_enable.Checked = (slowConfig.get_property(settings.BACKUP_SCA) == 1);


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
                            c.Text = (slowConfig.get_property(settings.INDAC[chnNum]) >> 1).ToString();
                        }
                        else if (c is CheckBox)
                        {
                            var result = rx_inputdac_check.Match(c.Name);
                            chnNum = uint.Parse(result.Groups[1].Value);
                            (c as CheckBox).Checked = (slowConfig.get_property(settings.INDAC[chnNum]) & 0x01) == 1;
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
                                (checkBox as CheckBox).Checked = ((slowConfig.get_property(settings.DISCRIMINATOR_MASK1) & (1 << (chnNum - 18))) == 1);
                            }
                            else
                            {
                                (checkBox as CheckBox).Checked = ((slowConfig.get_property(settings.DISCRIMINATOR_MASK2) & (1 << chnNum)) == 1);
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
                            c.Text = (slowConfig.get_property(settings.PREAMP_GAIN[chnNum]) >> 2).ToString();
                        }
                        else if (c is CheckBox)
                        {
                            var result = rx_preamp_check.Match(c.Name);
                            chnNum = uint.Parse(result.Groups[1].Value);
                            (c as CheckBox).Checked = !((slowConfig.get_property(settings.PREAMP_GAIN[chnNum]) & 0x02) == 0x02);
                        }
                        else
                            continue;
                    }

                }
            }

            autoGain_Check.Checked = slowConfig.get_property(settings.AUTO_GAIN) == 1;
            gainSelect_Check.Checked = slowConfig.get_property(settings.GAIN_SELECT) == 1;
            adcExtInput_Check.Checked = slowConfig.get_property(settings.AUTO_GAIN) == 1;
            switchTDCon_Check.Checked = slowConfig.get_property(settings.SWITCH_TDC_ON) == 1;
            bandGap_Check.Checked = slowConfig.get_property(settings.EN_BANDGAP) == 1;
            dacEnable_Check.Checked = slowConfig.get_property(settings.EN_DAC) == 1;

        }
        #region USB interface operation
        // @@@   basic interface operation  -------------------

        //usb parameters
        // USB command send
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
        //data recieve method
        private bool DataRecieve(byte[] InData, ref int xferLen)
        {
            bool bResult;
            bResult = bulkInEndPt.XferData(ref InData, ref xferLen, true);
            return bResult;
        }

        private bool AFGcmdSend(byte[] OutData, int xferLen)
        {
            bool bResult = false;
            if (AFG3252_command_point == null)
            {
                bResult = false;
            }
            else
            {
                bResult = AFG3252_command_point.XferData(ref OutData, ref xferLen);
            }
            return bResult;
        }
        #endregion

        #region Thread-used function
        private void dataAcq_threadFunc(CancellationToken token)
        {
            byte[] data_buffer = new byte[512];
            int len;
            fileName = string.Format("{0:yyyyMMdd_HHmmss}", DateTime.Now) + ".dat";
            if (!Directory.Exists(fileDic))
            {
                Directory.CreateDirectory(fileDic);
            }

            BinaryWriter bw = new BinaryWriter(File.Open(fileDic + "\\\\" + fileName, FileMode.Append));
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
                        break;
                    }
                }
                else
                {
                    len = 512;
                    bResult = DataRecieve(data_buffer, ref len); // len could be changed for transmit actually num of byte that received
                    bw.Write(data_buffer, 0, len);
                }

            }
            bw.Flush();
            bw.Close();
            bw.Dispose();

        }


        #endregion

        #region other function
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
            TempCommand[1] = 0x05;      //0x05 stand for this is uart-form data
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

                //if (status != VI_SUCCESS)
                //    return (-1);
                //Sleep(10);
            }


            //ETX signal 0x03
            TempCommand[0] = 0x03;
            CommandSend(TempCommand, 2);
            //status = viWrite(instr, TempCommand, 2, &return_count);

            //if (status != VI_SUCCESS)
            //    return (-1);


            // sum check
            checksum += 0x05;
            letter_high = toascii((byte)(checksum >> 4));
            letter_low = toascii((byte)(checksum & 0x0F));

            TempCommand[0] = letter_high;

            CommandSend(TempCommand, 2);
            /*
            status = viWrite(instr, TempCommand, 2, &return_count);

            if (status != VI_SUCCESS)
                return (-1);
            //Sleep(10);
            */

            TempCommand[0] = letter_low;

            CommandSend(TempCommand, 2);
            /*
            status = viWrite(instr, TempCommand, 2, &return_count);

            if (status != VI_SUCCESS)
                return (-1);
                */

            //CR send

            TempCommand[0] = 0x0D;
            CommandSend(TempCommand, 2);
            /*
            status = viWrite(instr, TempCommand, 2, &return_count);

            if (status != VI_SUCCESS)
                return (-1);
                */
            Thread.Sleep(10);

            TempCommand[1] = 0x07;
            TempCommand[0] = 0x00;
            CommandSend(TempCommand, 2);
            /*
            status = viWrite(instr, TempCommand, 2, &return_count);

            if (status != VI_SUCCESS)
                return (-1);
                */
            return (1);

        }
        #endregion
    }
}

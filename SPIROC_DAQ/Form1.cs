using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CyUSB;

namespace SPIROC_DAQ
{
    public partial class Main_Form : Form
    {
        private USBDeviceList usbDevices;
        private CyUSBDevice myDevice;
        private CyBulkEndPoint bulkInEndPt;
        private CyBulkEndPoint bulkOutEndPt;
        private SC_model slowConfig;
        private bool usbStatus = false;
        private string fileDic;    // store path of data file
        private string fileName;
        private const int VID = 0x04B4;
        private const int PID = 0x1004;
        private CancellationTokenSource dataAcqTks = new CancellationTokenSource();
        private StringBuilder exceptionReport = new StringBuilder();

        private string rx_Command = @"\b[0-9a-fA-F]{4}\b";//match 16 bit Hex
        private string rx_Byte = @"\b[0-9a-fA-F]{2}\b";//match 8 bit Hex
        private string rx_Bit = @"\b[0-1]{12}\b"; //match 12bit Binary
        private string rx_Integer = @"^\d+$";   //匹配非负 整数

        private int settingChoosen = 0;


        public Main_Form()
        {
            InitializeComponent();
            File_path_showbox.Text = folderBrowserDialog1.SelectedPath;
            fileDic = folderBrowserDialog1.SelectedPath + "\\\\default_test";

            // Dynamic list of USB devices bound to CyUSB.sys
            usbDevices = new USBDeviceList(CyConst.DEVICES_CYUSB);
            slowConfig = new SC_model();
            slowConfig.save_settings(0);
            refreshParamPanel();
            //slowConfig.recall_settings(0);
            // Adding event handles for action of attachment and removal of device
            usbDevices.DeviceAttached += new EventHandler(deviceAttached);
            usbDevices.DeviceRemoved += new EventHandler(deviceRemoved);

            check_USB();
        }


        // Event handler
        private void deviceAttached(object sender, EventArgs e)
        {
            check_USB();
            return;
        }

        private void deviceRemoved(object sender, EventArgs e)
        {
            check_USB();
        }


        // private function
        private bool check_USB()
        {
            myDevice = usbDevices[VID, PID] as CyUSBDevice;

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
                return true;

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

                return false;

            }

        }



        private void refreshParamPanel()
        {
            trig_dac_value.Text = slowConfig.get_property(settings.TRIG_DAC).ToString();
            gain_sel_value.Text = slowConfig.get_property(settings.GAIN_DAC).ToString();
            hgShapeValue.Text = slowConfig.get_property(settings.HG_SS_TIME_CONSTANT).ToString();
            lgShapeValue.Text = slowConfig.get_property(settings.LG_SS_TIME_CONSTANT).ToString();
        }
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


        private void input_dac_num_ValueChanged(object sender, EventArgs e)
        {
            //input_dac_num.Value++;
        }

        private void normal_usbcon_button_Click(object sender, EventArgs e)
        {
            if (check_USB() != true)
            {
                MessageBox.Show("USB can't be connected", "Error");
            }
        }

        private void normal_config_button_Click(object sender, EventArgs e)
        {
            int byte_count = 0;

            byte[] cmdBytes = new byte[2];
            byte[] bit_block = new byte[117];  //SPIROC2b has 929 config bit, 929 / 8 = 116 ... 1, need 117 bytes
            byte_count = slowConfig.bit_transform(ref bit_block);


            // set probe/sc setting as slow control
            cmdBytes[1] = 0x06;
            cmdBytes[0] = 0x01;
            CommandSend(cmdBytes, 2);

            // start sc mode
            cmdBytes[1] = 0x08;
            cmdBytes[0] = 0x00;
            CommandSend(cmdBytes, 2);

            // send config data
            cmdBytes[1] = 0x03;
            for (int i = 0; i < byte_count; i++)
            {
                cmdBytes[0] = bit_block[i];
                CommandSend(cmdBytes, 2);
            }

            // show relative message
            textBox1.AppendText("Slow control config successed\n");
            normal_acq_button.Enabled = true;
            Config_status_label.Text = "Configured";
            Config_status_label.ForeColor = Color.Green;

        }

        private void normal_acq_button_Click(object sender, EventArgs e)
        {
            byte[] cmdBytes = new byte[2];
            // clear USB fifo

            // start acq cmd is 0x0100;
            cmdBytes[1] = 0x01;
            cmdBytes[0] = 0x00;

            dataAcqTks.Dispose();       //clean up old token source
            dataAcqTks = new CancellationTokenSource(); // generate a new token

            CommandSend(cmdBytes, 2);
            // check USB status
            if (usbStatus == false)
            {
                MessageBox.Show("USB is not connected");
            }

            // Start data acquision thread
            try
            {
                Task dataAcqTsk = Task.Factory.StartNew(() => this.dataAcq_threadFunc(dataAcqTks.Token), dataAcqTks.Token);
            }
            catch (AggregateException excption)
            {

                foreach (var v in excption.InnerExceptions)
                {

                    exceptionReport.AppendLine(excption.Message + " " + v.Message);
                }

            }
            normal_stop_button.Enabled = true;
            normal_acq_button.Enabled = false;

            Acq_status_label.ForeColor = Color.Firebrick;
            Acq_status_label.Text = "Acquiring";
        }

        private void normal_stop_button_Click(object sender, EventArgs e)
        {
            byte[] cmdBytes = new byte[2];

            // start acq cmd is 0x0200;
            cmdBytes[1] = 0x02;
            cmdBytes[0] = 0x00;

            CommandSend(cmdBytes, 2);

            // check USB status
            if (usbStatus == false)
            {
                MessageBox.Show("USB is not connected");
            }
            dataAcqTks.Cancel();

            normal_acq_button.Enabled = true;
            normal_stop_button.Enabled = false;

            Acq_status_label.ForeColor = Color.Black;
            Acq_status_label.Text = "IDLE";

        }

        private void File_path_select_button_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            fileDic = folderBrowserDialog1.SelectedPath;
            File_path_showbox.Text = fileDic;

        }

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

        private void trig_dac_value_TextChanged(object sender, EventArgs e)
        {
            uint value = 0;
            Regex rx_int = new Regex(rx_Integer);

            // check input valid
            if (rx_int.IsMatch(trig_dac_value.Text))
            {
                value = uint.Parse(trig_dac_value.Text);
                if (0 <= value && value <= 1023)
                {
                    slowConfig.set_property(settings.TRIG_DAC, value);
                    return;
                }
            }

            MessageBox.Show("value need be in range of 0-1023", "Value invalid");

        }

        private void gain_sel_value_TextChanged(object sender, EventArgs e)
        {
            uint value = 0;
            Regex rx_int = new Regex(rx_Integer);

            // check input valid
            if (rx_int.IsMatch(gain_sel_value.Text))
            {
                value = uint.Parse(gain_sel_value.Text);
                if (0 <= value && value <= 1023)
                {
                    slowConfig.set_property(settings.GAIN_DAC, value);
                    return;
                }
            }

            MessageBox.Show("value need be in range of 0-1023", "Value invalid");

        }


        private void inputDAC_switch_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void saveSetting_btn_Click(object sender, EventArgs e)
        {

            slowConfig.settingName = setting_name.Text;
            slowConfig.save_settings(settingChoosen);
            switch(settingChoosen)
            {
                case 1:
                    config_set1.Text = setting_name.Text;
                    break;
                case 2:
                    config_set2.Text = setting_name.Text;
                    break;
                case 3:
                    config_set3.Text = setting_name.Text;
                    break;
                case 4:
                    config_set4.Text = setting_name.Text;
                    break;
                default:
                    MessageBox.Show("Please choose which setting plot you want to use", "Error");
                    break;
            }
        }

        private void setting_Choosen(object sender, EventArgs e)
        {
            if (config_set1.Checked == true)
            {
                settingChoosen = 1;
            }
            else if (config_set2.Checked == true)
            {
                settingChoosen = 2;
            }
            else if (config_set3.Checked == true)
            {
                settingChoosen = 3;
            }
            else if (config_set4.Checked == true)
            {
                settingChoosen = 4;
            }
        }

        private void hgShapeValue_TextChanged(object sender, EventArgs e)
        {
            uint value = 0;
            Regex rx_int = new Regex(rx_Integer);

            // check input valid
            if (rx_int.IsMatch(hgShapeValue.Text))
            {
                value = uint.Parse(trig_dac_value.Text);
                if (0 <= value && value <= 7)
                {
                    slowConfig.set_property(settings.HG_SS_TIME_CONSTANT, value);
                    return;
                }
            }

            MessageBox.Show("value need be in range of 0-7", "Value invalid");

        }

        private void lgShapeValue_TextChanged(object sender, EventArgs e)
        {
            uint value = 0;
            Regex rx_int = new Regex(rx_Integer);

            // check input valid
            if (rx_int.IsMatch(lgShapeValue.Text))
            {
                value = uint.Parse(trig_dac_value.Text);
                if (0 <= value && value <= 7)
                {
                    slowConfig.set_property(settings.LG_SS_TIME_CONSTANT, value);
                    return;
                }
            }

            MessageBox.Show("value need be in range of 0-7", "Value invalid");

        }

        private void hgAmpComp_TextChanged(object sender, EventArgs e)
        {
            uint value = 0;
            Regex rx_int = new Regex(rx_Integer);

            // check input valid
            if (rx_int.IsMatch(hgAmpComp.Text))
            {
                value = uint.Parse(hgAmpComp.Text);
                if (0 <= value && value <= 15)
                {
                    slowConfig.set_property(settings.CAP_HG_PA_COMPENSATION, value);
                    return;
                }
            }

            MessageBox.Show("value need be in range of 0-15", "Value invalid");

        }

        private void lgAmpComp_TextChanged(object sender, EventArgs e)
        {
            uint value = 0;
            Regex rx_int = new Regex(rx_Integer);

            // check input valid
            if (rx_int.IsMatch(lgAmpComp.Text))
            {
                value = uint.Parse(trig_dac_value.Text);
                if (0 <= value && value <= 15)
                {
                    slowConfig.set_property(settings.CAP_LG_PA_COMPENSATION, value);
                    return;
                }
            }

            MessageBox.Show("value need be in range of 0-15", "Value invalid");

        }

        private void startrampDelay_TextChanged(object sender, EventArgs e)
        {
            uint value = 0;
            Regex rx_int = new Regex(rx_Integer);

            // check input valid
            if (rx_int.IsMatch(startrampDelay.Text))
            {
                value = uint.Parse(startrampDelay.Text);
                if (0 <= value && value <= 63)
                {
                    slowConfig.set_property(settings.DELAY_START_RAMP_TDC, value);
                    return;
                }
            }

            MessageBox.Show("value need be in range of 0-63", "Value invalid");

        }

        private void triggerDelay_TextChanged(object sender, EventArgs e)
        {
            uint value = 0;
            Regex rx_int = new Regex(rx_Integer);

            // check input valid
            if (rx_int.IsMatch(triggerDelay.Text))
            {
                value = uint.Parse(triggerDelay.Text);
                if (0 <= value && value <= 63)
                {
                    slowConfig.set_property(settings.DELAY_TRIGGER, value);
                    return;
                }
            }

            MessageBox.Show("value need be in range of 0-1023", "Value invalid");

        }

        private void validholdDelay_TextChanged(object sender, EventArgs e)
        {
            uint value = 0;
            Regex rx_int = new Regex(rx_Integer);

            // check input valid
            if (rx_int.IsMatch(validholdDelay.Text))
            {
                value = uint.Parse(validholdDelay.Text);
                if (0 <= value && value <= 63)
                {
                    slowConfig.set_property(settings.DELAY_VALIDHOLD, value);
                    return;
                }
            }

            MessageBox.Show("value need be in range of 0-63", "Value invalid");

        }

        private void rstcolDelay_TextChanged(object sender, EventArgs e)
        {
            uint value = 0;
            Regex rx_int = new Regex(rx_Integer);

            // check input valid
            if (rx_int.IsMatch(rstcolDelay.Text))
            {
                value = uint.Parse(rstcolDelay.Text);
                if (0 <= value && value <= 63)
                {
                    slowConfig.set_property(settings.DELAY_RSTCOL, value);
                    return;
                }
            }

            MessageBox.Show("value need be in range of 0-63", "Value invalid");

        }


        private void adcResolution_TextChanged(object sender, EventArgs e)
        {
            uint value = 0;
            Regex rx_bit = new Regex(rx_Bit);
            // check input valid
            if (rx_bit.IsMatch(adcResolution.Text))
            {
                value = uint.Parse(adcResolution.Text);
                if (0 <= value && value <= 4095)
                {
                    slowConfig.set_property(settings.ADC_GRAY, value);
                    return;
                }
            }

            MessageBox.Show("12bit Gray Code, LSB -> MSB", "Value invalid");
        }

        private void chipID_TextChanged(object sender, EventArgs e)
        {
            uint value = 0;
            Regex rx_bit = new Regex(rx_Bit);

            // check input valid
            if (rx_bit.IsMatch(chipID.Text))
            {
                value = uint.Parse(chipID.Text);
                if (0 <= value && value <= 255)
                {
                    slowConfig.set_property(settings.CHIPID, value);
                    return;
                }
            }

            MessageBox.Show("8bit chipID, and will show in data the way of Gray", "Value Error");
        }

        private void adcRampSlope_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint value = 0;
            switch (adcRampSlope_combo.SelectedIndex)
            {
                case 0:
                    value = 0;
                    break;
                case 1:
                    value = 2;
                    break;
                case 2:
                    value = 3;
                    break;
                default:
                    value = 1;  // error
                    break;
            }
            if (value != 1)
            {
                slowConfig.set_property(settings.ADC_RAMP_SLOPE, value);
                return;
            }
            MessageBox.Show("Item selected is invalid", "Value Invalid");
        }

        private void tdcRampSlope_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint value = 0;
            switch (tdcRampSlope_combo.SelectedIndex)
            {
                case 0:
                    value = 0;
                    break;
                case 1:
                    value = 1;
                    break;
                default:
                    value = 2;  // error
                    break;
            }
            if (value != 2)
            {
                slowConfig.set_property(settings.TDC_RAMP_SLOPE_GC, value);
                return;
            }
            MessageBox.Show("Item selected is invalid", "Value Invalid");
        }

        private void fastShaperFrom_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint value = 0;
            switch (fastShaperFrom_combo.SelectedIndex)
            {
                case 0:
                    value = 1;
                    break;
                case 1:
                    value = 0;
                    break;
                default:
                    value = 2;  // error
                    break;
            }
            if (value != 2)
            {
                slowConfig.set_property(settings.FS, value);
                return;
            }
            MessageBox.Show("Item selected is invalid", "Value Invalid");
        }

        private void adjust4BitDAC_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint value = 0;
            switch (adjust4BitDAC_combo.SelectedIndex)
            {
                case 0:
                    value = 0;
                    break;
                case 1:
                    value = 1;
                    break;
                default:
                    value = 2;  // error
                    break;
            }
            if (value != 2)
            {
                slowConfig.set_property(settings.ADJUST_4BIT_DAC, value);
                return;
            }
            MessageBox.Show("Item selected is invalid", "Value Invalid");
        }

        private void triggerExt_enable_CheckedChanged(object sender, EventArgs e)
        {
            uint value = 0;
            if(triggerExt_enable.Checked == true)
            {
                triggerExt_enable.Text = "\tEnable";
                value = 1;
            }
            else
            {
                triggerExt_enable.Text = "\tDisable";
                value = 0;
            }
            slowConfig.set_property(settings.TRIG_EXT, value);
        }

        private void flagTdcExt_enable_CheckedChanged(object sender, EventArgs e)
        {
            uint value = 0;
            if (flagTdcExt_enable.Checked == true)
            {
                flagTdcExt_enable.Text = "\tEnable";
                value = 1;
            }
            else
            {
                flagTdcExt_enable.Text = "\tDisable";
                value = 0;
            }
            slowConfig.set_property(settings.FLAG_TDC_EXT, value);
        }

        private void startRampAdcExt_enable_CheckedChanged(object sender, EventArgs e)
        {
            uint value = 0;
            if (startRampAdcExt_enable.Checked == true)
            {
                startRampAdcExt_enable.Text = "\tEnable";
                value = 1;
            }
            else
            {
                startRampAdcExt_enable.Text = "\tDisable";
                value = 0;
            }
            slowConfig.set_property(settings.START_RAMP_ADC_EXT, value);
        }

        private void startRampTdcExt_enable_CheckedChanged(object sender, EventArgs e)
        {
            uint value = 0;
            if (startRampTdcExt_enable.Checked == true)
            {
                startRampTdcExt_enable.Text = "\tEnable";
                value = 1;
            }
            else
            {
                startRampTdcExt_enable.Text = "\tDisable";
                value = 0;
            }
            slowConfig.set_property(settings.START_RAMP_TDC_EXT, value);
        }

        private void probe_enable_CheckedChanged(object sender, EventArgs e)
        {
            uint value = 0;
            if (probe_enable.Checked == true)
            {
                probe_enable.Text = "\tEnable";
                value = 1;
            }
            else
            {
                probe_enable.Text = "\tDisable";
                value = 0;
            }
            slowConfig.set_property(settings.PROBE_OTA, value);
        }

        private void analogOutput_enable_CheckedChanged(object sender, EventArgs e)
        {
            uint value = 0;
            if (analogOutput_enable.Checked == true)
            {
                analogOutput_enable.Text = "\tEnable";
                value = 1;
            }
            else
            {
                analogOutput_enable.Text = "\tDisable";
                value = 0;
            }
            slowConfig.set_property(settings.ENABLE_ANALOGUE_OUTPUT, value);
        }

        private void or36_enable_CheckedChanged(object sender, EventArgs e)
        {
            uint value = 0;
            if (or36_enable.Checked == true)
            {
                or36_enable.Text = "\tEnable";
                value = 0;
            }
            else
            {
                or36_enable.Text = "\tDisable";
                value = 1;
            }
            slowConfig.set_property(settings.EN_OR36, value);
        }

        private void backSCA_enable_CheckedChanged(object sender, EventArgs e)
        {
            uint value = 0;
            if (backSCA_enable.Checked == true)
            {
                backSCA_enable.Text = "\tEnable";
                value = 1;
            }
            else
            {
                backSCA_enable.Text = "\tDisable";
                value = 0;
            }
            slowConfig.set_property(settings.BACKUP_SCA, value);
        }

        private void recallSetting_btn_Click(object sender, EventArgs e)
        {
            slowConfig.recall_settings(settingChoosen);
            refreshParamPanel();
        }
    }
}

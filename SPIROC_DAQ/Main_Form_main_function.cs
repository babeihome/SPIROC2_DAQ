// this file save function that user defined and thread function
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            adcRampSlope_combo.SelectedText = adcramp_dic[slowConfig.get_property(settings.ADC_RAMP_SLOPE)];

            Dictionary<uint, string> tdcramp_dic = new Dictionary<uint, string>();
            tdcramp_dic.Add(0, "fast");
            tdcramp_dic.Add(1, "slow");
            tdcRampSlope_combo.SelectedText = tdcramp_dic[slowConfig.get_property(settings.TDC_RAMP_SLOPE_GC)];

            Dictionary<uint, string> fastshape_dic = new Dictionary<uint, string>();
            fastshape_dic.Add(0, "Low Gain");
            fastshape_dic.Add(1, "High Gain");
            fastShaperFrom_combo.SelectedText = fastshape_dic[slowConfig.get_property(settings.FS)];

            Dictionary<uint, string> adjust4bit_dic = new Dictionary<uint, string>();
            adjust4bit_dic.Add(0, "fine");
            adjust4bit_dic.Add(1, "coaese");
            adjust4BitDAC_combo.SelectedText = adjust4bit_dic[slowConfig.get_property(settings.ADJUST_4BIT_DAC)];


            triggerExt_enable.Checked = slowConfig.get_property(settings.TRIG_EXT) == 1;
            flagTdcExt_enable.Checked = slowConfig.get_property(settings.FLAG_TDC_EXT) == 1;
            startRampAdcExt_enable.Checked = slowConfig.get_property(settings.START_RAMP_ADC_EXT) == 1;
            startRampTdcExt_enable.Checked = slowConfig.get_property(settings.START_RAMP_TDC_EXT) == 1;
            probe_enable.Checked = slowConfig.get_property(settings.PROBE_OTA) == 1;
            analogOutput_enable.Checked = slowConfig.get_property(settings.ENABLE_ANALOGUE_OUTPUT) == 1;
            or36_enable.Checked = slowConfig.get_property(settings.EN_OR36) == 0;
            backSCA_enable.Checked = slowConfig.get_property(settings.BACKUP_SCA) == 1;

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
    }
}

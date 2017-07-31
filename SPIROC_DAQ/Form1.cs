using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
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


        public Main_Form()
        {
            InitializeComponent();
            File_path_showbox.Text = folderBrowserDialog1.SelectedPath;

            // Dynamic list of USB devices bound to CyUSB.sys
            usbDevices = new USBDeviceList(CyConst.DEVICES_CYUSB);
            slowConfig = new SC_model();
            slowConfig.save_settings(0);
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

        // @@@   basic interface operation  -------------------

        //usb parameters


        // USB command send
        private bool CommandSend(byte[] OutData, int xferLen)
        {
            bool bResult = false;
            if (bulkInEndPt == null)
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


        //  Auto-generated response function
        private void normal_task_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void input_dac_num_ValueChanged(object sender, EventArgs e)
        {
            input_dac_num.Value++;
        }

        private void normal_usbcon_button_Click(object sender, EventArgs e)
        {
            if(check_USB() != true)
            {
                MessageBox.Show("USB can't be connected","Error");
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
                cmdBytes[0] = 0x06;
                CommandSend(cmdBytes, 2);
            }

            // show relative message
            textBox1.AppendText("Slow control config successed");

        }

        private void normal_acq_button_Click(object sender, EventArgs e)
        {
            byte[] cmdBytes = new byte[2];
            // clear USB fifo

            // start acq cmd is 0x0100;
            cmdBytes[1] = 0x01;
            cmdBytes[0] = 0x00;


            // check USB status
            if (usbStatus == false)
            {
                MessageBox.Show("USB is not connected");
            }

            // Start data acquision thread
            try
            {
                Task dataAcqTsk = Task.Factory.StartNew(() => dataAcq_threadFunc(dataAcqTks.Token), dataAcqTks.Token);
            }
            catch (AggregateException excption)
            {
 
                foreach (var v in excption.InnerExceptions)
                {

                    exceptionReport.AppendLine(excption.Message + " " + v.Message);
                }
                    
            }
        }

        private void normal_stop_button_Click(object sender, EventArgs e)
        {
            byte[] cmdBytes = new byte[2];

            // start acq cmd is 0x0200;
            cmdBytes[1] = 0x02;
            cmdBytes[0] = 0x00;

            // check USB status
            if (usbStatus == false)
            {
                MessageBox.Show("USB is not connected");
            }
            dataAcqTks.Cancel();

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
            fileName = DateTime.Now.ToString() + ".dat";
            BinaryWriter bw = new BinaryWriter(File.Open(fileDic + fileName, FileMode.Append));
            bool bResult;
            while(true)
            {
                if(token.IsCancellationRequested == true)
                {
                    Thread.Sleep(100);
                    len = 512;
                    bResult = DataRecieve(data_buffer, ref len); // len could be changed for transmit actually num of byte that received
                    bw.Write(data_buffer, 0, len);   // data source, start_index, data_length
                    if(bResult == false)
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

            token.ThrowIfCancellationRequested();
            
        }

        #endregion


    }
}

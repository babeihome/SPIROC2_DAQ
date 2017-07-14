using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        private const int VID = 0x04B4;
        private const int PID = 0x1004;


        public Main_Form()
        {
            InitializeComponent();

            // Dynamic list of USB devices bound to CyUSB.sys
            usbDevices = new USBDeviceList(CyConst.DEVICES_CYUSB);

            // Adding event handles for action of attachment and removal of device
            usbDevices.DeviceAttached += new EventHandler(check_USB);
            usbDevices.DeviceRemoved += new EventHandler(check_USB);

            check_USB(null, null);
        }

        private void check_USB(object sender, EventArgs e)
        {
            myDevice = usbDevices[VID, PID] as CyUSBDevice;

            if (myDevice != null)
            {
                Usb_status_label.Text = "USB device connected";
                Usb_status_label.ForeColor = Color.Green;
                normal_config_button.Enabled = true;

                bulkOutEndPt = myDevice.EndPointOf(0x08) as CyBulkEndPoint; // EP8
                bulkInEndPt = myDevice.EndPointOf(0x82) as CyBulkEndPoint; //EP2

                bulkInEndPt.XferSize = bulkInEndPt.MaxPktSize * 8;  // transfer size means the max limits of data in USB driver
              
            }
            else
            {
                Usb_status_label.Text = "USB not connected";
                Usb_status_label.ForeColor = Color.Red;
                normal_config_button.Enabled = false;
                normal_acq_button.Enabled = false;
                normal_stop_button.Enabled = false;

                bulkOutEndPt = null;
                bulkInEndPt = null;

            }
                
        }

        // basic interface operation

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
        private bool DataRecieve(byte[] InData, int xferLen)
        {
            bool bResult;
            bResult = bulkInEndPt.XferData(ref InData, ref xferLen, true);
            return bResult;
        }
        private void normal_task_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void input_dac_num_ValueChanged(object sender, EventArgs e)
        {
            input_dac_num.Value++;
        }

        private void normal_usbcon_button_Click(object sender, EventArgs e)
        {

        }
    }
}

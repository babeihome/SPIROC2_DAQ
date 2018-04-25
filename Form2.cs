using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPIROC_DAQ
{
    public partial class Form2 : Form
    {
        public bool confirm = true;
        public Form2()
        {
            InitializeComponent();
        }

        private void confirm_btn_Click(object sender, EventArgs e)
        {
            confirm = true;
            this.Close();
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            confirm = false;
            this.Close();
        }
    }
}

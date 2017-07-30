namespace SPIROC_DAQ
{
    partial class Main_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Status_panel = new System.Windows.Forms.TableLayoutPanel();
            this.USB_status_group = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Usb_status_label = new System.Windows.Forms.Label();
            this.Configure_status_group = new System.Windows.Forms.GroupBox();
            this.Config_status_label = new System.Windows.Forms.Label();
            this.Acq_status_group = new System.Windows.Forms.GroupBox();
            this.Acq_status_label = new System.Windows.Forms.Label();
            this.Warning_group = new System.Windows.Forms.GroupBox();
            this.Warning_label = new System.Windows.Forms.Label();
            this.Status_label = new System.Windows.Forms.Label();
            this.Main_group = new System.Windows.Forms.GroupBox();
            this.Main_tab = new System.Windows.Forms.TabControl();
            this.Flow_tab = new System.Windows.Forms.TabPage();
            this.task_panel = new System.Windows.Forms.TableLayoutPanel();
            this.normal_label = new System.Windows.Forms.Label();
            this.normal_task_panel = new System.Windows.Forms.FlowLayoutPanel();
            this.normal_usbcon_button = new System.Windows.Forms.Button();
            this.normal_config_button = new System.Windows.Forms.Button();
            this.normal_acq_button = new System.Windows.Forms.Button();
            this.normal_stop_button = new System.Windows.Forms.Button();
            this.SC_tab = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.input_dac_num = new System.Windows.Forms.NumericUpDown();
            this.input_dac_label1 = new System.Windows.Forms.Label();
            this.Debug_tab = new System.Windows.Forms.TabPage();
            this.clear_button = new System.Windows.Forms.Button();
            this.File_group = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.file_path_label = new System.Windows.Forms.Label();
            this.File_path_showbox = new System.Windows.Forms.TextBox();
            this.File_path_select_button = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Msg_label = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Status_panel.SuspendLayout();
            this.USB_status_group.SuspendLayout();
            this.panel1.SuspendLayout();
            this.Configure_status_group.SuspendLayout();
            this.Acq_status_group.SuspendLayout();
            this.Warning_group.SuspendLayout();
            this.Main_group.SuspendLayout();
            this.Main_tab.SuspendLayout();
            this.Flow_tab.SuspendLayout();
            this.task_panel.SuspendLayout();
            this.normal_task_panel.SuspendLayout();
            this.SC_tab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input_dac_num)).BeginInit();
            this.File_group.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Status_panel
            // 
            this.Status_panel.ColumnCount = 1;
            this.Status_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Status_panel.Controls.Add(this.USB_status_group, 0, 0);
            this.Status_panel.Controls.Add(this.Configure_status_group, 0, 1);
            this.Status_panel.Controls.Add(this.Acq_status_group, 0, 2);
            this.Status_panel.Controls.Add(this.Warning_group, 0, 3);
            this.Status_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Status_panel.Location = new System.Drawing.Point(732, 43);
            this.Status_panel.Name = "Status_panel";
            this.Status_panel.RowCount = 4;
            this.Status_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Status_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Status_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Status_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Status_panel.Size = new System.Drawing.Size(182, 496);
            this.Status_panel.TabIndex = 6;
            // 
            // USB_status_group
            // 
            this.USB_status_group.Controls.Add(this.panel1);
            this.USB_status_group.Location = new System.Drawing.Point(3, 3);
            this.USB_status_group.Name = "USB_status_group";
            this.USB_status_group.Size = new System.Drawing.Size(169, 100);
            this.USB_status_group.TabIndex = 0;
            this.USB_status_group.TabStop = false;
            this.USB_status_group.Text = "USB Status";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Usb_status_label);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(163, 80);
            this.panel1.TabIndex = 0;
            // 
            // Usb_status_label
            // 
            this.Usb_status_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Usb_status_label.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Usb_status_label.ForeColor = System.Drawing.Color.Red;
            this.Usb_status_label.Location = new System.Drawing.Point(0, 0);
            this.Usb_status_label.Name = "Usb_status_label";
            this.Usb_status_label.Size = new System.Drawing.Size(163, 80);
            this.Usb_status_label.TabIndex = 0;
            this.Usb_status_label.Text = "USB not connected";
            this.Usb_status_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Configure_status_group
            // 
            this.Configure_status_group.Controls.Add(this.Config_status_label);
            this.Configure_status_group.Location = new System.Drawing.Point(3, 127);
            this.Configure_status_group.Name = "Configure_status_group";
            this.Configure_status_group.Size = new System.Drawing.Size(169, 100);
            this.Configure_status_group.TabIndex = 1;
            this.Configure_status_group.TabStop = false;
            this.Configure_status_group.Text = "Configure";
            // 
            // Config_status_label
            // 
            this.Config_status_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Config_status_label.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Config_status_label.ForeColor = System.Drawing.Color.Red;
            this.Config_status_label.Location = new System.Drawing.Point(3, 17);
            this.Config_status_label.Name = "Config_status_label";
            this.Config_status_label.Size = new System.Drawing.Size(163, 80);
            this.Config_status_label.TabIndex = 0;
            this.Config_status_label.Text = "Haven\'t Configure";
            this.Config_status_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Acq_status_group
            // 
            this.Acq_status_group.Controls.Add(this.Acq_status_label);
            this.Acq_status_group.Location = new System.Drawing.Point(3, 251);
            this.Acq_status_group.Name = "Acq_status_group";
            this.Acq_status_group.Size = new System.Drawing.Size(169, 100);
            this.Acq_status_group.TabIndex = 2;
            this.Acq_status_group.TabStop = false;
            this.Acq_status_group.Text = "Acq Status";
            // 
            // Acq_status_label
            // 
            this.Acq_status_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Acq_status_label.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Acq_status_label.Location = new System.Drawing.Point(3, 17);
            this.Acq_status_label.Name = "Acq_status_label";
            this.Acq_status_label.Size = new System.Drawing.Size(163, 80);
            this.Acq_status_label.TabIndex = 0;
            this.Acq_status_label.Text = "IDLE";
            this.Acq_status_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Warning_group
            // 
            this.Warning_group.Controls.Add(this.Warning_label);
            this.Warning_group.Location = new System.Drawing.Point(3, 375);
            this.Warning_group.Name = "Warning_group";
            this.Warning_group.Size = new System.Drawing.Size(169, 100);
            this.Warning_group.TabIndex = 3;
            this.Warning_group.TabStop = false;
            this.Warning_group.Text = "Warning";
            // 
            // Warning_label
            // 
            this.Warning_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Warning_label.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold);
            this.Warning_label.ForeColor = System.Drawing.SystemColors.Desktop;
            this.Warning_label.Location = new System.Drawing.Point(3, 17);
            this.Warning_label.Name = "Warning_label";
            this.Warning_label.Size = new System.Drawing.Size(163, 80);
            this.Warning_label.TabIndex = 0;
            this.Warning_label.Text = "OK";
            this.Warning_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Status_label
            // 
            this.Status_label.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Status_label.AutoSize = true;
            this.Status_label.Location = new System.Drawing.Point(802, 19);
            this.Status_label.Name = "Status_label";
            this.Status_label.Size = new System.Drawing.Size(41, 12);
            this.Status_label.TabIndex = 5;
            this.Status_label.Text = "Status";
            // 
            // Main_group
            // 
            this.Main_group.Controls.Add(this.Main_tab);
            this.Main_group.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Main_group.Location = new System.Drawing.Point(194, 43);
            this.Main_group.Name = "Main_group";
            this.Main_group.Size = new System.Drawing.Size(532, 496);
            this.Main_group.TabIndex = 4;
            this.Main_group.TabStop = false;
            this.Main_group.Text = "SPIROC_CONTROL";
            // 
            // Main_tab
            // 
            this.Main_tab.Controls.Add(this.Flow_tab);
            this.Main_tab.Controls.Add(this.SC_tab);
            this.Main_tab.Controls.Add(this.Debug_tab);
            this.Main_tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Main_tab.Location = new System.Drawing.Point(3, 17);
            this.Main_tab.Name = "Main_tab";
            this.Main_tab.SelectedIndex = 0;
            this.Main_tab.Size = new System.Drawing.Size(526, 476);
            this.Main_tab.TabIndex = 0;
            // 
            // Flow_tab
            // 
            this.Flow_tab.Controls.Add(this.task_panel);
            this.Flow_tab.Location = new System.Drawing.Point(4, 22);
            this.Flow_tab.Name = "Flow_tab";
            this.Flow_tab.Padding = new System.Windows.Forms.Padding(3);
            this.Flow_tab.Size = new System.Drawing.Size(518, 450);
            this.Flow_tab.TabIndex = 0;
            this.Flow_tab.Text = "Task";
            this.Flow_tab.UseVisualStyleBackColor = true;
            // 
            // task_panel
            // 
            this.task_panel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.task_panel.ColumnCount = 2;
            this.task_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.44101F));
            this.task_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.55899F));
            this.task_panel.Controls.Add(this.normal_label, 0, 0);
            this.task_panel.Controls.Add(this.normal_task_panel, 1, 0);
            this.task_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.task_panel.Location = new System.Drawing.Point(3, 3);
            this.task_panel.Name = "task_panel";
            this.task_panel.RowCount = 2;
            this.task_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.44144F));
            this.task_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.55856F));
            this.task_panel.Size = new System.Drawing.Size(512, 444);
            this.task_panel.TabIndex = 0;
            // 
            // normal_label
            // 
            this.normal_label.BackColor = System.Drawing.Color.Transparent;
            this.normal_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.normal_label.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.normal_label.Location = new System.Drawing.Point(4, 1);
            this.normal_label.Name = "normal_label";
            this.normal_label.Size = new System.Drawing.Size(77, 72);
            this.normal_label.TabIndex = 0;
            this.normal_label.Text = "NORMAL";
            this.normal_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // normal_task_panel
            // 
            this.normal_task_panel.Controls.Add(this.normal_usbcon_button);
            this.normal_task_panel.Controls.Add(this.normal_config_button);
            this.normal_task_panel.Controls.Add(this.normal_acq_button);
            this.normal_task_panel.Controls.Add(this.normal_stop_button);
            this.normal_task_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.normal_task_panel.Location = new System.Drawing.Point(88, 4);
            this.normal_task_panel.Name = "normal_task_panel";
            this.normal_task_panel.Padding = new System.Windows.Forms.Padding(3);
            this.normal_task_panel.Size = new System.Drawing.Size(420, 66);
            this.normal_task_panel.TabIndex = 1;
            this.normal_task_panel.Paint += new System.Windows.Forms.PaintEventHandler(this.normal_task_panel_Paint);
            // 
            // normal_usbcon_button
            // 
            this.normal_usbcon_button.BackColor = System.Drawing.Color.White;
            this.normal_usbcon_button.Font = new System.Drawing.Font("SimSun", 9F);
            this.normal_usbcon_button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.normal_usbcon_button.Location = new System.Drawing.Point(33, 6);
            this.normal_usbcon_button.Margin = new System.Windows.Forms.Padding(30, 3, 15, 3);
            this.normal_usbcon_button.Name = "normal_usbcon_button";
            this.normal_usbcon_button.Size = new System.Drawing.Size(74, 51);
            this.normal_usbcon_button.TabIndex = 0;
            this.normal_usbcon_button.Text = "USB connect";
            this.normal_usbcon_button.UseVisualStyleBackColor = false;
            this.normal_usbcon_button.Click += new System.EventHandler(this.normal_usbcon_button_Click);
            // 
            // normal_config_button
            // 
            this.normal_config_button.BackColor = System.Drawing.Color.White;
            this.normal_config_button.Enabled = false;
            this.normal_config_button.Location = new System.Drawing.Point(125, 6);
            this.normal_config_button.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.normal_config_button.Name = "normal_config_button";
            this.normal_config_button.Size = new System.Drawing.Size(74, 51);
            this.normal_config_button.TabIndex = 1;
            this.normal_config_button.Text = "CONFIG";
            this.normal_config_button.UseVisualStyleBackColor = false;
            this.normal_config_button.Click += new System.EventHandler(this.normal_config_button_Click);
            // 
            // normal_acq_button
            // 
            this.normal_acq_button.BackColor = System.Drawing.Color.White;
            this.normal_acq_button.Enabled = false;
            this.normal_acq_button.Location = new System.Drawing.Point(217, 6);
            this.normal_acq_button.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.normal_acq_button.Name = "normal_acq_button";
            this.normal_acq_button.Size = new System.Drawing.Size(74, 51);
            this.normal_acq_button.TabIndex = 2;
            this.normal_acq_button.Text = "Start ACQ";
            this.normal_acq_button.UseVisualStyleBackColor = false;
            this.normal_acq_button.Click += new System.EventHandler(this.normal_acq_button_Click);
            // 
            // normal_stop_button
            // 
            this.normal_stop_button.BackColor = System.Drawing.Color.White;
            this.normal_stop_button.Enabled = false;
            this.normal_stop_button.Location = new System.Drawing.Point(309, 6);
            this.normal_stop_button.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.normal_stop_button.Name = "normal_stop_button";
            this.normal_stop_button.Size = new System.Drawing.Size(74, 51);
            this.normal_stop_button.TabIndex = 3;
            this.normal_stop_button.Text = "Stop ACQ";
            this.normal_stop_button.UseVisualStyleBackColor = false;
            // 
            // SC_tab
            // 
            this.SC_tab.Controls.Add(this.groupBox2);
            this.SC_tab.Controls.Add(this.groupBox1);
            this.SC_tab.Location = new System.Drawing.Point(4, 22);
            this.SC_tab.Name = "SC_tab";
            this.SC_tab.Padding = new System.Windows.Forms.Padding(3);
            this.SC_tab.Size = new System.Drawing.Size(518, 450);
            this.SC_tab.TabIndex = 1;
            this.SC_tab.Text = "Slow Control";
            this.SC_tab.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 197);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(512, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.input_dac_num);
            this.groupBox1.Controls.Add(this.input_dac_label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(512, 194);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channel Control";
            // 
            // input_dac_num
            // 
            this.input_dac_num.Hexadecimal = true;
            this.input_dac_num.Location = new System.Drawing.Point(80, 18);
            this.input_dac_num.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.input_dac_num.Name = "input_dac_num";
            this.input_dac_num.Size = new System.Drawing.Size(56, 21);
            this.input_dac_num.TabIndex = 1;
            this.input_dac_num.ValueChanged += new System.EventHandler(this.input_dac_num_ValueChanged);
            // 
            // input_dac_label1
            // 
            this.input_dac_label1.AutoSize = true;
            this.input_dac_label1.Location = new System.Drawing.Point(9, 22);
            this.input_dac_label1.Name = "input_dac_label1";
            this.input_dac_label1.Size = new System.Drawing.Size(59, 12);
            this.input_dac_label1.TabIndex = 0;
            this.input_dac_label1.Text = "INPUT DAC";
            // 
            // Debug_tab
            // 
            this.Debug_tab.Location = new System.Drawing.Point(4, 22);
            this.Debug_tab.Name = "Debug_tab";
            this.Debug_tab.Padding = new System.Windows.Forms.Padding(3);
            this.Debug_tab.Size = new System.Drawing.Size(519, 450);
            this.Debug_tab.TabIndex = 2;
            this.Debug_tab.Text = "Debug";
            this.Debug_tab.UseVisualStyleBackColor = true;
            // 
            // clear_button
            // 
            this.clear_button.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.clear_button.Location = new System.Drawing.Point(69, 545);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(62, 24);
            this.clear_button.TabIndex = 3;
            this.clear_button.Text = "clear";
            this.clear_button.UseVisualStyleBackColor = true;
            // 
            // File_group
            // 
            this.File_group.Controls.Add(this.flowLayoutPanel1);
            this.File_group.Dock = System.Windows.Forms.DockStyle.Fill;
            this.File_group.Location = new System.Drawing.Point(194, 545);
            this.File_group.Name = "File_group";
            this.File_group.Size = new System.Drawing.Size(532, 51);
            this.File_group.TabIndex = 2;
            this.File_group.TabStop = false;
            this.File_group.Text = "File Path";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.file_path_label);
            this.flowLayoutPanel1.Controls.Add(this.File_path_showbox);
            this.flowLayoutPanel1.Controls.Add(this.File_path_select_button);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(526, 31);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // file_path_label
            // 
            this.file_path_label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.file_path_label.AutoSize = true;
            this.file_path_label.Location = new System.Drawing.Point(3, 7);
            this.file_path_label.Name = "file_path_label";
            this.file_path_label.Size = new System.Drawing.Size(89, 12);
            this.file_path_label.TabIndex = 0;
            this.file_path_label.Text = "File Directory";
            // 
            // File_path_showbox
            // 
            this.File_path_showbox.Location = new System.Drawing.Point(98, 3);
            this.File_path_showbox.Name = "File_path_showbox";
            this.File_path_showbox.Size = new System.Drawing.Size(361, 21);
            this.File_path_showbox.TabIndex = 1;
            // 
            // File_path_select_button
            // 
            this.File_path_select_button.Location = new System.Drawing.Point(465, 3);
            this.File_path_select_button.Name = "File_path_select_button";
            this.File_path_select_button.Size = new System.Drawing.Size(52, 20);
            this.File_path_select_button.TabIndex = 2;
            this.File_path_select_button.Text = "Select";
            this.File_path_select_button.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(13, 43);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox1.Size = new System.Drawing.Size(175, 496);
            this.textBox1.TabIndex = 1;
            // 
            // Msg_label
            // 
            this.Msg_label.AutoSize = true;
            this.Msg_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Msg_label.Location = new System.Drawing.Point(13, 10);
            this.Msg_label.Name = "Msg_label";
            this.Msg_label.Size = new System.Drawing.Size(175, 30);
            this.Msg_label.TabIndex = 0;
            this.Msg_label.Text = "Message";
            this.Msg_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.16469F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.83531F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 187F));
            this.tableLayoutPanel1.Controls.Add(this.Msg_label, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.File_group, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.clear_button, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Main_group, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.Status_label, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.Status_panel, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.215827F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.51483F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.424084F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(927, 609);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 609);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Main_Form";
            this.Text = "SPIROC_DIF_Control";
            this.Status_panel.ResumeLayout(false);
            this.USB_status_group.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.Configure_status_group.ResumeLayout(false);
            this.Acq_status_group.ResumeLayout(false);
            this.Warning_group.ResumeLayout(false);
            this.Main_group.ResumeLayout(false);
            this.Main_tab.ResumeLayout(false);
            this.Flow_tab.ResumeLayout(false);
            this.task_panel.ResumeLayout(false);
            this.normal_task_panel.ResumeLayout(false);
            this.SC_tab.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input_dac_num)).EndInit();
            this.File_group.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Status_panel;
        private System.Windows.Forms.GroupBox USB_status_group;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Usb_status_label;
        private System.Windows.Forms.GroupBox Configure_status_group;
        private System.Windows.Forms.Label Config_status_label;
        private System.Windows.Forms.GroupBox Acq_status_group;
        private System.Windows.Forms.Label Acq_status_label;
        private System.Windows.Forms.GroupBox Warning_group;
        private System.Windows.Forms.Label Warning_label;
        private System.Windows.Forms.Label Status_label;
        private System.Windows.Forms.GroupBox Main_group;
        private System.Windows.Forms.TabControl Main_tab;
        private System.Windows.Forms.TabPage Flow_tab;
        private System.Windows.Forms.TabPage SC_tab;
        private System.Windows.Forms.TabPage Debug_tab;
        private System.Windows.Forms.Button clear_button;
        private System.Windows.Forms.GroupBox File_group;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label file_path_label;
        private System.Windows.Forms.TextBox File_path_showbox;
        private System.Windows.Forms.Button File_path_select_button;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label Msg_label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel task_panel;
        private System.Windows.Forms.Label normal_label;
        private System.Windows.Forms.FlowLayoutPanel normal_task_panel;
        private System.Windows.Forms.Button normal_usbcon_button;
        private System.Windows.Forms.Button normal_config_button;
        private System.Windows.Forms.Button normal_acq_button;
        private System.Windows.Forms.Button normal_stop_button;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown input_dac_num;
        private System.Windows.Forms.Label input_dac_label1;
    }
}


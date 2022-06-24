namespace CAPC.userForm
{
    partial class Home
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.stateDis_zhuanpan_shangliao = new Arc_ratio.stateDis();
            this.stateDis_xuanzhuanTiehe = new Arc_ratio.stateDis();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_0_zhuanPan = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_count_clear = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_2_moZu = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_1_scara = new System.Windows.Forms.Label();
            this.stateDis_r2r_1 = new Arc_ratio.stateDis();
            this.stateDis_sanzui_tujiao = new Arc_ratio.stateDis();
            this.stateDis_sizui_tujiao = new Arc_ratio.stateDis();
            this.stateDis_r2r_2 = new Arc_ratio.stateDis();
            this.stateDis_simo = new Arc_ratio.stateDis();
            this.stateDis_jingya = new Arc_ratio.stateDis();
            this.stateDis_tiemo_fanglou = new Arc_ratio.stateDis();
            this.stateDis_dabiao = new Arc_ratio.stateDis();
            this.stateDis_dingwei_1 = new Arc_ratio.stateDis();
            this.stateDis_dingwei_2 = new Arc_ratio.stateDis();
            this.stateDis_tuopan_xialiao = new Arc_ratio.stateDis();
            this.stateDis_tuopan_shangliao = new Arc_ratio.stateDis();
            this.stateDis_scara = new Arc_ratio.stateDis();
            this.stateDis_r2r_3 = new Arc_ratio.stateDis();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_tujiao_ng = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_scara_ng = new System.Windows.Forms.Label();
            this.stateDis_chongya = new Arc_ratio.stateDis();
            this.stateDis_jiance = new Arc_ratio.stateDis();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_3_UPH = new System.Windows.Forms.Label();
            this.btn_sys_reset = new ReWriteTextBox.ButtonSquare(this.components);
            this.btn_sys_start = new ReWriteTextBox.ButtonSquare(this.components);
            this.btn_sys_pause = new ReWriteTextBox.ButtonSquare(this.components);
            this.btn_sys_stop = new ReWriteTextBox.ButtonSquare(this.components);
            this.btn_sys_purge = new ReWriteTextBox.ButtonSquare(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(2, 71);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(779, 278);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // stateDis_zhuanpan_shangliao
            // 
            this.stateDis_zhuanpan_shangliao.IsErr = false;
            this.stateDis_zhuanpan_shangliao.Location = new System.Drawing.Point(726, 229);
            this.stateDis_zhuanpan_shangliao.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_zhuanpan_shangliao.Name = "stateDis_zhuanpan_shangliao";
            this.stateDis_zhuanpan_shangliao.Size = new System.Drawing.Size(23, 23);
            this.stateDis_zhuanpan_shangliao.TabIndex = 3;
            // 
            // stateDis_xuanzhuanTiehe
            // 
            this.stateDis_xuanzhuanTiehe.IsErr = false;
            this.stateDis_xuanzhuanTiehe.Location = new System.Drawing.Point(288, 192);
            this.stateDis_xuanzhuanTiehe.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_xuanzhuanTiehe.Name = "stateDis_xuanzhuanTiehe";
            this.stateDis_xuanzhuanTiehe.Size = new System.Drawing.Size(15, 15);
            this.stateDis_xuanzhuanTiehe.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "PCB Loaded:";
            // 
            // lbl_0_zhuanPan
            // 
            this.lbl_0_zhuanPan.AutoSize = true;
            this.lbl_0_zhuanPan.Location = new System.Drawing.Point(117, 17);
            this.lbl_0_zhuanPan.Name = "lbl_0_zhuanPan";
            this.lbl_0_zhuanPan.Size = new System.Drawing.Size(41, 12);
            this.lbl_0_zhuanPan.TabIndex = 18;
            this.lbl_0_zhuanPan.Text = "999999";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lbl_3_UPH);
            this.groupBox1.Controls.Add(this.btn_count_clear);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lbl_2_moZu);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lbl_1_scara);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbl_0_zhuanPan);
            this.groupBox1.Location = new System.Drawing.Point(12, 358);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 105);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Prod Data";
            // 
            // btn_count_clear
            // 
            this.btn_count_clear.Location = new System.Drawing.Point(164, 52);
            this.btn_count_clear.Name = "btn_count_clear";
            this.btn_count_clear.Size = new System.Drawing.Size(46, 23);
            this.btn_count_clear.TabIndex = 23;
            this.btn_count_clear.Text = "CLEAR";
            this.btn_count_clear.UseVisualStyleBackColor = true;
            this.btn_count_clear.Click += new System.EventHandler(this.btn_count_clear_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "Finished Chip:";
            // 
            // lbl_2_moZu
            // 
            this.lbl_2_moZu.AutoSize = true;
            this.lbl_2_moZu.Location = new System.Drawing.Point(117, 63);
            this.lbl_2_moZu.Name = "lbl_2_moZu";
            this.lbl_2_moZu.Size = new System.Drawing.Size(41, 12);
            this.lbl_2_moZu.TabIndex = 22;
            this.lbl_2_moZu.Text = "999999";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "Plastics Loaded:";
            // 
            // lbl_1_scara
            // 
            this.lbl_1_scara.AutoSize = true;
            this.lbl_1_scara.Location = new System.Drawing.Point(117, 40);
            this.lbl_1_scara.Name = "lbl_1_scara";
            this.lbl_1_scara.Size = new System.Drawing.Size(41, 12);
            this.lbl_1_scara.TabIndex = 20;
            this.lbl_1_scara.Text = "999999";
            // 
            // stateDis_r2r_1
            // 
            this.stateDis_r2r_1.IsErr = false;
            this.stateDis_r2r_1.Location = new System.Drawing.Point(658, 192);
            this.stateDis_r2r_1.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_r2r_1.Name = "stateDis_r2r_1";
            this.stateDis_r2r_1.Size = new System.Drawing.Size(23, 23);
            this.stateDis_r2r_1.TabIndex = 20;
            // 
            // stateDis_sanzui_tujiao
            // 
            this.stateDis_sanzui_tujiao.IsErr = false;
            this.stateDis_sanzui_tujiao.Location = new System.Drawing.Point(557, 206);
            this.stateDis_sanzui_tujiao.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_sanzui_tujiao.Name = "stateDis_sanzui_tujiao";
            this.stateDis_sanzui_tujiao.Size = new System.Drawing.Size(23, 23);
            this.stateDis_sanzui_tujiao.TabIndex = 21;
            // 
            // stateDis_sizui_tujiao
            // 
            this.stateDis_sizui_tujiao.IsErr = false;
            this.stateDis_sizui_tujiao.Location = new System.Drawing.Point(502, 206);
            this.stateDis_sizui_tujiao.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_sizui_tujiao.Name = "stateDis_sizui_tujiao";
            this.stateDis_sizui_tujiao.Size = new System.Drawing.Size(23, 23);
            this.stateDis_sizui_tujiao.TabIndex = 22;
            // 
            // stateDis_r2r_2
            // 
            this.stateDis_r2r_2.IsErr = false;
            this.stateDis_r2r_2.Location = new System.Drawing.Point(404, 192);
            this.stateDis_r2r_2.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_r2r_2.Name = "stateDis_r2r_2";
            this.stateDis_r2r_2.Size = new System.Drawing.Size(23, 23);
            this.stateDis_r2r_2.TabIndex = 23;
            // 
            // stateDis_simo
            // 
            this.stateDis_simo.IsErr = false;
            this.stateDis_simo.Location = new System.Drawing.Point(345, 170);
            this.stateDis_simo.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_simo.Name = "stateDis_simo";
            this.stateDis_simo.Size = new System.Drawing.Size(23, 23);
            this.stateDis_simo.TabIndex = 24;
            // 
            // stateDis_jingya
            // 
            this.stateDis_jingya.IsErr = false;
            this.stateDis_jingya.Location = new System.Drawing.Point(278, 161);
            this.stateDis_jingya.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_jingya.Name = "stateDis_jingya";
            this.stateDis_jingya.Size = new System.Drawing.Size(15, 15);
            this.stateDis_jingya.TabIndex = 25;
            // 
            // stateDis_tiemo_fanglou
            // 
            this.stateDis_tiemo_fanglou.IsErr = false;
            this.stateDis_tiemo_fanglou.Location = new System.Drawing.Point(224, 134);
            this.stateDis_tiemo_fanglou.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_tiemo_fanglou.Name = "stateDis_tiemo_fanglou";
            this.stateDis_tiemo_fanglou.Size = new System.Drawing.Size(15, 15);
            this.stateDis_tiemo_fanglou.TabIndex = 26;
            // 
            // stateDis_dabiao
            // 
            this.stateDis_dabiao.IsErr = false;
            this.stateDis_dabiao.Location = new System.Drawing.Point(179, 161);
            this.stateDis_dabiao.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_dabiao.Name = "stateDis_dabiao";
            this.stateDis_dabiao.Size = new System.Drawing.Size(15, 15);
            this.stateDis_dabiao.TabIndex = 27;
            // 
            // stateDis_dingwei_1
            // 
            this.stateDis_dingwei_1.IsErr = false;
            this.stateDis_dingwei_1.Location = new System.Drawing.Point(193, 237);
            this.stateDis_dingwei_1.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_dingwei_1.Name = "stateDis_dingwei_1";
            this.stateDis_dingwei_1.Size = new System.Drawing.Size(15, 15);
            this.stateDis_dingwei_1.TabIndex = 28;
            // 
            // stateDis_dingwei_2
            // 
            this.stateDis_dingwei_2.IsErr = false;
            this.stateDis_dingwei_2.Location = new System.Drawing.Point(278, 229);
            this.stateDis_dingwei_2.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_dingwei_2.Name = "stateDis_dingwei_2";
            this.stateDis_dingwei_2.Size = new System.Drawing.Size(15, 15);
            this.stateDis_dingwei_2.TabIndex = 29;
            // 
            // stateDis_tuopan_xialiao
            // 
            this.stateDis_tuopan_xialiao.IsErr = false;
            this.stateDis_tuopan_xialiao.Location = new System.Drawing.Point(77, 184);
            this.stateDis_tuopan_xialiao.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_tuopan_xialiao.Name = "stateDis_tuopan_xialiao";
            this.stateDis_tuopan_xialiao.Size = new System.Drawing.Size(23, 23);
            this.stateDis_tuopan_xialiao.TabIndex = 30;
            // 
            // stateDis_tuopan_shangliao
            // 
            this.stateDis_tuopan_shangliao.IsErr = false;
            this.stateDis_tuopan_shangliao.Location = new System.Drawing.Point(67, 271);
            this.stateDis_tuopan_shangliao.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_tuopan_shangliao.Name = "stateDis_tuopan_shangliao";
            this.stateDis_tuopan_shangliao.Size = new System.Drawing.Size(23, 23);
            this.stateDis_tuopan_shangliao.TabIndex = 31;
            // 
            // stateDis_scara
            // 
            this.stateDis_scara.IsErr = false;
            this.stateDis_scara.Location = new System.Drawing.Point(151, 271);
            this.stateDis_scara.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_scara.Name = "stateDis_scara";
            this.stateDis_scara.Size = new System.Drawing.Size(23, 23);
            this.stateDis_scara.TabIndex = 32;
            // 
            // stateDis_r2r_3
            // 
            this.stateDis_r2r_3.IsErr = false;
            this.stateDis_r2r_3.Location = new System.Drawing.Point(224, 262);
            this.stateDis_r2r_3.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_r2r_3.Name = "stateDis_r2r_3";
            this.stateDis_r2r_3.Size = new System.Drawing.Size(23, 23);
            this.stateDis_r2r_3.TabIndex = 33;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.lbl_tujiao_ng);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.lbl_scara_ng);
            this.groupBox2.Location = new System.Drawing.Point(245, 358);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(226, 105);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "NG Data";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(163, 52);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 23);
            this.button2.TabIndex = 23;
            this.button2.Text = "CLEAR";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "Dispensing_NG：";
            // 
            // lbl_tujiao_ng
            // 
            this.lbl_tujiao_ng.AutoSize = true;
            this.lbl_tujiao_ng.Location = new System.Drawing.Point(104, 40);
            this.lbl_tujiao_ng.Name = "lbl_tujiao_ng";
            this.lbl_tujiao_ng.Size = new System.Drawing.Size(41, 12);
            this.lbl_tujiao_ng.TabIndex = 20;
            this.lbl_tujiao_ng.Text = "999999";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(40, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "SCARA_NG：";
            // 
            // lbl_scara_ng
            // 
            this.lbl_scara_ng.AutoSize = true;
            this.lbl_scara_ng.Location = new System.Drawing.Point(104, 17);
            this.lbl_scara_ng.Name = "lbl_scara_ng";
            this.lbl_scara_ng.Size = new System.Drawing.Size(41, 12);
            this.lbl_scara_ng.TabIndex = 18;
            this.lbl_scara_ng.Text = "999999";
            // 
            // stateDis_chongya
            // 
            this.stateDis_chongya.IsErr = false;
            this.stateDis_chongya.Location = new System.Drawing.Point(593, 229);
            this.stateDis_chongya.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_chongya.Name = "stateDis_chongya";
            this.stateDis_chongya.Size = new System.Drawing.Size(15, 15);
            this.stateDis_chongya.TabIndex = 35;
            // 
            // stateDis_jiance
            // 
            this.stateDis_jiance.IsErr = false;
            this.stateDis_jiance.Location = new System.Drawing.Point(456, 170);
            this.stateDis_jiance.Margin = new System.Windows.Forms.Padding(2);
            this.stateDis_jiance.Name = "stateDis_jiance";
            this.stateDis_jiance.Size = new System.Drawing.Size(15, 15);
            this.stateDis_jiance.TabIndex = 36;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(81, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 24;
            this.label7.Text = "UPH:";
            // 
            // lbl_3_UPH
            // 
            this.lbl_3_UPH.AutoSize = true;
            this.lbl_3_UPH.Location = new System.Drawing.Point(117, 84);
            this.lbl_3_UPH.Name = "lbl_3_UPH";
            this.lbl_3_UPH.Size = new System.Drawing.Size(41, 12);
            this.lbl_3_UPH.TabIndex = 25;
            this.lbl_3_UPH.Text = "999999";
            // 
            // btn_sys_reset
            // 
            this.btn_sys_reset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_sys_reset.BackgroundImage")));
            this.btn_sys_reset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_sys_reset.ButtonState = false;
            this.btn_sys_reset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_sys_reset.FlatAppearance.BorderSize = 0;
            this.btn_sys_reset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_sys_reset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_sys_reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_sys_reset.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_sys_reset.ImageAct = ((System.Drawing.Image)(resources.GetObject("btn_sys_reset.ImageAct")));
            this.btn_sys_reset.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btn_sys_reset.ImageNormal")));
            this.btn_sys_reset.Location = new System.Drawing.Point(485, 408);
            this.btn_sys_reset.Name = "btn_sys_reset";
            this.btn_sys_reset.Size = new System.Drawing.Size(52, 25);
            this.btn_sys_reset.TabIndex = 37;
            this.btn_sys_reset.Text = "RESET";
            this.btn_sys_reset.UseVisualStyleBackColor = true;
            this.btn_sys_reset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_sys_start_MouseDown);
            this.btn_sys_reset.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_sys_start_MouseUp);
            // 
            // btn_sys_start
            // 
            this.btn_sys_start.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_sys_start.BackgroundImage")));
            this.btn_sys_start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_sys_start.ButtonState = false;
            this.btn_sys_start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_sys_start.FlatAppearance.BorderSize = 0;
            this.btn_sys_start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_sys_start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_sys_start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_sys_start.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_sys_start.ImageAct = ((System.Drawing.Image)(resources.GetObject("btn_sys_start.ImageAct")));
            this.btn_sys_start.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btn_sys_start.ImageNormal")));
            this.btn_sys_start.Location = new System.Drawing.Point(543, 408);
            this.btn_sys_start.Name = "btn_sys_start";
            this.btn_sys_start.Size = new System.Drawing.Size(52, 25);
            this.btn_sys_start.TabIndex = 38;
            this.btn_sys_start.Text = "START";
            this.btn_sys_start.UseVisualStyleBackColor = true;
            this.btn_sys_start.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_sys_start_MouseDown);
            this.btn_sys_start.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_sys_start_MouseUp);
            // 
            // btn_sys_pause
            // 
            this.btn_sys_pause.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_sys_pause.BackgroundImage")));
            this.btn_sys_pause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_sys_pause.ButtonState = false;
            this.btn_sys_pause.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_sys_pause.FlatAppearance.BorderSize = 0;
            this.btn_sys_pause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_sys_pause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_sys_pause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_sys_pause.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_sys_pause.ImageAct = ((System.Drawing.Image)(resources.GetObject("btn_sys_pause.ImageAct")));
            this.btn_sys_pause.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btn_sys_pause.ImageNormal")));
            this.btn_sys_pause.Location = new System.Drawing.Point(601, 408);
            this.btn_sys_pause.Name = "btn_sys_pause";
            this.btn_sys_pause.Size = new System.Drawing.Size(52, 25);
            this.btn_sys_pause.TabIndex = 39;
            this.btn_sys_pause.Text = "PAUSE";
            this.btn_sys_pause.UseVisualStyleBackColor = true;
            this.btn_sys_pause.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_sys_start_MouseDown);
            this.btn_sys_pause.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_sys_start_MouseUp);
            // 
            // btn_sys_stop
            // 
            this.btn_sys_stop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_sys_stop.BackgroundImage")));
            this.btn_sys_stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_sys_stop.ButtonState = false;
            this.btn_sys_stop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_sys_stop.FlatAppearance.BorderSize = 0;
            this.btn_sys_stop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_sys_stop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_sys_stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_sys_stop.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_sys_stop.ImageAct = ((System.Drawing.Image)(resources.GetObject("btn_sys_stop.ImageAct")));
            this.btn_sys_stop.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btn_sys_stop.ImageNormal")));
            this.btn_sys_stop.Location = new System.Drawing.Point(659, 408);
            this.btn_sys_stop.Name = "btn_sys_stop";
            this.btn_sys_stop.Size = new System.Drawing.Size(52, 25);
            this.btn_sys_stop.TabIndex = 40;
            this.btn_sys_stop.Text = "STOP";
            this.btn_sys_stop.UseVisualStyleBackColor = true;
            this.btn_sys_stop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_sys_start_MouseDown);
            this.btn_sys_stop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_sys_start_MouseUp);
            // 
            // btn_sys_purge
            // 
            this.btn_sys_purge.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_sys_purge.BackgroundImage")));
            this.btn_sys_purge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_sys_purge.ButtonState = false;
            this.btn_sys_purge.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_sys_purge.FlatAppearance.BorderSize = 0;
            this.btn_sys_purge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_sys_purge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_sys_purge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_sys_purge.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_sys_purge.ImageAct = ((System.Drawing.Image)(resources.GetObject("btn_sys_purge.ImageAct")));
            this.btn_sys_purge.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btn_sys_purge.ImageNormal")));
            this.btn_sys_purge.Location = new System.Drawing.Point(717, 408);
            this.btn_sys_purge.Name = "btn_sys_purge";
            this.btn_sys_purge.Size = new System.Drawing.Size(52, 25);
            this.btn_sys_purge.TabIndex = 41;
            this.btn_sys_purge.Text = "PURGE";
            this.btn_sys_purge.UseVisualStyleBackColor = true;
            this.btn_sys_purge.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_sys_start_MouseDown);
            this.btn_sys_purge.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_sys_start_MouseUp);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(782, 489);
            this.Controls.Add(this.btn_sys_purge);
            this.Controls.Add(this.btn_sys_stop);
            this.Controls.Add(this.btn_sys_pause);
            this.Controls.Add(this.btn_sys_start);
            this.Controls.Add(this.btn_sys_reset);
            this.Controls.Add(this.stateDis_jiance);
            this.Controls.Add(this.stateDis_chongya);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.stateDis_r2r_3);
            this.Controls.Add(this.stateDis_scara);
            this.Controls.Add(this.stateDis_tuopan_shangliao);
            this.Controls.Add(this.stateDis_tuopan_xialiao);
            this.Controls.Add(this.stateDis_dingwei_2);
            this.Controls.Add(this.stateDis_dingwei_1);
            this.Controls.Add(this.stateDis_dabiao);
            this.Controls.Add(this.stateDis_tiemo_fanglou);
            this.Controls.Add(this.stateDis_jingya);
            this.Controls.Add(this.stateDis_simo);
            this.Controls.Add(this.stateDis_r2r_2);
            this.Controls.Add(this.stateDis_sizui_tujiao);
            this.Controls.Add(this.stateDis_sanzui_tujiao);
            this.Controls.Add(this.stateDis_r2r_1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.stateDis_xuanzhuanTiehe);
            this.Controls.Add(this.stateDis_zhuanpan_shangliao);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Home";
            this.Text = "Home";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Arc_ratio.stateDis stateDis_zhuanpan_shangliao;
        private Arc_ratio.stateDis stateDis_xuanzhuanTiehe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_0_zhuanPan;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_count_clear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_2_moZu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_1_scara;
        private Arc_ratio.stateDis stateDis_r2r_1;
        private Arc_ratio.stateDis stateDis_sanzui_tujiao;
        private Arc_ratio.stateDis stateDis_sizui_tujiao;
        private Arc_ratio.stateDis stateDis_r2r_2;
        private Arc_ratio.stateDis stateDis_simo;
        private Arc_ratio.stateDis stateDis_jingya;
        private Arc_ratio.stateDis stateDis_tiemo_fanglou;
        private Arc_ratio.stateDis stateDis_dabiao;
        private Arc_ratio.stateDis stateDis_dingwei_1;
        private Arc_ratio.stateDis stateDis_dingwei_2;
        private Arc_ratio.stateDis stateDis_tuopan_xialiao;
        private Arc_ratio.stateDis stateDis_tuopan_shangliao;
        private Arc_ratio.stateDis stateDis_scara;
        private Arc_ratio.stateDis stateDis_r2r_3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_tujiao_ng;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbl_scara_ng;
        private Arc_ratio.stateDis stateDis_chongya;
        private Arc_ratio.stateDis stateDis_jiance;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_3_UPH;
        private ReWriteTextBox.ButtonSquare btn_sys_reset;
        private ReWriteTextBox.ButtonSquare btn_sys_start;
        private ReWriteTextBox.ButtonSquare btn_sys_pause;
        private ReWriteTextBox.ButtonSquare btn_sys_stop;
        private ReWriteTextBox.ButtonSquare btn_sys_purge;
    }
}
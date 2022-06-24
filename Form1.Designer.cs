namespace CAPC
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.clock = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.user = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.machinestate = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button_home = new System.Windows.Forms.Button();
            this.button_io = new System.Windows.Forms.Button();
            this.button_manual = new System.Windows.Forms.Button();
            this.button_para = new System.Windows.Forms.Button();
            this.button_data = new System.Windows.Forms.Button();
            this.button_err = new System.Windows.Forms.Button();
            this.button_user = new System.Windows.Forms.Button();
            this.pictureBox38 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.timer_err = new System.Windows.Forms.Timer(this.components);
            this.timer_tw = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.yjcd_ccd = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clear = new System.Windows.Forms.ToolStripMenuItem();
            this.Scara = new System.Windows.Forms.ListBox();
            this.yjcd_scara = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clear_scara = new System.Windows.Forms.ToolStripMenuItem();
            this.lvWarnAndError = new System.Windows.Forms.ListView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox38)).BeginInit();
            this.panel1.SuspendLayout();
            this.yjcd_ccd.SuspendLayout();
            this.yjcd_scara.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel4,
            this.clock,
            this.toolStripSeparator7,
            this.toolStripStatusLabel2,
            this.user,
            this.toolStripSeparator6,
            this.toolStripStatusLabel,
            this.machinestate});
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            resources.ApplyResources(this.toolStripStatusLabel4, "toolStripStatusLabel4");
            // 
            // clock
            // 
            resources.ApplyResources(this.clock, "clock");
            this.clock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.clock.Name = "clock";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            // 
            // user
            // 
            this.user.Name = "user";
            resources.ApplyResources(this.user, "user");
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            resources.ApplyResources(this.toolStripStatusLabel, "toolStripStatusLabel");
            // 
            // machinestate
            // 
            this.machinestate.Name = "machinestate";
            resources.ApplyResources(this.machinestate, "machinestate");
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button_home
            // 
            resources.ApplyResources(this.button_home, "button_home");
            this.button_home.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_home.FlatAppearance.BorderSize = 0;
            this.button_home.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_home.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_home.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button_home.Name = "button_home";
            this.button_home.UseVisualStyleBackColor = true;
            this.button_home.Click += new System.EventHandler(this.button_home_Click);
            // 
            // button_io
            // 
            resources.ApplyResources(this.button_io, "button_io");
            this.button_io.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_io.FlatAppearance.BorderSize = 0;
            this.button_io.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_io.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_io.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button_io.Name = "button_io";
            this.button_io.UseVisualStyleBackColor = true;
            this.button_io.Click += new System.EventHandler(this.button_io_Click);
            // 
            // button_manual
            // 
            resources.ApplyResources(this.button_manual, "button_manual");
            this.button_manual.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_manual.FlatAppearance.BorderSize = 0;
            this.button_manual.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_manual.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_manual.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button_manual.Name = "button_manual";
            this.button_manual.UseVisualStyleBackColor = true;
            this.button_manual.Click += new System.EventHandler(this.button_manual_Click);
            // 
            // button_para
            // 
            resources.ApplyResources(this.button_para, "button_para");
            this.button_para.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_para.FlatAppearance.BorderSize = 0;
            this.button_para.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_para.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_para.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button_para.Name = "button_para";
            this.button_para.UseVisualStyleBackColor = true;
            this.button_para.Click += new System.EventHandler(this.button_para_Click);
            // 
            // button_data
            // 
            resources.ApplyResources(this.button_data, "button_data");
            this.button_data.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_data.FlatAppearance.BorderSize = 0;
            this.button_data.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_data.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_data.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button_data.Name = "button_data";
            this.button_data.UseVisualStyleBackColor = true;
            this.button_data.Click += new System.EventHandler(this.button_data_Click);
            // 
            // button_err
            // 
            resources.ApplyResources(this.button_err, "button_err");
            this.button_err.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_err.FlatAppearance.BorderSize = 0;
            this.button_err.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_err.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_err.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button_err.Name = "button_err";
            this.button_err.UseVisualStyleBackColor = true;
            this.button_err.Click += new System.EventHandler(this.button_err_Click);
            // 
            // button_user
            // 
            resources.ApplyResources(this.button_user, "button_user");
            this.button_user.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_user.FlatAppearance.BorderSize = 0;
            this.button_user.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_user.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_user.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button_user.Name = "button_user";
            this.button_user.UseVisualStyleBackColor = true;
            this.button_user.Click += new System.EventHandler(this.button_user_Click);
            // 
            // pictureBox38
            // 
            resources.ApplyResources(this.pictureBox38, "pictureBox38");
            this.pictureBox38.Name = "pictureBox38";
            this.pictureBox38.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(149)))), ((int)(((byte)(215)))));
            this.panel1.Controls.Add(this.button_user);
            this.panel1.Controls.Add(this.button_err);
            this.panel1.Controls.Add(this.button_data);
            this.panel1.Controls.Add(this.button_para);
            this.panel1.Controls.Add(this.button_manual);
            this.panel1.Controls.Add(this.button_io);
            this.panel1.Controls.Add(this.button_home);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // BottomToolStripPanel
            // 
            resources.ApplyResources(this.BottomToolStripPanel, "BottomToolStripPanel");
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // TopToolStripPanel
            // 
            resources.ApplyResources(this.TopToolStripPanel, "TopToolStripPanel");
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // RightToolStripPanel
            // 
            resources.ApplyResources(this.RightToolStripPanel, "RightToolStripPanel");
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // LeftToolStripPanel
            // 
            resources.ApplyResources(this.LeftToolStripPanel, "LeftToolStripPanel");
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // ContentPanel
            // 
            resources.ApplyResources(this.ContentPanel, "ContentPanel");
            // 
            // timer_err
            // 
            this.timer_err.Tick += new System.EventHandler(this.timer_err_Tick);
            // 
            // timer_tw
            // 
            this.timer_tw.Enabled = true;
            this.timer_tw.Tick += new System.EventHandler(this.timer_tw_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 2000;
            this.timer2.Tick += new System.EventHandler(this.StaetTCP);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.listBox1.ContextMenuStrip = this.yjcd_ccd;
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
            // 
            // yjcd_ccd
            // 
            this.yjcd_ccd.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.yjcd_ccd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clear});
            this.yjcd_ccd.Name = "yjcd_ccd";
            resources.ApplyResources(this.yjcd_ccd, "yjcd_ccd");
            // 
            // clear
            // 
            this.clear.Name = "clear";
            resources.ApplyResources(this.clear, "clear");
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // Scara
            // 
            this.Scara.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Scara.ContextMenuStrip = this.yjcd_scara;
            resources.ApplyResources(this.Scara, "Scara");
            this.Scara.FormattingEnabled = true;
            this.Scara.Name = "Scara";
            // 
            // yjcd_scara
            // 
            this.yjcd_scara.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.yjcd_scara.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clear_scara});
            this.yjcd_scara.Name = "yjcd_ccd";
            resources.ApplyResources(this.yjcd_scara, "yjcd_scara");
            // 
            // clear_scara
            // 
            this.clear_scara.Name = "clear_scara";
            resources.ApplyResources(this.clear_scara, "clear_scara");
            this.clear_scara.Click += new System.EventHandler(this.clear_scara_Click);
            // 
            // lvWarnAndError
            // 
            this.lvWarnAndError.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            resources.ApplyResources(this.lvWarnAndError, "lvWarnAndError");
            this.lvWarnAndError.ForeColor = System.Drawing.Color.Red;
            this.lvWarnAndError.GridLines = true;
            this.lvWarnAndError.Name = "lvWarnAndError";
            this.lvWarnAndError.UseCompatibleStateImageBehavior = false;
            this.lvWarnAndError.View = System.Windows.Forms.View.Details;
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.pictureBox38);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.Scara);
            this.Controls.Add(this.lvWarnAndError);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox38)).EndInit();
            this.panel1.ResumeLayout(false);
            this.yjcd_ccd.ResumeLayout(false);
            this.yjcd_scara.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel user;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel machinestate;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel clock;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_home;
        private System.Windows.Forms.Button button_io;
        private System.Windows.Forms.Button button_manual;
        private System.Windows.Forms.Button button_para;
        private System.Windows.Forms.Button button_data;
        private System.Windows.Forms.Button button_err;
        private System.Windows.Forms.Button button_user;
        private System.Windows.Forms.PictureBox pictureBox38;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.Timer timer_err;
        private System.Windows.Forms.Timer timer_tw;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ListView lvWarnAndError;
        private System.Windows.Forms.ListBox Scara;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip yjcd_ccd;
        private System.Windows.Forms.ToolStripMenuItem clear;
        private System.Windows.Forms.ContextMenuStrip yjcd_scara;
        private System.Windows.Forms.ToolStripMenuItem clear_scara;
    }
}


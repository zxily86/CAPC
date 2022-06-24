namespace CAPC.userForm
{
    partial class Alarm
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
            this.label1 = new System.Windows.Forms.Label();
            this.lvWarnAndError = new System.Windows.Forms.ListBox();
            this.data = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("黑体", 15F);
            this.label1.Location = new System.Drawing.Point(22, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 54;
            this.label1.Text = "历史报警";
            // 
            // lvWarnAndError
            // 
            this.lvWarnAndError.FormattingEnabled = true;
            this.lvWarnAndError.ItemHeight = 12;
            this.lvWarnAndError.Location = new System.Drawing.Point(12, 44);
            this.lvWarnAndError.Name = "lvWarnAndError";
            this.lvWarnAndError.Size = new System.Drawing.Size(568, 376);
            this.lvWarnAndError.TabIndex = 55;
            // 
            // data
            // 
            this.data.FormattingEnabled = true;
            this.data.Location = new System.Drawing.Point(426, 16);
            this.data.Name = "data";
            this.data.Size = new System.Drawing.Size(121, 20);
            this.data.TabIndex = 56;
            this.data.SelectedIndexChanged += new System.EventHandler(this.data_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(361, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 57;
            this.label2.Text = "查询日期:";
            // 
            // Alarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(595, 427);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.data);
            this.Controls.Add(this.lvWarnAndError);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "Alarm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "报警信息";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lvWarnAndError;
        private System.Windows.Forms.ComboBox data;
        private System.Windows.Forms.Label label2;
    }
}
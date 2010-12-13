namespace WarthogInterface
{
    partial class frmMain
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cbActiveDevice = new System.Windows.Forms.ComboBox();
            this.lblActiveDevice = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslHostPort = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslRules = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslActivity = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cbActiveDevice
            // 
            this.cbActiveDevice.FormattingEnabled = true;
            this.cbActiveDevice.Location = new System.Drawing.Point(92, 12);
            this.cbActiveDevice.Name = "cbActiveDevice";
            this.cbActiveDevice.Size = new System.Drawing.Size(221, 21);
            this.cbActiveDevice.TabIndex = 10;
            // 
            // lblActiveDevice
            // 
            this.lblActiveDevice.AutoSize = true;
            this.lblActiveDevice.Location = new System.Drawing.Point(12, 15);
            this.lblActiveDevice.Name = "lblActiveDevice";
            this.lblActiveDevice.Size = new System.Drawing.Size(74, 13);
            this.lblActiveDevice.TabIndex = 11;
            this.lblActiveDevice.Text = "Active Device";
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(125, 49);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 12;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslActivity,
            this.tsslHostPort,
            this.tsslRules});
            this.statusStrip1.Location = new System.Drawing.Point(0, 186);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(325, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslHostPort
            // 
            this.tsslHostPort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsslHostPort.Name = "tsslHostPort";
            this.tsslHostPort.Size = new System.Drawing.Size(78, 17);
            this.tsslHostPort.Text = "hostname:port";
            // 
            // tsslRules
            // 
            this.tsslRules.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsslRules.Name = "tsslRules";
            this.tsslRules.Size = new System.Drawing.Size(42, 17);
            this.tsslRules.Text = "0 Rules";
            // 
            // tsslActivity
            // 
            this.tsslActivity.Name = "tsslActivity";
            this.tsslActivity.Size = new System.Drawing.Size(13, 17);
            this.tsslActivity.Text = "0";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 208);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.lblActiveDevice);
            this.Controls.Add(this.cbActiveDevice);
            this.Name = "frmMain";
            this.Text = "HogDaemon";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox cbActiveDevice;
        private System.Windows.Forms.Label lblActiveDevice;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslHostPort;
        private System.Windows.Forms.ToolStripStatusLabel tsslRules;
        private System.Windows.Forms.ToolStripStatusLabel tsslActivity;
    }
}


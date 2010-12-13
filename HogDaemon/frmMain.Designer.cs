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
            this.btnReload = new System.Windows.Forms.Button();
            this.lblHostname = new System.Windows.Forms.Label();
            this.tbHostname = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblRules = new System.Windows.Forms.Label();
            this.lblRulesNum = new System.Windows.Forms.Label();
            this.lblCmdNum = new System.Windows.Forms.Label();
            this.lblCmd = new System.Windows.Forms.Label();
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
            this.cbActiveDevice.Size = new System.Drawing.Size(188, 21);
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
            this.btnStartStop.Location = new System.Drawing.Point(108, 179);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 12;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(148, 91);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(75, 23);
            this.btnReload.TabIndex = 14;
            this.btnReload.Text = "Reload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // lblHostname
            // 
            this.lblHostname.AutoSize = true;
            this.lblHostname.Location = new System.Drawing.Point(31, 42);
            this.lblHostname.Name = "lblHostname";
            this.lblHostname.Size = new System.Drawing.Size(55, 13);
            this.lblHostname.TabIndex = 15;
            this.lblHostname.Text = "Hostname";
            // 
            // tbHostname
            // 
            this.tbHostname.Location = new System.Drawing.Point(92, 39);
            this.tbHostname.Name = "tbHostname";
            this.tbHostname.Size = new System.Drawing.Size(161, 20);
            this.tbHostname.TabIndex = 16;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(67, 91);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(92, 65);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(50, 20);
            this.tbPort.TabIndex = 19;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(60, 68);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 18;
            this.lblPort.Text = "Port";
            // 
            // lblRules
            // 
            this.lblRules.AutoSize = true;
            this.lblRules.Location = new System.Drawing.Point(88, 130);
            this.lblRules.Name = "lblRules";
            this.lblRules.Size = new System.Drawing.Size(73, 13);
            this.lblRules.TabIndex = 20;
            this.lblRules.Text = "Rules Loaded";
            // 
            // lblRulesNum
            // 
            this.lblRulesNum.AutoSize = true;
            this.lblRulesNum.Location = new System.Drawing.Point(167, 130);
            this.lblRulesNum.Name = "lblRulesNum";
            this.lblRulesNum.Size = new System.Drawing.Size(13, 13);
            this.lblRulesNum.TabIndex = 21;
            this.lblRulesNum.Text = "0";
            // 
            // lblCmdNum
            // 
            this.lblCmdNum.AutoSize = true;
            this.lblCmdNum.Location = new System.Drawing.Point(167, 143);
            this.lblCmdNum.Name = "lblCmdNum";
            this.lblCmdNum.Size = new System.Drawing.Size(13, 13);
            this.lblCmdNum.TabIndex = 23;
            this.lblCmdNum.Text = "0";
            // 
            // lblCmd
            // 
            this.lblCmd.AutoSize = true;
            this.lblCmd.Location = new System.Drawing.Point(60, 143);
            this.lblCmd.Name = "lblCmd";
            this.lblCmd.Size = new System.Drawing.Size(101, 13);
            this.lblCmd.TabIndex = 22;
            this.lblCmd.Text = "Commands/Second";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 214);
            this.Controls.Add(this.lblCmdNum);
            this.Controls.Add(this.lblCmd);
            this.Controls.Add(this.lblRulesNum);
            this.Controls.Add(this.lblRules);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbHostname);
            this.Controls.Add(this.lblHostname);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.lblActiveDevice);
            this.Controls.Add(this.cbActiveDevice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "frmMain";
            this.Text = "HogDaemon";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox cbActiveDevice;
        private System.Windows.Forms.Label lblActiveDevice;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Label lblHostname;
        private System.Windows.Forms.TextBox tbHostname;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblRules;
        private System.Windows.Forms.Label lblRulesNum;
        private System.Windows.Forms.Label lblCmdNum;
        private System.Windows.Forms.Label lblCmd;
    }
}


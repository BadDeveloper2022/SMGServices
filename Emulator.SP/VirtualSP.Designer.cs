namespace Emulator.SP
{
    partial class VirtualSP
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabSMS = new System.Windows.Forms.TabPage();
            this.gpSession = new System.Windows.Forms.GroupBox();
            this.rtbSession = new System.Windows.Forms.RichTextBox();
            this.tabStart = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbbSMGIP = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lbStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbAddress = new System.Windows.Forms.Label();
            this.nudSMGPort = new System.Windows.Forms.NumericUpDown();
            this.cbbSPNumber = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.btnSend = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbbNumber = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rtbContent = new System.Windows.Forms.RichTextBox();
            this.tabSMS.SuspendLayout();
            this.gpSession.SuspendLayout();
            this.tabStart.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSMGPort)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSMS
            // 
            this.tabSMS.Controls.Add(this.groupBox2);
            this.tabSMS.Controls.Add(this.gpSession);
            this.tabSMS.Location = new System.Drawing.Point(4, 22);
            this.tabSMS.Name = "tabSMS";
            this.tabSMS.Padding = new System.Windows.Forms.Padding(3);
            this.tabSMS.Size = new System.Drawing.Size(562, 323);
            this.tabSMS.TabIndex = 1;
            this.tabSMS.Text = "短消息";
            this.tabSMS.UseVisualStyleBackColor = true;
            // 
            // gpSession
            // 
            this.gpSession.Controls.Add(this.rtbSession);
            this.gpSession.Location = new System.Drawing.Point(6, 151);
            this.gpSession.Name = "gpSession";
            this.gpSession.Size = new System.Drawing.Size(546, 166);
            this.gpSession.TabIndex = 11;
            this.gpSession.TabStop = false;
            this.gpSession.Text = "会话";
            // 
            // rtbSession
            // 
            this.rtbSession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSession.Location = new System.Drawing.Point(3, 17);
            this.rtbSession.Name = "rtbSession";
            this.rtbSession.Size = new System.Drawing.Size(540, 146);
            this.rtbSession.TabIndex = 1;
            this.rtbSession.Text = "";
            // 
            // tabStart
            // 
            this.tabStart.Controls.Add(this.groupBox1);
            this.tabStart.Controls.Add(this.groupBox3);
            this.tabStart.Location = new System.Drawing.Point(4, 22);
            this.tabStart.Name = "tabStart";
            this.tabStart.Padding = new System.Windows.Forms.Padding(3);
            this.tabStart.Size = new System.Drawing.Size(562, 323);
            this.tabStart.TabIndex = 0;
            this.tabStart.Text = "开始";
            this.tabStart.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbbSPNumber);
            this.groupBox3.Controls.Add(this.nudSMGPort);
            this.groupBox3.Controls.Add(this.lbAddress);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.lbStatus);
            this.groupBox3.Controls.Add(this.btnStop);
            this.groupBox3.Controls.Add(this.btnStart);
            this.groupBox3.Controls.Add(this.cbbSMGIP);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(11, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(538, 150);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "开始";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "SP号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "服务器地址：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "服务器端口：";
            // 
            // cbbSMGIP
            // 
            this.cbbSMGIP.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cbbSMGIP.FormattingEnabled = true;
            this.cbbSMGIP.Items.AddRange(new object[] {
            "127.0.0.1"});
            this.cbbSMGIP.Location = new System.Drawing.Point(99, 68);
            this.cbbSMGIP.Name = "cbbSMGIP";
            this.cbbSMGIP.Size = new System.Drawing.Size(160, 21);
            this.cbbSMGIP.TabIndex = 4;
            this.cbbSMGIP.Text = "127.0.0.1";
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("宋体", 9.5F);
            this.btnStart.Location = new System.Drawing.Point(308, 109);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 30);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "开机";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("宋体", 9.5F);
            this.btnStop.Location = new System.Drawing.Point(411, 109);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 30);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "关机";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("宋体", 9.5F);
            this.lbStatus.ForeColor = System.Drawing.Color.Blue;
            this.lbStatus.Location = new System.Drawing.Point(354, 32);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(46, 13);
            this.lbStatus.TabIndex = 10;
            this.lbStatus.Text = "未连接";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(306, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "地址：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(306, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "状态：";
            // 
            // lbAddress
            // 
            this.lbAddress.AutoSize = true;
            this.lbAddress.Font = new System.Drawing.Font("宋体", 9.5F);
            this.lbAddress.ForeColor = System.Drawing.Color.Blue;
            this.lbAddress.Location = new System.Drawing.Point(353, 71);
            this.lbAddress.Name = "lbAddress";
            this.lbAddress.Size = new System.Drawing.Size(56, 13);
            this.lbAddress.TabIndex = 11;
            this.lbAddress.Text = "0.0.0.0";
            // 
            // nudSMGPort
            // 
            this.nudSMGPort.Font = new System.Drawing.Font("宋体", 9.5F);
            this.nudSMGPort.Location = new System.Drawing.Point(99, 109);
            this.nudSMGPort.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudSMGPort.Minimum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.nudSMGPort.Name = "nudSMGPort";
            this.nudSMGPort.Size = new System.Drawing.Size(60, 22);
            this.nudSMGPort.TabIndex = 12;
            this.nudSMGPort.Value = new decimal(new int[] {
            8801,
            0,
            0,
            0});
            // 
            // cbbSPNumber
            // 
            this.cbbSPNumber.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cbbSPNumber.FormattingEnabled = true;
            this.cbbSPNumber.Items.AddRange(new object[] {
            "106559284130016",
            "106559284250108",
            "106559213250218",
            "106551114250008",
            "106559423142508"});
            this.cbbSPNumber.Location = new System.Drawing.Point(99, 28);
            this.cbbSPNumber.Name = "cbbSPNumber";
            this.cbbSPNumber.Size = new System.Drawing.Size(160, 21);
            this.cbbSPNumber.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rtbLog);
            this.groupBox1.Location = new System.Drawing.Point(11, 174);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(538, 140);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "控制台输出";
            // 
            // rtbLog
            // 
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Location = new System.Drawing.Point(3, 17);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(532, 120);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabStart);
            this.tabControl1.Controls.Add(this.tabSMS);
            this.tabControl1.Location = new System.Drawing.Point(7, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(570, 349);
            this.tabControl1.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Font = new System.Drawing.Font("宋体", 11F);
            this.btnSend.Location = new System.Drawing.Point(442, 43);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 50);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbbNumber);
            this.groupBox2.Controls.Add(this.btnSend);
            this.groupBox2.Controls.Add(this.rtbContent);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(9, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(543, 135);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "新消息";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "手机号码：";
            // 
            // cbbNumber
            // 
            this.cbbNumber.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cbbNumber.FormattingEnabled = true;
            this.cbbNumber.Items.AddRange(new object[] {
            "8613020320822",
            "8613020320830",
            "8613845454545",
            "8613545677778",
            "8618650335176"});
            this.cbbNumber.Location = new System.Drawing.Point(87, 20);
            this.cbbNumber.Name = "cbbNumber";
            this.cbbNumber.Size = new System.Drawing.Size(160, 21);
            this.cbbNumber.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(40, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "内容：";
            // 
            // rtbContent
            // 
            this.rtbContent.Location = new System.Drawing.Point(87, 61);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.Size = new System.Drawing.Size(319, 60);
            this.rtbContent.TabIndex = 3;
            this.rtbContent.Text = "";
            // 
            // VirtualSP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.tabControl1);
            this.Name = "VirtualSP";
            this.Text = "SP模拟器";
            this.tabSMS.ResumeLayout(false);
            this.gpSession.ResumeLayout(false);
            this.tabStart.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSMGPort)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabSMS;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.GroupBox gpSession;
        private System.Windows.Forms.RichTextBox rtbSession;
        private System.Windows.Forms.TabPage tabStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbbSPNumber;
        private System.Windows.Forms.NumericUpDown nudSMGPort;
        private System.Windows.Forms.Label lbAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cbbSMGIP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ComboBox cbbNumber;
        private System.Windows.Forms.RichTextBox rtbContent;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;

    }
}


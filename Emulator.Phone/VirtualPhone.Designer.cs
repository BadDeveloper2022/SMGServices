namespace Emulator.Phone
{
    partial class VirtualPhone
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabStart = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbAddress = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.cbbSMGIP = new System.Windows.Forms.ComboBox();
            this.cbbNumber = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabSMS = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.rtbContent = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbbSPNumber = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.gpSession = new System.Windows.Forms.GroupBox();
            this.rtbSession = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbGroup = new System.Windows.Forms.ListBox();
            this.nudSMGPort = new System.Windows.Forms.NumericUpDown();
            this.tabControl1.SuspendLayout();
            this.tabStart.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabSMS.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gpSession.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSMGPort)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabStart);
            this.tabControl1.Controls.Add(this.tabSMS);
            this.tabControl1.Location = new System.Drawing.Point(8, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(570, 349);
            this.tabControl1.TabIndex = 0;
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nudSMGPort);
            this.groupBox3.Controls.Add(this.lbAddress);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.lbStatus);
            this.groupBox3.Controls.Add(this.btnStop);
            this.groupBox3.Controls.Add(this.btnStart);
            this.groupBox3.Controls.Add(this.cbbSMGIP);
            this.groupBox3.Controls.Add(this.cbbNumber);
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(306, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "状态：";
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
            // cbbNumber
            // 
            this.cbbNumber.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cbbNumber.FormattingEnabled = true;
            this.cbbNumber.Items.AddRange(new object[] {
            "8613020320822",
            "8613020320830"});
            this.cbbNumber.Location = new System.Drawing.Point(99, 28);
            this.cbbNumber.Name = "cbbNumber";
            this.cbbNumber.Size = new System.Drawing.Size(160, 21);
            this.cbbNumber.TabIndex = 3;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "服务器地址：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "手机号：";
            // 
            // tabSMS
            // 
            this.tabSMS.Controls.Add(this.groupBox2);
            this.tabSMS.Controls.Add(this.gpSession);
            this.tabSMS.Controls.Add(this.groupBox4);
            this.tabSMS.Location = new System.Drawing.Point(4, 22);
            this.tabSMS.Name = "tabSMS";
            this.tabSMS.Padding = new System.Windows.Forms.Padding(3);
            this.tabSMS.Size = new System.Drawing.Size(562, 323);
            this.tabSMS.TabIndex = 1;
            this.tabSMS.Text = "短消息";
            this.tabSMS.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSend);
            this.groupBox2.Controls.Add(this.rtbContent);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cbbSPNumber);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(173, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 139);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "新消息";
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Font = new System.Drawing.Font("宋体", 9.5F);
            this.btnSend.Location = new System.Drawing.Point(288, 15);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 30);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // rtbContent
            // 
            this.rtbContent.Location = new System.Drawing.Point(62, 63);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.Size = new System.Drawing.Size(302, 60);
            this.rtbContent.TabIndex = 3;
            this.rtbContent.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "内容：";
            // 
            // cbbSPNumber
            // 
            this.cbbSPNumber.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cbbSPNumber.FormattingEnabled = true;
            this.cbbSPNumber.Items.AddRange(new object[] {
            "106559284130016"});
            this.cbbSPNumber.Location = new System.Drawing.Point(62, 21);
            this.cbbSPNumber.Name = "cbbSPNumber";
            this.cbbSPNumber.Size = new System.Drawing.Size(180, 21);
            this.cbbSPNumber.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "SP号码：";
            // 
            // gpSession
            // 
            this.gpSession.Controls.Add(this.rtbSession);
            this.gpSession.Location = new System.Drawing.Point(173, 151);
            this.gpSession.Name = "gpSession";
            this.gpSession.Size = new System.Drawing.Size(379, 166);
            this.gpSession.TabIndex = 11;
            this.gpSession.TabStop = false;
            this.gpSession.Text = "会话";
            // 
            // rtbSession
            // 
            this.rtbSession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSession.Location = new System.Drawing.Point(3, 17);
            this.rtbSession.Name = "rtbSession";
            this.rtbSession.Size = new System.Drawing.Size(373, 146);
            this.rtbSession.TabIndex = 1;
            this.rtbSession.Text = "";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lbGroup);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(161, 311);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "会话组";
            // 
            // lbGroup
            // 
            this.lbGroup.Font = new System.Drawing.Font("宋体", 10F);
            this.lbGroup.ForeColor = System.Drawing.Color.Blue;
            this.lbGroup.FormattingEnabled = true;
            this.lbGroup.Location = new System.Drawing.Point(7, 23);
            this.lbGroup.Name = "lbGroup";
            this.lbGroup.Size = new System.Drawing.Size(145, 277);
            this.lbGroup.TabIndex = 9;
            this.lbGroup.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbGroup_MouseDoubleClick);
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
            8802,
            0,
            0,
            0});
            // 
            // VirtualPhone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "VirtualPhone";
            this.Text = "短消息模拟器";
            this.tabControl1.ResumeLayout(false);
            this.tabStart.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabSMS.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gpSession.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudSMGPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSMS;
        private System.Windows.Forms.TabPage tabStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cbbSMGIP;
        private System.Windows.Forms.ComboBox cbbNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox lbGroup;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Label lbAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.GroupBox gpSession;
        private System.Windows.Forms.RichTextBox rtbSession;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox rtbContent;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbbSPNumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudSMGPort;


    }
}
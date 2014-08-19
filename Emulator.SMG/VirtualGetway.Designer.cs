namespace Emulator.SMG
{
    partial class VirtualGetway
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSP = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbSPList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtbSPLog = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nudSPPort = new System.Windows.Forms.NumericUpDown();
            this.nudSPCount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSPStop = new System.Windows.Forms.Button();
            this.btnSPStart = new System.Windows.Forms.Button();
            this.cbbSPIP = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabSMSC = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbPhoneList = new System.Windows.Forms.ListBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rtbSMSCLog = new System.Windows.Forms.RichTextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.nudSMSCPort = new System.Windows.Forms.NumericUpDown();
            this.nudSMSCCount = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSMSCStop = new System.Windows.Forms.Button();
            this.btnSMSCStart = new System.Windows.Forms.Button();
            this.cbbSMSCIP = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabSP.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSPPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSPCount)).BeginInit();
            this.tabSMSC.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSMSCPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSMSCCount)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSP);
            this.tabControl1.Controls.Add(this.tabSMSC);
            this.tabControl1.Location = new System.Drawing.Point(7, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(570, 349);
            this.tabControl1.TabIndex = 1;
            // 
            // tabSP
            // 
            this.tabSP.Controls.Add(this.groupBox4);
            this.tabSP.Controls.Add(this.groupBox1);
            this.tabSP.Controls.Add(this.groupBox3);
            this.tabSP.Location = new System.Drawing.Point(4, 22);
            this.tabSP.Name = "tabSP";
            this.tabSP.Padding = new System.Windows.Forms.Padding(3);
            this.tabSP.Size = new System.Drawing.Size(562, 323);
            this.tabSP.TabIndex = 0;
            this.tabSP.Text = "SP网关中心";
            this.tabSP.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lbSPList);
            this.groupBox4.Location = new System.Drawing.Point(381, 9);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(168, 305);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "在线列表";
            // 
            // lbSPList
            // 
            this.lbSPList.Font = new System.Drawing.Font("宋体", 10F);
            this.lbSPList.ForeColor = System.Drawing.Color.Blue;
            this.lbSPList.FormattingEnabled = true;
            this.lbSPList.Location = new System.Drawing.Point(12, 22);
            this.lbSPList.Name = "lbSPList";
            this.lbSPList.Size = new System.Drawing.Size(150, 277);
            this.lbSPList.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rtbSPLog);
            this.groupBox1.Location = new System.Drawing.Point(11, 159);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(364, 155);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "控制台输出";
            // 
            // rtbSPLog
            // 
            this.rtbSPLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSPLog.Location = new System.Drawing.Point(3, 17);
            this.rtbSPLog.Name = "rtbSPLog";
            this.rtbSPLog.Size = new System.Drawing.Size(358, 135);
            this.rtbSPLog.TabIndex = 0;
            this.rtbSPLog.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nudSPPort);
            this.groupBox3.Controls.Add(this.nudSPCount);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.btnSPStop);
            this.groupBox3.Controls.Add(this.btnSPStart);
            this.groupBox3.Controls.Add(this.cbbSPIP);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(11, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(364, 135);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SP网关设置";
            // 
            // nudSPPort
            // 
            this.nudSPPort.Font = new System.Drawing.Font("宋体", 9.5F);
            this.nudSPPort.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSPPort.Location = new System.Drawing.Point(95, 61);
            this.nudSPPort.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudSPPort.Minimum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.nudSPPort.Name = "nudSPPort";
            this.nudSPPort.Size = new System.Drawing.Size(90, 22);
            this.nudSPPort.TabIndex = 11;
            this.nudSPPort.Value = new decimal(new int[] {
            8801,
            0,
            0,
            0});
            // 
            // nudSPCount
            // 
            this.nudSPCount.Font = new System.Drawing.Font("宋体", 9.5F);
            this.nudSPCount.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSPCount.Location = new System.Drawing.Point(95, 99);
            this.nudSPCount.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSPCount.Name = "nudSPCount";
            this.nudSPCount.Size = new System.Drawing.Size(60, 22);
            this.nudSPCount.TabIndex = 10;
            this.nudSPCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "SP监听数：";
            // 
            // btnSPStop
            // 
            this.btnSPStop.Enabled = false;
            this.btnSPStop.Font = new System.Drawing.Font("宋体", 9.5F);
            this.btnSPStop.Location = new System.Drawing.Point(283, 66);
            this.btnSPStop.Name = "btnSPStop";
            this.btnSPStop.Size = new System.Drawing.Size(70, 30);
            this.btnSPStop.TabIndex = 7;
            this.btnSPStop.Text = "停止";
            this.btnSPStop.UseVisualStyleBackColor = true;
            this.btnSPStop.Click += new System.EventHandler(this.btnSPStop_Click);
            // 
            // btnSPStart
            // 
            this.btnSPStart.Font = new System.Drawing.Font("宋体", 9.5F);
            this.btnSPStart.Location = new System.Drawing.Point(283, 20);
            this.btnSPStart.Name = "btnSPStart";
            this.btnSPStart.Size = new System.Drawing.Size(70, 30);
            this.btnSPStart.TabIndex = 6;
            this.btnSPStart.Text = "开启";
            this.btnSPStart.UseVisualStyleBackColor = true;
            this.btnSPStart.Click += new System.EventHandler(this.btnSPStart_Click);
            // 
            // cbbSPIP
            // 
            this.cbbSPIP.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cbbSPIP.FormattingEnabled = true;
            this.cbbSPIP.Items.AddRange(new object[] {
            "127.0.0.1"});
            this.cbbSPIP.Location = new System.Drawing.Point(95, 23);
            this.cbbSPIP.Name = "cbbSPIP";
            this.cbbSPIP.Size = new System.Drawing.Size(160, 21);
            this.cbbSPIP.TabIndex = 4;
            this.cbbSPIP.Text = "127.0.0.1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "监听端口：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "监听地址：";
            // 
            // tabSMSC
            // 
            this.tabSMSC.Controls.Add(this.groupBox2);
            this.tabSMSC.Controls.Add(this.groupBox5);
            this.tabSMSC.Controls.Add(this.groupBox6);
            this.tabSMSC.Location = new System.Drawing.Point(4, 22);
            this.tabSMSC.Name = "tabSMSC";
            this.tabSMSC.Padding = new System.Windows.Forms.Padding(3);
            this.tabSMSC.Size = new System.Drawing.Size(562, 323);
            this.tabSMSC.TabIndex = 1;
            this.tabSMSC.Text = "SMSC网关中心";
            this.tabSMSC.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbPhoneList);
            this.groupBox2.Location = new System.Drawing.Point(381, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(168, 305);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "在线列表";
            // 
            // lbPhoneList
            // 
            this.lbPhoneList.Font = new System.Drawing.Font("宋体", 10F);
            this.lbPhoneList.ForeColor = System.Drawing.Color.Blue;
            this.lbPhoneList.FormattingEnabled = true;
            this.lbPhoneList.Location = new System.Drawing.Point(12, 22);
            this.lbPhoneList.Name = "lbPhoneList";
            this.lbPhoneList.Size = new System.Drawing.Size(150, 277);
            this.lbPhoneList.TabIndex = 9;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rtbSMSCLog);
            this.groupBox5.Location = new System.Drawing.Point(11, 159);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(364, 155);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "控制台输出";
            // 
            // rtbSMSCLog
            // 
            this.rtbSMSCLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSMSCLog.Location = new System.Drawing.Point(3, 17);
            this.rtbSMSCLog.Name = "rtbSMSCLog";
            this.rtbSMSCLog.Size = new System.Drawing.Size(358, 135);
            this.rtbSMSCLog.TabIndex = 0;
            this.rtbSMSCLog.Text = "";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.nudSMSCPort);
            this.groupBox6.Controls.Add(this.nudSMSCCount);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.btnSMSCStop);
            this.groupBox6.Controls.Add(this.btnSMSCStart);
            this.groupBox6.Controls.Add(this.cbbSMSCIP);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Location = new System.Drawing.Point(11, 9);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(364, 135);
            this.groupBox6.TabIndex = 11;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "SMSC网关设置";
            // 
            // nudSMSCPort
            // 
            this.nudSMSCPort.Font = new System.Drawing.Font("宋体", 9.5F);
            this.nudSMSCPort.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSMSCPort.Location = new System.Drawing.Point(95, 61);
            this.nudSMSCPort.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudSMSCPort.Minimum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.nudSMSCPort.Name = "nudSMSCPort";
            this.nudSMSCPort.Size = new System.Drawing.Size(90, 22);
            this.nudSMSCPort.TabIndex = 18;
            this.nudSMSCPort.Value = new decimal(new int[] {
            8802,
            0,
            0,
            0});
            // 
            // nudSMSCCount
            // 
            this.nudSMSCCount.Font = new System.Drawing.Font("宋体", 9.5F);
            this.nudSMSCCount.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSMSCCount.Location = new System.Drawing.Point(95, 99);
            this.nudSMSCCount.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSMSCCount.Name = "nudSMSCCount";
            this.nudSMSCCount.Size = new System.Drawing.Size(60, 22);
            this.nudSMSCCount.TabIndex = 17;
            this.nudSMSCCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "监听数：";
            // 
            // btnSMSCStop
            // 
            this.btnSMSCStop.Enabled = false;
            this.btnSMSCStop.Font = new System.Drawing.Font("宋体", 9.5F);
            this.btnSMSCStop.Location = new System.Drawing.Point(283, 66);
            this.btnSMSCStop.Name = "btnSMSCStop";
            this.btnSMSCStop.Size = new System.Drawing.Size(70, 30);
            this.btnSMSCStop.TabIndex = 15;
            this.btnSMSCStop.Text = "停止";
            this.btnSMSCStop.UseVisualStyleBackColor = true;
            this.btnSMSCStop.Click += new System.EventHandler(this.btnSMSCStop_Click);
            // 
            // btnSMSCStart
            // 
            this.btnSMSCStart.Font = new System.Drawing.Font("宋体", 9.5F);
            this.btnSMSCStart.Location = new System.Drawing.Point(283, 20);
            this.btnSMSCStart.Name = "btnSMSCStart";
            this.btnSMSCStart.Size = new System.Drawing.Size(70, 30);
            this.btnSMSCStart.TabIndex = 14;
            this.btnSMSCStart.Text = "开启";
            this.btnSMSCStart.UseVisualStyleBackColor = true;
            this.btnSMSCStart.Click += new System.EventHandler(this.btnSMSCStart_Click);
            // 
            // cbbSMSCIP
            // 
            this.cbbSMSCIP.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cbbSMSCIP.FormattingEnabled = true;
            this.cbbSMSCIP.Items.AddRange(new object[] {
            "127.0.0.1"});
            this.cbbSMSCIP.Location = new System.Drawing.Point(95, 23);
            this.cbbSMSCIP.Name = "cbbSMSCIP";
            this.cbbSMSCIP.Size = new System.Drawing.Size(160, 21);
            this.cbbSMSCIP.TabIndex = 12;
            this.cbbSMSCIP.Text = "127.0.0.1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "监听端口：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "监听地址：";
            // 
            // VirtualGetway
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "VirtualGetway";
            this.Text = "短信网关服务模拟器";
            this.tabControl1.ResumeLayout(false);
            this.tabSP.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSPPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSPCount)).EndInit();
            this.tabSMSC.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSMSCPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSMSCCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSP;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtbSPLog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSPStop;
        private System.Windows.Forms.Button btnSPStart;
        private System.Windows.Forms.ComboBox cbbSPIP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabSMSC;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox lbSPList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lbPhoneList;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RichTextBox rtbSMSCLog;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSMSCStop;
        private System.Windows.Forms.Button btnSMSCStart;
        private System.Windows.Forms.ComboBox cbbSMSCIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudSPCount;
        private System.Windows.Forms.NumericUpDown nudSPPort;
        private System.Windows.Forms.NumericUpDown nudSMSCPort;
        private System.Windows.Forms.NumericUpDown nudSMSCCount;
    }
}


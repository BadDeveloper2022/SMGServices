using Emulator.SMG.Utils;
using SMG.SGIP.Base;
using SMG.TcpSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Emulator.SMG
{
    public partial class VirtualGetway : Form
    {
        TcpSocketServer tcpSPServer;
        TcpSocketServer tcpSMSCServer;
        
        public VirtualGetway()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            Sequence.SetNodeNumber(1380591);
        }

        private void btnSPStart_Click(object sender, EventArgs e)
        {
            if (cbbSPIP.Text == "")
            {
                return;
            }

            if (tcpSPServer == null || !tcpSPServer.Listened)
            {
                tcpSPServer = new TcpSocketServer(cbbSPIP.Text, (int)nudSPPort.Value);
                SPServerHandler.Register(this, tcpSPServer);
                tcpSPServer.Listen((int)nudSPPort.Value);
            }
        }

        private void btnSPStop_Click(object sender, EventArgs e)
        {
            if (tcpSPServer != null)
            {
                tcpSPServer.Stop();
            }
        }

        private void btnSMSCStart_Click(object sender, EventArgs e)
        {
            if (cbbSMSCIP.Text == "")
            {
                return;
            }

            if (tcpSMSCServer == null || !tcpSMSCServer.Listened)
            {
                tcpSMSCServer = new TcpSocketServer(cbbSMSCIP.Text, (int)nudSMSCPort.Value);
                SMSCServerHandler.Register(this, tcpSMSCServer);
                tcpSMSCServer.Listen((int)nudSMSCCount.Value);
            }
        }

        private void btnSMSCStop_Click(object sender, EventArgs e)
        {
            if (tcpSMSCServer != null)
            {
                tcpSMSCServer.Stop();
            }
        }

    }
}

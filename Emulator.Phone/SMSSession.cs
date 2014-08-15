using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Emulator.Phone
{
    public partial class SMSSession : Form
    {
        public SMSSession()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }

        public void Bind()
        {
            //lbSessions.Items.Clear();
            lbSessions.Items.AddRange(SMSHistory.GetSessions().ToArray());
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

        }

        private void lbSessions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbSessions.SelectedIndex != -1)
            {
                SMSItem item = new SMSItem();
                item.ShowDialog(this);
            }  
        }

    }
}

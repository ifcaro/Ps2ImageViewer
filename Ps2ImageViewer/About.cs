using System;
using System.Windows.Forms;

namespace Ps2ImageViewer
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void WebSiteLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ps2dev.ifcaro.net");
        }
    }
}

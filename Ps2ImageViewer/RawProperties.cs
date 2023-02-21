using System;
using System.Windows.Forms;

namespace Ps2ImageViewer
{
    public partial class RawProperties : Form
    {
        public int SelectedWidth { get; set; }
        public int SelectedHeight { get; set; }

        public RawProperties()
        {

            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {

            if (!int.TryParse(WidthTextBox.Text, out int width))
            {
                MessageBox.Show(Strings.THE_ENTERED_NUMBER_IS_NOT_VALID, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!int.TryParse(HeightTextBox.Text, out int height))
            {
                MessageBox.Show(Strings.THE_ENTERED_NUMBER_IS_NOT_VALID, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SelectedWidth = width;
            SelectedHeight = height;

            Close();
        }

        private void WidthTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char KeyAscii = e.KeyChar;

            if(KeyAscii == 13)
            {
                e.Handled = true;
                OkButton_Click(sender, e);
            }

            if((KeyAscii < 48 || KeyAscii > 57) && KeyAscii != 8)
            {
                e.Handled = true;
            }
        }
    }
}

using System;
using System.Windows.Forms;

namespace Ps2ImageViewer
{
    public partial class IIFFormat : Form
    {
        private Formats.IIF.Clut _clut;

        public IIFFormat(Formats.IIF.Clut clut)
        {
            _clut = clut;

            InitializeComponent();
        }

        private void IIFFormat_Load(object sender, EventArgs e)
        {
            FormatsComboBox.Items.Add("RGBA32");
            FormatsComboBox.Items.Add("RGB24");
            FormatsComboBox.Items.Add("RGBA16");

            if (_clut.clutsize > 0 && _clut.clutsize < 256)
            {
                FormatsComboBox.Items.Add("CLUT8_RGBA32");
                FormatsComboBox.Items.Add("CLUT8_RGBA16");
            }

            if (_clut.clutsize > 0 && _clut.clutsize < 16)
            {
                FormatsComboBox.Items.Add("CLUT4_RGBA32");
                FormatsComboBox.Items.Add("CLUT4_RGBA16");
            }

            FormatsComboBox.SelectedIndex = 0;
        }
    }
}

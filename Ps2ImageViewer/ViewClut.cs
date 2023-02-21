using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ps2ImageViewer
{
    public partial class ViewClut : Form
    {
        private Bitmap bitmap;

        public Formats.IIF.Clut Clut { get; set; }

        public ViewClut()
        {
            InitializeComponent();
        }

        private void ViewClut_Load(object sender, EventArgs e)
        {
            bitmap = new Bitmap(256 + 1, 256 + 1);

            int x = 0;
            int y = 0;

            for(int n = 0; n <= Clut.clutsize; n++)
            {
                for (int c = 0; c <= 7; c++)
                {
                    for (int d = 0; d <= 7; d++)
                    {
                        bitmap.SetPixel(x + c, y + d, System.Drawing.Color.FromArgb(Clut.cluta[n], Clut.clutr[n], Clut.clutg[n], Clut.clutb[n]));
                    }
                }
                if (x < 15 * 7)
                {
                    x = x + 7;
                }
                else
                {
                    x = 0;
                    y = y + 7;
                }
            }
            clut.Image = bitmap;
            clut.Refresh();
        }

        private void ViewClut_FormClosing(object sender, FormClosingEventArgs e)
        {
            bitmap.Dispose();
        }
    }
}

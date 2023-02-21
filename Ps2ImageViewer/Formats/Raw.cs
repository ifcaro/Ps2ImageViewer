using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ps2ImageViewer.Formats
{
    public class Raw
    {
        public static Bitmap LoadRaw(string archivo, int w, int h, bool alpha)
        {
            int X = 0;
            int Y = 0;
            int pos = 1;

            int r;
            int g;
            int b;
            int a;

            Bitmap bitmap = new Bitmap(w, h);

            using (FileStream fs = new FileStream(archivo, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    do
                    {
                        r = br.ReadByte();
                        g = br.ReadByte();
                        b = br.ReadByte();

                        if (alpha)
                        {
                            a = br.ReadByte();
                            if (a != 0)
                            {
                                a *= 2;
                            }
                            if (a > 255)
                            {
                                a = 255;
                            }
                        }
                        else
                        {
                            a = 255;
                        }

                        bitmap.SetPixel(X, Y, System.Drawing.Color.FromArgb(a, r, g, b));

                        if (X < w - 1)
                        {
                            X++;
                        }
                        else
                        {
                            X = 0;
                            Y++;
                        }
                    } while (fs.Position < fs.Length);
                }
            }

            return bitmap;
        }

        public static void SaveRaw(Bitmap bitmap, string archivo, bool alpha)
        {
            using (FileStream fs = new FileStream(archivo, FileMode.Create))
            {
                using (BinaryWriter br = new BinaryWriter(fs))
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            br.Write((byte)bitmap.GetPixel(x, y).R);
                            br.Write((byte)bitmap.GetPixel(x, y).G);
                            br.Write((byte)bitmap.GetPixel(x, y).B);
                            if (alpha)
                            {
                                br.Write((byte)(bitmap.GetPixel(x, y).A / 2));
                            }
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace Ps2ImageViewer.Formats
{
    public class IIF
    {
        public class Clut
        {
            public int clutsize;
            public int[] clutr = new int[256];
            public int[] clutg = new int[256];
            public int[] clutb = new int[256];
            public int[] cluta = new int[256];
        }

        public static Bitmap LoadIIF(string archivo, Clut clut, out string clutType)
        {
            int w = 0;
            int h = 0;
            int psm = 0;
            int X = 0;
            int Y = 0;
            int paso = 0;

            int r = 0;
            int g = 0;
            int b = 0;
            int a = 255;
            int byt;

            Bitmap bitmap = null;

            clutType = string.Empty;

            using (FileStream fs = new FileStream(archivo, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    string cabecera = Encoding.ASCII.GetString(br.ReadBytes(3));

                    if (cabecera == "IIF")
                    {
                        int ver = br.ReadByte();
                        w = br.ReadInt32();
                        h = br.ReadInt32();
                        psm = br.ReadInt32();

                        bitmap = new Bitmap(w, h);

                        if (psm > 2)
                        {
                            ReadClut(psm, br, clut);
                        }
                        do
                        {
                            if (psm == 2)
                            {
                                byt = br.ReadUInt16();
                                r = (byt & 31) * 8;
                                g = ((byt / 32) & 31) * 8;
                                b = ((byt / 1024) & 31) * 8;
                                if (a != 0)
                                {
                                    a = 128;
                                }
                            }
                            else
                            {
                                if (psm == 0)
                                {
                                    r = br.ReadByte();
                                    g = br.ReadByte();
                                    b = br.ReadByte();
                                    a = br.ReadByte();
                                }
                                else if (psm == 1)
                                {
                                    r = br.ReadByte();
                                    g = br.ReadByte();
                                    b = br.ReadByte();
                                }
                                else if (psm == 3 || psm == 4)
                                {
                                    r = br.ReadByte();
                                }
                                else
                                {
                                    if (paso == 0)
                                    {
                                        byt = br.ReadByte();
                                        r = byt & 15;
                                        g = (byt / 16) & 15;
                                    }
                                }
                            }

                            if (psm < 3)
                            {
                                if (a != 0)
                                {
                                    a = a * 2;
                                }
                                if (a > 255)
                                {
                                    a = 255;
                                }
                                bitmap.SetPixel(X, Y, System.Drawing.Color.FromArgb(a, r, g, b));
                            }
                            else if (psm < 5)
                            {
                                a = 0;
                                if (clut.cluta[r] != 0)
                                {
                                    a = clut.cluta[r] * 2;
                                }
                                if (a > 255)
                                {
                                    a = 255;
                                }
                                bitmap.SetPixel(X, Y, System.Drawing.Color.FromArgb(a, clut.clutr[r], clut.clutg[r], clut.clutb[r]));
                            }
                            else
                            {
                                if (paso == 0)
                                {
                                    a = 0;
                                    if (clut.cluta[r] != 0)
                                    {
                                        a = clut.cluta[r] * 2;
                                    }
                                    if (a > 255)
                                    {
                                        a = 255;
                                    }
                                    bitmap.SetPixel(X, Y, System.Drawing.Color.FromArgb(a, clut.clutr[r], clut.clutg[r], clut.clutb[r]));
                                    paso = 1;
                                }
                                else
                                {
                                    a = 0;
                                    if (clut.cluta[r] != 0)
                                    {
                                        a = clut.cluta[r] * 2;
                                    }
                                    if (a > 255)
                                    {
                                        a = 255;
                                    }
                                    bitmap.SetPixel(X, Y, System.Drawing.Color.FromArgb(a, clut.clutr[g], clut.clutg[g], clut.clutb[g]));
                                    paso = 0;
                                }
                            }
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
                    else
                    {
                        return null;
                    }
                }
            }

            if (psm == 0)
            {
                clutType = "RGBA32";
            }
            else if (psm == 1)
            {
                clutType = "RGB24";
            }
            else if (psm == 2)
            {
                clutType = "RGBA16";
            }
            else if (psm == 3)
            {
                clutType = "CLUT8_RGBA32";
            }
            else if (psm == 4)
            {
                clutType = "CLUT8_RGBA16";
            }
            else if (psm == 5)
            {
                clutType = "CLUT4_RGBA32";
            }
            else if (psm == 6)
            {
                clutType = "CLUT4_RGBA16";
            }

            return bitmap;
        }


        public static void SaveIIF_RGBA32(Bitmap bitmap, string archivo)
        {
            using (FileStream fs = new FileStream(archivo, FileMode.Create))
            {
                using (BinaryWriter br = new BinaryWriter(fs))
                {
                    br.Write("IIF1".ToCharArray());
                    br.Write(bitmap.Width);
                    br.Write(bitmap.Height);
                    br.Write(0);

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            br.Write((byte)bitmap.GetPixel(x, y).R);
                            br.Write((byte)bitmap.GetPixel(x, y).G);
                            br.Write((byte)bitmap.GetPixel(x, y).B);
                            br.Write((byte)(bitmap.GetPixel(x, y).A / 2));
                        }
                    }
                }
            }
        }

        public static void SaveIIF_RGB24(Bitmap bitmap, string archivo)
        {
            using (FileStream fs = new FileStream(archivo, FileMode.Create))
            {
                using (BinaryWriter br = new BinaryWriter(fs))
                {
                    br.Write("IIF1".ToCharArray());
                    br.Write(bitmap.Width);
                    br.Write(bitmap.Height);
                    br.Write(1);

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            double a = bitmap.GetPixel(x, y).A;
                            br.Write((byte)Math.Round((bitmap.GetPixel(x, y).R * a) / 255));
                            br.Write((byte)Math.Round((bitmap.GetPixel(x, y).G * a) / 255));
                            br.Write((byte)Math.Round((bitmap.GetPixel(x, y).B * a) / 255));
                        }
                    }
                }
            }
        }

        public static void SaveIIF_RGBA16(Bitmap bitmap, string archivo)
        {
            using (FileStream fs = new FileStream(archivo, FileMode.Create))
            {
                using (BinaryWriter br = new BinaryWriter(fs))
                {
                    br.Write("IIF1".ToCharArray());
                    br.Write(bitmap.Width);
                    br.Write(bitmap.Height);
                    br.Write(2);

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            double a = bitmap.GetPixel(x, y).A;
                            int res = (int)(((bitmap.GetPixel(x, y).B * a) / 255) / 8);

                            res = ((res * 32) | (int)Math.Round(((bitmap.GetPixel(x, y).G * a) / 255) / 8d));
                            res = ((res * 32) | (int)Math.Round(((bitmap.GetPixel(x, y).R * a) / 255) / 8d));

                            br.Write((short)res);
                        }
                    }
                }
            }
        }

        public static void SaveIIF_CLUT8RGBA32(Bitmap bitmap, Clut clut, string archivo)
        {
            using (FileStream fs = new FileStream(archivo, FileMode.Create))
            {
                using (BinaryWriter br = new BinaryWriter(fs))
                {
                    br.Write("IIF1".ToCharArray());
                    br.Write(bitmap.Width);
                    br.Write(bitmap.Height);
                    br.Write(3);

                    for (int n = 0; n < 256; n++)
                    {
                        int pc = n;

                        if (n > 7 && n < 16)
                        {
                            pc = n + 8;
                        }
                        else if (n > 15 && n < 24)
                        {
                            pc = n - 8;
                        }
                        else if (n > 39 && n < 48)
                        {
                            pc = n + 8;
                        }
                        else if (n > 47 && n < 56)
                        {
                            pc = n - 8;
                        }
                        else if (n > 71 && n < 80)
                        {
                            pc = n + 8;
                        }
                        else if (n > 79 && n < 88)
                        {
                            pc = n - 8;
                        }
                        else if (n > 103 && n < 112)
                        {
                            pc = n + 8;
                        }
                        else if (n > 111 && n < 120)
                        {
                            pc = n - 8;
                        }
                        else if (n > 135 && n < 144)
                        {
                            pc = n + 8;
                        }
                        else if (n > 143 && n < 152)
                        {
                            pc = n - 8;
                        }
                        else if (n > 167 && n < 176)
                        {
                            pc = n + 8;
                        }
                        else if (n > 175 && n < 184)
                        {
                            pc = n - 8;
                        }
                        else if (n > 199 && n < 208)
                        {
                            pc = n + 8;
                        }
                        else if (n > 207 && n < 216)
                        {
                            pc = n - 8;
                        }
                        else if (n > 231 && n < 240)
                        {
                            pc = n + 8;
                        }
                        else if (n > 239 && n < 248)
                        {
                            pc = n - 8;
                        }
                        br.Write((byte)(clut.clutr[pc]));
                        br.Write((byte)(clut.clutg[pc]));
                        br.Write((byte)(clut.clutb[pc]));

                        if (clut.cluta[pc] > 0)
                        {
                            br.Write((byte)(clut.cluta[pc]));
                        }
                        else
                        {
                            br.Write((byte)(clut.cluta[pc]));
                        }
                    }

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            int r = (bitmap.GetPixel(x, y).R);
                            int g = (bitmap.GetPixel(x, y).G);
                            int b = (bitmap.GetPixel(x, y).B);
                            int a = (int)Math.Round((bitmap.GetPixel(x, y).A + 1) / 2d);

                            for (int n = 0; n <= clut.clutsize; n++)
                            {
                                if (clut.clutr[n] == r && clut.clutg[n] == g && clut.clutb[n] == b && clut.cluta[n] == a)
                                {
                                    br.Write((byte)n);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void SaveIIF_CLUT4RGBA32(Bitmap bitmap, Clut clut, string archivo)
        {
            using (FileStream fs = new FileStream(archivo, FileMode.Create))
            {
                using (BinaryWriter br = new BinaryWriter(fs))
                {
                    br.Write("IIF1".ToCharArray());
                    br.Write(bitmap.Width);
                    br.Write(bitmap.Height);
                    br.Write(5);

                    for (int n = 0; n < 16; n++)
                    {
                        br.Write((byte)(clut.clutr[n]));
                        br.Write((byte)(clut.clutg[n]));
                        br.Write((byte)(clut.clutb[n]));
                        br.Write((byte)(clut.cluta[n]));
                    }

                    int paso = 0;
                    int res = 0;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            int r = (bitmap.GetPixel(x, y).R);
                            int g = (bitmap.GetPixel(x, y).G);
                            int b = (bitmap.GetPixel(x, y).B);
                            int a = (bitmap.GetPixel(x, y).A + 1) / 2;


                            for (int n = 0; n <= clut.clutsize; n++)
                            {
                                if (clut.clutr[n] == r && clut.clutg[n] == g && clut.clutb[n] == b && clut.cluta[n] == a)
                                {
                                    if (paso == 0)
                                    {
                                        res = (byte)n;
                                        paso = 1;
                                    }
                                    else
                                    {
                                        res = (res) | (byte)(n * 16);
                                        paso = 0;
                                        br.Write((byte)(res));
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void SaveIIF_CLUT8RGBA16(Bitmap bitmap, Clut clut, string archivo)
        {
            using (FileStream fs = new FileStream(archivo, FileMode.Create))
            {
                using (BinaryWriter br = new BinaryWriter(fs))
                {
                    br.Write("IIF1".ToCharArray());
                    br.Write(bitmap.Width);
                    br.Write(bitmap.Height);
                    br.Write(4);

                    for (int n = 0; n < 256; n++)
                    {
                        int pc = n;

                        if (n > 7 && n < 16)
                        {
                            pc = n + 8;
                        }
                        else if (n > 15 && n < 24)
                        {
                            pc = n - 8;
                        }
                        else if (n > 39 && n < 48)
                        {
                            pc = n + 8;
                        }
                        else if (n > 47 && n < 56)
                        {
                            pc = n - 8;
                        }
                        else if (n > 71 && n < 80)
                        {
                            pc = n + 8;
                        }
                        else if (n > 79 && n < 88)
                        {
                            pc = n - 8;
                        }
                        else if (n > 103 && n < 112)
                        {
                            pc = n + 8;
                        }
                        else if (n > 111 && n < 120)
                        {
                            pc = n - 8;
                        }
                        else if (n > 135 && n < 144)
                        {
                            pc = n + 8;
                        }
                        else if (n > 143 && n < 152)
                        {
                            pc = n - 8;
                        }
                        else if (n > 167 && n < 176)
                        {
                            pc = n + 8;
                        }
                        else if (n > 175 && n < 184)
                        {
                            pc = n - 8;
                        }
                        else if (n > 199 && n < 208)
                        {
                            pc = n + 8;
                        }
                        else if (n > 207 && n < 216)
                        {
                            pc = n - 8;
                        }
                        else if (n > 231 && n < 240)
                        {
                            pc = n + 8;
                        }
                        else if (n > 239 && n < 248)
                        {
                            pc = n - 8;
                        }

                        double a = clut.cluta[pc];
                        int res = (int)Math.Floor(((clut.clutb[pc] * a) / 255) / 8);

                        res = (short)(res * 32) | (short)Math.Round(((short)((clut.clutg[pc] * a) / 255) / 8d));
                        res = (short)(res * 32) | (short)Math.Round(((short)((clut.clutr[pc] * a) / 255) / 8d));
                        br.Write((short)res);
                    }

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            int r = (bitmap.GetPixel(x, y).R);
                            int g = (bitmap.GetPixel(x, y).G);
                            int b = (bitmap.GetPixel(x, y).B);
                            int a = (bitmap.GetPixel(x, y).A + 1) / 2;

                            for (int n = 0; n < 256; n++)
                            {
                                if (clut.clutr[n] == r && clut.clutg[n] == g && clut.clutb[n] == b && clut.cluta[n] == a)
                                {
                                    br.Write((byte)(n));
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void SaveIIF_CLUT4RGBA16(Bitmap bitmap, Clut clut, string archivo)
        {
            using (FileStream fs = new FileStream(archivo, FileMode.Create))
            {
                using (BinaryWriter br = new BinaryWriter(fs))
                {
                    br.Write("IIF1".ToCharArray());
                    br.Write(bitmap.Width);
                    br.Write(bitmap.Height);
                    br.Write(6);

                    for (int n = 0; n < 16; n++)
                    {
                        double a = clut.cluta[n];
                        int ret = (int)Math.Floor(((clut.clutb[n] * a) / 255) / 8);
                        ret = (short)(ret * 32) | (short)Math.Round(((short)((clut.clutg[n] * a) / 255) / 8d));
                        ret = (short)(ret * 32) | (short)Math.Round(((short)((clut.clutr[n] * a) / 255) / 8d));
                        br.Write((short)ret);
                    }

                    int paso = 0;
                    int res = 0;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            int r = (bitmap.GetPixel(x, y).R);
                            int g = (bitmap.GetPixel(x, y).G);
                            int b = (bitmap.GetPixel(x, y).B);
                            int a = (bitmap.GetPixel(x, y).A + 1) / 2;

                            for (int n = 0; n <= clut.clutsize; n++)
                            {
                                if (clut.clutr[n] == r && clut.clutg[n] == g && clut.clutb[n] == b && clut.cluta[n] == a)
                                {
                                    if (paso == 0)
                                    {
                                        res = (byte)(n);
                                        paso = 1;
                                    }
                                    else
                                    {
                                        res = (res) | (byte)(n * 16);
                                        paso = 0;
                                        br.Write((byte)res);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void ReadClut(int psm, BinaryReader br, Clut clut)
        {
            int n = 0;
            int size = 0;

            int pc = 0;

            int r = 0;
            int g = 0;
            int b = 0;
            int a = 255;

            switch (psm)
            {
                case 3: // IIF_CLUT8_RGBA32
                    size = 256;
                    break;
                case 4: // IIF_CLUT8_RGBA16
                    size = 256;
                    break;
                case 5: // IIF_CLUT4_RGBA32
                    size = 16;
                    break;
                case 6: // IIF_CLUT4_RGBA16
                    size = 16;
                    break;
                case 7: // IIF_GIF256
                    size = 256;
                    break;
                case 8: // IIF_GIF16
                    size = 16;
                    break;
            }

            do
            {
                if (psm == 3 || psm == 5)
                {
                    r = br.ReadByte();
                    g = br.ReadByte();
                    b = br.ReadByte();
                    a = br.ReadByte();
                }
                else
                {
                    int byt = br.ReadUInt16();
                    r = (byt & 31) * 8;
                    g = ((byt / 32) & 31) * 8;
                    b = ((byt / 1024) & 31) * 8;
                }

                clut.clutr[pc] = r;
                clut.clutg[pc] = g;
                clut.clutb[pc] = b;
                clut.cluta[pc] = a;

                n++;
                pc = n;

                if (psm == 3 || psm == 4)
                {
                    if (n > 7 && n < 16)
                    {
                        pc = n + 8;
                    }
                    else if (n > 15 && n < 24)
                    {
                        pc = n - 8;
                    }
                    else if (n > 39 && n < 48)
                    {
                        pc = n + 8;
                    }
                    else if (n > 47 && n < 56)
                    {
                        pc = n - 8;
                    }
                    else if (n > 71 && n < 80)
                    {
                        pc = n + 8;
                    }
                    else if (n > 79 && n < 88)
                    {
                        pc = n - 8;
                    }
                    else if (n > 103 && n < 112)
                    {
                        pc = n + 8;
                    }
                    else if (n > 111 && n < 120)
                    {
                        pc = n - 8;
                    }
                    else if (n > 135 && n < 144)
                    {
                        pc = n + 8;
                    }
                    else if (n > 143 && n < 152)
                    {
                        pc = n - 8;
                    }
                    else if (n > 167 && n < 176)
                    {
                        pc = n + 8;
                    }
                    else if (n > 175 && n < 184)
                    {
                        pc = n - 8;
                    }
                    else if (n > 199 && n < 208)
                    {
                        pc = n + 8;
                    }
                    else if (n > 207 && n < 216)
                    {
                        pc = n - 8;
                    }
                    else if (n > 231 && n < 240)
                    {
                        pc = n + 8;
                    }
                    else if (n > 239 && n < 248)
                    {
                        pc = n - 8;
                    }
                }

            } while (n < size);

            clut.clutsize = size - 1;
        }
    }
}

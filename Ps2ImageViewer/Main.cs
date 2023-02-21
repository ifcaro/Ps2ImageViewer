using Ps2ImageViewer.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Ps2ImageViewer
{
    public partial class Main : Form
    {
        private Config _config = new Config();
        private ViewClut clutForm;

        private IIF.Clut clut = new IIF.Clut();
        private Bitmap bitmap;

        public Main()
        {
            _config.ReadConfig("config.cfg");

            string selectedLanguage = _config.Get("General", "Language");
            bool positionSaved = false;
            int x;
            int y;
            int w;
            int h;

            if (int.TryParse(_config.Get("General", "x_pos"), out x))
            {
                Left = x;
                positionSaved = true;
            }

            if (int.TryParse(_config.Get("General", "y_pos"), out y))
            {
                Top = y;
                positionSaved = true;
            }

            if (int.TryParse(_config.Get("General", "width"), out w))
            {
                Width = w;
                positionSaved = true;
            }

            if (int.TryParse(_config.Get("General", "height"), out h))
            {
                Height = h;
                positionSaved = true;
            }

            if (positionSaved)
            {
                StartPosition = FormStartPosition.Manual;
            }

            if (!string.IsNullOrEmpty(selectedLanguage))
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }

            ToolStripMenuItem[] languageMenuItems = SetLocale();
            InitializeComponent();

            LanguageToolStripMenuItem.DropDownItems.AddRange(languageMenuItems);
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutForm = new About();

            aboutForm.ShowDialog();
        }

        private void ViewClutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clutForm == null)
            {
                clutForm = new ViewClut();
                clutForm.Disposed += (a, b) =>
                {
                    clutForm = null;
                };
                clutForm.Clut = clut;
                clutForm.Show();
            }
            else
            {
                clutForm.Focus();
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ImagePictureBox.AllowDrop = true;

            toolStripStatusLabel.Text = Strings.READY;

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                OpenFile(args[1]);
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            _config.Set("General", "x_pos", $"{Left}");
            _config.Set("General", "y_pos", $"{Top}");
            _config.Set("General", "width", $"{Height}");
            _config.Set("General", "height", $"{Width}");

            _config.SaveConfig("config.cfg");
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = Strings.ALL_FORMATS + "|*.bmp;*.jpg;*.png;*.gif;*.iif;*.raw|BMP (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|GIF (*.gif)|*.gif|IIF (*.iif)|*.iif|RAW (*.raw)|*.raw";
            openFileDialog.FileName = "";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName == "") return;
            clut.clutsize = 0;
            OpenFile(openFileDialog.FileName);
        }

        private void OpenFile(string fileName)
        {
            string ext = fileName.Substring(fileName.Length - 4, 4);

            if (ext == ".bmp" || ext == ".jpg" || ext == ".png" || ext == ".gif")
            {
                toolStripStatusLabel.Text = Strings.LOADING + " " + ext;

                if (bitmap != null)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }

                bitmap = new Bitmap(fileName);

                ImagePictureBox.Image = bitmap;
                clut.clutsize = ReadPalette(fileName.Substring(fileName.Length - 3, 3));
            }
            else if (ext == ".iif")
            {
                toolStripStatusLabel.Text = Strings.LOADING + " " + ext;

                if (bitmap != null)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }

                string clutType;
                
                bitmap = IIF.LoadIIF(fileName, clut, out clutType);

                if (bitmap != null)
                {
                    toolStripStatusLabel.Text = Strings.LOADED + " IIF (" + bitmap.Width + "x" + bitmap.Height + ") " + clutType;
                    ImagePictureBox.Image = bitmap;
                }
                else
                {
                    MessageBox.Show(Strings.INVALID_IIF, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else if (ext == ".raw")
            {
                toolStripStatusLabel.Text = Strings.LOADING + " " + ext;

                RawProperties rawPropertiesForm = new RawProperties();
                rawPropertiesForm.ShowDialog();

                if (bitmap != null)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }

                if (rawPropertiesForm.Rgb24RadioButton.Checked)
                {
                    bitmap = Raw.LoadRaw(fileName, rawPropertiesForm.SelectedWidth, rawPropertiesForm.SelectedHeight, false);
                }
                else
                {
                    bitmap = Raw.LoadRaw(fileName, rawPropertiesForm.SelectedWidth, rawPropertiesForm.SelectedHeight, true);
                }

                toolStripStatusLabel.Text = Strings.LOADED + " RAW (" + bitmap.Width + "x" + bitmap.Height + ")";

                ImagePictureBox.Image = bitmap;
                clut.clutsize = ReadPalette(fileName.Substring(fileName.Length - 3, 3));
            }
            SaveAsToolStripMenuItem.Enabled = true;
        }

        private int ReadPalette(string formato)
        {
            Color[] color = new Color[256];

            Color rec;

            int n = 0;

            bool existe;

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    rec = bitmap.GetPixel(x, y);

                    existe = false;

                    for (int s = 0; s <= n; s++)
                    {
                        if (rec.ToArgb() == color[s].ToArgb())
                        {
                            existe = true;
                        }
                    }

                    if (n == 255)
                    {
                        return 0;
                    }

                    if (!existe)
                    {
                        color[n] = rec;
                        n++;
                    }
                }
            }

            int result = n;

            for (n = 0; n <= result; n++)
            {
                clut.clutr[n] = color[n].R;
                clut.clutg[n] = color[n].G;
                clut.clutb[n] = color[n].B;
                clut.cluta[n] = (int)Math.Round(color[n].A / 2d);
            }

            toolStripStatusLabel.Text = Strings.LOADED + " " + formato.ToUpper() + " (" + bitmap.Width + "x" + bitmap.Height + ")";

            return result;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|RAW RGB24 (*.raw)|*.raw|RAW RGBA32 (*.raw)|*.raw|IIF (*.iif)|*.iif";
            saveFileDialog.FileName = "";
            saveFileDialog.ShowDialog();

            var ext_ = saveFileDialog.FileName.Split('.');
            var ext = ext_[ext_.Length - 1];

            if (saveFileDialog.FilterIndex == 1)
            {
                if (ext != "png")
                {
                    saveFileDialog.FileName = saveFileDialog.FileName + ".png";
                }
                toolStripStatusLabel.Text = Strings.SAVING + " " + saveFileDialog.FileName;
                bitmap.Save(saveFileDialog.FileName, ImageFormat.Png);
                toolStripStatusLabel.Text = "Guardado";
            }
            else if (saveFileDialog.FilterIndex == 2)
            {
                if (ext != "jpg")
                {
                    saveFileDialog.FileName = saveFileDialog.FileName + ".jpg";
                }
                toolStripStatusLabel.Text = Strings.SAVING + " " + saveFileDialog.FileName;
                bitmap.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
            }
            else if (saveFileDialog.FilterIndex == 3)
            {
                if (ext != "bmp")
                {
                    saveFileDialog.FileName = saveFileDialog.FileName + ".bmp";
                }
                toolStripStatusLabel.Text = Strings.SAVING + " " + saveFileDialog.FileName;
                bitmap.Save(saveFileDialog.FileName, ImageFormat.Bmp);
            }
            else if (saveFileDialog.FilterIndex == 4)
            {
                if (ext != "gif")
                {
                    saveFileDialog.FileName = saveFileDialog.FileName + ".gif";
                }
                toolStripStatusLabel.Text = Strings.SAVING + " " + saveFileDialog.FileName;
                bitmap.Save(saveFileDialog.FileName, ImageFormat.Gif);
            }
            else if (saveFileDialog.FilterIndex == 5)
            {
                if (ext != "raw")
                {
                    saveFileDialog.FileName = saveFileDialog.FileName + ".raw";
                }
                toolStripStatusLabel.Text = Strings.SAVING + " " + saveFileDialog.FileName;
                Raw.SaveRaw(bitmap, saveFileDialog.FileName, false);
                toolStripStatusLabel.Text = Strings.SAVED;
            }
            else if (saveFileDialog.FilterIndex == 6)
            {
                if (ext != "raw")
                {
                    saveFileDialog.FileName = saveFileDialog.FileName + ".raw";
                }
                toolStripStatusLabel.Text = Strings.SAVING + " " + saveFileDialog.FileName;
                Raw.SaveRaw(bitmap, saveFileDialog.FileName, true);
                toolStripStatusLabel.Text = Strings.SAVED;
            }
            else if (saveFileDialog.FilterIndex == 7)
            {
                if (ext != "iif")
                {
                    saveFileDialog.FileName = saveFileDialog.FileName + ".iif";
                }
                toolStripStatusLabel.Text = Strings.SAVING + " " + saveFileDialog.FileName;

                IIFFormat iif = new IIFFormat(clut);

                if (iif.ShowDialog() == DialogResult.OK)
                {
                    if (iif.FormatsComboBox.SelectedIndex == 0)
                    {
                        IIF.SaveIIF_RGBA32(bitmap, saveFileDialog.FileName);
                    }
                    else if (iif.FormatsComboBox.SelectedIndex == 1)
                    {
                        IIF.SaveIIF_RGB24(bitmap, saveFileDialog.FileName);
                    }
                    else if (iif.FormatsComboBox.SelectedIndex == 2)
                    {
                        IIF.SaveIIF_RGBA16(bitmap, saveFileDialog.FileName);
                    }
                    else if (iif.FormatsComboBox.SelectedIndex == 3)
                    {
                        IIF.SaveIIF_CLUT8RGBA32(bitmap, clut, saveFileDialog.FileName);
                    }
                    else if (iif.FormatsComboBox.SelectedIndex == 4)
                    {
                        IIF.SaveIIF_CLUT8RGBA16(bitmap, clut, saveFileDialog.FileName);
                    }
                    else if (iif.FormatsComboBox.SelectedIndex == 5)
                    {
                        IIF.SaveIIF_CLUT4RGBA32(bitmap, clut, saveFileDialog.FileName);
                    }
                    else if (iif.FormatsComboBox.SelectedIndex == 6)
                    {
                        IIF.SaveIIF_CLUT4RGBA16(bitmap, clut, saveFileDialog.FileName);
                    }

                    toolStripStatusLabel.Text = Strings.SAVED;
                }
            }
        }

        private void ImagePictureBox_DragDrop(object sender, DragEventArgs e)
        {
            string fileName = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            OpenFile(fileName);
        }

        private void ImagePictureBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private ToolStripMenuItem[] SetLocale()
        {
            List<ToolStripMenuItem> languageMenuItems = new List<ToolStripMenuItem>();
            bool found = false;

            var defaultItem = new ToolStripMenuItem("English");
            defaultItem.Tag = CultureInfo.InvariantCulture;
            defaultItem.Click += Item_Click;
            languageMenuItems.Add(defaultItem);

            string executablePath = Path.GetDirectoryName(Application.ExecutablePath);
            string[] directories = Directory.GetDirectories(executablePath);
            foreach (string s in directories)
            {
                try
                {
                    DirectoryInfo langDirectory = new DirectoryInfo(s);
                    CultureInfo cultureInfo = CultureInfo.GetCultureInfo(langDirectory.Name);
                    var item = new ToolStripMenuItem(cultureInfo.DisplayName);
                    item.Tag = cultureInfo;
                    item.Click += Item_Click;
                    item.Checked = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == cultureInfo.TwoLetterISOLanguageName;

                    found |= item.Checked;

                    languageMenuItems.Add(item);
                }
                catch (Exception)
                {

                }
            }

            if (!found)
            {
                languageMenuItems[0].Checked = true;
            }

            return languageMenuItems.ToArray();
        }

        private void Item_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in LanguageToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
            }

            ToolStripMenuItem selectedItem = (ToolStripMenuItem)sender;
            selectedItem.Checked = true;

            System.Threading.Thread.CurrentThread.CurrentUICulture = (CultureInfo)selectedItem.Tag;

            MessageBox.Show(Strings.RESTART_APP_TO_APPLY_CHANGES, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

            _config.Set("General", "Language", $"{((CultureInfo)selectedItem.Tag).IetfLanguageTag}");
        }
    }
}

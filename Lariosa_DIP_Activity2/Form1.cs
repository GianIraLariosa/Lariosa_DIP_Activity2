using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebCamLib;

namespace Lariosa_DIP_Activity2
{
    public partial class Form1 : Form
    {
        Bitmap image1;
        Bitmap image2;
        Bitmap imageB, imageA, colorgreen, resultImage;
        private Device selectedDevice;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            btnLoadbkgimg.Visible = false;
            btnLoadimg.Visible = false;
            btnsubtract.Visible = false;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            image1 = new Bitmap(openFileDialog.FileName);
            pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image2 = new Bitmap(image1.Width, image1.Height);

            for (int x = 0; x < image2.Width; x++)
            {
                for (int y = 0; y < image2.Height; y++)
                {
                    Color pixelColor = image1.GetPixel(x, y);
                    image2.SetPixel(x, y, pixelColor);
                }
            }

            pictureBox2.Image = image2;
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null)
            {
                copyToolStripMenuItem_Click(sender, e);
            }


            for (int x = 0; x < image2.Width; x++)
            {
                for (int y = 0; y < image2.Height; y++)
                {
                    Color pixelColor = image2.GetPixel(x, y);

                    int grayvalue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                    Color gray = Color.FromArgb(grayvalue, grayvalue, grayvalue);

                    image2.SetPixel(x, y, gray);
                }
            }

            pictureBox2.Image = image2;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null)
            {
                copyToolStripMenuItem_Click(sender, e);
            }

            for (int x = 0; x < image2.Width; x++)
            {
                for (int y = 0; y < image2.Height; y++)
                {
                    Color pixelColor = image2.GetPixel(x, y);

                    Color invert = Color.FromArgb(255 - pixelColor.R, 255 - pixelColor.B, 255 - pixelColor.G);

                    image2.SetPixel(x, y, invert);
                }
            }

            pictureBox2.Image = image2;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null)
            {
                copyToolStripMenuItem_Click(sender, e);
            }

            Bitmap resultimg = new Bitmap(image2.Width, image2.Height);

            for (int x = 0; x < image2.Width; x++)
            {
                for (int y = 0; y < image2.Height; y++)
                {
                    Color pixelColor = image2.GetPixel(x, y);

                    int grayvalue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                    Color gray = Color.FromArgb(grayvalue, grayvalue, grayvalue);

                    resultimg.SetPixel(x, y, gray);
                }
            }

            int[] histogram = new int[256];
            Color sample;

            for (int x = 0; x < image2.Width; x++)
            {
                for (int y = 0; y < image2.Height; y++)
                {
                    sample = resultimg.GetPixel(x, y);
                    histogram[sample.R]++;
                }
            }

            Bitmap mygraph = new Bitmap(256, 800);
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 800; y++)
                {
                    mygraph.SetPixel(x, y, Color.White);
                }
            }

            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < Math.Min(histogram[x] / 5, 800); y++)
                {
                    mygraph.SetPixel(x, 799 - y, Color.Black);
                }
            }

            pictureBox2.Image = mygraph;

        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double tr, tg, tb;
            int Red, Green, Blue;
            if (pictureBox2.Image == null)
            {
                copyToolStripMenuItem_Click(sender, e);
            }

            for (int x = 0; x < image2.Width; x++)
            {
                for (int y = 0; y < image2.Height; y++)
                {
                    Color pixelColor = image2.GetPixel(x, y);

                    tr = 0.393 * (pixelColor.R) + 0.769 * (pixelColor.G) + 0.189 * (pixelColor.B);
                    tg = 0.349 * (pixelColor.R) + 0.686 * (pixelColor.G) + 0.168 * (pixelColor.B);
                    tb = 0.272 * (pixelColor.R) + 0.534 * (pixelColor.G) + 0.131 * (pixelColor.B);

                    if (tr > 255)
                    {
                        Red = 255;
                    }
                    else
                    {
                        Red = (int)tr;
                    }

                    if (tg > 255)
                    {
                        Green = 255;
                    }
                    else
                    {
                        Green = (int)tg;
                    }

                    if (tb > 255)
                    {
                        Blue = 255;
                    }
                    else
                    {
                        Blue = (int)tb;
                    }

                    Color sepia = Color.FromArgb(Red, Green, Blue);

                    image2.SetPixel(x, y, sepia);
                }
            }

            pictureBox2.Image = image2;
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Images|*.png;*.jpg";
            ImageFormat format;

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    default:
                        format = ImageFormat.Png;
                        break;
                }
                pictureBox2.Image.Save(saveFileDialog.FileName, format);
            }
        }

        
private void webcamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitializeDevices();
        }

        private void subtractionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnLoadbkgimg.Visible = true;
            btnLoadimg.Visible = true;
            btnsubtract.Visible = true;
        }

        private void btnsubtract_Click(object sender, EventArgs e)
        {
            Bitmap image2 = new Bitmap(imageB.Width, imageB.Height);

            Color mygreen = Color.FromArgb(0, 0, 255);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;

            for (int x = 0; x < imageB.Width; x++)
            {
                for (int y = 0; y < imageB.Height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractvalue = Math.Abs(grey - greygreen);
                    if (subtractvalue > threshold)
                        image2.SetPixel(x, y, pixel);
                    else
                        image2.SetPixel(x, y, backpixel);
                }
            }

            pictureBox3.Image = image2;
        }

        private void InitializeDevices()
        {
            // Get all available devices
            Device[] devices = DeviceManager.GetAllDevices();

            // Let the user choose a device (you can implement your own logic for this)
            int selectedDeviceIndex = 0; // You might want to show a dialog or UI to let the user choose

            // Get the selected device
            selectedDevice = DeviceManager.GetDevice(selectedDeviceIndex);

            // Initialize the device and show the preview in the PictureBox
            if (selectedDevice != null)
            {
                selectedDevice.ShowWindow(pictureBox1);
            }
            else
            {
                MessageBox.Show("No webcam selected.");
            }
        }
    

        

        private void btnLoadimg_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            imageB = new Bitmap(openFileDialog.FileName);
            pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
        }

        private void btnLoadbkgimg_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            imageA = new Bitmap(openFileDialog.FileName);
            pictureBox2.Image = Image.FromFile(openFileDialog.FileName);
        }
        
    }
}

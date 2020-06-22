using System;
using System.Drawing;
using System.Windows.Forms;

namespace NasaImagesAsyncApp
{
    public partial class ApodMain : Form
    {
        private ApodDownloader _apodDownloader;
        public ApodMain()
        {
            InitializeComponent();
            _apodDownloader = new ApodDownloader();
        }

        private void APOD_Main_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            var selectedDate = dateTimePicker1.Value;
            var image = await _apodDownloader.DownloadImageForDate(selectedDate);
            if (image != null)
            {
                pictureBox1.Image = (Image)(new Bitmap(image, new Size(pictureBox1.Width, pictureBox1.Height)));
            }
            button1.Enabled = true;
        }
    }
}

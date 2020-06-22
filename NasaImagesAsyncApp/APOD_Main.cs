using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NasaImagesAsyncApp
{
    public partial class ApodMain : Form
    {
        public ApodMain()
        {
            InitializeComponent();
        }

        private void APOD_Main_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            var apodDownloader = new ApodDownloader();
            var selectedDate = dateTimePicker1.Value;
            var image = await apodDownloader.DownloadImageForDate(selectedDate);
            pictureBox1.Image = (Image)(new Bitmap(image, new Size(pictureBox1.Width,pictureBox1.Height)));
            button1.Enabled = true;
        }
    }
}

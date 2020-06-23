using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NasaImagesAsyncApp
{
    public partial class ApodMain : Form
    {
        private readonly ApodDownloader _apodDownloader;
        private const string ImageFileExtension = ".jpg";
        private Image _image;
        private readonly string _baseDirectory;
    public ApodMain()
        {
            InitializeComponent();
            _apodDownloader = new ApodDownloader();
            _baseDirectory = _apodDownloader.BaseDirectory;
            if (!Directory.Exists(_baseDirectory))
            {
                Directory.CreateDirectory(_baseDirectory);
            }
        }

        private void APOD_Main_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            var selectedDate = dateTimePicker1.Value;
            var fileName = _apodDownloader.FormatDate(selectedDate);
            
            if (File.Exists(_baseDirectory + fileName))
            {
                _image = Image.FromFile(_baseDirectory + fileName + ImageFileExtension);
            }
            else
            {
                _image = await _apodDownloader.DownloadImageForDate(selectedDate);
                
            }
            if (_image != null)
            {
                _image.Save(_baseDirectory + fileName + ImageFileExtension, _image.RawFormat);
                pictureBox1.Image = new Bitmap(_image, new Size(pictureBox1.Width, pictureBox1.Height));
            }
            button1.Enabled = true;
        }
    }
}

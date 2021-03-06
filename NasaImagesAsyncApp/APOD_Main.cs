﻿using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
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
            var apiKey = ConfigurationManager.AppSettings["api_key"];
            _apodDownloader = new ApodDownloader(apiKey);
            _baseDirectory = _apodDownloader.BaseDirectory;
            if (!Directory.Exists(_baseDirectory))
            {
                Directory.CreateDirectory(_baseDirectory);
            }
        }

        private void APOD_Main_Load(object sender, EventArgs e)
        {

        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            var selectedDatesRange = monthCalendar1.SelectionRange;
            var dateStrings = _apodDownloader.GetSelectionRangeDateStrings(selectedDatesRange);

            foreach (string fileName in dateStrings)
            {
                await LoadImage(fileName);
            }
            
            button1.Enabled = true;
        }

        private async Task LoadImage(string fileName)
        {
            if (File.Exists(_baseDirectory + fileName))
            {
                _image = Image.FromFile(_baseDirectory + fileName + ImageFileExtension);
            }
            else
            {
                _image = await _apodDownloader.DownloadImageForDate(fileName);
            }

            if (_image != null)
            {
                SaveAndShowImage(fileName);
            }
        }

        private void SaveAndShowImage(string fileName)
        {
            _image.Save(_baseDirectory + fileName + ImageFileExtension, _image.RawFormat);
            pictureBox1.Image = new Bitmap(_image, new Size(pictureBox1.Width, pictureBox1.Height));
        }
    }
}

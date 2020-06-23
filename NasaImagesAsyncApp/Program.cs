using System;
using System.Windows.Forms;

namespace NasaImagesAsyncApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// API to use: GET https://api.nasa.gov/planetary/apod
        /// query params: date (YYYY-MM-DD, default today), hd (bool, default false), api-key (string, default DEMO_KEY)
        /// personal key (needs to be saved in a JSON or something to remove it from here:
        /// 9D6EVFHS4mLEdvx6ZtN5i2XRv84kkfL3OwJYJLuQ
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ApodMain());
        }
    }
}

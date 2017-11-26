using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MyOrthoClient.Controllers;

namespace MyOrthoClient.Views
{
    public static class ResultBase
    {
        public static BitmapImage ConvertToBitMap(this UserControl uc, int score)
        {
            var path = ScoreProvider.ImageResult(score).Split(new char[] { '.' });
            var image = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\" + path[0] + " - Copy." + path[1], UriKind.Absolute));

            return image;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace KinectImageViewer
{
    /// <summary>
    /// Interaction logic for FullscreenPics.xaml
    /// </summary>
    public partial class FullscreenPics : Window
    {
        protected string[] picFiles;
        protected int currentImg = 0;

        public FullscreenPics(int shownImg)
        {
            currentImg = shownImg;
            InitializeComponent();            
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            picFiles = Directory.GetFiles(@"images");
            ShowCurrentImage();            
        }

        private void previousBtn_Click(object sender, RoutedEventArgs e)
        {
            if (picFiles.Length > 0)
            {
                currentImg = currentImg == 0 ? picFiles.Length - 1 : --currentImg;
                ShowCurrentImage();
            }
        }

        private void nextBtn_Click(object sender, System.EventArgs e)
        {
            if (picFiles.Length > 0)
            {
                currentImg = currentImg == picFiles.Length - 1 ? 0 : ++currentImg;
                ShowCurrentImage();
            }
        }

        protected void ShowCurrentImage()
        {
            if (currentImg >= 0 && currentImg <= picFiles.Length - 1)
            {
                BitmapImage bm = new BitmapImage(new Uri(picFiles[currentImg], UriKind.RelativeOrAbsolute));
                FullscreenImageBox.Source = bm;
            }
        }

        private void Exit(object sender, System.EventArgs e)
        {
            this.Close();
        }

    }
}

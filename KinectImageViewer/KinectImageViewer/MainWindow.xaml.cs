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


namespace KinectImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        progLogic pL = new progLogic();

        protected string[] picFiles;
        protected int currentImg = -1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void previousBtn_Click(object sender, RoutedEventArgs e)
        {
            if (picFiles.Length > 0)
            {
                currentImg = currentImg == 0 ? picFiles.Length - 1 : --currentImg;
                ShowCurrentImage();
            }
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
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
               //mainImage = System.Windows.Controls.Image.FromFile(picFiles[currentImg]);
            }
        }


        private void importBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "JPEG|*.jpg|Bitmaps|*.bmp";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                picFiles = openFileDialog.FileNames;
                currentImg = 0;
                ShowCurrentImage();
            }
        }

        private void helpBtn_Click(object sender, RoutedEventArgs e)
        {
            Help h = new Help();
            h.Show();
        }


    }
}

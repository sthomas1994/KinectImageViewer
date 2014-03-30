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
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        progLogic pL = new progLogic();

        protected string[] picFiles;
        protected int currentImg = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            picFiles = Directory.GetFiles(@"images");
            ShowCurrentImage();
            ShowNextImage();
            ShowSecondNextImage();
            ShowPreviousImage();
            ShowSecondPreviousImage();
        }

        private void previousBtn_Click(object sender, RoutedEventArgs e)
        {
            if (picFiles.Length > 0)
            {
                currentImg = currentImg == 0 ? picFiles.Length - 1 : --currentImg;
                ShowCurrentImage();
                ShowNextImage();
                ShowSecondNextImage();
                ShowPreviousImage();
                ShowSecondPreviousImage();
            }
        }

        private void nextBtn_Click(object sender, System.EventArgs e)
        {
            if (picFiles.Length > 0)
            {
                currentImg = currentImg == picFiles.Length - 1 ? 0 : ++currentImg;
                ShowCurrentImage();
                ShowNextImage();
                ShowSecondNextImage();
                ShowPreviousImage();
                ShowSecondPreviousImage();
            }
        }

        protected void ShowCurrentImage()
        {
            if (currentImg >= 0 && currentImg <= picFiles.Length - 1)
            {
                BitmapImage bm = new BitmapImage(new Uri(picFiles[currentImg], UriKind.RelativeOrAbsolute));
                ImageBox.Source = bm;
            }
        }

        protected void ShowNextImage()
        {
            int nextImg = currentImg + 1;

            if (nextImg > picFiles.Length - 1)
            {
                BitmapImage bm = new BitmapImage(new Uri(picFiles[0], UriKind.RelativeOrAbsolute));
                NextImageBox.Source = bm;
            }           
            else
            {
                BitmapImage bm = new BitmapImage(new Uri(picFiles[nextImg], UriKind.RelativeOrAbsolute));
                NextImageBox.Source = bm;
            }
        }

        protected void ShowSecondNextImage()
        {
            int Img = currentImg + 2;

            if (Img > picFiles.Length)
            {
                BitmapImage bm = new BitmapImage(new Uri(picFiles[1], UriKind.RelativeOrAbsolute));
                SecondNextImageBox.Source = bm;
            }
            else if (Img > picFiles.Length - 1)
            {
                BitmapImage bm = new BitmapImage(new Uri(picFiles[0], UriKind.RelativeOrAbsolute));
                SecondNextImageBox.Source = bm;
            }
            else
            {
                BitmapImage bm = new BitmapImage(new Uri(picFiles[Img], UriKind.RelativeOrAbsolute));
                SecondNextImageBox.Source = bm;
            }
        }

        protected void ShowSecondPreviousImage()
        {
            int Img = currentImg - 2;

            if (Img < -1)
            {
                BitmapImage bm = new BitmapImage(new Uri(picFiles[picFiles.Length - 2], UriKind.RelativeOrAbsolute));
                SecondPreviousImageBox.Source = bm;
            }
            else if (Img < 0)
            {
                BitmapImage bm = new BitmapImage(new Uri(picFiles[picFiles.Length - 1], UriKind.RelativeOrAbsolute));
                SecondPreviousImageBox.Source = bm;
            }
            else
            {
                BitmapImage bm = new BitmapImage(new Uri(picFiles[Img], UriKind.RelativeOrAbsolute));
                SecondPreviousImageBox.Source = bm;
            }
        }

        protected void ShowPreviousImage()
        {
            int Img = currentImg - 1;

            if (Img < 0)
            {
                BitmapImage bm = new BitmapImage(new Uri(picFiles[picFiles.Length - 1], UriKind.RelativeOrAbsolute));
                PreviousImageBox.Source = bm;
            }
            else
            {
                BitmapImage bm = new BitmapImage(new Uri(picFiles[Img], UriKind.RelativeOrAbsolute));
                PreviousImageBox.Source = bm;
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

        void OnMouseDownPlayMedia(object sender, MouseButtonEventArgs args)
        {

            // The Play method will begin the media if it is not currently active or  
            // resume media if it is paused. This has no effect if the media is 
            // already running.
            myMediaElement.Play();

            // Initialize the MediaElement property values.
            InitializePropertyValues();

        }

        // Pause the media. 
        void OnMouseDownPauseMedia(object sender, MouseButtonEventArgs args)
        {

            // The Pause method pauses the media if it is currently running. 
            // The Play method can be used to resume.
            myMediaElement.Pause();

        }

        // Stop the media. 
        void OnMouseDownStopMedia(object sender, MouseButtonEventArgs args)
        {

            // The Stop method stops and resets the media to be played from 
            // the beginning.
            myMediaElement.Stop();

        }

        // Change the volume of the media. 
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            myMediaElement.Volume = (double)volumeSlider.Value;
        }

        // Change the speed of the media. 
        private void ChangeMediaSpeedRatio(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            myMediaElement.SpeedRatio = (double)speedRatioSlider.Value;
        }

        // When the media opens, initialize the "Seek To" slider maximum value 
        // to the total number of miliseconds in the length of the media clip. 
        private void Element_MediaOpened(object sender, EventArgs e)
        {
            timelineSlider.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        // When the media playback is finished. Stop() the media to seek to media start. 
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            myMediaElement.Stop();
        }

        // Jump to different parts of the media (seek to).  
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            int SliderValue = (int)timelineSlider.Value;

            // Overloaded constructor takes the arguments days, hours, minutes, seconds, miniseconds. 
            // Create a TimeSpan with miliseconds equal to the slider value.
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            myMediaElement.Position = ts;
        }

        void InitializePropertyValues()
        {
            // Set the media's starting Volume and SpeedRatio to the current value of the 
            // their respective slider controls.
            myMediaElement.Volume = (double)volumeSlider.Value;
            myMediaElement.SpeedRatio = (double)speedRatioSlider.Value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}

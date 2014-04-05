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
using System.Windows.Threading;
using System.Threading;


namespace KinectImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        progLogic pL = new progLogic();

        DispatcherTimer ticks = new DispatcherTimer();
        DispatcherTimer pauser = new DispatcherTimer();

        protected string[] picFiles;
        protected int currentImg = 0;
        protected bool isPlaying = false;
        protected bool mediaOpened = false;
        protected bool tabOpened = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            String i = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string[] ext = { ".jpg", ".jpeg", ".gif", ".png", ".bmp", ".tiff"};
            picFiles = Directory.GetFiles(i, "*.*")
                .Where(f => ext.Contains(new FileInfo(f).Extension.ToLower())).ToArray();
            Console.WriteLine(picFiles.Length);   
            ShowCurrentImage();
            ShowNextImage();
            ShowSecondNextImage();
            ShowPreviousImage();
            ShowSecondPreviousImage();
        }

        private void pause_Media(object sender, EventArgs e)
        {
            myMediaElement.Pause();
            isPlaying = false;
            pauser.Stop();
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

        private void fullscrnBtn_Click(object sender, RoutedEventArgs e)
        {
            FullscreenPics f = new FullscreenPics(currentImg, this);
            f.Show();
        }

        private void fullscrnVidBtn_Click(object sender, RoutedEventArgs e)
        {
            myMediaElement.Pause();
            double timeIn = myMediaElement.Position.TotalMilliseconds;
            double maxTime = DurationSlider.Maximum;
            double volumeIn = volumeSlider.Value;
            double speedIn = speedRatioSlider.Value;
            FullscreenVid v = new FullscreenVid(currentImg, isPlaying, timeIn, volumeIn, speedIn, maxTime, mediaOpened);
            v.Show();
            isPlaying = false;
        }

        void OnMouseDownPlayMedia(object sender, MouseButtonEventArgs args)
        {

            // The Play method will begin the media if it is not currently active or  
            // resume media if it is paused. This has no effect if the media is 
            // already running.
            myMediaElement.Play();
            isPlaying = true;

            // Initialize the MediaElement property values.
            InitializePropertyValues();

        }

        // Pause the media. 
        void OnMouseDownPauseMedia(object sender, MouseButtonEventArgs args)
        {

            // The Pause method pauses the media if it is currently running. 
            // The Play method can be used to resume.
            myMediaElement.Pause();
            isPlaying = false;

        }

        // Pause the media. 
        void OnMouseOverPauseMedia(object sender, MouseButtonEventArgs args)
        {

            // The Pause method pauses the media if it is currently running. 
            // The Play method can be used to resume.
            myMediaElement.Pause();
            isPlaying = false;

        }

        // Stop the media. 
        void OnMouseDownStopMedia(object sender, MouseButtonEventArgs args)
        {

            // The Stop method stops and resets the media to be played from 
            // the beginning.
            myMediaElement.Stop();
            isPlaying = true;

        }

        // Change the volume of the media. 
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            try
            {
                myMediaElement.Volume = (double)volumeSlider.Value;
            }
            catch(NullReferenceException n)
            {
                
            }
                
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
            mediaOpened = true;
            //timelineSlider.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            DurationSlider.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            ticks.Interval = TimeSpan.FromMilliseconds(1);
            ticks.Tick += ticks_Tick;
            ticks.Start();
        }

        // When the media playback is finished. Stop() the media to seek to media start. 
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            myMediaElement.Stop();
            isPlaying = false;
        }

        // Jump to different parts of the media (seek to).  
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            if(!isPlaying)
            {
                int SliderValue = (int)DurationSlider.Value;

                // Overloaded constructor takes the arguments days, hours, minutes, seconds, miniseconds. 
                // Create a TimeSpan with miliseconds equal to the slider value.
                TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
                myMediaElement.Position = ts;
            }
        }

        //Updating the Slider Value of Media(Video Duration) 
        void ticks_Tick(object sender, object e)
        {
            DurationSlider.Value = myMediaElement.Position.TotalMilliseconds;
            //DurationText.Text = Milliseconds_to_Minute((long)VideoPlayer.Position.TotalMilliseconds);
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

        public void setCurrentImg(int n)
        {
            currentImg = n;
            ShowCurrentImage();
            ShowNextImage();
            ShowSecondNextImage();
            ShowPreviousImage();
            ShowSecondPreviousImage();
        }

        private delegate void setImageDelegate(string controlName, BitmapFrame frame);
        private void setImage(string controlName, BitmapFrame frame)
        {
            var control = FindName(controlName);
            if (control != null)
                ((System.Windows.Controls.Image)control).Source = frame;
        }

        private void makeThumbnails(BitmapFrame frame, object state)
        {
            Dispatcher.Invoke(new setImageDelegate(setImage), (string)state, frame);
        }

        private void makeJpeg(BitmapFrame frame, object state)
        {
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(frame);

            string filename = (string)state + ".jpg";
            using (var fs = new FileStream(filename, FileMode.Create))
                encoder.Save(fs);
        }

        private void image_Button_Click(object sender, RoutedEventArgs e)
        {
            var source = myMediaElement.Source;

            VideoScreenShot.CaptureScreenAsync(source, new Dictionary<TimeSpan, object> { 
				{TimeSpan.FromSeconds(10), "image0"} ,
				{TimeSpan.FromSeconds(20), "image1"} ,
				{TimeSpan.FromSeconds(30), "image2"} ,
				{TimeSpan.FromSeconds(40), "image3"} ,
			}, .1, makeJpeg, makeThumbnails);
            //VideoScreenShot.CaptureScreenAsync(source, TimeSpan.FromSeconds(10), "image0", .1, makeJpeg, makeThumbnails);
            //VideoScreenShot.CaptureScreenAsync(source, TimeSpan.FromSeconds(43) + TimeSpan.FromMilliseconds(760), "image1", makeThumbnails);
        }

        private void TabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            tabOpened = true;
            myMediaElement.Play();
            isPlaying = true;
            if(tabOpened)
            {
                pauser.Interval = TimeSpan.FromMilliseconds(100);
            }
            else
            {
                pauser.Interval = TimeSpan.FromMilliseconds(650);
            }
            pauser.Tick += pause_Media;
            pauser.Start();
            var source = myMediaElement.Source;

            VideoScreenShot.CaptureScreenAsync(source, new Dictionary<TimeSpan, object> { 
				{TimeSpan.FromSeconds(10), "image0"} ,
				{TimeSpan.FromSeconds(20), "image1"} ,
				{TimeSpan.FromSeconds(30), "image2"} ,
				{TimeSpan.FromSeconds(40), "image3"} ,
			}, .1, makeJpeg, makeThumbnails);
            //VideoScreenShot.CaptureScreenAsync(source, TimeSpan.FromSeconds(10), "image0", .1, makeJpeg, makeThumbnails);
            //VideoScreenShot.CaptureScreenAsync(source, TimeSpan.FromSeconds(43) + TimeSpan.FromMilliseconds(760), "image1", makeThumbnails);
        }
    }
}

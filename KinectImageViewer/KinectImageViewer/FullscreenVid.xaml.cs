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

namespace KinectImageViewer
{
    /// <summary>
    /// Interaction logic for FullscreenVid.xaml
    /// </summary>
    public partial class FullscreenVid : Window
    {
        DispatcherTimer ticks = new DispatcherTimer();

        protected string[] vidFiles;
        protected int currentVid = 0;
        protected bool isPlaying = false;
        protected double time = 0;
        protected double volume = 0;
        protected double speed = 0;
        protected double maxTime = 0;
        protected bool mediaOpened = false;

        public FullscreenVid(int shownVid, bool playing, double timeIn, double volumeIn, double speedIn, double maxTimeIn, bool opened)
        {
            currentVid = shownVid;
            InitializeComponent();
            time = timeIn;
            volume = volumeIn;
            speed = speedIn;
            maxTime = maxTimeIn;
            mediaOpened = opened;     
            /*
            if(mediaOpened)
            {
                timelineSlider.Maximum = (int)maxTime;
                timelineSlider.Value = (int)time;
                speedRatioSlider.Value = (int)speed;
                volumeSlider.Value = (int)volume;
                InitializePropertyValues(); 
                int intTime = (int)time; 
                TimeSpan ts = new TimeSpan(0, 0, 0, 0, intTime);
                myVideoElement.Position = ts;           
            }
             */ 
            isPlaying = playing;
            if(isPlaying)
            {
                myVideoElement.Play();
            }

        }

        /*
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timelineSlider.Maximum = myVideoElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            ticks.Interval = TimeSpan.FromMilliseconds(1);
            ticks.Tick += ticks_Tick;
            ticks.Start();
            timelineSlider.Value = time;
            speedRatioSlider.Value = speed;
            volumeSlider.Value = volume;

        }
         */

        protected void ShowCurrentVideo()
        {
            if (currentVid >= 0 && currentVid <= vidFiles.Length - 1)
            {
                myVideoElement.Source = new Uri(vidFiles[currentVid], UriKind.Absolute);
            }
        }


        private void previousBtn_Click(object sender, RoutedEventArgs e)
        {
            myVideoElement.Stop();
            isPlaying = false;
            if (vidFiles.Length > 0)
            {
                currentVid = currentVid == 0 ? vidFiles.Length - 1 : --currentVid;
                ShowCurrentVideo();
            }
        }

        private void nextBtn_Click(object sender, System.EventArgs e)
        {
            myVideoElement.Stop();
            isPlaying = false;
            if (vidFiles.Length > 0)
            {
                currentVid = currentVid == vidFiles.Length - 1 ? 0 : ++currentVid;

                ShowCurrentVideo();
            }
        }

        void OnMouseDownPlayMedia(object sender, MouseButtonEventArgs args)
        {

            // The Play method will begin the media if it is not currently active or  
            // resume media if it is paused. This has no effect if the media is 
            // already running.
            myVideoElement.Play();
            isPlaying = true;

            // Initialize the MediaElement property values.
            InitializePropertyValues();

        }

        // Pause the media. 
        void OnMouseDownPauseMedia(object sender, MouseButtonEventArgs args)
        {

            // The Pause method pauses the media if it is currently running. 
            // The Play method can be used to resume.
            myVideoElement.Pause();
            isPlaying = false;

        }

        // Stop the media. 
        void OnMouseDownStopMedia(object sender, MouseButtonEventArgs args)
        {

            // The Stop method stops and resets the media to be played from 
            // the beginning.
            myVideoElement.Stop();
            isPlaying = false;

        }

        // Change the volume of the media. 
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            try
            {
                myVideoElement.Volume = (double)volumeSlider.Value;
            }
            catch (NullReferenceException)
            {

            }

        }


        // Change the speed of the media. 
        private void ChangeMediaSpeedRatio(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            myVideoElement.SpeedRatio = (double)speedRatioSlider.Value;
        }

        // When the media opens, initialize the "Seek To" slider maximum value 
        // to the total number of miliseconds in the length of the media clip. 
        private void Element_MediaOpened(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                myVideoElement.Pause();
            }
            if (mediaOpened)
            {
                timelineSlider.Maximum = maxTime;
                timelineSlider.Value = time;
                speedRatioSlider.Value = speed;
                volumeSlider.Value = volume;
                InitializePropertyValues();
                int intTime = (int)time;
                TimeSpan ts = new TimeSpan(0, 0, 0, 0, intTime);
                myVideoElement.Position = ts;
            }
            else if (!mediaOpened)
            {
                InitializePropertyValues();
                timelineSlider.Maximum = myVideoElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                //timelineSlider.Value = (int)time;
                //speedRatioSlider.Value = speed;
                //volumeSlider.Value = volume;               
            }                
            ticks.Interval = TimeSpan.FromMilliseconds(1);
            ticks.Tick += ticks_Tick;
            ticks.Start();
            if (isPlaying)
            {
                myVideoElement.Play();
            }
        }

        // When the media playback is finished. Stop() the media to seek to media start. 
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            myVideoElement.Stop();
        }

        // Jump to different parts of the media (seek to).  
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            if(!isPlaying)
            {
                int SliderValue = (int)timelineSlider.Value;

                // Overloaded constructor takes the arguments days, hours, minutes, seconds, miniseconds. 
                // Create a TimeSpan with miliseconds equal to the slider value.
                TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
                myVideoElement.Position = ts;
            }

        }

        //Updating the Slider Value of Media(Video Duration) 
        void ticks_Tick(object sender, object e)
        {
            timelineSlider.Value = myVideoElement.Position.TotalMilliseconds;
            //DurationText.Text = Milliseconds_to_Minute((long)VideoPlayer.Position.TotalMilliseconds);
        }

        void InitializePropertyValues()
        {
            // Set the media's starting Volume and SpeedRatio to the current value of the 
            // their respective slider controls.
            myVideoElement.Volume = (double)volumeSlider.Value;
            myVideoElement.SpeedRatio = (double)speedRatioSlider.Value;

        }

        private void Exit(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            String v = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            string[] v_ext = { ".mp4", ".wmv", ".wma", ".mov", ".avi" };
            vidFiles = Directory.GetFiles(v, "*.*")
                .Where(g => v_ext.Contains(new FileInfo(g).Extension.ToLower())).ToArray();
            Console.WriteLine(vidFiles.Length); 
        }



    }
}

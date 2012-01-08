using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Cirrious.MandelBrot.Code;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Cirrious.MandelBrot
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MandelBrot.Code.Mandelbrot _mandelBrot;

        private DispatcherTimer _dispatcherTimer;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _mandelBrot = new Mandelbrot();
            _dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1.0);
            _dispatcherTimer.Tick += new EventHandler(t_Tick);
            _dispatcherTimer.Start();

            var blockTimer = new System.Windows.Threading.DispatcherTimer();
            blockTimer.Interval = TimeSpan.FromSeconds(10.0);
            blockTimer.Tick += (s, e) =>
                                   {
                                       SwitchBottomStackPanels();
                                   };
            blockTimer.Start();
            SwitchBottomStackPanels();
        }

        private void SwitchBottomStackPanels()
        {
            if (StackPanelA.Visibility == Visibility.Visible)
            {
                StackPanelA.Visibility = Visibility.Collapsed;
                StackPanelB.Visibility = Visibility.Visible;
            }
            else
            {
                StackPanelA.Visibility = Visibility.Visible;
                StackPanelB.Visibility = Visibility.Collapsed;
            }
        }


        void t_Tick(object sender, EventArgs e)
        {
            _mandelBrot.NextLine();
            this.ContentPanel.Background = new ImageBrush() { ImageSource = _mandelBrot.Source };
            _dispatcherTimer.Interval = _mandelBrot.IsComplete
                                            ? TimeSpan.FromSeconds(2.0)
                                            : TimeSpan.FromMilliseconds(1.0);
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            var mediaElement = (MediaElement)sender;
            var timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(5.0)
            };
            timer.Tick += (o, args) =>
                              {
                                  mediaElement.Position = TimeSpan.Zero;
                                  mediaElement.Play();
                                  timer.Stop();
                              };
            timer.Start();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            ShowWeb("http://www.jamendo.com/en/album/85671");
        }
        private void HyperlinkButtonAlpha_Click(object sender, RoutedEventArgs e)
        {
            ShowWeb("http://alphalabs.cc");
        }

        private void ShowWeb(string url)
        {
            try
            {
                var task = new WebBrowserTask() { Uri = new Uri(url) };
                task.Show();
            }
            catch (Exception)
            {

            }
        }
    }
}
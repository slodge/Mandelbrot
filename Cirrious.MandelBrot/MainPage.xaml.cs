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
using Cirrious.MandelBrot.Code;
using Microsoft.Phone.Controls;

namespace Cirrious.MandelBrot
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MandelBrot.Code.Mandelbrot _mandelBrot;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _mandelBrot = new Mandelbrot();
            var t = new System.Windows.Threading.DispatcherTimer();
            t.Interval = TimeSpan.FromMilliseconds(1.0);
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }

        void t_Tick(object sender, EventArgs e)
        {
            _mandelBrot.NextLine();
            this.ContentPanel.Background = new ImageBrush() {ImageSource = _mandelBrot.Source };
        }
    }
}
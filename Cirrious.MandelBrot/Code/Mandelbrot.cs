using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Cirrious.MandelBrot.Code
{
    public class Mandelbrot
    {
        private const int BASE_HEIGHT = 800;
        private const int BASE_WIDTH = 480;

        private double FINISH_SCALE = 2;
        private double HEIGHT;
        private double SCALE_DOWN = 256;
        private double SCALE_UP = 256;
        private double WIDTH;
        private WriteableBitmap _bitmap;
        private int _currentSettings;

        private List<Color> _colors;
        private double _s;
        private double _x;
        private double _xIncrement;
        private double _xmax;
        private double _xmin;
        private double _yIncrement;
        private double _ymax;
        private double _ymin;

        private readonly Random _random = new Random();

        private readonly List<Action> _colorActions;

        public Mandelbrot()
        {
            _bitmap = new WriteableBitmap(BASE_WIDTH, BASE_HEIGHT);
            _colorActions = new List<Action>()
                                {
                                    () => ColorArrayHSVGeneric(0,360,1.00,1.0),
                                    () => ColorArrayHSVGeneric(0,360,0.60,1.0),
                                    () => ColorArrayHSVGeneric(0,360,1.00,0.75),
                                    () => ColorArrayHSVGeneric(0,360,0.60,0.75),
                                    () => ColorArrayHSVGeneric(0,180,1.00,1.0),
                                    () => ColorArrayHSVGeneric(0,180,0.60,1.0),
                                    () => ColorArrayHSVGeneric(0,180,1.00,0.75),
                                    () => ColorArrayHSVGeneric(0,180,0.60,0.75),
                                    () => ColorArrayHSVGeneric(180,360,1.00,1.0),
                                    () => ColorArrayHSVGeneric(180,360,0.60,1.0),
                                    () => ColorArrayHSVGeneric(180,360,1.00,0.75),
                                    () => ColorArrayHSVGeneric(180,360,0.60,0.75),
                                    ColorArrayOne,
                                    ColorArrayTwo,
                                    ColorArrayThree,
                                    ColorArrayFour,
                                    ColorArrayFive,
                                    ColorArraySix,
                                    () => ColorArrayGeneric(1,0,0),
                                    () => ColorArrayGeneric(0,1,0),
                                    () => ColorArrayGeneric(0,0,1),
                                    () => ColorArrayGeneric(1,0,0),
                                    () => ColorArrayGeneric(1,0,0.5),
                                    () => ColorArrayGeneric(0,1.0,0.5),
                                    () => ColorArrayGeneric(0.5,1,0),
                                    () => ColorArrayGeneric(0,1,0.5),
                                    () => ColorArrayGeneric(0.5,0,1),
                                    () => ColorArrayGeneric(0,0.5,1),
                                };
            InitNextSettings();
        }

        public BitmapSource Source
        {
            get { return _bitmap; }
        }

         private void ColorArrayHSVGeneric(double startH, double endH, double s, double v)
        {
            _colors = new List<Color>();
            double increment = (endH - startH)/500;
            for (int i = 0; i < 500; i++)
                _colors.Add(FromHSV(i*increment + startH, s,v));
        }

        private static Color FromHSV(double h, double s, double v)
        {
            // see http://www.tech-faq.com/hsv.html
            int r, g, b;
            HSVHelper.HsvToRgb(h,s,v, out r, out g, out b);
            return Color.FromArgb(255, (byte) (r), (byte) (g), (byte) (b));
        }

        private void ColorArrayOne()
        {
            _colors = new List<Color>();
            for (int i = 0; i < 128; i++)
                _colors.Add(Color.FromArgb(255, (byte)(255 - (i * 2)), (byte)(255 - (i * 2)), 0));
            for (int i = 0; i < 128; i++)
                _colors.Add(Color.FromArgb(255, (byte)(i * 2), (byte)(i * 2), 0));
        }

        private void ColorArrayTwo()
        {
            _colors = new List<Color>();
            byte red = 0;
            byte green = 0;
            byte blue = 0;

            // red to yellow
            red = 255;
            for (int i = 0; i < 64; i++)
            {
                _colors.Add(Color.FromArgb(255, red, green, blue));
                green = (byte)(green + 4);
            }

            green = 255;
            for (int i = 0; i < 64; i++)
            {
                _colors.Add(Color.FromArgb(255, red, green, blue));
                red = (byte)(red - 4);
            }
            red = 0;

            green = 255;
            for (int i = 0; i < 64; i++)
            {
                _colors.Add(Color.FromArgb(255, red, green, blue));
                blue = (byte)(blue + 4);
            }

            blue = 255;
            for (int i = 0; i < 64; i++)
            {
                _colors.Add(Color.FromArgb(255, red, green, blue));
                green = (byte)(green - 4);
            }
        }

        private void ColorArrayThree()
        {
            _colors = new List<Color>();
            byte red = 0;
            byte green = 0;
            byte blue = 0;

            red = 0;
            for (int i = 0; i < 64; i++)
            {
                _colors.Add(Color.FromArgb(255, red, green, blue));
                red = (byte)(red + 4);
            }

            red = 255;
            for (int i = 0; i < 64; i++)
            {
                _colors.Add(Color.FromArgb(255, red, green, blue));
                blue = (byte)(blue + 4);
            }
            red = 255;
            blue = 255;

            for (int i = 0; i < 64; i++)
            {
                _colors.Add(Color.FromArgb(255, red, green, blue));
                blue = (byte)(blue - 4);
                green = (byte)(green + 4);
            }
            green = 255;
            blue = 0;

            for (int i = 0; i < 64; i++)
            {
                _colors.Add(Color.FromArgb(255, red, green, blue));
                blue = (byte)(blue + 4);
            }
        }

        private void ColorArrayFour()
        {
            _colors = new List<Color>();
            for (int i = 0; i < 128; i++)
                _colors.Add(Color.FromArgb(255, (byte)(255 - (i * 2)), 0, (byte)(255 - (i * 2))));
            for (int i = 0; i < 128; i++)
                _colors.Add(Color.FromArgb(255, (byte)(i * 2), 0, (byte)(i * 2)));
        }

        private void ColorArrayFive()
        {
            _colors = new List<Color>();
            for (int i = 0; i < 128; i++)
                _colors.Add(Color.FromArgb(255, 0, (byte) (255 - (i*2)), (byte) (255 - (i*2))));
            for (int i = 0; i < 128; i++)
                _colors.Add(Color.FromArgb(255, 0, (byte) (i*2), (byte) (i*2)));
        }

        private void ColorArraySix()
        {
            _colors = new List<Color>();
            for (int i = 0; i < 128; i++)
                _colors.Add(Color.FromArgb(255, (byte)(255 - (i * 2)), (byte)(255 - (i * 2)), (byte)(255 - (i * 2))));
            for (int i = 0; i < 128; i++)
                _colors.Add(Color.FromArgb(255, (byte)(i * 2), (byte)(i * 2), (byte)(i * 2)));
        }

        private void ColorArrayGeneric(double red, double green, double blue)
        {
            _colors = new List<Color>();
            for (int i = 0; i < 128; i++)
                _colors.Add(Color.FromArgb(255, (byte)(red * (255 - (i * 2))), (byte)(green * (255 - (i * 2))), (byte)(blue * (255 - (i * 2)))));
            for (int i = 0; i < 128; i++)
                _colors.Add(Color.FromArgb(255, (byte)(red * i * 2), (byte)(green * i * 2), (byte)(blue * i * 2)));
        }

        private void NextColorArray()
        {
            var which = _random.Next(_colorActions.Count);
            _colorActions[which]();

            var reverse = _random.Next(2) == 1;
            if (reverse)
                _colors.Reverse();
        }

        private double[][] _boundaries = new double[][]
                                             {
                                                 new[] {-2.1, -1.0, -1.3, 1.3},
                                                 new[] {-1.8, 0.7, -1, 1},
                                                 new[]
                                                     {
                                                         -1.30989899,
                                                         -1.19878788,
                                                         -0.40390572,
                                                         -0.26922559
                                                     },

                                                 new[]
                                                     {
                                                         0.28689
                                                         , 0.28694
                                                         , 0.0142
                                                         , 0.0143
                                                     },
                                                 new []
                                                     {
                                                         0.382116699
                                                         , 0.402429199
                                                         , 0.261474609
                                                         , 0.283349609
                                                     },
                                                 new []
                                                     {
                                                         -5.0
                                                         , 2
                                                         , -4
                                                         , 4
                                                     },
                                                 new []
                                                     {
                                                         -1.291601
                                                         , -1.129101
                                                         , -0.4046875
                                                         , -0.2296875
                                                     },
                                                 new []
                                                     {
                                                         -1.195434570
                                                         , -1.175122070
                                                         , -0.31257324
                                                         , -0.29069824
                                                     },
                                                 new []
                                                     {
                                                         -1.185322952
                                                         , -1.184688186
                                                         , -0.3001724243
                                                         , -0.299488830
                                                     },
                                                 new []
                                                     {
                                                         -1.185040283
                                                         , -1.185000610
                                                         , -0.299955797
                                                         , -0.299913072
                                                     },
                                                     new []
                                                         {
                                                             0.100756835,
                                                             0.182006835,
                                                             0.616259765,
                                                             0.703759765,
                                                         },
                                                         new double[]
                                                             {
                                                                 0.103216552,
                                                                 0.113372802,
                                                                 0.632409667,
                                                                 0.643347167,
                                                             }, 
                                                             new double[]
                                                                 {
                                                                     0.10804672,
                                                                     0.10931625,
                                                                     0.634268188,
                                                                     0.635635375
                                                                 }, 
                                             };

        private void NextSettingsSelection()
        {
            var which = _random.Next(_boundaries.GetLength(0));
            var boundary = _boundaries[which];
            _xmin = boundary[0];
            _xmax = boundary[1];
            _ymin = boundary[2];
            _ymax = boundary[3];
        }

        private void InitNextSettings()
        {
            NextColorArray();
            NextSettingsSelection();

            SCALE_DOWN = 64;
            SCALE_UP = 64;
            FINISH_SCALE = 1;

            //text_settings.Text = "x in %.4f,%.4f; y in %.4f,%.4f" % [$xmin,$xmax,$ymin,$ymax]

            NextLevel();
        }

        private void NextSettings()
        {
            InitNextSettings();
        }

        private void NextLevel()
        {
            if (SCALE_UP <= FINISH_SCALE)
            {
                NextSettings();
                return;
            }

            SCALE_DOWN = SCALE_DOWN/8;
            SCALE_UP = SCALE_UP/8;

            WIDTH = BASE_WIDTH/SCALE_DOWN;
            HEIGHT = BASE_HEIGHT/SCALE_DOWN;

            _xIncrement = (_xmax - _xmin)/WIDTH;
            _yIncrement = (_ymax - _ymin)/HEIGHT;

            _x = _xmin;
            _s = 0;
        }

        private void SetPixel(double x, double y, Color color)
        {
            /*
            // very inefficient... add a small rectangle!
            var rec = new Rectangle();
            rec.Margin = new Thickness(x * SCALE_UP, y * SCALE_UP, 0, 0);
            rec.Width = SCALE_UP;
            rec.Height = SCALE_UP;
            rec.Fill = new SolidColorBrush(color);
            canvas.Children.Add(rec);
            */
            _bitmap.FillRectangle((int) (x*SCALE_UP), (int) (y*SCALE_UP), (int) ((x + 1)*SCALE_UP),
                                  (int) ((y + 1)*SCALE_UP), color);
        }

        public void NextLine()
        {
            if (_s >= WIDTH)
            {
                NextLevel();
                return;
            }

            double y = _ymin;

            for (int z = 0; z < HEIGHT; z++)
            {
                double x1 = 0.0;
                double y1 = 0.0;
                double looper = 0.0;
                while (looper < 100 && x1*x1 + y1*y1 < 4)
                {
                    looper = looper + 1.0;
                    double xx = (x1*x1) - (y1*y1) + _x;
                    y1 = 2*x1*y1 + y;
                    x1 = xx;
                }

                // Get the percent of where the looper stopped
                double perc = looper/(100.0);

                // Get that part of a 255 scale
                var val = (int) Math.Floor(perc*255);

                // Use that number to set the color
                SetPixel(_s, z, _colors[val]);
                y = _yIncrement + y;
            }
            _x = _x + _xIncrement;
            _s = _s + 1;
        }
    }
}
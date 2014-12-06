using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Asteroids_Rebirth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    interface IDrawing
    {
        Canvas canvas { get; set; }
        void draw();
    }
    public partial class MainWindow : Window
    {
       // Spaceship spaceship;
        Game game;
        bool FoolScreen = false;
        public MainWindow()
        {

            InitializeComponent();


        }

        private void Keys(object sender, KeyEventArgs e)
        {

           
            game.control();

            //Activates foolscreen mod
            if (Keyboard.IsKeyDown(Key.Enter) && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                if (FoolScreen == false)
                {
                    this.WindowStyle = WindowStyle.None;
                    this.WindowState = WindowState.Maximized;

                }
                else
                {
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowState = WindowState.Normal;
                }
                FoolScreen = !FoolScreen;
            }
          
        }

        private void Scailing(object sender, double ScaleWidth, double ScaleHeight)
        {

                Canvas c = sender as Canvas;
                ScaleTransform st = new ScaleTransform();
                c.RenderTransform = st;
                    st.ScaleX *= ScaleWidth;
                    st.ScaleY *= ScaleHeight;
                    return;

        }

        private void test(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(canvas.ActualWidth + " " + myViewBox.ActualWidth + " " + this.Width + " " + this.ActualHeight);
            MessageBox.Show(Math.Sin(Math.PI * (-34) / 180.0).ToString());
        }

        private void sizechangerefresh(object sender, SizeChangedEventArgs e)
        {
            //Scailing(canvas, this.ActualWidth / 497.0, this.Height / 320.0);
        }

        private void construct(object sender, RoutedEventArgs e)
        {
            myViewBox.StretchDirection = StretchDirection.Both;
            myViewBox.Stretch = Stretch.Fill;
           game = new Game(canvas);
            game.InitGame();
            game.Loop();
        }

        private void mouseclick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(e.GetPosition(canvas).X.ToString()+" "+ e.GetPosition(canvas).Y.ToString());
        }

    }
}

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
        Spaceship spaceship;
        bool FoolScreen = false;
        public MainWindow()
        {

            InitializeComponent();


        }

        private void Keys(object sender, KeyEventArgs e)
        {

            if (Keyboard.IsKeyDown(Key.W)){
                spaceship.moveUp();

            }
            if (Keyboard.IsKeyDown(Key.S))
            {
                spaceship.moveDown();
            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                spaceship.angle -= 3;

            }
            if (Keyboard.IsKeyDown(Key.D))
            {
                spaceship.angle += 3;

            }
            spaceship.draw();

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
            spaceship = new Spaceship(canvas, 20, 10);

        }
    }
}

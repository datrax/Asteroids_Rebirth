using System;
using System.Collections.Generic;
using System.Drawing;
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
        public bool InGame { get; set; }
        public MainWindow()
        {

            InitializeComponent();
            InGame = false;
            myViewBox.Visibility = Visibility.Hidden;
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
               new Uri(Environment.CurrentDirectory + @"\menu.png"));
            myBrush.ImageSource = image.Source;
            menu.Background = myBrush;

            myBrush = new ImageBrush();
            image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
               new Uri(Environment.CurrentDirectory + @"\back.jpg"));
            myBrush.ImageSource = image.Source;
            grid.Background = myBrush;

        }

        private void Keys(object sender, KeyEventArgs e)
        {
            if(InGame){
            if (Keyboard.IsKeyDown(Key.Escape))
            {
                game.Pause();
                if (MessageBox.Show("Would you like to quit?", "Pause", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    menu.Visibility = Visibility.Visible;
                    myViewBox.Visibility = Visibility.Hidden;

                    FoolScreen = false;
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowState = WindowState.Normal;

                    InGame = false;
                }
                else
                {
                    game.Continue();
                }

 
            }
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

        }



        private void mouseclick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine((e.GetPosition(canvas).X - 20).ToString());
            System.Diagnostics.Debug.WriteLine((e.GetPosition(canvas).Y - 10).ToString());
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            InGame = true;

            
            myViewBox.StretchDirection = StretchDirection.Both;
            myViewBox.Stretch = Stretch.Fill;
            game = new Game(canvas,Information,menu,this);
            game.InitGame(0);
            if (MessageBox.Show("Would you like to start game in fullscreen mode?\n" + Environment.NewLine + "Keep in mind :While playing you can always change fullscreen mode by pressing ALT+ENTER ", "Pause", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                FoolScreen = true;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                FoolScreen = false;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
            }
            menu.Visibility = Visibility.Hidden;
            myViewBox.Visibility = Visibility.Visible;
            game.Loop();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

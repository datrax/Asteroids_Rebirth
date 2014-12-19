using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

        int score;
        public MainWindow()
        {

            InitializeComponent();
      
        }

        private void Keys(object sender, KeyEventArgs e)
        {
            if (InGame)
            {
                if (Keyboard.IsKeyDown(Key.Escape))
                {
                    game.Pause();
                    if (MessageBox.Show("Would you like to quit?", "Pause", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        menu.Visibility = Visibility.Visible;
                        gameViewBox.Visibility = Visibility.Hidden;

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
            // System.Diagnostics.Debug.WriteLine((e.GetPosition(canvas).X - 20).ToString());
            // System.Diagnostics.Debug.WriteLine((e.GetPosition(canvas).Y - 10).ToString());

        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            InGame = true;


            gameViewBox.StretchDirection = StretchDirection.Both;
            gameViewBox.Stretch = Stretch.Fill;
            game = new Game(canvas,  this);
            game.InitGame(0);
            if (MessageBox.Show("Would you like to start game in fullscreen mode?\n" + Environment.NewLine + "Keep in mind :While playing you can always change fullscreen mode by pressing ALT+ENTER ", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
            gameViewBox.Visibility = Visibility.Visible;
            Information.Visibility = Visibility.Visible;
            game.Loop();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowRecords(object sender, RoutedEventArgs e)
        {

            Records records = new Records();
            records.Owner = this;
            records.Show();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Record.Visibility = Visibility.Hidden;
            menu.Visibility = Visibility.Visible;
        }

        private void AddRecord(object sender, RoutedEventArgs e)
        {

            BinaryFormatter formatter = new BinaryFormatter();
            int score = int.Parse(Information.Content.ToString().Substring(Information.Content.ToString().IndexOf(':') + 1, Information.Content.ToString().IndexOf('L') - Information.Content.ToString().IndexOf(':') - 2));
            record obj = new record(nameOfPlayer.Text, score, DateTime.Now);
            using (var fStream = new FileStream("file.dat", FileMode.Append, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(fStream, obj);
            }
            Record.Visibility = Visibility.Hidden;
            menu.Visibility = Visibility.Visible;
        }



        private void NameChanged(object sender, TextChangedEventArgs e)
        {
            nameOfPlayer.Focus();
            nameOfPlayer.SelectionStart = nameOfPlayer.Text.Length;
           
            if (nameOfPlayer.Text.Length > 0)
            {
                if (nameOfPlayer.Text.Length > 15)
                {
                    nameOfPlayer.Text = nameOfPlayer.Text.Substring(0, 15);
                }
                YesButton.IsEnabled = true;
                YesButton.Opacity = 1;
            }
            else
            {
               
                YesButton.IsEnabled = false;
                YesButton.Opacity = 0.3;
            }
        }

        private void SetDefault(object sender, RoutedEventArgs e)
        {
            InGame = false;
            gameViewBox.Visibility = Visibility.Hidden;
            Record.Visibility = Visibility.Hidden;
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
            canvas.Background = myBrush;

            myBrush = new ImageBrush();
            image.Source = new BitmapImage(
              new Uri(Environment.CurrentDirectory + @"\rec_back.jpg"));
            myBrush.ImageSource = image.Source;
            Record.Background = myBrush;
            YesButton.IsEnabled = false;
            YesButton.Opacity = 0.3;
        }
    }
}
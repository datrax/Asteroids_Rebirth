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
using System.Windows.Threading;

namespace Asteroids_Rebirth
{
    public partial class MainWindow : Window
    {

        public class Game
        {
            DispatcherTimer dispatcherTimer;
            Spaceship spaceship;
            List<Asteroids> asteroids;
            Label information;
            Grid menu;
            MainWindow window;
            int score;
            int wave;
            private Canvas canvas;

            public Game(Canvas canvas, Label information, Grid menu, MainWindow window)
            {
                this.window = window;
                this.menu = menu;
                this.information = information;
                this.canvas = canvas;
                wave = 0;
                score = 0;
            }


            public void InitGame(int wave)
            {
                canvas.Children.Clear();
                asteroids = new List<Asteroids>();

                asteroids.Add(new Asteroids(canvas, 0, 0, 20, 2, wave));
                asteroids.Add(new Asteroids(canvas, 50, 10, 20, 1, wave));
                asteroids.Add(new Asteroids(canvas, 50, 40, 20, 2, wave));
                asteroids.Add(new Asteroids(canvas, 0, 40, 20, 1, wave));
                spaceship = new Spaceship(canvas, 38, 27);
            }

            public void Loop()
            {
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 16);
                dispatcherTimer.Start();
            }
            public void Pause()
            {
                dispatcherTimer.Stop();

            }
            public void Continue()
            {
                dispatcherTimer.Start();
            }

            private void dispatcherTimer_Tick(object sender, EventArgs e)
            {
                //   if (spaceship != null)
                information.Content = "Score: " + score.ToString() + " Lives: " + spaceship.lives + " Wave: " + wave;
                if (spaceship.lives == 0)
                {
                    this.Pause();
                    menu.Visibility = Visibility.Visible;
                    window.myViewBox.Visibility = Visibility.Hidden;
                    window.WindowStyle = WindowStyle.SingleBorderWindow;
                    window.WindowState = WindowState.Normal;
                }
                spaceship.physics();
                if (asteroids.Count == 0)
                {
                    MessageBox.Show("You've sucessfully completed " + wave.ToString() + " wave Accept you're ready for the next one");
                    canvas.Children.Clear();
                    spaceship.lives = 3;
                    InitGame(++wave);
                }
                for (int i = 0; i < asteroids.Count; i++)
                    asteroids[i].physics();
                if (spaceship.alive && !spaceship.transparent)
                {
                    int hitAsteroid = colision(spaceship.colision.colisionpoints.ToArray(), asteroids);
                    if (hitAsteroid >= 0)
                    {
                        spaceship.destroy();

                        split(hitAsteroid);

                    };

                }
                if (spaceship.laserIsEnabled)
                {
                    int hitAsteroid = colision(new Point[] { new Point(spaceship.laser.X1, spaceship.laser.Y1), new Point(spaceship.laser.X2, spaceship.laser.Y2) }, asteroids);
                    if (hitAsteroid >= 0)
                    {
                        split(hitAsteroid);
                        score += 10 * (wave + 1);
                    };
                }
                spaceship.draw();
                for (int i = 0; i < asteroids.Count; i++)
                    asteroids[i].draw();





            }
            private void split(int k)
            {
                double x = asteroids[k].positionX;
                double y = asteroids[k].positionY;
                int color = asteroids[k].color;
                if (asteroids[k].size == 20)
                {
                    asteroids.Add(new Asteroids(canvas, x, y, 7, color, wave));
                    asteroids.Add(new Asteroids(canvas, x, y, 7, color, wave));
                    asteroids.Add(new Asteroids(canvas, x, y, 7, color, wave));
                }
                canvas.Children.Remove(asteroids[k].Sprite);
                asteroids.RemoveAt(k);
            }
            private int colision(Point[] shipVertexes, List<Asteroids> asteroids)
            {
                for (int asteroidNumber = 0; asteroidNumber < asteroids.Count; asteroidNumber++)
                {
                    Point[] asteroidVertexes = asteroids[asteroidNumber].colision.colisionpoints.ToArray();
                    for (int i = 0; i < shipVertexes.Length - 1; i++)
                    {
                        for (int j = 0; j < asteroidVertexes.Length - 1; j++)
                        {
                            double ax1 = shipVertexes[i].X;
                            double ay1 = shipVertexes[i].Y;
                            double ax2 = shipVertexes[i + 1].X;
                            double ay2 = shipVertexes[i + 1].Y;
                            double bx1 = asteroidVertexes[j].X;
                            double by1 = asteroidVertexes[j].Y;
                            double bx2 = asteroidVertexes[j + 1].X;
                            double by2 = asteroidVertexes[j + 1].Y;
                            double v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
                            double v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
                            double v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
                            double v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
                            if ((v1 * v2 < 0) && (v3 * v4 < 0)) return asteroidNumber;
                        }
                    }
                }
                return -1;
            }
        }
    }
}

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
          protected  DispatcherTimer loopTimer;
          protected Spaceship spaceship;
          protected List<Asteroids> asteroids;
          protected Label scoreInformation;
          protected MainWindow window;
          protected int score;
          protected int wave;
          protected   Canvas canvas;

            public Game(Canvas canvas, MainWindow window)
            {
                this.window = window;
                this.scoreInformation = window.Information;
                this.canvas = canvas;
                wave = 0;
                score = 0;
            }


            public virtual void InitGame(int wave)
            {
                canvas.Children.Clear();
                asteroids = new List<Asteroids>();      
                asteroids.Add(new Asteroids(canvas, 0, 0, 20, 2, wave));
                asteroids.Add(new Asteroids(canvas, 50, 10, 20, 1, wave));
                asteroids.Add(new Asteroids(canvas, 50, 40, 20, 2, wave));
                asteroids.Add(new Asteroids(canvas, 0, 40, 20, 1, wave));
                spaceship = new Spaceship(canvas, 38, 27,10);
            }

            public virtual void Loop()
            {
                loopTimer = new DispatcherTimer();
                loopTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                loopTimer.Interval = new TimeSpan(0, 0, 0, 0, 16);
                loopTimer.Start();
               
            }
            public virtual void Pause()
            {
                loopTimer.Stop();

            }
            public virtual void Continue()
            {
                loopTimer.Start();
            }

           protected virtual void dispatcherTimer_Tick(object sender, EventArgs e)
            {
                scoreInformation.Content = "Score: " + score.ToString() + " Lives: " + spaceship.lives + " Wave: " + wave;
                if (spaceship.lives == 0)
                {
                    this.Pause();
                    window.Record.Visibility = Visibility.Visible;
                    window.ScoreText.Content = "Your Score: " + score.ToString();
                    window.nameOfPlayer.Focus();
                    window.Information.Visibility = Visibility.Hidden;
                    window.gameViewBox.Visibility = Visibility.Hidden;
                    window.WindowStyle = WindowStyle.SingleBorderWindow;
                    window.WindowState = WindowState.Normal;
                }
                spaceship.physics();
                if (asteroids.Count == 0)
                {              
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
            protected virtual void split(int k)
            {
                double x = asteroids[k].positionX;
                double y = asteroids[k].positionY;
                if (asteroids[k].size <= 20&&asteroids[k].size>=6)
                {
                    AddSmallAsteroids(x,y,asteroids[k].size, asteroids[k].color);
                }
                canvas.Children.Remove(asteroids[k].Sprite);
                asteroids.RemoveAt(k);
            }

            protected virtual  void AddSmallAsteroids(double posx,double posy,int size, int color)
            {
                Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                 int amount ;
                if(size==20)
                 amount = random.Next(2, 5);
                else
                    amount = random.Next(2, 3);
                for (int i = 0; i < amount; i++)
                {
                    int newsize=size/amount;
                    newsize += random.Next(0, 1);
                    System.Threading.Thread.Sleep(1);
                    asteroids.Add(new Asteroids(canvas, posx, posy, newsize, color, wave));
                }
            }
            public int colision(Point[] shipVertexes, List<Asteroids> asteroids)
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

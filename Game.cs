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
            Spaceship spaceship;
            List<Asteroids> asteroids;
            private Canvas canvas;

            public Game(Canvas canvas)
            {
                this.canvas = canvas;

            }
            public void InitGame()
            {


  
                
                asteroids = new List<Asteroids>();
              
                asteroids.Add(new Asteroids(canvas, 10, 10, 20, 2));
                asteroids.Add(new Asteroids(canvas, 80, 90, 20, 1));
                asteroids.Add(new Asteroids(canvas, 45, 10, 20, 2));
                asteroids.Add(new Asteroids(canvas, 10, 60,20, 1));
                spaceship = new Spaceship(canvas, 38, 27);


            }

            public void Loop()
            {
                DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 16);
                dispatcherTimer.Start();
            }
            private void dispatcherTimer_Tick(object sender, EventArgs e)
            {
                if (spaceship != null)
                {
                    spaceship.physics();
                    for (int i = 0; i < asteroids.Count;i++ )
                        asteroids[i].physics();
                    if (spaceship.alive)
                    {
                        int k=colision(spaceship, asteroids);
                        if ( k>=0)
                        {
                            spaceship.explose();
                            split(k);

                        };
                    }

                    spaceship.draw();
                    for (int i = 0; i < asteroids.Count;i++ )
                        asteroids[i].draw();
                
                    
                }

                    
            }
            private void split(int k)
            {
                double x = asteroids[k].positionX;
                double y = asteroids[k].positionY;
                int color = asteroids[k].color;
                if (asteroids[k].size == 20)
                {
                    asteroids.Add(new Asteroids(canvas, x, y, 7, color));
                    asteroids.Add(new Asteroids(canvas, x, y, 7, color));
                    asteroids.Add(new Asteroids(canvas, x, y, 7, color));
                }
                canvas.Children.Remove(asteroids[k].Sprite);
                asteroids.RemoveAt(k);
            }
            private int colision(Spaceship ship,List<Asteroids>asteroids)
            {
                Point[]spoints=ship.colision.colisionpoints.ToArray();
                for (int t = 0; t < asteroids.Count; t++)
                {
                    Point[] apoints = asteroids[t].colision.colisionpoints.ToArray();
                    for (int i = 0; i < spoints.Length - 1; i++)
                    {
                        for (int j = 0; j < apoints.Length - 1; j++)
                        {
                            double ax1 = spoints[i].X;
                            double ay1 = spoints[i].Y;
                            double ax2 = spoints[i + 1].X;
                            double ay2 = spoints[i + 1].Y;
                            double bx1 = apoints[j].X;
                            double by1 = apoints[j].Y;
                            double bx2 = apoints[j + 1].X;
                            double by2 = apoints[j + 1].Y;
                            double v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
                            double v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
                            double v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
                            double v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
                            if ((v1 * v2 < 0) && (v3 * v4 < 0)) return t;
                        }
                    }
                }
                return -1;
            }
        }
    }
}

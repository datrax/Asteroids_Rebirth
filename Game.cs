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
            private Canvas canvas;

            public Game(Canvas canvas)
            {
                this.canvas = canvas;
            }
            public void InitGame(){

               spaceship = new Spaceship(canvas, 20, 10);
            }
            public void control()
            {
     
          

                
              
            }
            public void Loop()
            {
                DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0,0,16);
                dispatcherTimer.Start();
            }
            private void dispatcherTimer_Tick(object sender, EventArgs e)
            {
                spaceship.physics();
               spaceship.draw();
            }
        }
    }
}

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
using System.Drawing;
namespace Asteroids_Rebirth
{
    class Spaceship : IDrawing
    {
        public int lives { get; set; }
        //  private bool vectorgraphic = false;
        public double positionX { get; set; }
        public double positionY { get; set; }
        public double speedX { get; set; }
        public double speedY { get; set; }

        public bool laserIsEnabled{ get; set; }
        public Line laser { get; set; }

        private DateTime laserTime { get; set; }
        public double centerY { get; set; }
        public double centerX { get; set; }
        public double Angle { get; set; }
        public double THRUST_MAGNUTIDE { get; set; }
        public double FRICTION_MAGNITUDE { get; set; }
        public Colision colision;
        public bool alive { get; set; }
        public bool transparent { get; set; }
        public int size;
        public Canvas canvas { get; set; }
        SpriteAnimator explosion;
        SpriteAnimator thrusters;
        System.Windows.Controls.Image Sprite;

        void LoadPicture(double height)
        {

            Sprite = new System.Windows.Controls.Image
             {

                 Height = height,
                 Width = 159 / 315.0 * height,
             };
        }
        private void CreateAPolygon(double size)
        {
            double scalesize = size / 12.0 * 20.0;
            // Add Polygon to the page
            colision = new Colision(centerX, centerY, positionX, positionY,scalesize, Environment.CurrentDirectory + @"\shipvertexes.txt");

            // canvas.Children.Add(colision.Polygon);

        }
        public Spaceship(Canvas canvas, double x, double y,double size)
        {
            livingtimer = new System.Windows.Threading.DispatcherTimer();
            laserTime = DateTime.Now;
            laser = new Line(){
               Stroke = System.Windows.Media.Brushes.IndianRed,
               StrokeThickness=0.1
            };
            
            lives = 3;
            THRUST_MAGNUTIDE = 0.018;
            FRICTION_MAGNITUDE = 0.015;
            this.canvas = canvas;
            this.positionX = x;
            this.positionY = y;
            Angle = 0;
            LoadPicture(size);
            canvas.Children.Add(Sprite);
            canvas.Children.Add(laser);
            canvas.Children.Add(new Line());
            thrusters = new SpriteAnimator(Environment.CurrentDirectory + @"\sprite.png", 159, 315, 1, 3, Sprite);
            explosion = new SpriteAnimator(Environment.CurrentDirectory + @"\spritesheet1.png", 100, 100, 9, 9, Sprite);
            alive = true;
            transparent = true;
            centerX = Sprite.Width / 2.0;
            centerY = Sprite.Height / 2.0;
            CreateAPolygon(size);
            transparent = true;
            livingtimer.Interval = new TimeSpan(0, 0, 0, 5);
            livingtimer.Tick += new EventHandler(stoptransparent);
            livingtimer.Start();
            draw();

        }
        public void draw()
        {
            Random random=new Random();
            if (transparent)
                Sprite.Opacity =random.Next(4,9)/10.0;
            else
                Sprite.Opacity = 1;
             
            Canvas.SetTop(Sprite, positionY);
            Canvas.SetLeft(Sprite, positionX);
            RotateTransform rotateTransform1 =
             new RotateTransform(Angle);
            rotateTransform1.CenterX = centerX;
            rotateTransform1.CenterY = centerY;
            Sprite.RenderTransform = rotateTransform1;
            if (laserIsEnabled)
                laser.Opacity = 1;
            else
                laser.Opacity = 0;

        }
        public void physics()
        {

            double directionX = Math.Sin(Math.PI * Angle / 180.0);
            double directionY = -Math.Cos(Math.PI * Angle / 180.0);
            double ax = 0, ay = 0;

            if (alive)
            {


                
                if (Keyboard.IsKeyDown(Key.W))
                {
                    ax += THRUST_MAGNUTIDE * directionX;
                    ay += THRUST_MAGNUTIDE * directionY;
                    thrusters.Animation(40, true, 0);
                }
                else
                {
                    thrusters.stop();
                }
                if (Keyboard.IsKeyDown(Key.A))
                {

                    Angle -= 8;
                    if (Angle < 360) Angle += 360;
                    colision.rotate(Math.PI * (-8) / 180.0);
                }
                if (Keyboard.IsKeyDown(Key.D))
                {
                    Angle += 8;
                    if (Angle > 360) Angle -= 360;
                    colision.rotate(Math.PI * (8) / 180.0);
                }

                if (Keyboard.IsKeyDown(Key.Space) && (DateTime.Now - laserTime).Milliseconds > 200)
                {
                    laserTime = DateTime.Now;
                    laserIsEnabled = true;
                    laser.X1 = this.colision.colisionpoints[5].X;
                    laser.Y1 = this.colision.colisionpoints[5].Y;
                    laser.X2 = this.colision.colisionpoints[5].X + directionX * 500;
                    laser.Y2 = this.colision.colisionpoints[5].Y + directionY * 500;
                }
                else
                    laserIsEnabled = false;

            }

            ax -= FRICTION_MAGNITUDE * speedX;
            ay -= FRICTION_MAGNITUDE * speedY;

            speedX += ax;
            speedY += ay;

            positionX += speedX;
            positionY += speedY;

            colision.move(speedX, speedY);

            if (positionX >= canvas.Width + 10)
            {
                positionX -= (canvas.Width + 10 + 5);
                colision.move(-(canvas.Width + 10 + 5), 0);
            }
            else
                if (positionX <= -10)
                {
                    positionX += (canvas.Width + 10);
                    colision.move((canvas.Width + 10), 0);
                }

            if (positionY >= canvas.Height + 10)
            {
                positionY -= (canvas.Height + 10 + 7);
                colision.move(0, -(canvas.Height + 10 + 7));
            }
            else
                if (positionY <= -10)
                {
                    positionY += (canvas.Height + 10);
                    colision.move(0, (canvas.Height + 10));
                }

        }
        System.Windows.Threading.DispatcherTimer livingtimer;


        public void destroy()
        {
            laserIsEnabled = false;
            thrusters.stop();
            alive = false;
            Sprite.Source = null;
            Angle = 0;
            explosion.Animation(40, false, 10);
            
            livingtimer.Tick += new EventHandler(rebirth);
            livingtimer.Interval = new TimeSpan(0, 0, 0, 4);
            livingtimer.Start();
        }

                public void rebirth(object sender, EventArgs e)
        {

            alive = true;
            lives -= 1;
            positionX = 38;
            positionY = 27;
            Angle = 0;
            colision = new Colision(centerX, centerY, positionX, positionY,20, Environment.CurrentDirectory + @"\shipvertexes.txt");
            livingtimer.Stop();
            livingtimer.Tick -= new EventHandler(rebirth);
            transparent = true;
            livingtimer.Interval = new TimeSpan(0, 0, 0, 5);
            livingtimer.Tick += new EventHandler(stoptransparent);
            livingtimer.Start();
        }

        
        private void stoptransparent(object sender, EventArgs e)
        {
            

            transparent = false;
            livingtimer.Stop();
            livingtimer.Tick -= new EventHandler(stoptransparent);
        }

    }
}

using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Runtime.Serialization;
namespace Asteroids_Rebirth
{
  [DataContract]
    public class Spaceship : IDrawing
    {
        public int lives { get; set; }
        [DataMember]
        public double positionX { get; set; }
        [DataMember]
        public double positionY { get; set; }
        public double speedX { get; set; }
        public double speedY { get; set; }

        public bool laserIsEnabled { get; set; }
        public Line laser { get; set; }

        private DateTime laserTime { get; set; }
        public double centerY { get; set; }
        public double centerX { get; set; }
        [DataMember]
        public double Angle { get; set; }
        public const double speedAngle = 6;
        private const double THRUST_MAGNUTIDE = 0.018;
        private const double FRICTION_MAGNITUDE = 0.015;
        public Colision colision;
        public bool alive { get; set; }
        public bool transparent { get; set; }
        public double size { get; set; }
        public Canvas canvas { get; set; }
        SpriteAnimator explosion { get; set; }
        SpriteAnimator thrusters { get; set; }
        public System.Windows.Controls.Image Sprite { get; set; }

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
            colision = new Colision(centerX, centerY, positionX, positionY, scalesize, Environment.CurrentDirectory + @"\Resources\Vertexes\ship.txt");

               // canvas.Children.Add(colision.Polygon);

        }
        public Spaceship(Canvas canvas, double x, double y, double size)
        {
            livingtimer = new System.Windows.Threading.DispatcherTimer();
            laserTime = DateTime.Now;
            laser = new Line()
            {
                Stroke = System.Windows.Media.Brushes.IndianRed,
                StrokeThickness = 0.1
            };

            lives = 3;

            this.canvas = canvas;
            this.positionX = x;
            this.positionY = y;
            Angle = 0;
            this.size = size;
            LoadPicture(size);
            canvas.Children.Add(Sprite);
            canvas.Children.Add(laser);
            canvas.Children.Add(new Line());
            thrusters = new SpriteAnimator(Environment.CurrentDirectory + @"\Resources\Textures\ship.png", 159, 315, 1, 3, Sprite);
            explosion = new SpriteAnimator(Environment.CurrentDirectory + @"\Resources\Textures\fire.png", 100, 100, 9, 9, Sprite);
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
            Random random = new Random();
            if (transparent)
                Sprite.Opacity = random.Next(4, 9) / 10.0;
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

                if (Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Up))
                {
                    ax += THRUST_MAGNUTIDE * directionX;
                    ay += THRUST_MAGNUTIDE * directionY;
                    thrusters.Animation(40, true, 0);
                }
                else
                {
                    thrusters.stop();
                }

                ax -= FRICTION_MAGNITUDE * speedX;
                ay -= FRICTION_MAGNITUDE * speedY;

                speedX += ax;
                speedY += ay;

                positionX += speedX;
                positionY += speedY;
                colision.move(speedX, speedY);
                if (Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left))
                {
                    Angle -= speedAngle;
                    if (Angle < 360) Angle += 360;
                    colision.rotate(Math.PI * (-speedAngle) / 180.0);
                }
                if (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right))
                {
                    Angle += speedAngle;
                    if (Angle > 360) Angle -= 360;
                    colision.rotate(Math.PI * (speedAngle) / 180.0);
                }
                if (Keyboard.IsKeyDown(Key.Space) && (DateTime.Now - laserTime).Milliseconds > 200)
                {
                    laserTime = DateTime.Now;
                    laserIsEnabled = true;
                    laser.X1 = this.colision.colisionpoints[5].X;
                    laser.Y1 = this.colision.colisionpoints[5].Y;
                    laser.X2 = this.colision.colisionpoints[5].X + directionX * 100;
                    laser.Y2 = this.colision.colisionpoints[5].Y + directionY * 100;
                }
                else
                    laserIsEnabled = false;

            }

            if (colision.centerX - size / 2.0 >= canvas.Width + 3)
            {
                positionX -= (canvas.Width + size + 3);
                colision.move(-(canvas.Width + size + 3), 0);
            }
            else
                if (colision.centerX + size / 2 <= -3)
                {
                    positionX += (canvas.Width + size);
                    colision.move((canvas.Width + size), 0);
                }

            if (colision.centerY - size / 2.0 >= canvas.Height + 3)
            {
                positionY -= (canvas.Height + size + 3);
                colision.move(0, -(canvas.Height + size + 3));
            }
            else
                if (colision.centerY + size / 2 <= -3)
                {
                    positionY += (canvas.Height + size);
                    colision.move(0, (canvas.Height + size));
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
            speedX = 0;
            speedY = 0;
            alive = true;
            lives -= 1;
            positionX = 38;
            positionY = 27;
            Angle = 0;
            double scalesize = size / 12.0 * 20.0;
            colision = new Colision(centerX, centerY, positionX, positionY, scalesize, Environment.CurrentDirectory + @"\Resources\Vertexes\ship.txt");
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

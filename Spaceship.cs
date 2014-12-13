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
      //  private bool vectorgraphic = false;
        public double positionX { get; set; }
        public double positionY { get; set; }
        public double speedX { get; set; }
        public double speedY { get; set; }

        public double centerY { get; set; }
        public double centerX { get; set; }
        public double Angle { get; set; }
        public double THRUST_MAGNUTIDE { get; set; }
        public double FRICTION_MAGNITUDE { get; set; }
        public Colision colision;
        public bool alive { get; set; }
        public Canvas canvas { get; set; }
        SpriteAnimator explosion;
        SpriteAnimator thrusters;
        System.Windows.Controls.Image Sprite;

        void LoadPicture(int height)
        {

            Sprite = new System.Windows.Controls.Image
             {

                 Height = height,
                 Width = 159 / 315.0 * height,
             };
        }
        private void CreateAPolygon()
        {
            
            // Add Polygon to the page
            colision = new Colision(centerX,centerY,positionX,positionY,"shipvertexes.txt");
           
           // canvas.Children.Add(colision.Polygon);

           

           /* RotateTransform rotateTransform1 = new RotateTransform(90,this.positionX+centerX,this.positionY+centerY);

            yellowPolygon.RenderTransform = rotateTransform1;
            System.Diagnostics.Debug.WriteLine(colision[0]);*/

        }
        public Spaceship(Canvas canvas, double x, double y)
        {

            
            
            THRUST_MAGNUTIDE = 0.018;
            FRICTION_MAGNITUDE = 0.015;
            this.canvas = canvas;
            this.positionX = x;
            this.positionY = y;
            Angle = 0;
            LoadPicture(12);
            canvas.Children.Add(Sprite);
            thrusters = new SpriteAnimator(Environment.CurrentDirectory + @"\sprite.png", 159, 315, 1, 3, Sprite);
            explosion = new SpriteAnimator(Environment.CurrentDirectory + @"\spritesheet1.png", 100, 100, 9, 9, Sprite);
            alive = true;
            centerX = Sprite.Width / 2.0;
            centerY = Sprite.Height / 2.0;
            CreateAPolygon();
            draw();

        }
        public void draw()
        {

            Canvas.SetTop(Sprite, positionY);
            Canvas.SetLeft(Sprite, positionX);
            RotateTransform rotateTransform1 =
             new RotateTransform(Angle);
            rotateTransform1.CenterX = centerX;
            rotateTransform1.CenterY = centerY;
            Sprite.RenderTransform = rotateTransform1;

            
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
                    thrusters.Animation(40,true,0);
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
                if (Keyboard.IsKeyDown(Key.F))
                {
              

                    return;

                }
            }
            if (Keyboard.IsKeyDown(Key.R))
            {
                alive = true;
                positionX = 38;
                positionY = 27;
                Angle = 0;
                colision = new Colision(centerX, centerY, positionX, positionY, "shipvertexes.txt");

            }
            ax -= FRICTION_MAGNITUDE * speedX;
            ay -= FRICTION_MAGNITUDE * speedY;

            speedX += ax;
            speedY += ay;

            positionX += speedX;
            positionY += speedY;

            colision.move(speedX, speedY);

            if (positionX >= canvas.Width + 10){
                positionX -= (canvas.Width + 10 + 5);
                colision.move(-(canvas.Width+10+5), 0);
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
                colision.move(0,-(canvas.Height + 10+7));
            }
            else
                if (positionY <= -10)
                {
                    positionY += (canvas.Height + 10);
                    colision.move(0,(canvas.Height + 10));
                }

        }






        internal void explose()
        {
            thrusters.stop();
            alive = false;
            Sprite.Source = null;
            Angle = 0;
            explosion.Animation(40, false, 10);
        }
    }
}

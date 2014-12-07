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
        public double positionX { get; set; }
        public double positionY { get; set; }
        public double speedX { get; set; }
        public double speedY { get; set; }

        public double Angle { get; set; }
        public double THRUST_MAGNUTIDE { get; set; }
        public double FRICTION_MAGNITUDE { get; set; }
        public bool alive { get; set; }
        public Canvas canvas { get; set; }
        SpriteLoader explosion;
        SpriteLoader thrusters;
        System.Windows.Controls.Image shownSprite;

        void LoadPicture(int height)
        {

            shownSprite = new System.Windows.Controls.Image
             {

                 Height = height,
                 Width = 159 / 315.0 * height,
             };
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
            canvas.Children.Add(shownSprite);
            thrusters = new SpriteLoader(Environment.CurrentDirectory + @"\sprite.png", 159, 315, 1, 3, shownSprite);
            explosion = new SpriteLoader(Environment.CurrentDirectory + @"\spritesheet1.png", 100, 100, 9, 9, shownSprite);
            alive = true;
            draw();
        }
        public void draw()
        {

            Canvas.SetTop(shownSprite, positionY);
            Canvas.SetLeft(shownSprite, positionX);
            RotateTransform rotateTransform1 =
             new RotateTransform(Angle);
            rotateTransform1.CenterX = shownSprite.Width / 2.0;
            rotateTransform1.CenterY = shownSprite.Height / 2.0;
            shownSprite.RenderTransform = rotateTransform1;
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
                    thrusters.loopAnimation(40);
                }
                else
                {
                    thrusters.stop();
                }
                if (Keyboard.IsKeyDown(Key.A))
                {

                    Angle -= 8;

                }
                if (Keyboard.IsKeyDown(Key.D))
                    Angle += 8;
                if (Keyboard.IsKeyDown(Key.F))
                {
                    alive = false;
                    explosion.animation(60);
                    //to save right position of fire
                    shownSprite.Source = null;
                    Angle = 0;

                }
            }
            if (Keyboard.IsKeyDown(Key.R))
            {
                alive = true;

            }
            ax -= FRICTION_MAGNITUDE * speedX;
            ay -= FRICTION_MAGNITUDE * speedY;

            speedX += ax;
            speedY += ay;

            positionX += speedX;
            positionY += speedY;


            if (positionX >= canvas.Width + 10)
                positionX -= (canvas.Width + 10 + 5);
            else
                if (positionX <= -10)
                    positionX += (canvas.Width + 10);

            if (positionY >= canvas.Height + 10)
                positionY -= (canvas.Height + 10 + 7);
            else
                if (positionY <= -10)
                    positionY += (canvas.Height + 10);

        }


    }
}

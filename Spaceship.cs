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

namespace Asteroids_Rebirth
{
    class Spaceship:IDrawing
    {
        public double positionX{get;set;}
        public double positionY{get;set;}
        public double speedX { get; set; }
        public double speedY { get; set; }

        public double currentAngle { get; set; }
        public double THRUST_MAGNUTIDE { get; set; }
        public double FRICTION_MAGNITUDE { get; set; }
      
        public Canvas canvas { get; set; }

        Image image;

        void LoadPicture()
        {
           image = new Image
            {
                Width = 3.53,
                Height = 7,
                //Name = "BodyName",
                Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\F5S4.png")),
            };
        }
        public Spaceship(Canvas canvas,double x,double y)
        {
            THRUST_MAGNUTIDE = 0.018;
            FRICTION_MAGNITUDE = 0.005;
            this.canvas = canvas;
            this.positionX = x;
            this.positionY = y;
            currentAngle = 0;
            LoadPicture();
            canvas.Children.Add(image);
            draw();
        }
        public void draw()
        {
            
            Canvas.SetTop(image, positionY);
            Canvas.SetLeft(image, positionX);
            RotateTransform rotateTransform1 =
             new RotateTransform(currentAngle);
            rotateTransform1.CenterX = 1.76;
            rotateTransform1.CenterY = 3.5;
            image.RenderTransform = rotateTransform1;
        }
        public void physics()
        {

            double directionX = Math.Sin(Math.PI * currentAngle / 180.0);
            double directionY = -Math.Cos(Math.PI * currentAngle / 180.0);
            double ax = 0, ay = 0;
            if(Keyboard.IsKeyDown(Key.W))
            {
                ax += THRUST_MAGNUTIDE * directionX;
                ay += THRUST_MAGNUTIDE * directionY;
            }
            ax -= FRICTION_MAGNITUDE * speedX;
            ay -= FRICTION_MAGNITUDE * speedY;

            speedX += ax;
            speedY += ay;

             positionX += speedX;
             positionY += speedY;

          
             if (positionX >= canvas.Width+10)
                 positionX -= (canvas.Width+10+5);
            else
             if (positionX <= -10)
                 positionX+= (canvas.Width + 10);

             if (positionY >= canvas.Height + 10)
                 positionY -= (canvas.Height + 10 + 7);
             else
                 if (positionY <= -10)
                     positionY += (canvas.Height + 10);
             
        }


    }
}

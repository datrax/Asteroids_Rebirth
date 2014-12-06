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

        public double Angle { get; set; }
        public double THRUST_MAGNUTIDE { get; set; }
        public double FRICTION_MAGNITUDE { get; set; }
      
        public Canvas canvas { get; set; }
        ImageSource[] sprites = new ImageSource[3];
        int framenumber = 0;
        Image image;

        void LoadPicture(int height)
        {
           
           image = new Image
            {
               
                Height = height,
                Width = 159 / 315.0 * height,
                //Name = "BodyName",
                Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\F5S4.png")),
            };
        }
        public Spaceship(Canvas canvas,double x,double y)
        {
            
            THRUST_MAGNUTIDE = 0.018;
            FRICTION_MAGNITUDE = 0.015;
            this.canvas = canvas;
            this.positionX = x;
            this.positionY = y;
            Angle = 0;
           sprites[0] = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\1.png"));
            sprites[1] = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\2.png"));
            sprites[2] = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\3.png"));
            LoadPicture(12);
           // image.Source = sprites[0];
            canvas.Children.Add(image);
            draw();
        }
        public void draw()
        {
            
            Canvas.SetTop(image, positionY);
            Canvas.SetLeft(image, positionX);
            RotateTransform rotateTransform1 =
             new RotateTransform(Angle);
            rotateTransform1.CenterX = image.Width/2.0;
            rotateTransform1.CenterY = image.Height/2.0;
            image.RenderTransform = rotateTransform1;
        }
        public void physics()
        {

            double directionX = Math.Sin(Math.PI * Angle / 180.0);
            double directionY = -Math.Cos(Math.PI * Angle / 180.0);
            double ax = 0, ay = 0;
            if(Keyboard.IsKeyDown(Key.W))
            {
                ax += THRUST_MAGNUTIDE * directionX;
                ay += THRUST_MAGNUTIDE * directionY;
                FireSprite();
            }
            else
            {
                OrdinarSprite();
            }
                  if (Keyboard.IsKeyDown(Key.A))
                {
                    //if (spaceship.oldangle != spaceship.currentAngle)

                    Angle -= 8;

                }
                if (Keyboard.IsKeyDown(Key.D))
                    Angle += 8;
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

        private void OrdinarSprite()
        {
            image.Source = sprites[0];
        }

        private void FireSprite()
        {

            image.Source = sprites[(framenumber++)%2+1];
            
        }


    }
}

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
        public double x{get;set;}
        public double y{get;set;}
        public double speedX { get; set; }
        public double speedY { get; set; }
        public double angle { get; set; }
        public Canvas canvas { get; set; }
        Image image;

        void LoadPicture()
        {
           image = new Image
            {
                Width = 10,
                Height = 10,
                Name = "BodyName",
                Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\F5S4.png")),
            };
        }
        public Spaceship(Canvas canvas,double x,double y)
        {
            this.canvas = canvas;
            this.x = x;
            this.y = y;
            angle = 0;
            LoadPicture();
            canvas.Children.Add(image);
            draw();
        }
        public void draw()
        {
            Canvas.SetTop(image, y);
            Canvas.SetLeft(image, x);
            RotateTransform rotateTransform1 =
             new RotateTransform(angle);
            rotateTransform1.CenterX = 3;
            rotateTransform1.CenterY = 5;
            image.RenderTransform = rotateTransform1;
        }
        public void moveUp()
        {
            x += Math.Sin(Math.PI * angle / 180.0);
            y -= Math.Cos(Math.PI * angle / 180.0);
        }
        public void moveDown()
        {
            x -= Math.Sin(Math.PI * angle / 180.0);
            y += Math.Cos(Math.PI * angle / 180.0);
        }

    }
}

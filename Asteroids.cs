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
     class TextureKeeper
    {
        public SpriteLoader Textures;

        public TextureKeeper(string path)
        {
           Textures=new SpriteLoader(path,320,240,1,3);
        }
    }

    class Asteroids:IDrawing
    {
        static TextureKeeper BigRed = new TextureKeeper(Environment.CurrentDirectory + @"\Big_red.png");
        static TextureKeeper BigGray = new TextureKeeper(Environment.CurrentDirectory + @"\Big_gray.png");
        public double positionX { get; set; }
        public double positionY { get; set; }
        public double speedX { get; set; }
        public double speedY { get; set; }
        public double centerY { get; set; }
        public double centerX { get; set; }
        public double Angle { get; set; }
        public double speedAngle { get; set; }
        public Canvas canvas { get; set; }

        public int size;//1-small,2-medium 3 - big
        public int color;//1-gray 2-red 3-yellow
        public Colision colision;
        public System.Windows.Controls.Image Sprite;

        void LoadPicture(int height)
        {

            Sprite = new System.Windows.Controls.Image
            {

                Height = height,
                Width = 320 / 240.0 * height,
            };
        }
        private void CreateAPolygon(int shape)
        {

            // Add Polygon to the page

            {
                if (shape == 0)
                    colision = new Colision(centerX, centerY, positionX, positionY,size, Environment.CurrentDirectory + @"\big.txt");
                if (shape == 1)
                    colision = new Colision(centerX, centerY, positionX, positionY, size, Environment.CurrentDirectory + @"\big1.txt");
                if (shape == 2)
                    colision = new Colision(centerX, centerY, positionX, positionY, size, Environment.CurrentDirectory + @"\big2.txt");
            }
         //  canvas.Children.Add(colision.Polygon);
        }
        public Asteroids(Canvas canvas, double x, double y,int size,int color,int wave)
        {
            this.canvas = canvas;
            this.positionX = x;
            this.positionY = y;
            this.size = size;
            this.color = color;
            int speed = 0;
            speed = (int)Math.Round(-1 / 9.0 * size + 38 / 9.0);
            speed += wave;
            System.Threading.Thread.Sleep(1);
            Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            speedX = random.Next(-speed, speed) / 10.0;
            System.Threading.Thread.Sleep(1);
            speedY = random.Next(-speed, speed) / 10.0;
            if (speedX == 0) speedX = 0.1;
            if (speedY == 0) speedY = 0.1;
            Angle = 0;
            speedAngle = random.Next(-2, 2);
            LoadPicture(size);
            int shape = random.Next(0, 2);
            if (color == 1)
                Sprite.Source = BigGray.Textures.images[shape];
            if (color == 2)
                Sprite.Source = BigRed.Textures.images[shape];
           /* Sprite.Source = BigRed.Textures.images[2];
            speedX = 0;
            speedY = 0;
            speedAngle = 0;*/
                canvas.Children.Add(Sprite);
                centerX = Sprite.Width / 2.0;
                centerY = Sprite.Height / 2.0;
                CreateAPolygon(shape);
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
            positionX += speedX;
            positionY += speedY;

            colision.move(speedX, speedY);

            Angle += speedAngle;
           colision.rotate(Math.PI * (speedAngle) / 180.0);

            if (colision.centerX-size/2.0 >= canvas.Width+3 )
            {
                positionX -= (canvas.Width + size+3);
                colision.move(-(canvas.Width + size+3), 0);
            }
            else
                if (colision.centerX + size/2 <= -3)
                {
                    positionX += (canvas.Width +size);
                    colision.move((canvas.Width+size), 0);
                }

            if (colision.centerY - size / 2.0 >= canvas.Height + 3)
            {
                positionY -= (canvas.Height + size + 3);
                colision.move(0,-(canvas.Height + size + 3));
            }
            else
                if (colision.centerY + size / 2 <= -3)
                {
                    positionY += (canvas.Height + size);
                    colision.move(0,(canvas.Height + size));
                }
        }
    }
}

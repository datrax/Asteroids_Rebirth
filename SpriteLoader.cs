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
    public class SpriteLoader
    {

        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        int frameNumber = 0;
        List<ImageSource> images;
        System.Windows.Controls.Image img=new System.Windows.Controls.Image();
        public  SpriteLoader(string path, int sizeX, int sizeY, int rows, int columns,System.Windows.Controls.Image img)
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            
            images = new List<ImageSource>();
            Bitmap source = new Bitmap(path);
            this.img = img;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle(j*sizeX, i*sizeY, sizeX, sizeY);
                    Bitmap sourcepart = source.Clone(srcRect, source.PixelFormat);
                    BitmapSource sd = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
           sourcepart.GetHbitmap(),
           IntPtr.Zero,
           System.Windows.Int32Rect.Empty,
           BitmapSizeOptions.FromEmptyOptions());
                    images.Add(sd);
                }
        }
        public  void loopAnimation(int time)
        {

            if (!dispatcherTimer.IsEnabled)
            {
                dispatcherTimer.Tick += new EventHandler(nextframe);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, time);
                dispatcherTimer.Start();
                nextframe(null, null);
            }
        }
        private  void nextframe(object sender, EventArgs e)
        {
            if (frameNumber >=images.LongCount())
                frameNumber = 1;

            img.Source = images[frameNumber++];
           
        }
        public void stop()
        {
            dispatcherTimer.Stop();
            img.Source = images[0];
            frameNumber = 0;
            dispatcherTimer.Tick -= new EventHandler(nextframe);
        }
        public void animation(int time)
        {
            if (!dispatcherTimer.IsEnabled)
            {
               
                dispatcherTimer.Tick += new EventHandler(frame);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, time);
                frameNumber = 10;
                dispatcherTimer.Start();
            }
        }
        private void frame(object sender, EventArgs e)
        {
            if (frameNumber >= images.LongCount())
            {
                dispatcherTimer.Stop();
                frameNumber = 0;
                dispatcherTimer.Tick -= new EventHandler(frame);
                return;
            }
            img.Source = images[frameNumber++];
 
            
        }
    }
}

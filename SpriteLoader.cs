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

       public List<ImageSource> images;


        public SpriteLoader(string path, int sizeX, int sizeY, int rows, int columns)
        {

            images = new List<ImageSource>();
            Bitmap source = new Bitmap(path);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle(j * sizeX, i * sizeY, sizeX, sizeY);
                    Bitmap sourcepart = source.Clone(srcRect, source.PixelFormat);
                    BitmapSource sd = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap( sourcepart.GetHbitmap(), IntPtr.Zero,  System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    images.Add(sd);
                }
        }
    }
   
}

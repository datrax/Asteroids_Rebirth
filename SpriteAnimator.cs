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
    public class SpriteAnimator : SpriteLoader
    {

        System.Windows.Threading.DispatcherTimer dispatcherTimer { get; set; }
        int frameNumber { get; set; }
        System.Windows.Controls.Image img { get; set; }
        public SpriteAnimator(string path, int sizeX, int sizeY, int rows, int columns, System.Windows.Controls.Image img)
            : base(path, sizeX, sizeY, rows, columns)
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            frameNumber = 0;
            this.img = img;
            img = new System.Windows.Controls.Image();

        }

        public SpriteAnimator() 
        {           
        }
        public void Animation(int time, bool isloop, int defaultframe)
        {

            if (!dispatcherTimer.IsEnabled)
            {
                frameNumber = defaultframe;
                if (!isloop)
                    dispatcherTimer.Tick += new EventHandler(onetimeanimation);
                else
                    dispatcherTimer.Tick += new EventHandler(loopanimation);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, time);
                dispatcherTimer.Start();
                loopanimation(null, null);
            }
        }
        private void loopanimation(object sender, EventArgs e)
        {
            if (frameNumber >= images.LongCount())
                frameNumber = 1;

            img.Source = images[frameNumber++];

        }

        private void onetimeanimation(object sender, EventArgs e)
        {
            if (frameNumber >= images.LongCount())
            {
                dispatcherTimer.Stop();
                frameNumber = 0;
                dispatcherTimer.Tick -= new EventHandler(onetimeanimation);
                return;
            }
            img.Source = images[frameNumber++];


        }
        public void stop()
        {
            dispatcherTimer.Stop();
            img.Source = images[0];
            frameNumber = 0;
            dispatcherTimer.Tick -= new EventHandler(loopanimation);
        }
    }
}

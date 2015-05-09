using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Asteroids_Rebirth
{
    public partial class MultiPlayerGameServer : MainWindow.Game
    {
        private Spaceship secondSpaceship;
        private NATUPNPLib.UPnPNATClass upnpnat;
        private bool waitingForClient = true;
        private Thread network;
        public MultiPlayerGameServer(Canvas canvas, MainWindow window)
            : base(canvas, window)
        {
            this.window = window;
            this.scoreInformation = window.Information;
            this.canvas = canvas;
            wave = 0;
            score = 0;

        }

        public override void InitGame(int wave)
        {
            canvas.Children.Clear();
            asteroids = new List<Asteroids>();
            asteroids.Add(new Asteroids(canvas, 0, 0, 20, 2, 1.1, 0, 2, 0));
            asteroids.Add(new Asteroids(canvas, 50, 10, 20, 1, 1.1, 1.1, 2, 0));
            asteroids.Add(new Asteroids(canvas, 50, 40, 20, 2, -1.1, 1.1, 0, 0));
            asteroids.Add(new Asteroids(canvas, 0, 40, 20, 1, -1.1, 0, 4, 0));
            spaceship = new Spaceship(canvas, 38, 27, 10);

            spaceship.positionX -= 20;
            spaceship.colision.move(-20, 0);
            secondSpaceship = new Spaceship(canvas, 38, 27, 10);
            secondSpaceship.Sprite.Source = spaceship.thrusters.images[1];
        }

        public override void Loop()
        {
            base.Loop();
            network = new Thread(ServerInitialization);
            network.SetApartmentState(ApartmentState.STA);
            network.Start();

        }

        private void ServerInitialization()
        {
            try
            {
                String host = System.Net.Dns.GetHostName();
                // Получение ip-адреса.
                string ip = System.Net.Dns.GetHostByName(host).AddressList[0].ToString();
                upnpnat = new NATUPNPLib.UPnPNATClass();
                // Открываем порт и слушаем его
                IPAddress ipAd = IPAddress.Parse(ip);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAd, 20910);
                Socket sListener = new Socket(ipAd.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                TcpListener listener = new TcpListener(ipAd, 20910);
                listener.Start(10);
                // После того как порт открыт, пробрасываем его через роутер
                NATUPNPLib.IStaticPortMappingCollection mappings = upnpnat.StaticPortMappingCollection;
                try
                {
                    mappings.Add(20910, "TCP", 20910, ip, true, "Asteroids-Rebirth");
                }
                catch (Exception)
                {//если роутер отсутствует ничего не делаем 
                }
                listener.Stop();
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                Socket handler;
                while (true)
                {

                    //Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                    handler = sListener.Accept();
                    waitingForClient = false;
                    string data = null;
                    byte[] bytes = new byte[4096];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    byte[] g = Encoding.UTF32.GetBytes(bytes.ToString());

                    Spaceship c = JsonConvert.DeserializeObject<Spaceship>(data);


                    var array1 = new double[3];

                    window.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate()
                    {
                        secondSpaceship.positionX = c.positionX;
                        secondSpaceship.positionY = c.positionY;
                        secondSpaceship.Angle = c.Angle;
                        secondSpaceship.laser.X1 = c.laserEdgeX;
                        secondSpaceship.laser.Y1 = c.laserEdgeY;

                        double directionX = Math.Sin(Math.PI * c.Angle / 180.0);
                        double directionY = -Math.Cos(Math.PI * c.Angle / 180.0);
                        secondSpaceship.laser.X2 = c.laserEdgeX + directionX * 100;
                        secondSpaceship.laser.Y2 = c.laserEdgeY + directionY * 100;
                        secondSpaceship.laserIsEnabled = c.laserIsEnabled;



                        if (c.lastHitAsteroidNumber < asteroids.Count && c.lastHitAsteroidNumber != -1 && c.lastHitAsteroidNumber != spaceship.lastHitAsteroidNumber)
                        {
                            var k = c.lastHitAsteroidNumber;
                            split(k);
                        }

                    });




                    string json = JsonConvert.SerializeObject(spaceship);
                    byte[] msg = Encoding.UTF8.GetBytes(json);

                    handler.Send(msg);
                    spaceship.lastHitAsteroidNumber = -1;


                }
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                NATUPNPLib.IStaticPortMappingCollection mappings =
                        upnpnat.StaticPortMappingCollection;

                mappings.Remove(20910, "TCP");
                network.Abort();
            }

        }
        protected override void split(int k)
        {
            double x = asteroids[k].positionX;
            double y = asteroids[k].positionY;
            int color = asteroids[k].color;
            if (asteroids[k].size == 20)
            {

                if (k % 2 == 0)
                {
                    asteroids.Add(new Asteroids(canvas, x, y, 8, color, -1.4, 1.2, 2, wave));
                    asteroids.Add(new Asteroids(canvas, x, y, 5, color, 0, -1.2, 0, wave));
                    asteroids.Add(new Asteroids(canvas, x, y, 7, color, 1.4, 1.2, 1, wave));
                }
                else
                {
                    asteroids.Add(new Asteroids(canvas, x, y, 10, color, -1.4, -1.2, 2, wave));
                    asteroids.Add(new Asteroids(canvas, x, y, 6, color, 1.4, -1.2, 0, wave));
                    asteroids.Add(new Asteroids(canvas, x, y, 9, color, 1.4, 1.2, -1, wave));
                    asteroids.Add(new Asteroids(canvas, x, y, 7, color, -1.4, 1.2, 3, wave));
                }
            }
            else if (asteroids[k].size == 7)
            {
                asteroids.Add(new Asteroids(canvas, x, y, 4, color, -2.28, 2.28, 6, wave));
                asteroids.Add(new Asteroids(canvas, x, y, 4, color, 2.1, 0.5, -3, wave));
            }
            else if (asteroids[k].size == 8)
            {
                asteroids.Add(new Asteroids(canvas, x, y, 4, color, 1.28, 2.28, -6, wave));
                asteroids.Add(new Asteroids(canvas, x, y, 6, color, 0, 2.0, 4, wave));
            }
            else if (asteroids[k].size == 9 || asteroids[k].size == 10)
            {
                asteroids.Add(new Asteroids(canvas, x, y, 4, color, -1.8, 1.2, 2, wave));
                asteroids.Add(new Asteroids(canvas, x, y, 4, color, 0, -1.9, 0, wave));
                asteroids.Add(new Asteroids(canvas, x, y, 4, color, 1.9, 2.2, 1, wave));
            }
            canvas.Children.Remove(asteroids[k].Sprite);
            asteroids.RemoveAt(k);
        }
        protected override void AddSmallAsteroids(double posx, double posy, int size, int color)
        {
            Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            int amount;
            if (size == 20)
                amount = random.Next(2, 5);
            else
                amount = random.Next(2, 3);
            for (int i = 0; i < amount; i++)
            {
                int newsize = size / amount;
                newsize += random.Next(0, 1);
                System.Threading.Thread.Sleep(1);
                asteroids.Add(new Asteroids(canvas, posx, posy, newsize, color, wave));
            }
        }

        public override void Pause()
        {
            base.Pause();
            network.Abort();
        }

        protected override void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            while (waitingForClient) ;
            scoreInformation.Content = "Score: " + score.ToString() + " Lives: " + spaceship.lives + " Wave: " + wave;
            if (spaceship.lives == 0 || secondSpaceship.lives == 0)
            {
                this.Pause();
                window.Record.Visibility = Visibility.Visible;
                window.ScoreText.Content = "Your Score: " + score.ToString();
                window.nameOfPlayer.Focus();
                window.Information.Visibility = Visibility.Hidden;
                window.gameViewBox.Visibility = Visibility.Hidden;
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.WindowState = WindowState.Normal;
            }
            spaceship.physics();
            if (asteroids.Count == 0)
            {
                canvas.Children.Clear();
                spaceship.lives = 3;
                InitGame(++wave);
            }
            for (int i = 0; i < asteroids.Count; i++)
                asteroids[i].physics();
            if (spaceship.alive && !spaceship.transparent)
            {
                int hitAsteroid = colision(spaceship.colision.colisionpoints.ToArray(), asteroids);
                if (hitAsteroid >= 0)
                {
                    spaceship.lastHitAsteroidNumber = hitAsteroid;
                    spaceship.destroy();
                    split(hitAsteroid);

                };

            }
            if (spaceship.laserIsEnabled)
            {
                int hitAsteroid = colision(new Point[] { new Point(spaceship.laser.X1, spaceship.laser.Y1), new Point(spaceship.laser.X2, spaceship.laser.Y2) }, asteroids);
                if (hitAsteroid >= 0)
                {
                    spaceship.lastHitAsteroidNumber = hitAsteroid;
                    split(hitAsteroid);
                    score += 10 * (wave + 1);
                };
            }
            spaceship.draw();
            secondSpaceship.draw();
            secondSpaceship.laser.X1 = 5;
            secondSpaceship.laser.Y1 = 5;
            secondSpaceship.laser.X2 = 35;
            secondSpaceship.laser.Y2 = 35;
            for (int i = 0; i < asteroids.Count; i++)
                asteroids[i].draw();

        }

    }
}

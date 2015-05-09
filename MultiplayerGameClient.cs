
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Asteroids_Rebirth
{
    public partial class MultiPlayerGameClient : MainWindow.Game
    {
        private bool waitingForClient = true;
        public string ip;
        private Thread network;
        private Spaceship secondSpaceship;
        public MultiPlayerGameClient(Canvas canvas, MainWindow window)
            : base(canvas, window)
        {
            this.window = window;
            this.scoreInformation = window.Information;
            this.canvas = canvas;
            ip = window.ip.Text;
            wave = 0;
            score = 0;
        }

        public override void Loop()
        {
            base.Loop();
            network = new Thread(ClientInitialization);
            network.SetApartmentState(ApartmentState.STA);
            network.Start();
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
        public override void InitGame(int wave)
        {
            canvas.Children.Clear();
            asteroids = new List<Asteroids>();
            asteroids.Add(new Asteroids(canvas, 0, 0, 20, 2, 1.1, 0, 2, 0));
            asteroids.Add(new Asteroids(canvas, 50, 10, 20, 1, 1.1, 1.1, 2, 0));
            asteroids.Add(new Asteroids(canvas, 50, 40, 20, 2, -1.1, 1.1, 0, 0));
            asteroids.Add(new Asteroids(canvas, 0, 40, 20, 1, -1.1, 0, 4, 0));
            spaceship = new Spaceship(canvas, 38, 27, 10);

            secondSpaceship = new Spaceship(canvas, 38, 20, 10);
            secondSpaceship.Sprite.Source = spaceship.thrusters.images[1];
        }
        private void ClientInitialization()
        {
           
                IPHostEntry ipHost;
                IPAddress ipAddr;
                IPEndPoint ipEndPoint;

                Socket socket=null;
                bool doing = true;
                ipAddr = IPAddress.Parse("94.232.213.85");
                if(ip!="Host ip")
                    ipAddr = IPAddress.Parse(ip);
                ipEndPoint = new IPEndPoint(ipAddr, 20910);

                try
                {
                // Соединяем сокет с удаленной точкой
   
                do
                {
                    // Буфер для входящих данных
                    byte[] bytes = new byte[4096];



                    // Устанавливаем удаленную точку для сокета
                    socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(ipEndPoint);
                    double[] array = new double[3];

                    waitingForClient = false;
                    string json = null;

                    json = JsonConvert.SerializeObject(spaceship);
                    byte[] msg = Encoding.UTF8.GetBytes(json);

                    int bytesSent = socket.Send(msg); // Отправляем данные через сокет

                        spaceship.lastHitAsteroidNumber = -1;

                    // Получаем ответ от сервера
                    int bytesRec = socket.Receive(bytes);
                    string data = null;
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    Spaceship c = JsonConvert.DeserializeObject<Spaceship>(data);
                    window.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (ThreadStart) delegate()
                        {
                            secondSpaceship.positionX = c.positionX;
                            secondSpaceship.positionY = c.positionY;
                            secondSpaceship.Angle = c.Angle;
                            secondSpaceship.laser.X1 = c.laserEdgeX;
                            secondSpaceship.laser.Y1 = c.laserEdgeY;

                            double directionX = Math.Sin(Math.PI*c.Angle/180.0);
                            double directionY = -Math.Cos(Math.PI*c.Angle/180.0);
                            secondSpaceship.laser.X2 = c.laserEdgeX + directionX*100;
                            secondSpaceship.laser.Y2 = c.laserEdgeY + directionY*100;
                            secondSpaceship.laserIsEnabled = c.laserIsEnabled;
                      //      secondSpaceship.Sprite.Source = spaceship.Sprite.Source.CloneCurrentValue();
                            if (c.lastHitAsteroidNumber<asteroids.Count&&c.lastHitAsteroidNumber != -1&&c.lastHitAsteroidNumber!=spaceship.lastHitAsteroidNumber)
                            {
                                var k = c.lastHitAsteroidNumber;
                                split(k);

                            }
                        });
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();

                    Thread.Sleep(1);

                } while (doing);
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Соединение с сервером было потеряно или разорвано");
            }
            finally
            {
                    // Освобождаем сокет
                if (socket != null)
                {
                   //socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
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
            if (spaceship.lives == 0||secondSpaceship.lives==0)
            {
                this.Pause();
                window.Record.Visibility = Visibility.Visible;
                window.ScoreText.Content = "Your Score: " + score.ToString();
                window.nameOfPlayer.Focus();
                window.Information.Visibility = Visibility.Hidden;
                window.gameViewBox.Visibility = Visibility.Hidden;
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.WindowState = WindowState.Normal;
                network.Abort();
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
            for (int i = 0; i < asteroids.Count; i++)
                asteroids[i].draw();

        }

    }
}

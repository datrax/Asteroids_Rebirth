
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
        public bool stopThread = false;
        private DateTime time;
        public MultiPlayerGameClient(Canvas canvas, MainWindow window)
            : base(canvas, window)
        {
            this.window = window;
            this.scoreInformation = window.Information;
            this.canvas = canvas;
            ip = window.ip.Text;
            wave = 0;
            score = 0;
            time = DateTime.Now;
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
            waitingForClient = true;
        }
        Socket socket = null;
        private void ClientInitialization()
        {

            IPHostEntry ipHost;
            IPAddress ipAddr;
            IPEndPoint ipEndPoint;

            
            bool doing = true;
            ipAddr = IPAddress.Parse("94.232.213.85");
            if (ip != "")
                ipAddr = IPAddress.Parse(ip);
            ipEndPoint = new IPEndPoint(ipAddr, 20910);

            try
            {


                do
                {
                    // Буфер для входящих данных
                    byte[] bytes = new byte[1024];
                    waitingForClient = false;

                    string json = null;

                    json = JsonConvert.SerializeObject(spaceship);
                    if (stopThread) 
                        json = "stop";
                    byte[] msg = Encoding.UTF8.GetBytes(json);
                    // Устанавливаем удаленную точку для сокета
                    socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(ipEndPoint);
                    int bytesSent = socket.Send(msg); // Отправляем данные через сокет
                    if (json == "stop") break;
                    spaceship.lastHitAsteroidNumber = -1;

                    // Получаем ответ от сервера
                    int bytesRec = socket.Receive(bytes);
                    string data = null;
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    if (data == "stop")
                    {
                //        MessageBox.Show("Server has left the game");
                        break;
                    }             
                    Spaceship c = JsonConvert.DeserializeObject<Spaceship>(data);

                    window.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (ThreadStart)delegate()
                        {
                            secondSpaceship.positionX = c.positionX;
                            secondSpaceship.positionY = c.positionY;
                            secondSpaceship.Angle = c.Angle;
                            secondSpaceship.laser.X1 = c.laserEdgeX;
                            secondSpaceship.laser.Y1 = c.laserEdgeY;
                            secondSpaceship.astAmount = c.astAmount;
                            secondSpaceship.alive = c.alive;

                            double directionX = Math.Sin(Math.PI * c.Angle / 180.0);
                            double directionY = -Math.Cos(Math.PI * c.Angle / 180.0);
                            secondSpaceship.laser.X2 = c.laserEdgeX + directionX * 100;
                            secondSpaceship.laser.Y2 = c.laserEdgeY + directionY * 100;
                            secondSpaceship.laserIsEnabled = c.laserIsEnabled;
                            //      secondSpaceship.Sprite.Source = spaceship.Sprite.Source.CloneCurrentValue();
                            if (c.lastHitAsteroidNumber < asteroids.Count && c.lastHitAsteroidNumber != -1 && c.lastHitAsteroidNumber != spaceship.lastHitAsteroidNumber)
                            {
                                var k = c.lastHitAsteroidNumber;
                                split(k);

                            }
                        });
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    Thread.Sleep(8);
                } while (doing);

            }
            catch (Exception ex)
            {
             
            }
            finally
            {

                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();

                }
                catch
                {
                }

                finally
                {
                    window.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (ThreadStart) delegate()
                        {
                            base.Pause();
                            window.Record.Visibility = Visibility.Visible;
                            window.ScoreText.Content = "Your Score: " + score.ToString();
                            window.nameOfPlayer.Focus();
                            window.Information.Visibility = Visibility.Hidden;
                            window.gameViewBox.Visibility = Visibility.Hidden;
                            window.WindowStyle = WindowStyle.SingleBorderWindow;
                            window.WindowState = WindowState.Normal;
                        });
                    network.Abort();
                }
            }
        }


        public override void Pause()
        {                      
            base.Pause();
            stopThread = true;
        }

       
        protected override void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            while (waitingForClient) ;
            spaceship.astAmount = asteroids.Count;
            window.InfForServer.Content = "";
            scoreInformation.Content = "Score: " + score.ToString() + " Lives: " + spaceship.lives + " Wave: " + wave;
            if (spaceship.lives == 0 || secondSpaceship.lives == 0)
            {
                this.Pause();
                stopThread = true;
          /*     
                network.Abort();*/
            }
            spaceship.physics();
            
            if (secondSpaceship.alive == false&&(DateTime.Now-time).Seconds>3) 
            {
                secondSpaceship.destroy();
                time = DateTime.Now;
            }
           if((DateTime.Now-time).Seconds>3)
                secondSpaceship.Sprite.Source = spaceship.thrusters.images[1];
            
            
            if (asteroids.Count == 0&&secondSpaceship.astAmount==0)
            {
                canvas.Children.Clear();
                spaceship.lives = 3;
                InitGame(++wave);
            }
            for (int i = 0; i < asteroids.Count; i++)
                asteroids[i].physics();
            int hitAsteroid = -1;
            if (spaceship.alive && !spaceship.transparent)
            {
                hitAsteroid = colision(spaceship.colision.colisionpoints.ToArray(), asteroids);
                if (hitAsteroid >= 0)
                {
                    spaceship.lastHitAsteroidNumber = hitAsteroid;
                    spaceship.destroy();
                    split(hitAsteroid);

                };

            }

            if (hitAsteroid == -1 && spaceship.laserIsEnabled)
            {
                hitAsteroid = colision(new Point[] { new Point(spaceship.laser.X1, spaceship.laser.Y1), new Point(spaceship.laser.X2, spaceship.laser.Y2) }, asteroids);
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

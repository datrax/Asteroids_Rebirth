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
            base.InitGame(wave);
            spaceship.positionX -= 20;
            spaceship.colision.move(-20,0);
            secondSpaceship = new Spaceship(canvas, 38, 27, 10);
        }

        public override void Loop()
        {
            base.Loop();
            network = new Thread(ServerInitialization);
            network.Start();
        }

        private void ServerInitialization()
        {
            try{
             String host = System.Net.Dns.GetHostName();
                // Получение ip-адреса.
                string ip = System.Net.Dns.GetHostByName(host).AddressList[0].ToString();
            upnpnat = new NATUPNPLib.UPnPNATClass();
                // Открываем порт и слушаем его
                IPAddress ipAd = IPAddress.Parse(ip);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAd, 20903);
                Socket sListener = new Socket(ipAd.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                TcpListener listener = new TcpListener(ipAd, 20903);
                listener.Start(10);
                // После того как порт открыт, пробрасываем его через роутер
                NATUPNPLib.IStaticPortMappingCollection mappings = upnpnat.StaticPortMappingCollection;
                try
                {
                    mappings.Add(20903, "TCP", 20903, ip, true, "Test Open Port");
                }
                catch (Exception)
                {//если роутер отсутствует ничего не делаем 
                }
                listener.Stop();
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                while (true)
                {
                    
                    //Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                    Socket handler = sListener.Accept();
                    waitingForClient = false;
                    string data = null;
                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    byte[] g = Encoding.UTF32.GetBytes(bytes.ToString());
                    //   Spaceship c = JsonConvert.DeserializeObject<Dictionary<String, Object>>(jsonStr);

                   
                   var array = JsonConvert.DeserializeObject<double[]>(data);
                    var array1 = new double[3];
                 //   Spaceship c = JsonConvert.DeserializeObject<Spaceship>(data);
                    window.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (ThreadStart) delegate()
                        {                        
                            secondSpaceship.positionX = array[0];
                            secondSpaceship.positionY = array[1];
                            secondSpaceship.Angle = array[2];
                    //        secondSpaceship.laserIsEnabled = Convert.ToBoolean(array[3]);
                            secondSpaceship.Sprite.Source = spaceship.Sprite.Source.CloneCurrentValue();
                            
                           
                        });

                //    array1[3] = Convert.ToDouble(spaceship.laserIsEnabled);
                    array1[0] = spaceship.positionX;
                    array1[1] = spaceship.positionY;
                    array1[2] = spaceship.Angle;
                    string json = JsonConvert.SerializeObject(array1);
                    byte[] msg = Encoding.UTF8.GetBytes(json);
                    handler.Send(msg);
                    if (data.IndexOf("Stop") > -1)
                    {
                        Console.WriteLine("Сервер завершил соединение с клиентом.");
                        break;
                    }
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                NATUPNPLib.IStaticPortMappingCollection mappings =
                        upnpnat.StaticPortMappingCollection;

                mappings.Remove(20903, "TCP");
                network.Abort();
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
            if (spaceship.lives == 0)
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
                    spaceship.destroy();
                    split(hitAsteroid);

                };

            }
            if (spaceship.laserIsEnabled)
            {
                int hitAsteroid = colision(new Point[] { new Point(spaceship.laser.X1, spaceship.laser.Y1), new Point(spaceship.laser.X2, spaceship.laser.Y2) }, asteroids);
                if (hitAsteroid >= 0)
                {
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

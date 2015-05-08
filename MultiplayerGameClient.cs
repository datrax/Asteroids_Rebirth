using System.Windows.Documents;
using Newtonsoft.Json;
using System;
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
        private Thread network;
        private Spaceship secondSpaceship;
        public MultiPlayerGameClient(Canvas canvas, MainWindow window)
            : base(canvas, window)
        {
            this.window = window;
            this.scoreInformation = window.Information;
            this.canvas = canvas;
            wave = 0;
            score = 0;
        }

        public override void Loop()
        {
            base.Loop();
            network = new Thread(ClientInitialization);
            network.Start();
        }
        public override void InitGame(int wave)
        {
            base.InitGame(wave);
            secondSpaceship = new Spaceship(canvas, 38, 20, 10);
        }
        private void ClientInitialization()
        {

            IPHostEntry ipHost;
            IPAddress ipAddr;
            IPEndPoint ipEndPoint;

            Socket socket;
            bool doing = true;
            ipAddr = IPAddress.Parse("193.161.14.209");
            ipEndPoint = new IPEndPoint(ipAddr, 20903);
       

            // Соединяем сокет с удаленной точкой
            
            do
            {
                // Буфер для входящих данных
                byte[] bytes = new byte[1024];

               // ipHost = Dns.Resolve(Dns.GetHostName());
                //IPAddress ipAddr = ipHost.AddressList[0];
            


                // Устанавливаем удаленную точку для сокета
                socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipEndPoint);
                double[] array = new double[3];
              
                        array[0] = spaceship.positionX;
                        array[1] = spaceship.positionY;
                        array[2] = spaceship.Angle;
              
                string json = JsonConvert.SerializeObject(array);
                byte[] msg = Encoding.UTF8.GetBytes(json);

                // Отправляем данные через сокет
                int bytesSent = socket.Send(msg);

                // Получаем ответ от сервера
                int bytesRec = socket.Receive(bytes);
                string data = null;
                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                array = JsonConvert.DeserializeObject<double[]>(data);
                //   Spaceship c = JsonConvert.DeserializeObject<Spaceship>(data);
                window.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate()
                    {
                        secondSpaceship.positionX = array[0];
                        secondSpaceship.positionY = array[1];
                        secondSpaceship.Angle = array[2];
                        secondSpaceship.Sprite.Source = spaceship.Sprite.Source.Clone();
                    });
             //   Console.WriteLine("\nОтвет от сервера: {0}\n\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));

                /*   if (message.IndexOf("Stop") != -1)
             
                 * doing = false;*/
                Thread.Sleep(1);
            }
            while (doing);
            // Освобождаем сокет

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        public override void Pause()
        {
            base.Pause();
            network.Abort();
        }
        protected override void dispatcherTimer_Tick(object sender, EventArgs e)
        {
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
            for (int i = 0; i < asteroids.Count; i++)
                asteroids[i].draw();

        }

    }
}

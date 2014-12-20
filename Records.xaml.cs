using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Asteroids_Rebirth
{
    /// <summary>
    /// Interaction logic for Records.xaml
    /// </summary>
    public partial class Records : Window
    {

        public Records()
        {
            InitializeComponent();
            ReadFromFile();
        }
        private void ReadFromFile()
        {
            try
            {
                using (FileStream inStr = new FileStream(Environment.CurrentDirectory + @"\Resources\file.dat", FileMode.Open))
                {

                    BinaryFormatter bf = new BinaryFormatter();
                    List<record> listofrecords = new List<record>();
                    while (inStr.Position < inStr.Length)
                    {
                        record list = bf.Deserialize(inStr) as record;
                        listofrecords.Add(list);

                    }
                    listofrecords = listofrecords.OrderByDescending(x => x.Score).ToList();
                    for (int i = 0; i < listofrecords.Count; i++)
                    {
                        listofrecords[i].Pos = i + 1;
                    }
                    rec.ItemsSource = listofrecords;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}

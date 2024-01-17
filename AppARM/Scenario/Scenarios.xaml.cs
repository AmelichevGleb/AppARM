using AppARM.Parser;
using AppARM.TestXML;
using AppARM.Weather;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AppARM.WeatherSokol;
using AppARM.TestXML;
using AppARM.Parser;
using System.Net.Sockets;
using System.Xml.Linq;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using static Mono.Xml.MiniParser;

namespace AppARM.Scenario
{
    /// <summary>
    /// Логика взаимодействия для Scenarios.xaml
    /// </summary>
    public partial class Scenarios : Window
    {
        private string ipServer;
        private string portServer;

        private Socket tcpClient;
        private NetworkStream tcpStream;
        private Files files = new Files();

        public Scenarios()
        {
            InitializeComponent();
            B_Disconnect.IsEnabled = false;
            BT_Ping.IsEnabled = false;
        }

        //отправка сообщения на устройства. (особая постановка байт)
        private void SendReceive(string _Message)
        {
            byte[] bytes = new byte[1024];
            byte[] t1 = new byte[85];
            byte[] t = new byte[81];
            t = Encoding.Default.GetBytes(_Message);
            Console.WriteLine("byte array: " + BitConverter.ToString(t1));
            Int32 datasize = t.Length;
            Console.WriteLine(t.Length);
            Console.WriteLine("byte array: " + BitConverter.ToString(t1));
            for (int i = 3; i >= 0; i--)
            {
                t1[i] = (byte)(datasize % 256);
                datasize = datasize / 256;
            }
            int l = 0;
            Console.WriteLine("byte Array: " + BitConverter.ToString(t1));
            for (int j = 4; j < 85; j++)
            {
                t1[j] = t[j - 4];
            }
            Console.WriteLine("byte Array: " + BitConverter.ToString(t1));
            tmp = t1;
            tcpClient.Send(tmp);
            byte[] data = new byte[1024];
            int length = tcpClient.Receive(data);
            string message = Encoding.UTF8.GetString(data, 0, length);
            Console.WriteLine(message);
            Test.Text += "ответ \"Устройства\":\n" + message + '\n';
        }

        private void MouseClick_AddTextLable(object sender, MouseButtonEventArgs e)
        {
            Test.Text += "ffff \n";
        }
        private void MouseClick_ClearLable(object sender, MouseButtonEventArgs e)
        {
            Test.Text = string.Empty; 
        }

        private void Button_Click_Disconnect(object sender, RoutedEventArgs e)
        {   
            tcpClient.Dispose();
            B_Connect.IsEnabled = true;
            B_Disconnect.IsEnabled = false;
            BT_Ping.IsEnabled = false;
        }


        //запросить пинг устройства
        //<?xml version = "1.0" encoding="utf-8"?>
        //<command>
        //<action>ping</action>
        //</command>
        byte[] tmp;
        byte[] bytes = new byte[1024];
       
        private void BT_Ping_Click(object sender, RoutedEventArgs e)
        {
            string text = "<?xml version =\"1.0\" encoding=\"utf-8\"?><command>\n<action>ping</action>\n</command>";
            Test.Text += "Выполнение команды: \"ping\" \n";
            SendReceive(text);
        }

        private void BT_Stop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Connect(object sender, RoutedEventArgs e)
        {
            ipServer = TB_IPAdress.Text;
            portServer = TB_Port.Text;
            B_Connect.IsEnabled = false;
            B_Disconnect.IsEnabled = true;
            BT_Ping.IsEnabled = true;
            Console.WriteLine(ipServer + " " + portServer);
            try
            {
                tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipaddress = IPAddress.Parse(ipServer);
                EndPoint point = new IPEndPoint(ipaddress, Convert.ToInt32(portServer));
                tcpClient.Connect(point);
            }
            catch (Exception ex)
            {
                files.ReadExeption(ex);
                Test.Text += "Подключение не удачное \n";
                B_Connect.IsEnabled = true;
                B_Disconnect.IsEnabled = false;
                BT_Ping.IsEnabled = false;
            }
        }    
    }
}

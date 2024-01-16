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

namespace AppARM.Scenario
{
    /// <summary>
    /// Логика взаимодействия для Scenarios.xaml
    /// </summary>
    public partial class Scenarios : Window
    {
        private string ipServer;
        private string portServer;

        private TcpClient tcpClient;
        private Files files = new Files();

        public Scenarios()
        {
            InitializeComponent();
            B_Disconnect.IsEnabled = false;
        }
        public void test()
        {
            /*
             * byte[] bytes = new byte[1024];
           
           IPHostEntry ipHost = Dns.GetHostEntry("localhost");
           IPAddress ipAddr = ipHost.AddressList[0];
           IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 8081);
           Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
           sender.Connect(ipEndPoint);
           string text = "<?xml version =\"1.0\" encoding=\"utf-8\"?><command>\n<action>ping</action>\n</command>";
           byte[] t1 = new byte[85];
           byte[] t = new byte[81];
           t = Encoding.Default.GetBytes(text);
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
           // Отправляем данные через сокет    
           int bytesSent = sender.Send(tmp);
           // Получаем ответ от сервера
           int bytesRec = sender.Receive(bytes);
           Console.WriteLine("\nОтвет от сервера: {0}\n\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));
           // Используем рекурсию для неоднократного вызова SendMessageFromSocket()

           // Освобождаем сокет
           sender.Shutdown(SocketShutdown.Both);
           sender.Close();
      */
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
        }

        private void BT_Ping_Click(object sender, RoutedEventArgs e)
        {

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


            Console.WriteLine(ipServer + " " + portServer);
            try
            {
               tcpClient= new TcpClient();
                tcpClient.Connect(ipServer, Convert.ToInt32(portServer));
                Test.Text += "Подключение удачное \n";
            }
            catch (Exception ex)
            {
                files.ReadExeption(ex);
                Test.Text += "Подключение не удачное \n";
                B_Connect.IsEnabled = true;
                B_Disconnect.IsEnabled = false;
            }

        }

      
    }
}

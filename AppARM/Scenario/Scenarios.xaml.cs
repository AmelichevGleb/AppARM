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


        byte[] tmp;
        byte[] bytes = new byte[1024];

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
        private void SendReceive(string _command)
        {
            int str = 150;
            //  byte[] bytes = new byte[4086];
           
            byte[] t = Encoding.Default.GetBytes(_command);
            byte[] t1 = new byte[t.Length +4];

                      // Console.WriteLine("byte array: " + BitConverter.ToString(t1));
            Int32 datasize = t.Length;
            Console.WriteLine(t.Length);
         //   Console.WriteLine("byte array: " + BitConverter.ToString(t1));
            for (int i = 3; i >= 0; i--)
            {
                t1[i] = (byte)(datasize % 256);
                datasize = datasize / 256;
            }
            int l = 0;
            Console.WriteLine("byte Array: " + BitConverter.ToString(t1));
            for (int j = 4; j < t1.Length; j++)
            {
                t1[j] = t[j - 4];
            }
            Console.WriteLine("byte Array: " + BitConverter.ToString(t1));
           
            tmp = t1;
            Console.WriteLine("Жду чуда {0}", System.Text.Encoding.Default.GetString(tmp));
            tcpClient.Send(tmp);
            byte[] data = new byte[1024]; // изменить размерность после !!!!!!!!!!
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


        //____________________________ОСНОВНЫЕ_КНОПКИ_______________________________________________

        //Кнопка подключения к устройству
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
                Test.Text += "Подключение удачное \n";
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

        //Кнопка отключения от устройства 
        private void Button_Click_Disconnect(object sender, RoutedEventArgs e)
        {
            tcpClient.Dispose();
            Test.Text += "Отключение от устройства \n";
            B_Connect.IsEnabled = true;
            B_Disconnect.IsEnabled = false;
            BT_Ping.IsEnabled = false;
        }

        //Кнопка отправка команды пинг на подключенное устройство
        private void BT_Ping_Click(object sender, RoutedEventArgs e)
        {
            /*запросить пинг устройства
            <?xml version = "1.0" encoding="utf-8"?>
            <command>
            <action>ping</action>
            </command>
            */
            string command = "<?xml version =\"1.0\" encoding=\"utf-8\"?>\n<command>\n<action>ping</action>\n</command>";
            Test.Text += "Выполнение команды: \"ping\" \n";
            Console.WriteLine(command); 
            SendReceive(command);
        }

        //Кнопка запуска сценария
        private void srq_Click(object sender, RoutedEventArgs e)
        {
            string soundCheck = "internal";
            var command = new StringBuilder();
            if (CB_Sound.IsChecked == true) { soundCheck = "external"; }
            command.AppendFormat("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<command>\n<action>start</action>\n" +
                "<parameters>\n<scenario>{0}</scenario>\n<audio>{1}<audio/>\n</parameters>\n</command>", CB_Script_number.Text, soundCheck);
            Console.WriteLine(command);
            SendReceive(Convert.ToString(command));

        }






        private void BT_Stop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }




        //__________________________________________________________________________________________
    }
}

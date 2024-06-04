using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Net.Sockets;

using AppARM.Parser;
using AppARM.FilesLogs;
using AppARM.Weather;
using AppARM.WeatherSokol;

namespace AppARM.Scenario
{
    /// <summary>
    /// Логика взаимодействия для Scenarios.xaml
    /// </summary>
    public partial class Scenarios : Window
    {
        private string ipServer;
        private string portServer;


        byte[] sendMassive;
        byte[] bytes = new byte[1024];

        private Socket tcpClient;
        private NetworkStream tcpStream;
        private Files files = new Files();

        public Scenarios()
        {
            InitializeComponent();
            B_Disconnect.IsEnabled = false;
            BT_Ping.IsEnabled = false;
            B_StartScenario.IsEnabled = false;
        }

        //отправка сообщения на устройства. (особая постановка байт)
        private void SendReceive(string _command)
        {   
            // перезапись 1-го массива установа первых 4-х байт
            byte[] massive1 = Encoding.Default.GetBytes(_command);
            byte[] massive2 = new byte[massive1.Length +4];
            Int32 datasize = massive1.Length;

            Console.WriteLine(massive1.Length);
            for (int i = 3; i >= 0; i--)
            {
                massive2[i] = (byte)(datasize % 256);
                datasize = datasize / 256;
            }
            Console.WriteLine("byte Array: " + BitConverter.ToString(massive2));
            for (int j = 4; j < massive2.Length; j++)
            {
                massive2[j] = massive1[j - 4];
            }
            Console.WriteLine("byte Array: " + BitConverter.ToString(massive2));
            sendMassive = massive2; 
            tcpClient.Send(sendMassive);
            byte[] data = new byte[1024]; // изменить размерность после !!!!!!!!!!
            
            int length = tcpClient.Receive(data);
            string message = Encoding.UTF8.GetString(data, 0, length);
            Console.WriteLine(message);
            Test.Text += "ответ \"Устройства\":\n" + message + '\n';
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (tcpClient == null) { MessageBox.Show("ыыы"); }
            else { tcpClient.Dispose(); }
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
            B_StartScenario.IsEnabled = true;

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
                files.ReadException(ex);
                Test.Text += "Подключение не удачное \n";
                B_Connect.IsEnabled = true;
                B_Disconnect.IsEnabled = false;
                BT_Ping.IsEnabled = false;
                B_StartScenario.IsEnabled = false;
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
            B_StartScenario.IsEnabled = false;
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
            string command = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><command>\n<action>ping</action>\n</command>";
            Test.Text += "Выполнение команды: \"ping\" \n";
            Console.WriteLine(command); 
            SendReceive(command);
        }

        //Кнопка запуска сценария
        private void Button_Click_StartScenario(object sender, RoutedEventArgs e)
        {
            string soundCheck = "internal";
            var command = new StringBuilder();
            if (CB_Sound.IsChecked == true) { soundCheck = "external"; }
            command.AppendFormat("<?xml version=\"1.0\" encoding=\"utf-8\" ?><command>\n<action>start</action>\n" +
                "<parameters>\n<scenario>{0}</scenario>\n<audio>{1}<audio/>\n</parameters>\n</command>", CB_Script_number.Text, soundCheck);
            Console.WriteLine(command);
            SendReceive(Convert.ToString(command));
        }



        private void BT_Stop_Click(object sender, RoutedEventArgs e)
        {
            var command = new StringBuilder();
            command.AppendFormat("<?xml version=\"1.0\" encoding=\"utf-8\" ?><command>\n<action>stop</action>\n%1</command>");
            Console.WriteLine(command);
            SendReceive(Convert.ToString(command));

        }

        private void BС_StartScenarios(object sender, RoutedEventArgs e)
        {
            
            string type = null ;
            var command = new StringBuilder();
            string NumberCommand = TB_Command.Text;
            string typeSiren = "discontinuous";
            int working_Launch = 0;
            //Особенность сеанса:
            //рабочий запуск 
            if (CB_WL.IsChecked == true) {
                working_Launch = 1;
            }
            //сирены непрерывно
            if (CB_SC.IsChecked == true)
            {
                typeSiren = "continuous";
            }
            //Типы устройств 
            if (CB_APU.IsChecked == true) { type += "apu;"; }
            if (CB_TLF.IsChecked == true) { type += "tlf;"; }
            if (CB_RTU.IsChecked == true) { type += "rtu;"; }
            if (CB_SRN.IsChecked == true) { type += "srn;"; }

            command.AppendFormat("<?xml version=\"1.0\" encoding=\"utf-8\" ?><command>\n<action>start</action>\n" +
                                "<parameters>\n<seance startmode=\"{0}\">\n<types>{1}</types>\n"+
                                "<consoles>{2}</consoles>\n<terminals>{3}</terminals>\n<p160command>{4}</p160command>\n" +
                                "<sirenmode>{5} </sirenmode>\n</seance>\n</parameters>\n</command>", working_Launch, type,TB_IdAPU.Text ,TB_IdDevise.Text, NumberCommand, typeSiren);

            Console.WriteLine("--->");
            Console.WriteLine(command);
            Console.WriteLine("<---");
            SendReceive(Convert.ToString(command));
        }
        //__________________________________________________________________________________________
    }
}

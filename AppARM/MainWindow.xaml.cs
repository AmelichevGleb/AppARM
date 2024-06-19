using System;
using System.Text;
using System.Windows;
using System.Net.Sockets;
using System.Xml.Linq;
using System.Threading;

using AppARM.CheckMeteos;
using AppARM.Parser;
using AppARM.ServerListren;
using AppARM.Test;
using AppARM.WeatherSokol;
using AppARM.FilesLogs;
using AppARM.Scripts;
using System.Windows.Threading;
using AppARM.PostgresSQL;
using AppARM.Structure;
using AppARM.SettingApp;
using AppARM.Device_Database;
using AppARM.Scenario;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Specialized;
using System.Net;
using System.Security.Policy;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;


namespace AppARM
{ 

    public partial class MainWindow : Window
    {

        /// <summary>
        /// private Script script = new Script();
        /// private SettingForm settingApp;
        /// private CreateJsonRequest create = new CreateJsonRequest();
        /// private DispatcherTimer timer = new DispatcherTimer();
        /// private NetworkStream tcpStream;
        /// private static TcpClient newClient;
        /// private DataBaseMeteostation db;
        /// </summary>

        private GetWeather getWeather = new GetWeather();
        private Files files = new Files();
        private Thread threadServerListen;
        private ServerForListen startListen = new ServerForListen();
        private ServerReceiv startServerReceiv = new ServerReceiv();

        private UserControlMeteostation userControlMeteostation;
        private UserControlSetting userControlSetting;


        public MainWindow()
        {
            InitializeComponent();
            files.InitFileLogs();
            files.InitFileException();
          //  startListen.mainStart(this);  //Старт сервера прослушки 
          //  startServerReceiv.mainStartServerReceiver(this); //Старт сервера принимающего сигналы ЧС

            //Таймер 
            // timer.Interval = TimeSpan.FromSeconds(1);
            //  timer.Tick += timer_Tick;
            //   timer.Start();

        }
        //____________________________ТАЙМЕР________________________________________________________
        void timer_Tick(object sender, EventArgs e)  //пока отключен
        {
           lblTime.Content = DateTime.Now.ToLongTimeString();
        }

        //__________________________________________________________________________________________

        //____________________________ОСНОВНЫЕ_КНОПКИ_______________________________________________

        //Открытие окна с базой данных
        private void BC_DataBase(object sender, RoutedEventArgs e)
        {
            files.ReadFile("открытие окна База данных", false);
            LogServerPython1.Visibility = Visibility.Collapsed;
            string serverBD = Convert.ToString(Properties.Settings.Default.IP_adress);
            string portBD = Convert.ToString(Properties.Settings.Default.Port);
            string userBD = Convert.ToString(Properties.Settings.Default.Login_BD);
            string passwordBD = Convert.ToString(Properties.Settings.Default.Password_BD);
            DataBaseMeteostation dataBase = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD); //БД для метеостанции
            AlarmDB dataBaseAlarm = new AlarmDB(serverBD, portBD, userBD, passwordBD);                      //БД для датчиков ЧС
            AdditionalDb additionalDb = new AdditionalDb(serverBD, portBD, userBD, passwordBD);             //БД для сценариев
            UserControlDataBase userControlDataBase = new UserControlDataBase(dataBase, dataBaseAlarm, additionalDb);
            LogServerPython.Children.Add(userControlDataBase);
        }
        //Открытие окно с проверкой Метеостанций
        private void BС_CheckMeteo(object sender, RoutedEventArgs e)
        {
            LogServerPython1.Visibility = Visibility.Collapsed;
            userControlMeteostation = new UserControlMeteostation();  
            LogServerPython.Children.Add(userControlMeteostation);
            files.ReadFile("открытие окна Метеостанция", false);
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }
        //настройки конфигураций 
        public void BC_Settings(object sender, RoutedEventArgs e)
        {
            LogServerPython1.Visibility = Visibility.Collapsed;
            userControlSetting = new UserControlSetting(threadServerListen);
            LogServerPython.Children.Add(userControlSetting);
            files.ReadFile("открытие окна настроек", false);
        }
        private void BT_Server_Click(object sender, RoutedEventArgs e)
        {
            LogServerPython1.Visibility = Visibility.Visible;
        }
        private bool active = true;
        ServerReceiv serverReceiver;
        ServerForListen serverForListner;
        private void BT_TestStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (active)
                {
                    serverReceiver = new ServerReceiv();
                    serverForListner = new ServerForListen();
                    active = false;

                    MessageBox.Show("Сервер запущен");
                    serverReceiver.StartServerReceiver(1, this);
                    serverForListner.StartServerListen(1, this);
                    Uri resourceUri = new Uri("Images/rupor-on.png", UriKind.Relative);

                    string path = Environment.CurrentDirectory;
                    //process = Process.Start(path + @"\jsGen.exe");
                    //Convert.ToString("Images/rupor-off.png");
                    ImageRupor.Source = new BitmapImage(resourceUri);
                }
                else
                {

                    active = true;
                    MessageBox.Show("Сервер остановлен");
                    serverReceiver.StartServerReceiver(2, this);
                    serverForListner.StartServerListen(2, this);
                    Uri resourceUri = new Uri("Images/rupor-off.png", UriKind.Relative);
                  //  foreach (Process process in Process.GetProcesses())
                  //  {
                       
                       // if ((process.ProcessName == "jsGen") || (process.ProcessName == "node"))
                     //   {
                       //     process.Kill();
                       // }
                   // }
                    ImageRupor.Source = new BitmapImage(resourceUri);
                }
            }
            catch (Exception _ex)
            {
                files.ReadException(_ex);
            }
        }

        //__________________________________________________________________________________________

        //_____________________________СТАРЫЙ_КОД___________________________________________________

        private async void BC_Script(object sender, RoutedEventArgs e)
        {
            Scenarios scenario = new Scenarios();
            scenario.Show();
        }

        //Для тестовых проверок 
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Scenarios scenario = new Scenarios();
            scenario.Show();
        }

        //__________________________________________________________________________________________

        //____________________________ОТКЛЮЧЕННЫЕ_КНОПКИ____________________________________________

        private void ConnectServer()
        {
            try
            {
                // Соединяемся с сервером
                //   newClient.Connect(TB_IP.Text, Convert.ToInt32(TB_Port.Text)); 
                //   BDisconnect.IsEnabled = true;
                //   BConnect.IsEnabled = false;
            }
            catch (SocketException ex)
            {
                files.ReadException(ex);
            }

            /*byte[] bytes = new byte[1024];
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

        //отключено
        //Отправка погоды НУЖНО ИЗМЕНИТЬ !!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private void BC_SendMeteo(object sender, RoutedEventArgs e)
        {
            string serverBD = Convert.ToString(Properties.Settings.Default.IP_adress);
            string portBD = Convert.ToString(Properties.Settings.Default.Port);
            string userBD = Convert.ToString(Properties.Settings.Default.Login_BD);
            string passwordBD = Convert.ToString(Properties.Settings.Default.Password_BD);

            DataBaseMeteostation dataBase = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            int port = 64700;
            var t = dataBase.GetDataBaseShort("Arm");
            if (t != null)
            {
                while (t.Read())
                {
                    Console.WriteLine("{0} {1} {2} {3} {4} {5}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5));
                    getWeather.ConnectMeteo(t.GetString(1), port, t.GetString(2), t.GetString(3), t.GetString(4), Properties.Settings.Default.IP_adress, Properties.Settings.Default.Port);
                }
            }

        }

        private void BC_XML1(object sender, RoutedEventArgs e)
        {
            XDocument xdoc = XDocument.Load("Start1.xml");
            if (xdoc is not null)
            {
                foreach (XElement per in xdoc.Elements("answer"))
                {
                    XElement? action = per.Element("action");
                    Console.WriteLine($"action: {action?.Value}");
                    XElement? age1 = per.Element("result");
                    Console.WriteLine($"Age: {age1?.Value}");
                    Console.WriteLine($"Age: {age1?.Attribute("code").Value}");
                    var t = per.Element("details");
                    Console.WriteLine($"total = :{t?.Attribute("total").Value}");
                    Console.WriteLine($"terminals = :{t?.Element("terminals").Value}");
                    Console.WriteLine($"consoles = :{t?.Element("consoles").Value}");
                }
            }
        }

        //отключено
        private void BC_Connect(object sender, RoutedEventArgs e)
        {
            ConnectServer();
        }
        //отключено
        private void BC_Disconnect(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("aaa");
            byte[] sendBytes = Encoding.UTF8.GetBytes("127.0.0.1;2222");
            Console.WriteLine(Encoding.UTF8.GetString(sendBytes));
            var t = Encoding.UTF8.GetString(sendBytes);
            ParserAll parserAll = new ParserAll();
            var l = parserAll.AddMassiveStringIP(t);
            Console.WriteLine("п = {0} , t = {1}",l.Item1 ,l.Item2 );   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         //   allAPU2.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //UserControlSetting userControlSetting = new UserControlSetting();
            //LogServerPython.Children.Add(userControlSetting);   
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void BT_Browser_Click(object sender, RoutedEventArgs e)
        {
            string IpMap = Properties.Settings.Default.IP_Server_maps;
            string PortMap = Properties.Settings.Default.Port_Server_maps;
            Process.Start(new ProcessStartInfo("http://" + IpMap + ":" +PortMap  + "/#55.77223,37.6141,13z"));
            ///#55.77223,37.6141,13z


        }

        class PostResponse
        {
            public string Type { get; set; }
            public string IP { get; set; }
        }

        private bool test2 = true; Process process;
        private static readonly HttpClient client = new HttpClient();
       

        private void BT_TestStop1_Click(object sender, RoutedEventArgs e)
        {
            //Post();

            //process.Kill();

            /*
            string IpMap = Properties.Settings.Default.IP_Server_maps;
            string PortMap = Properties.Settings.Default.Port_Server_maps;
          

            var postData = new PostData
            {
                State = "Привет Денис", 
            };


            var client = new HttpClient();
             client.BaseAddress = new Uri("http://" + IpMap + ":" + PortMap);
            ///client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
            var json = System.Text.Json.JsonSerializer.Serialize(postData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync("posts", content).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var postResponse = System.Text.Json.JsonSerializer.Deserialize<PostResponse>(responseContent, options);
              MessageBox.Show("Post successful! ID: " + postResponse.Id);

            }
            else
            {
                MessageBox.Show("Error: " + response.StatusCode);
            }
            */
        }

        //__________________________________________________________________________________________


    }
}

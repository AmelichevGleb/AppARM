using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Xml;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Xml.Linq;

using System.Net;
using AppARM.TestXML;
using AppARM.Scripts;
using System.Windows.Threading;


using AppARM.PostgresSQL;
using AppARM.Structure;
using AppARM.SettingApp;
using AppARM.Device_Database;
using AppARM.Scenario;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using AppARM.Test;
using AppARM.WeatherSokol;
using static AppARM.Test.Experiment;
using AppARM.Weather;
using AppARM.WeatherSokol;
using AppARM.CheckMeteos;
using Newtonsoft.Json;
using AppARM.Scenario;

namespace AppARM
{ 

    public partial class MainWindow : Window
    {


        private DataBase db;
        private static TcpClient newClient;
        private GetWeather getWeather = new GetWeather();
        private NetworkStream tcpStream;
        private APU apu = new APU();
        private Script script = new Script();
        private DispatcherTimer timer = new DispatcherTimer();
        private Files files = new Files();
        private Scenarios scenario = new Scenarios();
        private SettingForm settingApp;
        


      
        private static string staticIP = "192.168.1.40";
        private static IPAddress ip = IPAddress.Parse(staticIP);
        private static int staticPort = 30088;
        

        public MainWindow()
        {

            InitializeComponent();

            //Создание файла logs и apu
            files.InitFile();

            //Таймер 
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            
            //Тестовые кнопки убраны
            BDisconnect.IsEnabled = false;
            BConnect.IsEnabled = false;
            XML1.IsEnabled = false;
        

        }
        //____________________________ТАЙМЕР________________________________________________________
        void timer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = DateTime.Now.ToLongTimeString();
        }

        byte[] tmp;

        //__________________________________________________________________________________________

        //____________________________ОСНОВНЫЕ_КНОПКИ_______________________________________________

        //Открытие окна с базой данных
        private void BC_DataBase(object sender, RoutedEventArgs e)
        {  
            //Данные для БД
            string serverBD = Convert.ToString(Properties.Settings.Default.IP_adress);
            string portBD = Convert.ToString(Properties.Settings.Default.Port);
            string userBD = Convert.ToString(Properties.Settings.Default.Login_BD);
            string passwordBD = Convert.ToString(Properties.Settings.Default.Password_BD);

            DataBase dataBase = new DataBase(serverBD, portBD, userBD, passwordBD);
            DeviceDB deviceDB = new DeviceDB(dataBase);
            deviceDB.Show();
        }

        //Открытие окно с проверкой Метеостанций
        private void BС_CheckMeteo(object sender, RoutedEventArgs e)
        {
            CheckeMeteo checkMeteo = new CheckeMeteo();
            checkMeteo.Show();
        }
       
        //Отправка погоды НУЖНО ИЗМЕНИТЬ !!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private void BC_SendMeteo(object sender, RoutedEventArgs e)
        {
            string serverBD = Convert.ToString(Properties.Settings.Default.IP_adress);
            string portBD = Convert.ToString(Properties.Settings.Default.Port);
            string userBD = Convert.ToString(Properties.Settings.Default.Login_BD);
            string passwordBD = Convert.ToString(Properties.Settings.Default.Password_BD);


            DataBase dataBase = new DataBase(serverBD, portBD, userBD, passwordBD);
            //string server = "194.154.83.11";
            int port = 64700;
            //string ipSend = "127.0.0.1";
            //int portSend = 8000;
            //getWeather.ConnectMeteo(server, port, Properties.Settings.Default.IP_adress, Properties.Settings.Default.Port);
            //// вывести все ip из бд

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

        //настройки конфигураций 
        public async void BC_Settings(object sender, RoutedEventArgs e)
        {
            settingApp = new SettingForm();
            settingApp.Show();
        }


        CreateJsonRequest create =  new CreateJsonRequest();

        /*

            {
        "Name":"22",
        "Device_ip":"12",
        "Longitude":"32.22",
        "Latitude":"12.24",
        "Weather":
        {
            "Temperature":"22",
            "Wind_speed":"22",
            "Direction":"22",
            "Parameter": ""
        }
        */
        private string tableName = "Arm";

        //Для тестовых проверок 
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            scenario.Show();
        }

        private async void BC_Script(object sender, RoutedEventArgs e)
        {

            string serverBD = Convert.ToString(Properties.Settings.Default.IP_adress);
            string portBD = Convert.ToString(Properties.Settings.Default.Port);
            string userBD = Convert.ToString(Properties.Settings.Default.Login_BD);
            string passwordBD = Convert.ToString(Properties.Settings.Default.Password_BD);

            DataBase dataBase = new DataBase(serverBD, portBD, userBD, passwordBD);

            string t = "[";
            var ts = dataBase.GetDataBaseLong(tableName);

            try
            {
                int count = Convert.ToInt32(dataBase.CountDataInBD(tableName));
                if (ts != null)
                {
                    while (ts.Read())
                    {
                        Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7} ", ts.GetInt32(0), ts.GetString(1), ts.GetString(2), ts.GetString(3), ts.GetString(4), ts.GetString(5), ts.GetString(6), ts.GetString(7));

                        Console.WriteLine(t);
                        t += create.CreatJSON(ts.GetString(3), ts.GetString(1), ts.GetString(4), ts.GetString(5), ts.GetString(7), ts.GetString(8), ts.GetString(9), ts.GetString(6));

                        if (count != 1)
                        {
                            t += ",";
                        }
                        count--;
                        Console.WriteLine(t);
                    }
                }

                else
                {

                    dataBase.CreateTableApy(tableName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            //var t ="[" +create.CreatJSON("12", "232", 123, "22", "22.24", "45.24", "22", "22", "22", "22");
            // t = t + "," + create.CreatJSON("12", "232", 123, "22", "25.24", "35.24", "22", "22", "22", "22") + "]";
            Console.WriteLine(t);
            t += "]";
            Console.WriteLine(t);
            var url = "http://" + "127.0.0.1" + ":" + Convert.ToString(8000) + "/set_stations/";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(t);
            }
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
            var status = httpResponse.StatusCode.ToString();
            if (status == "OK")
            {
                //сообщение

            }
        }

        //__________________________________________________________________________________________

        //_____________________________СТАРЫЙ_КОД___________________________________________________

        private void SendFile()
        {
            /* string fileName = Environment.CurrentDirectory + @"\start.xml";
             Console.WriteLine(fileName);
             // Send file fileName to remote device
             //Console.WriteLine("Sending {0} to the host.", fileName);
             //client.SendFile(fileName);

             StreamWriter sWriter = new StreamWriter(tcpClient1.GetStream());

             byte[] bytes = File.ReadAllBytes(fileName);

             sWriter.WriteLine(bytes.Length.ToString());
             sWriter.Flush();

             sWriter.WriteLine(fileName);
             sWriter.Flush();

             Console.WriteLine("Sending file");
             tcpClient.Client.SendFile(fileName);
         */
        }
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
                files.ReadExeption(ex);
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
        private void DisconnectServer()
        {
            try
            {
                Console.WriteLine(newClient.Connected);
                newClient.Close();
                BDisconnect.IsEnabled = false;
                BConnect.IsEnabled = true;
            }
            catch (Exception ex)
            {
                files.ReadExeption(ex);
            }

        }

        //__________________________________________________________________________________________

        //____________________________ОТКЛЮЧЕННЫЕ_КНОПКИ____________________________________________


        //отключено
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
            DisconnectServer();
        }


        

        //__________________________________________________________________________________________

       
    }
}

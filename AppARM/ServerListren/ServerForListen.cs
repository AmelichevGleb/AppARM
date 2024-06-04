using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

using Newtonsoft.Json.Linq;
using AppARM.PostgresSQL;
using AppARM.Parser;
using AppARM.WeatherSokol;
using AppARM.FilesLogs;
using static AppARM.SettingApp.ServerReceiv;
using AppARM.Scenario;
using AppARM.Weather;

namespace AppARM.ServerListren
{

    //Сервер прослушки слушает входящие сообщения
    //Либо выгружает Базу Данных;
    //Либо делает запросы метеостанции 


    public class ServerForListen
    {

        public class ClientObject
        {

            private TcpClient client;
            private TcpClient newClientMeteo = null;
            private DataBaseMeteostation _dataBase = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
            private AlarmDB _alarmDataBase = new AlarmDB(serverBD, portBD, userBD, passwordBD);
            private ScenarioIndividual scenarioIndividual = new ScenarioIndividual();
            private Files files = new Files();

            static string serverBD = Convert.ToString(Properties.Settings.Default.IP_adress);
            static string portBD = Convert.ToString(Properties.Settings.Default.Port);
            static string userBD = Convert.ToString(Properties.Settings.Default.Login_BD);
            static string passwordBD = Convert.ToString(Properties.Settings.Default.Password_BD);

            static string ip4C = null;
            private byte[] Message = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 }; //Запрос к метеостанции
            private string needIP = "";
            private string needPort = "";

            public ClientObject(TcpClient tcpClient)
            {
                client = tcpClient;
            }
            static public string answer = "";




            public async void Process()
            {
                string imei = String.Empty;

                double temperature = 0;
                double windSpeed = 0;
                int directionWind = 0;

                byte[] bytes = new byte[8096];
                int i;
                NetworkStream stream = null;
                try
                {
                    stream = client.GetStream();
                    byte[] data = new byte[128]; // буфер для получаемых данных
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        string requests = Encoding.UTF8.GetString(bytes, 0, i);
                        Console.WriteLine(requests);

                        ParserAll parserAll = new ParserAll();
                        var request = parserAll.AddMassiveStringIP(requests);
                        Console.WriteLine("п = {0} , t = {1}", request.Item1, request.Item2);
                        if (request.Item2 == "")
                        {
                            await Task.Run(() =>
                            {
                                mainWindow.Dispatcher.Invoke((Action)(() => mainWindow.Test.Text += Convert.ToString(DateTime.Now) + " - " + "Запрос на получение базы данных" + '\n'));
                                files.ReadFile("запрос на получение базы данных", false);
                            });

                            Console.WriteLine("1 параметр");
                            string resultSTR = "";

                            string serverBD = Convert.ToString(Properties.Settings.Default.IP_adress);
                            string portBD = Convert.ToString(Properties.Settings.Default.Port);
                            string userBD = Convert.ToString(Properties.Settings.Default.Login_BD);
                            string passwordBD = Convert.ToString(Properties.Settings.Default.Password_BD);
                            DataBaseMeteostation dataBase = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);

                            var t = dataBase.GetDataBaseShort("meteostation");
                            while (t.Read())
                            {
                                resultSTR += Convert.ToString(t.GetInt32(0)) + ';' + t.GetString(1) + ';' + t.GetString(2) + ';' + t.GetString(3) + ';' + t.GetString(4) + ';' + t.GetString(5) + ';' + t.GetString(6) + '\n';
                            }
                            Console.WriteLine("Итоговая строка \n{0}", resultSTR);
                            var reply = Encoding.ASCII.GetBytes(resultSTR);
                            stream.Write(reply, 0, reply.Length);

                        }
                        else
                        {
                            // ip ; port 
                            try
                            {
                                await Task.Run(() =>
                                {

                                    mainWindow.Dispatcher.Invoke((Action)(() => mainWindow.Test.Text += Convert.ToString(DateTime.Now) + " - " + "Запрос на получение погоды с :" + request.Item1 + '\n'));
                                    string text = "запрос на получение погоды c метеостанции : " + Convert.ToString(request.Item1) + " " + Convert.ToString(request.Item2);
                                    files.ReadFile(text, false);
                                });

                                newClientMeteo = new TcpClient();
                                newClientMeteo.Connect(request.Item1, Convert.ToInt32(request.Item2));
                                NetworkStream tcpStream = newClientMeteo.GetStream();
                                tcpStream.Write(Message, 0, Message.Length);
                                byte[] msg = new byte[8096];
                                int count = tcpStream.Read(msg, 0, msg.Length);
                                ByteWeather byteWeather = new ByteWeather(msg);
                                var temp = byteWeather.ReturnPartWeater();


                                temperature = Convert.ToDouble(temp.Item1);
                                windSpeed = Convert.ToDouble(temp.Item2);
                                directionWind = Convert.ToInt32(temp.Item3);

                                Console.WriteLine("1: {0} , 2: {1} , 3: {2}", temp.Item1, temp.Item2, temp.Item3);
                                //string hex = BitConverter.ToString(bytes);
                                //data = Encoding.ASCII.GetString(bytes, 0, i);
                                //Console.WriteLine("{1}: Received: {0}", data, Thread.CurrentThread.ManagedThreadId);


                                string str = temp.Item1 + ';' + temp.Item2 + ';' + temp.Item3 + ';';
                                var reply = Encoding.ASCII.GetBytes(str);
                                stream.Write(reply, 0, reply.Length);
                                Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);
                            }
                            catch (Exception e)
                            {
                                string str = Convert.ToString(temperature) + ';' + Convert.ToString(windSpeed) + ';' + Convert.ToString(directionWind) + ';';
                                files.ReadFile("Метеостанция не отвечает", false);
                                var reply = Encoding.ASCII.GetBytes(str);
                                stream.Write(reply, 0, reply.Length);
                                Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                    if (client != null)
                        client.Close();
                }
            }
        }

        private static MainWindow mainWindow;
        private ServerListenRequest serverRequest;

        public class ServerListenRequest
        {
            public TcpListener server = null;
            TcpClient newClientMeteo = null;
            private Files files = new Files();
           
            private byte[] Message = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 }; //Запрос к метеостанции
            private volatile CancellationTokenSource _cts;
            public bool Active;

            public ServerListenRequest()
            {
                try
                {
                    string Ip = Properties.Settings.Default.IP_ServerListen;
                    string Port = Properties.Settings.Default.Port_ServerListen;
                    IPAddress localAddr = IPAddress.Parse(Ip);
                    server = new TcpListener(localAddr, Convert.ToInt32(Port));  //1

                    server.Start();
                    Active = true;
                    StartListener();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка запуска сервера опроса \n" +
                        "откройте файл exception.txt" +  "\nисправьте ошибку и перезагрузите программу");
                    files.ReadException(ex);
                }
            }
            public void StartListener()
            {
                try
                {
                    while (Active)
                    {
                        TcpClient client = server.AcceptTcpClient();
                        Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
                        t.Start(client);
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                    files.ReadException(e);
                    server.Stop();
                }
            }
            public async void HandleDeivce(Object obj)
            {
                TcpClient client = (TcpClient)obj;
                var stream = client.GetStream();
                
                double temperature = 0;
                double windSpeed = 0;
                int directionWind = 0;
                
                
                string data = null;
                byte[] bytes = new byte[8096];
                int i;

                try
                {
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // проверка на тип сообщения
                        string requests = Encoding.UTF8.GetString(bytes, 0, i);
                        Console.WriteLine(requests);

                        ParserAll parserAll = new ParserAll();
                        var request = parserAll.AddMassiveStringIP(requests);
                        Console.WriteLine("п = {0} , t = {1}", request.Item1, request.Item2);
                        if (request.Item2 == "")
                        {
                            await Task.Run(() =>
                            {
                                mainWindow.Dispatcher.Invoke((Action)(() => mainWindow.Test.Text += Convert.ToString(DateTime.Now) + " - " + "Запрос на получение базы данных" + '\n'));
                                files.ReadFile("запрос на получение базы данных", false);
                            });

                            Console.WriteLine("1 параметр");
                            string resultSTR = "";

                            string serverBD = Convert.ToString(Properties.Settings.Default.IP_adress);
                            string portBD = Convert.ToString(Properties.Settings.Default.Port);
                            string userBD = Convert.ToString(Properties.Settings.Default.Login_BD);
                            string passwordBD = Convert.ToString(Properties.Settings.Default.Password_BD);
                            DataBaseMeteostation dataBase = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);

                            var t = dataBase.GetDataBaseShort("meteostation");
                            while (t.Read())
                            {
                                resultSTR += Convert.ToString(t.GetInt32(0)) + ';' + t.GetString(1) + ';' + t.GetString(2) + ';' + t.GetString(3) + ';' + t.GetString(4) + ';' + t.GetString(5) + ';' + t.GetString(6) + '\n';
                            }
                            Console.WriteLine("Итоговая строка \n{0}", resultSTR);
                            var reply = Encoding.ASCII.GetBytes(resultSTR);
                            stream.Write(reply, 0, reply.Length);

                        }
                        else
                        {
                            // ip ; port 
                            try
                            {
                                await Task.Run(() =>
                                {

                             //       mainWindow.Dispatcher.Invoke((Action)(() => mainWindow.Test.Text += Convert.ToString(DateTime.Now) + " - " + "Запрос на получение погоды с :" + request.Item1 + '\n'));
                                    string text = "запрос на получение погоды c метеостанции : " + Convert.ToString(request.Item1) + " " + Convert.ToString(request.Item2);
                                    files.ReadFile(text, false);
                                });

                                newClientMeteo = new TcpClient();
                                newClientMeteo.Connect(request.Item1, Convert.ToInt32(request.Item2));
                                NetworkStream tcpStream = newClientMeteo.GetStream();
                                tcpStream.Write(Message, 0, Message.Length);
                                byte[] msg = new byte[8096];
                                int count = tcpStream.Read(msg, 0, msg.Length);
                                ByteWeather byteWeather = new ByteWeather(msg);
                                var temp = byteWeather.ReturnPartWeater();


                                temperature = Convert.ToDouble(temp.Item1);
                                windSpeed = Convert.ToDouble(temp.Item2);
                                directionWind = Convert.ToInt32(temp.Item3);

                                Console.WriteLine("1: {0} , 2: {1} , 3: {2}", temp.Item1, temp.Item2, temp.Item3);
                                string hex = BitConverter.ToString(bytes);
                                data = Encoding.ASCII.GetString(bytes, 0, i);
                                Console.WriteLine("{1}: Received: {0}", data, Thread.CurrentThread.ManagedThreadId);


                                string str = temp.Item1 + ';' + temp.Item2 + ';' + temp.Item3 + ';';
                                var reply = Encoding.ASCII.GetBytes(str);
                                stream.Write(reply, 0, reply.Length);
                                Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);
                            }
                            catch (Exception e)
                            {
                                string str = Convert.ToString(temperature) + ';' + Convert.ToString(windSpeed) + ';' + Convert.ToString(directionWind) + ';';
                                files.ReadFile("Метеостанция не отвечает", false);
                                var reply = Encoding.ASCII.GetBytes(str);
                                stream.Write(reply, 0, reply.Length);
                                Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e.ToString());
                    files.ReadException(e);
                    client.Close();
                }

            }
            public void ServerStop()
            {
                Active = false;
                server.Stop();
            }
        }






        public bool active = true;
        TcpListener listener = new TcpListener(IPAddress.Parse(Properties.Settings.Default.IP_ServerListen), Convert.ToInt32(Properties.Settings.Default.Port_ServerListen));
        public List<TcpClient> listConnectedClients = new List<TcpClient>();
        private Files files = new Files();
        public void StartServerListen(int _a, MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;

            Thread clientThread = new Thread(new ThreadStart(startS));
            if (_a == 1)
            {
                files.ReadFile("Запуск сервера listen ", false);
                active = true;
                clientThread.Start();
            }
            else
            {
                files.ReadFile("Остановка сервера listen ", false);
                try
                {
                    clientThread.Abort();
                    listener.Stop();
                    active = false;

                    foreach (TcpClient client in listConnectedClients)
                    {
                        client.Close();
                    }
                }
                catch (Exception e) { MessageBox.Show(e.Message); }

            }
        }

        void startS()
        {
            try
            {
                listener.Start();

                while (active)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    listConnectedClients.Add(client);
                    ClientObject clientObject = new ClientObject(client);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch //(Exception ex)
            {

            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }

    }
}

using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;

using AppARM.WeatherSokol;
using AppARM.Parser;
using AppARM.PostgresSQL;
using System.Threading.Tasks;
using System.Windows.Threading;
using AppARM.ServerListren;
using System.IO;
using System.Collections.Specialized;
using System.Windows.Markup;
using AppARM.Scenario;
using AppARM.FilesLogs;
using static System.Net.WebRequestMethods;
using System.Runtime.Remoting.Messaging;
using System.Collections.Generic;
using AppARM.Weather;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;


namespace AppARM.SettingApp
{

    // СЕРВЕР ПОЛУЧАЕТ ДАННЫЕ С ДАТЧИКОВ И РЕАГИРУЕТ НА ВСЕ


    internal class ServerReceiv
    {
        private static MainWindow mainWindow;
        private static ParserAll parser = new ParserAll();


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

            public async Task EmergencyCheckAddTextAsync(TcpClient _client)
            {
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        var message = new StringBuilder();
                        message.AppendFormat("Пришел сигнал ЧС с устройства с IP {0}", Convert.ToString(_client.Client.RemoteEndPoint));
                        files.ReadFile(Convert.ToString(message), false);
                        Console.WriteLine("ЧС");

                        mainWindow.Dispatcher.Invoke((Action)(() => mainWindow.MessageServer.Text += Convert.ToString(DateTime.Now) + " - " + Convert.ToString(message) + '\n'));
                    }));

                    Thread.Sleep(100);
                });
            }
            public void Send(string _ip, string _port, string _message)
            {
             
                       try
                        {
                            TcpClient tcpClient = new TcpClient();
                            tcpClient.Connect(_ip, Convert.ToInt32(_port));
                            NetworkStream stream = tcpClient.GetStream();
                            var requestData = Encoding.UTF8.GetBytes(_message);
                            stream.Write(requestData, 0, requestData.Length);
                            stream.Close();
                            tcpClient.Close();
                        }
                        catch (Exception ex)
                        {
                            string msg = "Неудалось оповестить " + _ip + "\n";
                            mainWindow.MessageServer.Text += msg;
                            files.ReadFile("Неудалось оповестить " + _ip, false);
                            files.ReadFile(Convert.ToString(ex), true);
                        }
            }
            public async Task NeedInfo(string[] _allIP4C, string[] _allPort4C, string _message)
            {
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke((Action)(async () =>
                    {
                        string value = "";
                        try
                        {
                            for (int i = 0; i < _allIP4C.Length - 1; i++)
                            {
                                value = _allIP4C[i];
                                var message = new StringBuilder();
                                message.AppendFormat("надо оповестить {0} {1}", _allIP4C[i], _allPort4C[i]);
                                files.ReadFile(Convert.ToString(message), false);
                                mainWindow.MessageServer.Text += message+ "\n";
                                await Task.Run(async () =>
                                {
                                try
                                {
  
                                        TcpClient tcpClient = new TcpClient();
                                    tcpClient.Connect(_allIP4C[i], Convert.ToInt32(_allPort4C[i]));
                                    NetworkStream stream = tcpClient.GetStream();
                                    var requestData = Encoding.UTF8.GetBytes(_message);
                                    stream.Write(requestData, 0, requestData.Length);
                                    stream.Close();
                                    tcpClient.Close();
                                        files.ReadFile("оповещение дошло " + _allIP4C[i], false);
                                    }
                                catch (Exception ex)
                                {
                                        await BadMessagAddTextAsync(_allIP4C[i], ex);
                                }
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            files.ReadFile("Неудалось оповестить " + value, false);
                            files.ReadFile(Convert.ToString(ex), true);
                        }
                    }));
                    Thread.Sleep(100);
                });
            }

            public async Task BadMessagAddTextAsync(string _ip, Exception ex)
            {
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        var message = new StringBuilder();
                        message.AppendFormat("Неудалось оповестить {0}", _ip);
                        files.ReadFile("Неудалось оповестить " + _ip, false);
                        files.ReadFile(Convert.ToString(ex), true);
                        mainWindow.MessageServer.Text += message + "\n";
                    }));
                    Thread.Sleep(100);
                });
            }

            public async Task WeatherAddTextAsync(string _temperature, string _windSpeed, string _directionWind)
            {
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        var message = new StringBuilder();
                        message.AppendFormat("Погода на месте тем. =  {0} v-вет. = {1} напр ={2} \n", _temperature, _windSpeed, _directionWind);
                        files.ReadFile(Convert.ToString(message), false);
                        mainWindow.MessageServer.Text += message;
                    }));
                    Thread.Sleep(100);
                });
            }
            public async void Process()
            {
                string imei = String.Empty;

                string temperature;
                string windSpeed;
                string directionWind;

               
                byte[] bytes = new byte[4];
                int i;
                NetworkStream stream = null;
                try
                {
                    stream = client.GetStream();
                    byte[] data = new byte[128]; // буфер для получаемых данных
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        string requests = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                        byte[] B4c = new byte[] { 0x00, 0x02, 0x01, 0xE2 };

                        if (BitConverter.ToString(bytes) == BitConverter.ToString(B4c))  //Проверка пришел сигнал ЧС
                        {
                            await  EmergencyCheckAddTextAsync(client);
                            ip4C = parser.AddMassiveStringIP(Convert.ToString(client.Client.RemoteEndPoint), "1");
                            var t = _dataBase.returnIp4CDevice("meteostation", ip4C);
                            needIP = t.Item1;
                            needPort = t.Item2;
                            Console.WriteLine(needPort);
                            try
                            {
                                newClientMeteo = new TcpClient();
                                newClientMeteo.Connect(needIP, Convert.ToInt32(needPort));
                                NetworkStream tcpStream = newClientMeteo.GetStream();
                                tcpStream.Write(Message, 0, Message.Length);
                                byte[] msg = new byte[8096];
                                int count = tcpStream.Read(msg, 0, msg.Length);
                                ByteWeather byteWeather = new ByteWeather(msg);
                                var temp = byteWeather.ReturnPartWeater();

                                temperature = temp.Item1;
                                windSpeed = temp.Item2;
                                directionWind = temp.Item3;

                                await  WeatherAddTextAsync(temperature, windSpeed, directionWind);
                                Console.WriteLine("1: {0} , 2: {1} , 3: {2}", temp.Item1, temp.Item2, temp.Item3);

                                //_____________________________

                                /*
                                //createJsonRequest.CreatJSON(needIP, "xxx", "xxx", "xxx", temperature, windSpeed, directionWind, "null");

                                //                                var url = "http://" + Properties.Settings.Default.IP_Server_maps + ":" + Convert.ToInt32(Properties.Settings.Default.Port_Server_maps) + "/set_stations/";
                                //                              var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                                //                            httpRequest.Method = "POST";
                                //                          httpRequest.Accept = "application/json";
                                //                        httpRequest.ContentType = "application/json";
                                //                      using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                                //                    {
                                //                      streamWriter.Write(json);
                                //                }
                                //              var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                                //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                //          {
                                //            var result = streamReader.ReadToEnd();
                                //      }
                                //    var status = httpResponse.StatusCode.ToString();
                                //  if (status == "OK")
                                //{
                                //сообщение
                                //}
                                */
                            }
                            catch (Exception e)
                            {
                                await Task.Run(() =>
                                {
                                    Application.Current.Dispatcher.Invoke((Action)(() =>
                                    {
                                        if (needIP == "")
                                        {
                                            var message = new StringBuilder();
                                            message.AppendFormat("нет таких метеостанций \n");
                                            files.ReadFile(Convert.ToString(message), false);
                                            mainWindow.MessageServer.Text += message;
                                        }
                                        else
                                        {
                                            var message = new StringBuilder();
                                            message.AppendFormat("не отвечает метеостанция \n");
                                            files.ReadFile("не отвечает метеостанция", false);
                                            mainWindow.MessageServer.Text += message;
                                        }
                                    }));

                                    Thread.Sleep(100);
                                });

                            }

                            var strIP = _alarmDataBase.GettingListOfDevices("alarmdb", ip4C);
                            var ts = parser.MassiveIPDanger(strIP.Item1);
                            var strscenario = scenarioIndividual.CommandGroup(true, strIP.Item2);
                            await NeedInfo(ts.Item1, ts.Item2, strscenario);
                            Console.WriteLine(strscenario);


                        }

                        Thread.Sleep(1000);
                        string str = "Hey Device!";
                        Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str);
                        stream.Write(reply, 0, reply.Length);
                        Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);
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



        private Files files = new Files();
        public List<TcpClient> listConnectedClients = new List<TcpClient>();
        public bool active = true;
        TcpListener listener = new TcpListener(IPAddress.Parse(Properties.Settings.Default.Ip_Server_Device), Convert.ToInt32(Properties.Settings.Default.Port_Server_Device));


        public  void StartServerReceiver(int _a, MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
           
            Thread clientThread = new Thread(new ThreadStart(startS));
            if (_a == 1)
            {
                files.ReadFile("Запуск сервера receiver", false);
                active = true;
                clientThread.Start();
            }
            else
            {
                files.ReadFile("Остановка сервера receiver", false);
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

//public void mainStartServerReceiver(MainWindow _mainWindows)
//{

//    //try
//    //{
//    //    mainWindow = _mainWindows;
//    //    // serverRequest.Port1 = Properties.Settings.Default.Port_ServerListen;
//    //    Console.WriteLine("Запуск сервера для опроса ...");
//    //    CancellationTokenSource token = new();
//    //    //serverRequest = new ServerListenRequest();
//    //    threadServerListen = new(
//    //        () => serverReceive = new Server("127.0.0.1", 5555));

//    //    threadServerListen.Start();
//    //}
//    //catch (Exception ex)
//    //{
//    //    MessageBox.Show("Ошибка запуска сервера прослушивания \n" +
//    //        "откройте файл exception.txt" + "\nисправьте ошибку и перезагрузите программу");
//    //    files.ReadException(ex);
//    //}
//}

//public async Task EmergencyCheckAddTextAsync(TcpClient _client)
//{
//    await Task.Run(() =>
//    {
//        Application.Current.Dispatcher.Invoke((Action)(() =>
//        {
//            var message = new StringBuilder();
//            message.AppendFormat("Пришел сигнал ЧС с устройства с IP {0}", Convert.ToString(_client.Client.RemoteEndPoint));
//            files.ReadFile(Convert.ToString(message), false);
//            Console.WriteLine("ЧС");

//            mainWindow.Dispatcher.Invoke((Action)(() => mainWindow.MessageServer.Text += Convert.ToString(DateTime.Now) + " - " + Convert.ToString(message) + '\n'));
//        }));

//        Thread.Sleep(100);
//   });
//}

//public async Task WeatherAddTextAsync(string _temperature, string _windSpeed, string _directionWind)
//{
//    await Task.Run(() =>
//    {
//        Application.Current.Dispatcher.Invoke((Action)(() =>
//        {
//            var message = new StringBuilder();
//            message.AppendFormat("Погода на месте тем. =  {0} v-вет. = {1} напр ={2} \n", _temperature, _windSpeed, _directionWind);
//            files.ReadFile(Convert.ToString(message), false);
//            mainWindow.MessageServer.Text += message;
//        }));
//        Thread.Sleep(100);
//    });
//}

//кого надо оповестить.
//public async Task NeedInfo(string[] _allIP4C,string[] _allPort4C, string _message)
//{
//    await Task.Run(() =>
//    {
//        Application.Current.Dispatcher.Invoke((Action)(() =>
//        {
//            string value ="";
//            try
//            {
//                for (int i = 0; i < _allIP4C.Length; i++)
//                {
//                    value = _allIP4C[i];
//                    var message = new StringBuilder();
//                    message.AppendFormat("надо оповестить {0} {1}\n", _allIP4C[i], _allPort4C[i]);
//                    files.ReadFile(Convert.ToString(message), false);
//                    mainWindow.MessageServer.Text += message;
//                    try {
//                        TcpClient tcpClient = new TcpClient();
//                        tcpClient.Connect(_allIP4C[i], Convert.ToInt32(_allPort4C[i]));
//                        NetworkStream stream = tcpClient.GetStream();
//                        var requestData = Encoding.UTF8.GetBytes(_message);
//                        stream.Write(requestData, 0, requestData.Length);
//                        stream.Close();
//                        tcpClient.Close();
//                    }
//                    catch (Exception ex)
//                    {
//                        files.ReadFile("Неудалось оповестить " + value, false);
//                        files.ReadFile(Convert.ToString(ex), true);
//                    }
//                }
//            }
//            catch (Exception ex) {            
//                files.ReadFile("Неудалось оповестить " + value, false);
//                files.ReadFile(Convert.ToString(ex), true);
//            }
//        }));
//        Thread.Sleep(100);
//    });
//}

//public async void HandleDeivce(Object obj)
//{
//    TcpClient client = (TcpClient)obj;
//    var stream = client.GetStream();
//    string imei = String.Empty;

//    string temperature;
//    string windSpeed;
//    string directionWind;

//    string data = null;
//    byte[] bytes = new byte[4];
//    int i;
//    //try
//    //{
//    //    //MessageServer
//    //    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
//    //    {
//    //        string requests = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
//    //        byte[] B4c = new byte[] { 0x00, 0x02, 0x01, 0xE2 };

//    //        if (BitConverter.ToString(bytes) == BitConverter.ToString(B4c))  //Проверка пришел сигнал ЧС
//    //        {
//    //         //   EmergencyCheckAddTextAsync(client);
//    //            ip4C = parser.AddMassiveStringIP(Convert.ToString(client.Client.RemoteEndPoint), "1");
//    //         //   var t = _dataBase.returnIp4CDevice("meteostation", ip4C);
//    //           // needIP = t.Item1;
//    //         //   needPort = t.Item2;
//    //            Console.WriteLine(needPort);
//    //            try
//    //            {
//    //                newClientMeteo = new TcpClient();
//    //                newClientMeteo.Connect(needIP, Convert.ToInt32(needPort));
//    //                NetworkStream tcpStream = newClientMeteo.GetStream();
//    //                tcpStream.Write(Message, 0, Message.Length);
//    //                byte[] msg = new byte[8096];
//    //                int count = tcpStream.Read(msg, 0, msg.Length);
//    //                ByteWeather byteWeather = new ByteWeather(msg);
//    //                var temp = byteWeather.ReturnPartWeater();


//    //                temperature = temp.Item1;
//    //                windSpeed = temp.Item2;
//    //                directionWind = temp.Item3;

//    //                WeatherAddTextAsync(temperature, windSpeed, directionWind);
//    //                Console.WriteLine("1: {0} , 2: {1} , 3: {2}", temp.Item1, temp.Item2, temp.Item3);


//    //                //Запуск оповещения 
//    //                //запрос к бд по IP

//    //                //_____________________________

//    //                /*
//    //                //createJsonRequest.CreatJSON(needIP, "xxx", "xxx", "xxx", temperature, windSpeed, directionWind, "null");

//    //                //                                var url = "http://" + Properties.Settings.Default.IP_Server_maps + ":" + Convert.ToInt32(Properties.Settings.Default.Port_Server_maps) + "/set_stations/";
//    //                //                              var httpRequest = (HttpWebRequest)WebRequest.Create(url);
//    //                //                            httpRequest.Method = "POST";
//    //                //                          httpRequest.Accept = "application/json";
//    //                //                        httpRequest.ContentType = "application/json";
//    //                //                      using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
//    //                //                    {
//    //                //                      streamWriter.Write(json);
//    //                //                }
//    //                //              var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
//    //                //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
//    //                //          {
//    //                //            var result = streamReader.ReadToEnd();
//    //                //      }
//    //                //    var status = httpResponse.StatusCode.ToString();
//    //                //  if (status == "OK")
//    //                //{
//    //                //сообщение
//    //                //}
//    //                */
//    //            }
//    //            catch (Exception e)
//    //            {
//    //                await Task.Run(() =>
//    //                {
//    //                    Application.Current.Dispatcher.Invoke((Action)(() =>
//    //                    {
//    //                        if (needIP == "")
//    //                        {
//    //                            var message = new StringBuilder();
//    //                            message.AppendFormat("нет таких метеостанций \n");
//    //                            files.ReadFile(Convert.ToString(message), false);
//    //                            mainWindow.MessageServer.Text += message;
//    //                        }
//    //                        else
//    //                        {
//    //                            var message = new StringBuilder();
//    //                            message.AppendFormat("не отвечает метеостанция \n");
//    //                            files.ReadFile("не отвечает метеостанция", false);
//    //                            mainWindow.MessageServer.Text += message;
//    //                        }
//    //                    }));

//    //                    Thread.Sleep(100);
//    //                });

//    //            }

//    //            var strIP = _alarmDataBase.GettingListOfDevices("alarmdb", ip4C);

//    //            var ts = parser.MassiveIPDanger(strIP.Item1);

//    //            var strscenario = scenarioIndividual.CommandGroup(true, strIP.Item2);
//    //            NeedInfo(ts.Item1,ts.Item2, strscenario);
//    //            Console.WriteLine(strscenario);
//    //        }

//    //        Thread.Sleep(1000);
//    //        string str = "Hey Device!";
//    //        Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str);
//    //        stream.Write(reply, 0, reply.Length);
//    //        Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);
//    //    }
//    //}
//    //catch (Exception ex) { }
//}


//public void StartListener()
//    {
//try
//{
//    while (true)
//    {
//      //  TcpClient client = server.AcceptTcpClient();
//        Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
//     //   t.Start(client);
//    }
//}
//catch (SocketException e)
//{
//   // files.ReadException(e);
//    Console.WriteLine("SocketException: {0}", e);
//    //server.Stop();
//}
//}

//class Server
//{
//    //СЮДА 1
//    //private TcpListener server = null;
//    //private TcpClient newClientMeteo = null;
//    //private DataBaseMeteostation _dataBase = new DataBaseMeteostation(serverBD, portBD, userBD, passwordBD);
//    //private AlarmDB _alarmDataBase = new AlarmDB(serverBD, portBD, userBD, passwordBD);
//    //private readonly ServerReceiv ex;
//    //private ScenarioIndividual scenarioIndividual = new ScenarioIndividual();
//    //private Files files = new Files();

//    //static string serverBD = Convert.ToString(Properties.Settings.Default.IP_adress);
//    //static string portBD = Convert.ToString(Properties.Settings.Default.Port);
//    //static string userBD = Convert.ToString(Properties.Settings.Default.Login_BD);
//    //static string passwordBD = Convert.ToString(Properties.Settings.Default.Password_BD);

//    //static string ip4C = null;
//    //private byte[] Message = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 }; //Запрос к метеостанции
//    //private string needIP = "";
//    //private string needPort = "";


//    //public Server(string ip, int port)
//    //{
//    //    try
//    //    {

//    //        IPAddress localAddr = IPAddress.Parse(ip);
//    //        server = new TcpListener(localAddr, port);
//    //        server.Start();
//    //        StartListener();
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        MessageBox.Show("Ошибка запуска сервера прослушивания \n" +
//    //            "откройте файл exception.txt" + "\nисправьте ошибку и перезагрузите программу");
//    //        files.ReadException(ex);
//    //    }
//    //}




//}
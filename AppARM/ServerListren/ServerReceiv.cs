using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
<<<<<<< HEAD
using System.Text.Json;
=======

>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
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
<<<<<<< HEAD
using System.Linq;
using static AppARM.SettingApp.ServerReceiv.ClientObject;
using Mono.Security.Interface;
=======
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180


namespace AppARM.SettingApp
{

    // СЕРВЕР ПОЛУЧАЕТ ДАННЫЕ С ДАТЧИКОВ И РЕАГИРУЕТ НА ВСЕ


    internal class ServerReceiv
    {
        private static MainWindow mainWindow;
        private static ParserAll parser = new ParserAll();
<<<<<<< HEAD
        public static List<meteo> sList = new List<meteo>();
=======

>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180

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

<<<<<<< HEAD
            public struct meteo
            {
                public string ip { get; set; }
                public string longitude { get; set; }
                public DateTime dataEnd { get; set; }

            }

=======
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            public ClientObject(TcpClient tcpClient)
            {
                client = tcpClient;
            }
            static public string answer = "";

<<<<<<< HEAD

            public async Task PostTextAsync(bool type,string ip4C)
=======
            public async Task EmergencyCheckAddTextAsync(TcpClient _client)
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            {
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
<<<<<<< HEAD
                        string area = "";

                           // var t = Post("1", _dataBase.returnLongitudeMeteostation(ip4C));
                        if (type)
                        {
                            area =  "Удалось перекрасить область по " + ip4C;
                            files.ReadFile("Удалось перекрасить область по " + ip4C, false);
                        }
                        else
                        {
                            area = "Не удалось перекрасить область по " + ip4C ;
                            files.ReadFile("Не удалось перекрасить область по " + ip4C , false);
                        }
                        mainWindow.Dispatcher.Invoke((Action)(() => mainWindow.MessageServer.Text += Convert.ToString(DateTime.Now) + " - " + Convert.ToString(area) + '\n'));
                    }));
                });
             }
            public async Task EmergencyCheckAddTextAsync(TcpClient _client, string ip4C)
            {
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                     
                        var checkAdd = MagicAdd(sList, _dataBase.returnLongitudeMeteostation(ip4C),Convert.ToString(client.Client.RemoteEndPoint));
                        if (checkAdd)
                        {
                            
                            string area = "";
        
                            var message = new StringBuilder();
                            message.AppendFormat("Пришел сигнал ЧС с устройства с IP {0}" + area, Convert.ToString(_client.Client.RemoteEndPoint));

                            files.ReadFile(Convert.ToString(message+ area), false);
                            Console.WriteLine("ЧС");
                            mainWindow.Dispatcher.Invoke((Action)(() => mainWindow.MessageServer.Text += Convert.ToString(DateTime.Now) + " - " + Convert.ToString(message) + '\n'));
                        }
                        else
                        {
                            var message = new StringBuilder();
                            message.AppendFormat("Тебе пока рано", Convert.ToString(_client.Client.RemoteEndPoint));
                            files.ReadFile(Convert.ToString(message), false);
                            mainWindow.Dispatcher.Invoke((Action)(() => mainWindow.MessageServer.Text += Convert.ToString(DateTime.Now) + " - " + Convert.ToString(message) + '\n'));
                        }
=======
                        var message = new StringBuilder();
                        message.AppendFormat("Пришел сигнал ЧС с устройства с IP {0}", Convert.ToString(_client.Client.RemoteEndPoint));
                        files.ReadFile(Convert.ToString(message), false);
                        Console.WriteLine("ЧС");

                        mainWindow.Dispatcher.Invoke((Action)(() => mainWindow.MessageServer.Text += Convert.ToString(DateTime.Now) + " - " + Convert.ToString(message) + '\n'));
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
                    }));

                    Thread.Sleep(100);
                });
            }
            public void Send(string _ip, string _port, string _message)
            {
             
                       try
                        {
<<<<<<< HEAD
                           
=======
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
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
<<<<<<< HEAD
                            string msg = "Не удалось оповестить " + _ip + "\n";
                            mainWindow.MessageServer.Text += Convert.ToString(DateTime.Now) + " - " +  msg;
                            files.ReadFile("Не удалось оповестить " + _ip, false);
                            files.ReadFile(Convert.ToString(ex), true);
                        }
            }
            public async Task NeedInfo(string[] _allIP4C, string[] _allPort4C, byte[] _message)
=======
                            string msg = "Неудалось оповестить " + _ip + "\n";
                            mainWindow.MessageServer.Text += msg;
                            files.ReadFile("Неудалось оповестить " + _ip, false);
                            files.ReadFile(Convert.ToString(ex), true);
                        }
            }
            public async Task NeedInfo(string[] _allIP4C, string[] _allPort4C, string _message)
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            {
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke((Action)(async () =>
                    {
                        string value = "";
                        try
                        {
<<<<<<< HEAD
                           // for (int i = 0; i < _allIP4C.Length - 1; i++)
                                for (int i = 0; i < _allIP4C.Length ; i++)
                                {
                                value = _allIP4C[i];
                                var message = new StringBuilder();
                                message.AppendFormat("Надо оповестить {0} {1}", _allIP4C[i], _allPort4C[i]);
                                files.ReadFile(Convert.ToString(message), false);
                                mainWindow.MessageServer.Text += Convert.ToString(DateTime.Now) + " - " + message + "\n";
=======
                            for (int i = 0; i < _allIP4C.Length - 1; i++)
                            {
                                value = _allIP4C[i];
                                var message = new StringBuilder();
                                message.AppendFormat("надо оповестить {0} {1}", _allIP4C[i], _allPort4C[i]);
                                files.ReadFile(Convert.ToString(message), false);
                                mainWindow.MessageServer.Text += message+ "\n";
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
                                await Task.Run(async () =>
                                {
                                try
                                {
  
<<<<<<< HEAD
                                    TcpClient tcpClient = new TcpClient();
                                    tcpClient.Connect(_allIP4C[i], Convert.ToInt32(_allPort4C[i]));
                                    NetworkStream stream = tcpClient.GetStream();
                                 
                                    stream.Write(_message, 0, _message.Length);
                                    stream.Close();
                                    tcpClient.Close();
                                    files.ReadFile("оповещение дошло " + _allIP4C[i], false);
                                   await GoodMessagAddTextAsync(_allIP4C[i]);
=======
                                        TcpClient tcpClient = new TcpClient();
                                    tcpClient.Connect(_allIP4C[i], Convert.ToInt32(_allPort4C[i]));
                                    NetworkStream stream = tcpClient.GetStream();
                                    var requestData = Encoding.UTF8.GetBytes(_message);
                                    stream.Write(requestData, 0, requestData.Length);
                                    stream.Close();
                                    tcpClient.Close();
                                        files.ReadFile("оповещение дошло " + _allIP4C[i], false);
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
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
<<<<<<< HEAD
                            files.ReadFile("Не удалось оповестить " + value, false);
                            files.ReadFile(Convert.ToString(ex), true);
                        }
                    }));
                    Thread.Sleep(1000);
=======
                            files.ReadFile("Неудалось оповестить " + value, false);
                            files.ReadFile(Convert.ToString(ex), true);
                        }
                    }));
                    Thread.Sleep(100);
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
                });
            }

            public async Task BadMessagAddTextAsync(string _ip, Exception ex)
            {
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        var message = new StringBuilder();
<<<<<<< HEAD
                        message.AppendFormat("Не удалось оповестить {0}", _ip);
                        files.ReadFile("Не удалось оповестить " + _ip, false);
                        files.ReadFile(Convert.ToString(ex), true);
                        mainWindow.MessageServer.Text += Convert.ToString(DateTime.Now) + " - " +  message + "\n";
                    }));
                    Thread.Sleep(100);
                });
            }
            public async Task GoodMessagAddTextAsync(string _ip)
            {
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        var message = new StringBuilder();
                        message.AppendFormat("Удалось оповестить {0}", _ip);
                        files.ReadFile("Удалось оповестить " + _ip, false);
                       
=======
                        message.AppendFormat("Неудалось оповестить {0}", _ip);
                        files.ReadFile("Неудалось оповестить " + _ip, false);
                        files.ReadFile(Convert.ToString(ex), true);
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
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
<<<<<<< HEAD

=======
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
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
<<<<<<< HEAD
                       
=======
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
                        string requests = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                        byte[] B4c = new byte[] { 0x00, 0x02, 0x01, 0xE2 };

                        if (BitConverter.ToString(bytes) == BitConverter.ToString(B4c))  //Проверка пришел сигнал ЧС
                        {
<<<<<<< HEAD
                            
                            //
                          
                            if (!CheckElementList(sList, Convert.ToString(client.Client.RemoteEndPoint))) {
                                ip4C = parser.AddMassiveStringIP(Convert.ToString(client.Client.RemoteEndPoint), "1");
                                await EmergencyCheckAddTextAsync(client, ip4C);
                                var  t1  = Post("2", _dataBase.returnLongitudeMeteostation(ip4C));

                                await PostTextAsync(t1 , ip4C);
                                /*

                                */
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

                                    await WeatherAddTextAsync(temperature, windSpeed, directionWind);
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
                                                message.AppendFormat("Нет таких метеостанций \n");
                                                files.ReadFile(Convert.ToString(message), false);
                                                mainWindow.MessageServer.Text += Convert.ToString(DateTime.Now) + " - " + message;
                                            }
                                            else
                                            {
                                                var message = new StringBuilder();
                                                message.AppendFormat("Не отвечает метеостанция \n");
                                                files.ReadFile("Не отвечает метеостанция", false);
                                                mainWindow.MessageServer.Text += Convert.ToString(DateTime.Now) + " - " +  message;
                                            }
                                        }));

                                        Thread.Sleep(100);
                                    });
                                }
                            

                            var strIP = _alarmDataBase.GettingListOfDevices("alarmdb", ip4C);
                            var ts = parser.MassiveIPDanger(strIP.Item1);
                            
                            //ааа
                            var strscenario = scenarioIndividual.CommandGroup(false, strIP.Item2);
                            Thread.Sleep(100);
                            await NeedInfo(ts.Item1, ts.Item2, strscenario);
                            Console.WriteLine(strscenario);
                            }
=======
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

>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180

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

<<<<<<< HEAD
        private static Files files = new Files();
        public List<TcpClient> listConnectedClients = new List<TcpClient>();
        public bool active = true;
        TcpListener listener = new TcpListener(IPAddress.Parse(Properties.Settings.Default.Ip_Server_Device), Convert.ToInt32(Properties.Settings.Default.Port_Server_Device));
        
        //Функция работает
        //функция проверки наличия элемента в списке
        //ВОПРОС ЛОГИРОВАНИЯ
        private static bool CheckElementList(List<meteo> _sList, string data)
        {
            Console.WriteLine("\nExists: Part with Id={0}: {1}", data,
                  _sList.Exists(x => x.ip == data));
            return _sList.Exists(x => x.ip == data);
        }

        //Функция работает 
        //функция удаление элемента из списка
        //ВОПРОС ЛОГИРОВАНИЯ
        private static void RemoveElementList(List<meteo> _sList, string data)
        {
            var itemToRemove = _sList.Single(r => r.ip == data);
            _sList.Remove(itemToRemove);
        }

        //ВОПРОС ЛОГИРОВАНИЯ

        public static bool MagicAdd(List<meteo> _sList, string _longitude,string data)
        {
            bool temp = CheckElementList(_sList, data);
            if (!temp)
            {
                Console.WriteLine("нету");
                DateTime tempData = DateTime.Now;
                _sList.Add(new meteo() { ip = data, longitude =  _longitude, dataEnd = tempData.AddMinutes(10) });
              
                return true;// второй параметр T
            }
            else
            {
                Console.WriteLine("Есть");
                return  false;
                //сравнить даты 
            }
            //Thread.Sleep(60000);

        }

        //ВОПРОС ЛОГИРОВАНИЯ

        public static  void DeleteForTime(List<meteo> _sList)
        {
            while (true)
            {
                Console.WriteLine("---------------------------------");
                foreach (meteo aPart in _sList.ToList())
                {

                    Console.WriteLine(aPart.ip + " " + aPart.longitude + " " + aPart.dataEnd);
                    if (aPart.dataEnd <= DateTime.Now)
                    {
                        Console.WriteLine("Пришло время удалить {0} \n отправь пост запрос пожалуйста", aPart.ip); RemoveElementList(_sList, aPart.ip);
                        Application.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            var message = new StringBuilder();
                          
                           // var postTemp =  Post("2", aPart.longitude);
                           // if (postTemp) { message.AppendFormat("область будет перекрашена "); }
                           // else { message.AppendFormat("неудалось перекрасить область в штатный "); }
                            //files.ReadFile("Неудалось оповестить " + _ip, false);
                            //files.ReadFile(Convert.ToString(ex), true);
                            mainWindow.MessageServer.Text += message + "\n";
                        }));
                    }
                  

                }
                Thread.Sleep(600000);
            }
        }

        class PostClass
        {
            public string Type { get; }
            public string IP { get; set; }
            public PostClass(string _type, string _longitude)
            {
                Type = _type;
                IP = _longitude;
            }
        }

        private static bool Post(string _type, string _longitude)
        {
            try
            {
                string IpMap = Properties.Settings.Default.IP_Server_maps;
                string url = "http://" + IpMap + "/index.html";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //var post = new StringBuilder();
                    //post.AppendFormat("{\"Type\":\"{0}\",\"IP\":\"{1}\"}", _type, _longitude);
                    PostClass postClass = new PostClass(_type, _longitude);
                    string json = JsonSerializer.Serialize(postClass);

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
                return true;
            }
            catch (Exception ex)
            {
                files.ReadFile("Не удалось перекрасить область", false);
                files.ReadFile("Не удалось перекрасить область ", true);
                files.ReadFile(ex.Message, true);
                return false;
            }
        }

        public static void F(List<meteo> _sList)
        {
            int a = 12;
            int l = 0;
            while (true)
            {
                a = a + 1;
                if (l == 10) { a = 12; l = 0; }
                else { l++; }
                string temp = Convert.ToString(a);
                Console.WriteLine("это наша {0}", temp);
                //MagicAdd(_sList, temp);
                Thread.Sleep(60000);

            }
        }
=======


        private Files files = new Files();
        public List<TcpClient> listConnectedClients = new List<TcpClient>();
        public bool active = true;
        TcpListener listener = new TcpListener(IPAddress.Parse(Properties.Settings.Default.Ip_Server_Device), Convert.ToInt32(Properties.Settings.Default.Port_Server_Device));

>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180

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
<<<<<<< HEAD
               
=======
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
                    TcpClient client = listener.AcceptTcpClient();
                    listConnectedClients.Add(client);
                    ClientObject clientObject = new ClientObject(client);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
<<<<<<< HEAD
                    Thread threadDelete = new Thread(() => DeleteForTime(sList));
                    threadDelete.Start();
=======
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
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
<<<<<<< HEAD


       

=======
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
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
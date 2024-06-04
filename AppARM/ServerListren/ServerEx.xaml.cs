using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.IO;
using Microsoft.Win32;

using AppARM.ServerListren;
using AppARM.SettingApp;
using Newtonsoft.Json.Linq;
using AppARM.Weather;
using AppARM.WeatherSokol;
using AppARM.Parser;
using AppARM.PostgresSQL;
using AppARM.WeatherSokol;
using AppARM.Udp_Client;
using System.Windows.Controls;

namespace AppARM.ServerListren
{


    public partial class ServerEx : Window
    {
        private TcpListener server = null;
        private static CreateJsonRequest createJsonRequest = new CreateJsonRequest();
        private static ParserAll parser = new ParserAll();
        private string fileName;
        class Server
        {

            DataBaseMeteostation meteostation = new DataBaseMeteostation(Properties.Settings.Default.IP_adress,
                   Convert.ToString(Properties.Settings.Default.Port), Properties.Settings.Default.Login_BD, Properties.Settings.Default.Password_BD);
            TcpListener server = null;
            TcpClient newClientMeteo = null;

            private byte[] Message = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 }; //Запрос к метеостанции

            private readonly ServerEx ex;
            public Server(string ip, int port, ServerEx serverEx)
            {
                IPAddress localAddr = IPAddress.Parse(ip);
                server = new TcpListener(localAddr, port);
                ex = serverEx;
                server.Start();
                StartListener();
            }

            public void StartListener()
            {
                try
                {
                    while (true)
                    {
                        Console.WriteLine("Waiting for a connection...");
                        TcpClient client = server.AcceptTcpClient();
                        Console.WriteLine("Connected!");
                        Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
                        t.Start(client);
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                    server.Stop();
                }
            }
            static string ip4C = null;
            public async Task EmergencyCheckAddTextAsync(TcpClient _client)
            {
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        var message = new StringBuilder();
                        message.AppendFormat("Пришел сигнал ЧС с устройства с IP {0} \n ", Convert.ToString(_client.Client.RemoteEndPoint));
                        Console.WriteLine("ЧС");
                        ip4C = parser.AddMassiveStringIP(Convert.ToString(_client.Client.RemoteEndPoint), "1");
                        ex.MessageServer.Text += message;
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
                        message.AppendFormat("Погода на месте тем. =  {0} v-вет. = {1} напр ={2} \n ", _temperature, _windSpeed, _directionWind);
                        ex.MessageServer.Text += message;
                    }));
                    Thread.Sleep(100);
                });
            }

            public async Task EmergencyCheckAsynca(TcpClient _client)
            {


            }
            public async void HandleDeivce(Object obj)
            {
                TcpClient client = (TcpClient)obj;
                var stream = client.GetStream();
                string imei = String.Empty;
                string json = null;
                string data = null;
                Byte[] bytes = new Byte[256];
                int i;
                string needIP = "";

                string temperature = null;
                string windSpeed = null;
                string directionWind = null;

                try
                {
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        byte[] B4c = new byte[] { 0x00, 0x02, 0x01, 0xE2 };
                        //проверка что пришел сигнал ЧС
                        if (data == Encoding.ASCII.GetString(B4c, 0, B4c.Length))
                        {
                            EmergencyCheckAddTextAsync(client);
                            var t = meteostation.returnIp4CDevice("meteostation", ip4C);
                            needIP = t.Item1;
                            try
                            {
                                newClientMeteo = new TcpClient();
                                newClientMeteo.Connect(needIP, 2222);
                                NetworkStream tcpStream = newClientMeteo.GetStream();
                                tcpStream.Write(Message, 0, Message.Length);
                                byte[] msg = new byte[8096];
                                int count = tcpStream.Read(msg, 0, msg.Length);
                                ByteWeather byteWeather = new ByteWeather(msg);
                                var temp = byteWeather.ReturnPartWeater();


                                temperature = temp.Item1;
                                windSpeed = temp.Item2;
                                directionWind = temp.Item3;

                                WeatherAddTextAsync(temperature, windSpeed, directionWind);
                                Console.WriteLine("1: {0} , 2: {1} , 3: {2}", temp.Item1, temp.Item2, temp.Item3);

                                createJsonRequest.CreatJSON(needIP, "xxx", "xxx", "xxx", temperature, windSpeed, directionWind, "null");

                                var url = "http://" + Properties.Settings.Default.IP_Server_maps + ":" + Convert.ToInt32(Properties.Settings.Default.Port_Server_maps) + "/set_stations/";
                                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                                httpRequest.Method = "POST";
                                httpRequest.Accept = "application/json";
                                httpRequest.ContentType = "application/json";
                                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                                {
                                    streamWriter.Write(json);
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
                                            ex.MessageServer.Text += message;
                                        }
                                        else
                                        {
                                            var message = new StringBuilder();
                                            message.AppendFormat("не отвечает метеостанция \n");
                                            ex.MessageServer.Text += message;
                                        }
                                    }));

                                    Thread.Sleep(100);
                                });

                            }
                        }

                        Console.WriteLine("---> {0} {1} {2} {3}", ip4C, temperature, windSpeed, directionWind);

                        string hex = BitConverter.ToString(bytes);
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("{1}: Received: {0}", data, Thread.CurrentThread.ManagedThreadId);

                        string str = "Hey Device!";
                        Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str);
                        stream.Write(reply, 0, reply.Length);
                        Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e.ToString());
                    client.Close();
                }

            }

        }

        public void AddText(string _text)
        {
            MessageServer.Text += _text;
        }
        public ServerEx()
        {
            InitializeComponent();
         //   CB_WL.IsChecked = true;
          //  CB_SC.IsChecked = true;
          //  CB_SRN.IsChecked = true;
           // CB_RTU.IsChecked = true;
            Console.WriteLine("Запуск сервера ждем ЧС {0} {1}", Properties.Settings.Default.IP_ServerListen, Properties.Settings.Default.Port_ServerListen);
            MessageServer.Text += "Сервер запущен: \n";
            Thread t = new Thread(delegate ()
            {

                //дополнить Ip из настроек
                Server myserver = new Server(Properties.Settings.Default.IP_ServerListen, Convert.ToInt32(Properties.Settings.Default.Port_ServerListen), this);
            });
            t.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            e.Cancel = true;
            Hide();
        }

        private void BT_AddPath_Click(object sender, RoutedEventArgs e)
        {
          
            
            try
            {
                var dlg = new OpenFileDialog();
                dlg.FileName = "sample";
                dlg.DefaultExt = ".wav";
                dlg.Filter = "wav file (.wav)|*.wav";
                //dlg.RestoreDirectory = true;
                var dlgResult = dlg.ShowDialog(this);
                if (dlgResult.HasValue && dlgResult.Value)
                {
                    fileName = dlg.FileName;
                    PathWav.Text = dlg.FileName;
                   
                }
            }                           
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClientUDP clientUDP = new ClientUDP();
            Console.WriteLine(fileName);
            Console.WriteLine(IPUDP.Text);
            Console.WriteLine(PortUDP.Text);
            clientUDP.UdpClientStart(fileName, IPUDP.Text, Convert.ToInt32(PortUDP.Text));
        }

        private void Button_Click_StartScenario(object sender, RoutedEventArgs e)
        {

        }
    }
}







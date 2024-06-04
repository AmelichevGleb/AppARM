using System;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;

using AppARM.WeatherSokol;
using AppARM.FilesLogs;
using AppARM.Parser;

namespace AppARM.CheckMeteos
{
    /// <summary>
    /// Логика взаимодействия для CheckeMeteo.xaml
    /// </summary>
    public partial class CheckeMeteo : Window
    {


        private DateTime dateTime = new DateTime();
        private Files files = new Files();
        private ParserAll parserAll = new ParserAll();

        // комманда опроса
        private byte[] Message = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 };

        private double temperature;
        private double windSpeed;
        private int directionWind;
        private static string ipAdress;
        private static string port;
        private static string request;

        public CheckeMeteo()
        {
            InitializeComponent();
        }

        //____________________________ОСНОВНЫЕ_КНОПКИ_______________________________________________

        private void Button_Click_Ok(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            Test.Text = string.Empty;
        }


        private async void B_Send_Click(object sender, RoutedEventArgs e)
        {
            ipAdress = TB_adress.Text;
            port = TB_port.Text;
            request = TB_request.Text; //получить текст запроса 
            Console.WriteLine(request + "!!!!!!");
            B_Send.IsEnabled = false;
            Console.WriteLine(ipAdress + " " + port);
            try
            {
                await Task.Run(() =>
                {
                    Dispatcher.Invoke((Action)(() => Test.Text += "Проверка по " + ipAdress + ":" + port + '\n'));
                    if ((ipAdress != "") && (port != ""))
                    {
                        if (request == "")
                        {
                            
                            GetWeather(Message);
                            Dispatcher.Invoke((Action)(() => Test.Text += "запрос :  " + (BitConverter.ToString(Message)) + "\n" + CorrectString() + '\n' + "температура: " + temperature + '\n' + "Скорость ветра: " + windSpeed + '\n' + "направление ветра: " + directionWind + '\n'));
                            Thread.Sleep(10);
                        }
                        else
                        {
                            byte[] temp = parserAll.AddMassiveByte(request);
                            GetWeather(temp);
                            Dispatcher.Invoke((Action)(() => Test.Text += "запрос :  " + (BitConverter.ToString(temp)) + "\n" +  CorrectString() + '\n' + "температура: " + temperature + '\n' + "Скорость ветра: " + windSpeed + '\n' + "направление ветра: " + directionWind + '\n'));
                            Thread.Sleep(10);
                        }
                    }
                });
                B_Send.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Test.Text += CorrectString() + (ex.Message) + "\n";
                files.ReadException(ex);
                B_Send.IsEnabled = true;
            }

        }

        //__________________________________________________________________________________________

        //cоздание строки с временем
        private string CorrectString()
        {
            string strTime = null;
            strTime = Convert.ToString(DateTime.Now) + " -";
            return strTime;
        }

        //Проверка подключенной станции
        private void GetWeather(byte[] _message)
        {
            try
            {
                TcpClient tcpClient = new TcpClient();
                // Test.Text += "Проверка по " + ipAdress + ":" + port + '\n'; 
                tcpClient.Connect(ipAdress, Convert.ToInt32(port));
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(_message, 0, _message.Length);

                byte[] bytes = new byte[tcpClient.ReceiveBufferSize];
                int bytesRead = stream.Read(bytes, 0, tcpClient.ReceiveBufferSize);
  
                ByteWeather byteWeather = new ByteWeather(bytes);
                var temp = byteWeather.ReturnPartWeater();

                temperature = Convert.ToDouble(temp.Item1);
                windSpeed = Convert.ToDouble(temp.Item2);
                directionWind = Convert.ToInt32(temp.Item3);

                Thread.Sleep(10);

                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke((Action)(() => Test.Text += Convert.ToString("Ошибка подключения") + '\n'));
                files.ReadException(ex);
            }

        }

        private void TB_text_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }


        //__________________________________________________________________________________________




    }
}

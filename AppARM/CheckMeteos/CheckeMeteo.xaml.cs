using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Text;
using System.Drawing.Drawing2D;
using System.Net;
using System.Threading;
using System.Linq.Expressions;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Interop;
using AppARM.WeatherSokol;
using AppARM.TestXML;
using AppARM.Parser;
using System.Runtime.Remoting.Messaging;

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
            //есть баг с кнопкой !!!



            ipAdress = TB_adress.Text;
            port = TB_port.Text;
            request = TB_request.Text; //получить текст запроса 
            Console.WriteLine(request + "!!!!!!");
            Test.Text += "запрос :" + (request) + "\n";
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
                            Dispatcher.Invoke((Action)(() => Test.Text += CorrectString() + '\n' + "температура: " + temperature + '\n' + "Скорость ветра: " + windSpeed + '\n' + "направление ветра: " + directionWind + '\n'));
                            Thread.Sleep(10);
                        }
                        else
                        {
                            byte[] t = parserAll.AddMassive(request);
                            GetWeather(t);
                            Dispatcher.Invoke((Action)(() => Test.Text += CorrectString() + '\n' + "температура: " + temperature + '\n' + "Скорость ветра: " + windSpeed + '\n' + "направление ветра: " + directionWind + '\n'));
                            Thread.Sleep(10);
                        }
                    }
                });
                B_Send.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Test.Text += CorrectString() + (ex.Message) + "\n";
                files.ReadExeption(ex);
                B_Send.IsEnabled = true;
            }

        }

        //__________________________________________________________________________________________

        //cоздание строки с временем
        private string CorrectString()
        {
            string str = null;
            str = Convert.ToString(DateTime.Now) + " -";
            return str;
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

                ByteWeather byteWeather = new ByteWeather(null, null, 0, null, null, null, bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], bytes[6], bytes[7], bytes[8],
                        bytes[9], bytes[10], bytes[11], bytes[12], bytes[13], bytes[14], bytes[15], bytes[16], bytes[17], bytes[18], bytes[19], bytes[20], bytes[21], bytes[22], bytes[23],
                        bytes[24], bytes[25], bytes[26], bytes[27], bytes[28], false);

                temperature = byteWeather.temperature;
                windSpeed = byteWeather.windSpeed;
                directionWind = byteWeather.directionWind;

                Thread.Sleep(10);

                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke((Action)(() => Test.Text += Convert.ToString("Ошибка подключения") + '\n'));
                files.ReadExeption(ex);
            }

        }

        private void TB_text_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }

        private void B_Test_Click(object sender, RoutedEventArgs e)
        {

        }


        //__________________________________________________________________________________________




    }
}

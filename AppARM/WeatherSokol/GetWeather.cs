using AppARM.TestXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AppARM.Structure;
using System.Windows;

namespace AppARM.WeatherSokol
{
    public class GetWeather
    {
        private Files files = new Files();
        byte[] Message = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 };

        public void ConnectMeteo(string _ip, int _port,string _location,string _longitude, string _lagatitude, string _ipSend , int _portSend)
        {
            try
            {
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(_ip, _port);
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(Message, 0, Message.Length);
                byte[] bytes = new byte[tcpClient.ReceiveBufferSize];
                int bytesRead = stream.Read(bytes, 0, tcpClient.ReceiveBufferSize);
                string returnData = Encoding.UTF8.GetString(bytes);
                ByteWeather byteWeather = new ByteWeather(_ip, _ipSend, _portSend,_location, _longitude, _lagatitude, bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], bytes[6], bytes[7], bytes[8],
                    bytes[9], bytes[10], bytes[11], bytes[12], bytes[13], bytes[14], bytes[15], bytes[16], bytes[17], bytes[18], bytes[19], bytes[20], bytes[21], bytes[22], bytes[23], bytes[24], bytes[25], bytes[26], bytes[27], bytes[28]);
                tcpClient.Dispose();
                stream.Close();
                Console.WriteLine("Конец");
            }
            catch (Exception ex)
            {
                ByteWeather byteWeather = new ByteWeather(_ip, _ipSend, _portSend, _location, _longitude, _lagatitude,0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00);

                files.ReadExeption(ex);
            }
        }

    }
}

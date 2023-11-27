using Mono.Security.Protocol.Ntlm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace AppARM.WeatherSokol
{
    internal class CreateJsonRequest
    {
        public void CreatJSON(string _ip, string _ipSend, int _portSend, string _location, string _longitude, string _lagatitude, string _temperature, string _windSpeed, string _directionWind, string _parametr)
        {

            Coords coords = new Coords()
            {
                Device_ip = _ip,
                Name = _location,
                Longitude = _longitude,
                Lagatitude = _lagatitude,

                weather = new Weather
                {
                    Temperature = _temperature,
                    Wind_Speed = _windSpeed,
                    Direction = _directionWind,
                    Parameter = _parametr
                }
            };

            string stringjson = JsonConvert.SerializeObject(coords);
            Console.WriteLine(stringjson);

            /// !!!!
            var url = "http://" + _ipSend + ":" + Convert.ToString(_portSend);
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(stringjson);
            }
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
            var status = httpResponse.StatusCode.ToString();
            if (status == "OK") { 
                //сообщение
                                  
            }
        }
    }
}
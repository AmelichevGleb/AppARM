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
        private string stringjson;
        public string CreatJSON(string _ip, string _location, string _longitude, string _lagatitude, string _temperature, string _windSpeed, string _directionWind, string _parametr)
        {
            stringjson = null;
            Coords coords = new Coords()
            {
                Name = _location,
                Device_ip = _ip,
                Longitude = _longitude,
                Latitude = _lagatitude,

                Weather = new Weathers
                {
                    Temperature = _temperature,
                    Wind_speed = _windSpeed,
                    Direction = _directionWind,
                    Parameter = _parametr
                }
            };

             stringjson = JsonConvert.SerializeObject(coords);
           
            Console.WriteLine(stringjson);
            return stringjson;
            /// !!!!
            /// 
            /*
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
            */
        }
    }
}
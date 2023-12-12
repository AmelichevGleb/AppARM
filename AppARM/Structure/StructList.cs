using AppARM.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppARM.Structure
{
    //Структура базы данных
    public class StructList
    {
        public int Id { get; set; }
        public string IP_device { get; set; }
        public string Port { get; set; }
        public string Location { get; set; }
        public string Longitude { get; set; }
        public string Lagatitude { get; set; }
        public string Description { get; set; }
       // public string Temperature { get; set; }
       // public string WindSpeed { get; set; }   
       // public string DirectionWind { get; set; }

        public StructList(int _id, string _ip_device, string _port ,string _location,string _longitude ,string _lagatitude,string _description ) //, string _temperature , string _windSpeed , string _directionWind )
        {
            this.Id = _id;
            this.IP_device = _ip_device;
            this.Port = _port;
            this.Location = _location;
            this.Longitude = _longitude;
            this.Lagatitude = _lagatitude;
            this.Description = _description;
        //   this.Temperature = _temperature;
        //   this.WindSpeed = _windSpeed;
        //  this.DirectionWind = _directionWind;
        }
    }
}

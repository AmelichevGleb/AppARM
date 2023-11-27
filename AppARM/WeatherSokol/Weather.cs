using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppARM.WeatherSokol
{
    public class Weather
    {
        public string Temperature { get; set; }
        public string Wind_Speed { get; set; }
        public string Direction { get; set; }
        public string Parameter { get; set; }
    }

    public class Coords
    {
        public string Device_ip { get; set; }
        public string Name { get; set; }
        public string Longitude { get; set; }
        public string Lagatitude { get; set; }
        public Weather weather { get; set; }

    }

}

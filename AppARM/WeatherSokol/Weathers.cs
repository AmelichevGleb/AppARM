using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppARM.WeatherSokol
{
    public class Weathers
    {
        public string Temperature { get; set; }
        public string Wind_speed { get; set; }
        public string Direction { get; set; }
        public string Parameter { get; set; }
    }

    public class Coords
    {
        public string Name { get; set; }
        public string Device_ip { get; set; }

        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public Weathers Weather { get; set; }

    }

}

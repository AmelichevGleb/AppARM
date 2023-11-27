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
        public StructList(int _id, string _ip_device, string _location,string _longitude ,string _lagatitude,string _description)
        {
            this.Id = _id;
            this.IP_device = _ip_device;
            this.Location = _location;
            this.Longitude = _longitude;
            this.Lagatitude = _lagatitude;
            this.Description = _description;

        }
        public int Id { get; set; }
        public string IP_device { get; set; }
        public string Location { get; set; }
        public string Longitude { get; set; }
        public string Lagatitude { get; set; }
        public string Description { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AppARM.TestXML;
using Newtonsoft.Json.Linq;

namespace AppARM.Structure
{
    public class ElementDataBase
    {
        private string id;
        private string ip_device;
        private string port;
        private string location;
        private string longitude;
        private string lagatitude;
        private string description;

        public ElementDataBase(string _id, string _ip_device, string port, string _location, string _longitude, string _lagatitude, string _description)
        {
            this.id = _id;
            this.ip_device = _ip_device;
            this.port = port;
            this.location = _location;
            this.longitude = _longitude;
            this.lagatitude = _lagatitude;
            this.description = _description;

        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string IP
        {
            get { return ip_device; }
            set { ip_device = value; }
        }

        public string Port
        {
            get { return port; }
            set { port = value; }
        }
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        public string Lagatitude
        {
            get { return lagatitude; }
            set { lagatitude = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        public class WorkElementDB
        {
            private Files files = new Files();

            //добавление элемента в список
            public bool AddNewElement(List<ElementDataBase> _element, string _id, string _ip_device, string _port, string _location, string _longitude, string _lagatitude, string _description)
            {
                try
                {
                    _element.Add(new ElementDataBase(_id, _ip_device, _port, _location, _longitude, _lagatitude, _description));
                    Console.WriteLine(_element.Count);
                    return true;
                }
                catch (Exception _ex)
                {
                    files.ReadExeption(_ex);
                    return false;
                }
            }
            //Показать элементы в списке 
            public void ShowElements(List<ElementDataBase> _element)
            {
                foreach (var a in _element)
                {
                    Console.WriteLine("id - {0}, ip - {1}, location - {2}, longitude - {3}, lagatitude - {4}, description - {5}", a.Id, a.IP, a.Location, a.Longitude, a.Lagatitude, a.Description);
                }
            }

            //Проверка элемента по ID
            public bool ExistById(List<ElementDataBase> _element, string _id)
            {
                var result = _element.Exists(x => x.Id == _id);
                return result;
            }

            //Проверка элемента по IP
            public bool ExistByIP(List<ElementDataBase> _element, string _ip)
            {
                var result = _element.Exists(x => x.IP == _ip);
                return result;
            }

            //Проверка элемента по Локации
            public bool ExistByLocation(List<ElementDataBase> _element, string _location)
            {
                var result = _element.Exists(x => x.Location == _location);
                return result;
            }

            //Проверка элемента по Долготе
            public bool ExistByLongitude(List<ElementDataBase> _element, string _longitude)
            {
                var result = _element.Exists(x => x.Longitude == _longitude);
                return result;
            }

            //Проверка элемента по Широте
            public bool ExistByLagatitude(List<ElementDataBase> _element, string _lagatitude)
            {
                var result = _element.Exists(x => x.Lagatitude == _lagatitude);
                return result;
            }

            //Провека по описанию
            public bool ExistByDescription(List<ElementDataBase> _element, string _description)
            {
                var result = _element.Exists(x => x.Description == _description);
                return result;
            }

            //Вернуть элемент по ID
            public Tuple<string, string, string, string, string, string, string> SearchById(List<ElementDataBase> _element, string _id)
            {
                string id = null;
                string ip_device = null;
                string port = null;
                string location = null;
                string longitude = null;
                string lagatitude = null;
                string description = null;

                if (ExistById(_element, _id))
                {
                    foreach (var x in _element)
                    {
                        if (x.Id == _id)
                        {
                            id = x.Id;
                            ip_device = x.IP;
                            port = x.Port;
                            location = x.Location;
                            longitude = x.Longitude;
                            lagatitude = x.Lagatitude;
                            description = x.Description;
                            break;
                        }
                    }
                }
                return Tuple.Create(id, ip_device, port, location, longitude, lagatitude, description);
            }

            //Вернуть элемент по IP
            public Tuple<string, string, string, string, string, string, string> SearchByIP(List<ElementDataBase> _element, string _ip)
            {

                string id = null;
                string ip_device = null;
                string port = null;
                string location = null;
                string longitude = null;
                string lagatitude = null;
                string description = null;

                if (ExistByIP(_element, _ip))
                {
                    foreach (var x in _element)
                    {
                        if (x.IP == _ip)
                        {
                            id = x.Id;
                            ip_device = x.IP;
                            port = x.Port;
                            location = x.Location;
                            longitude = x.Longitude;
                            lagatitude = x.Lagatitude;
                            description = x.Description;
                           
                            break;
                        }
                    }
                }
                return Tuple.Create(id, ip_device, port, location, longitude, lagatitude, description);
            }

            //Вернуть элемент по Локации
            public Tuple<string, string, string, string, string, string, string> SearchByLocation(List<ElementDataBase> _element, string _location)
            {

                string id = null;
                string ip_device = null;
                string port = null;
                string location = null;
                string longitude = null;
                string lagatitude = null;
                string description = null;
              

                if (ExistByLocation(_element, _location))
                {
                    foreach (var x in _element)
                    {
                        if (x.Location == _location)
                        {
                            id = x.Id;
                            ip_device = x.IP;
                            port = x.Port;
                            location = x.Location;
                            longitude = x.Longitude;
                            lagatitude = x.Lagatitude;
                            description = x.Description;
                            break;
                        }
                    }
                }
                return Tuple.Create(id, ip_device, port, location, longitude, lagatitude, description);
            }

            //Удалить элемент из списка по ID
            public void DeleteElement(List<ElementDataBase> _element, string _id)
            {
                _element.RemoveAt(int.Parse(_id) - 1);

            }
        }
    }
}
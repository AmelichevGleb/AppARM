using AppARM.FilesLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppARM.Structure
{
    internal class ElementAdditionalDataBase
    {
        private string id;
        private string ip_device;
        private string port;
        private string location;
        private string longitude;
        private string lagatitude;
        private string description;

        public ElementAdditionalDataBase(string _id, string _ip_device, string _port)
        {
            this.id = _id;
            this.ip_device = _ip_device;
            this.port = _port;
          

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
      


        public class WorkElementDBMeteo
        {
            private Files files = new Files();

            //добавление элемента в список
            public bool AddNewElement(List<ElementAdditionalDataBase> _element, string _id, string _ip_device, string _port)
            {
                try
                {
                    _element.Add(new ElementAdditionalDataBase(_id, _ip_device, _port));
                    Console.WriteLine(_element.Count);
                    return true;
                }
                catch (Exception _ex)
                {
                    files.ReadException(_ex);
                    return false;
                }
            }
            //Показать элементы в списке 
            public void ShowElements(List<ElementAdditionalDataBase> _element)
            {
                foreach (var a in _element)
                {
                    Console.WriteLine("id - {0}, ip - {1}, порт - {2}", a.Id, a.IP, a.Port);
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

            //Проверка элемента по порту
            public bool ExistByLocation(List<ElementDataBase> _element, string _port)
            {
                var result = _element.Exists(x => x.Location == _port);
                return result;
            }

           

            //Вернуть элемент по ID
            public Tuple<string, string, string> SearchById(List<ElementDataBase> _element, string _id)
            {
                string id = null;
                string ip_device = null;
                string port = null;
              

                if (ExistById(_element, _id))
                {
                    foreach (var x in _element)
                    {
                        if (x.Id == _id)
                        {
                            id = x.Id;
                            ip_device = x.IP;
                            port = x.Port;
                           
                            break;
                        }
                    }
                }
                return Tuple.Create(id, ip_device, port);
            }

            //Вернуть элемент по IP
            public Tuple<string, string, string> SearchByIP(List<ElementDataBase> _element, string _ip)
            {

                string id = null;
                string ip_device = null;
                string port = null;
               

                if (ExistByIP(_element, _ip))
                {
                    foreach (var x in _element)
                    {
                        if (x.IP == _ip)
                        {
                            id = x.Id;
                            ip_device = x.IP;
                            port = x.Port;
                           
                            break;
                        }
                    }
                }
                return Tuple.Create(id, ip_device, port);
            }

            //Вернуть элемент по Порту
            public Tuple<string, string, string> SearchByLocation(List<ElementDataBase> _element, string _port)
            {

                string id = null;
                string ip_device = null;
                string port = null;


                if (ExistByLocation(_element, _port))
                {
                    foreach (var x in _element)
                    {
                        if (x.Port == _port)
                        {
                            id = x.Id;
                            ip_device = x.IP;
                            port = x.Port;
                            
                            break;
                        }
                    }
                }
                return Tuple.Create(id, ip_device, port);
            }

            //Удалить элемент из списка по ID
            public void DeleteElement(List<ElementDataBase> _element, string _id)
            {
                _element.RemoveAt(int.Parse(_id) - 1);

            }
        }
    }
}
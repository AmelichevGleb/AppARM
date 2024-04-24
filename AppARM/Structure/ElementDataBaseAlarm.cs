using AppARM.FilesLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AppARM.Structure
{
    public class ElementDataBaseAlarm
    {
        private string id;
        private string ip_device;
        private string script;
        private string path;
        private string notify;


        /*
        string sqlStr = "CREATE TABLE if not exists " + _nametable +
              "(  id serial,\r\n" +
              "ip_device   text,\r\n" +
              "script text,\r\n" +
              "path   text,\r\n" +
              "notify   text\r\n)";
        */

        public ElementDataBaseAlarm(string _id, string _ip_device, string _script, string _path, string _notify)
        {
            this.id = _id;
            this.ip_device = _ip_device;
            this.script = _script;
            this.path = _path;
            this.notify = _notify;
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

        public string Script
        {
            get { return script; }
            set { script = value; }
        }
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        public string Notify
        {
            get { return notify; }
            set { notify = value; }
        }



        public class WorkElementDBAlarm
        {
            private Files files = new Files();

            //добавление элемента в список
            public bool AddNewElement(List<ElementDataBaseAlarm> _element,string _id, string _ip_device, string _script, string _path, string _notify) 
            {
                try
                {
                    _element.Add(new ElementDataBaseAlarm(_id, _ip_device, _script, _path, _notify));
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
            public void ShowElements(List<ElementDataBaseAlarm> _element)
            {
                foreach (var a in _element)
                {
                    Console.WriteLine("id - {0}, ip - {1}, location - {2}, longitude - {3}, lagatitude - {4}, description - {5}", a.Id, a.IP, a.script, a.path, a.notify);
                }
            }

            //Проверка элемента по ID
            public bool ExistById(List<ElementDataBaseAlarm> _element, string _id)
            {
                var result = _element.Exists(x => x.Id == _id);
                return result;
            }

            //Проверка элемента по IP
            public bool ExistByIP(List<ElementDataBaseAlarm> _element, string _ip)
            {
                var result = _element.Exists(x => x.IP == _ip);
                return result;
            }

            //Проверка элемента по номеру сценария
          
            public bool ExistByLocation(List<ElementDataBaseAlarm> _element, string _script)
            {
                var result = _element.Exists(x => x.Script == _script);
                return result;
            }

            //Проверка элемента по пути к файлу
          
            public bool ExistByLongitude(List<ElementDataBaseAlarm> _element, string _path)
            {
                var result = _element.Exists(x => x.Path == _path);
                return result;
            }

            //Проверка элемента по списку устройств
           
            public bool ExistByLagatitude(List<ElementDataBaseAlarm> _element, string _notify)
            {
                var result = _element.Exists(x => x.Notify == _notify);
                return result;
            }



            //Вернуть элемент по ID   
            public Tuple<string, string, string,  string, string> SearchById(List<ElementDataBaseAlarm> _element, string _id)
            {
                string id = null;
                string ip_device = null;
                string script = null;
                string path = null;
                string notify = null;



                if (ExistById(_element, _id))
                {
                    foreach (var x in _element)
                    {
                        if (x.Id == _id)
                        {
                            id = x.Id;
                            ip_device = x.IP;
                            script = x.Script;
                            path = x.Path;
                            notify = x.Notify;
                            break;
                        }
                    }
                }
                return Tuple.Create(id, ip_device, script, path, notify);
            }

            //Вернуть элемент по IP
           
            public Tuple<string, string, string,  string, string> SearchByIP(List<ElementDataBaseAlarm> _element, string _ip)
            {
                string id = null;
                string ip_device = null;
                string script = null;
                string path = null;
                string notify = null;


                if (ExistByIP(_element, _ip))
                {
                    foreach (var x in _element)
                    {
                        if (x.IP == _ip)
                        {
                            id = x.Id;
                            ip_device = x.IP;
                            script = x.Script;
                            path = x.Path;
                            notify = x.Notify;
                            break;
                        }
                    }
                }
                return Tuple.Create(id, ip_device, script, path, notify);
            }

            //Вернуть элемент по номеру сценария 
        
            public Tuple<string, string, string, string, string> SearchByLocation(List<ElementDataBaseAlarm> _element, string _script)
            {

                string id = null;
                string ip_device = null;
                string script = null;
                string path = null;
                string notify = null;


                if (ExistByLocation(_element, _script))
                {
                    foreach (var x in _element)
                    {
                        if (x.Script == _script)
                        {
                            id = x.Id;
                            ip_device = x.IP;
                            script = x.Script;
                            path = x.Path;
                            notify = x.Notify;
                            break;
                        }
                    }
                }
                return Tuple.Create(id, ip_device, script, path, notify);
            }

            //Удалить элемент из списка по ID
            public void DeleteElement(List<ElementDataBaseAlarm> _element, string _id)
            {
                _element.RemoveAt(int.Parse(_id) - 1);

            }
        }
    }
}
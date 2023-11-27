using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppARM.Structure
{
    public class APU
    {
        //Структура Apu


        private string ip = null;
        private string port = null;
        private string portServer = null;
        private string values = null;



        public string Ip { get { return ip; } set { ip = value; } }
        public string Port { get { return port; } set { port = value; } }
        public string PortServer { get { return portServer; } set { portServer = value; } }
        public string Value { get { return values; } set { values = value; } }
    }
}

using System;
using System.Text.RegularExpressions;
using System.Windows.Ink;


namespace AppARM.Parser
{
    public class ParserAll
    {
        readonly byte[] messageConst = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 };  // правильный массив
        byte[] testMassive = new byte[8]; 
        int countByte;

        //подсчет кол-ва ;
        public int CountByteSend(string _message)
        {
            countByte = 0;
            Console.WriteLine(_message);
            char ch = ';';
            foreach (char c in _message)
            {
                if (c == ch)
                {
                    countByte++;
                }
            }
            if (NextElementByte(_message))
            {
                countByte++;
            }
            return countByte;
        }
        //проверка наличие следующего элемента после разделителя
        public bool NextElementByte(string _message)
        {
            int index = _message.LastIndexOf(';');
            Console.WriteLine(index);
            Console.WriteLine(_message.Length);
            if (index + 1 < _message.Length)
            {
                return true;
            }
            else return false;
        }
        //заполнение массива тестового
        public byte[] AddMassiveByte(string _message)
        {
            string str = "";
            int i = 0;
            foreach (char c in _message)
            {               
                if (c != ';')
                {
                    str = str + c;
                }
                else
                {
                    testMassive[i] = Convert.ToByte(str);
                    i++;
                    str = "";
                }
            }
            testMassive[i] = Convert.ToByte(str);
            for (int i1 = 0; i1 < testMassive.Length; i1++)
            {
                Console.WriteLine(testMassive[i1]);
            }
            return testMassive;
        }

        //парсер для IP адреса и порта !!!!
        public Tuple<string,string> AddMassiveStringIP(string _message)
        { 

            string ip = null ;
            string port  = null;
            int count = 0;
            string[] words = _message.Split(';');
            foreach (var word in words)
            {   if (count == 0) { ip = word; }
                if (count == 1) { port = word; }     
                System.Console.WriteLine($"<{word}>");
                count++;
            }
            return Tuple.Create(ip, port);
        }
        // формат строки 127.0.0.1:60606 нужно оставить только IP
        public string AddMassiveStringIP(string _message,string _parametrs)
        {
            string ip = null;
            int count = 0;
            string[] words = _message.Split(':');
            foreach (var word in words)
            {
                if (count == 0) { ip = word; break; }
                System.Console.WriteLine($"<{ip}>");
            }
            return ip;
        }

        //public void MassiveIPDanger(string str)
        public Tuple <string[],string[]> MassiveIPDanger(string _str)
        {
            //127.0.0.1;127.0.0.2;127.0.0.3;127.0.0.4;127.0.0.5;127.0.0.6;127.0.0.7;127.0.0.8; 
            //для строки разделенной ';' и если на конец ; то делаем на -1 раз меньше

           //string str = "127.0.0.1-4444;127.0.0.2-4444;";
            string input = "abc][rfd][5][,][.";
            string[] parts1 = _str.Split(new string[] { ";" }, StringSplitOptions.None);
            string[] MassiveIP = new string[parts1.Length - 1];
            string[] MassivePort = new string[parts1.Length - 1];
            string str2;
            for (int i = 0; i < parts1.Length - 1; i++)
            {
                str2 = parts1[i];
                MassiveIP[i] = str2.Substring(0, str2.IndexOf('-'));
                MassivePort[i] = str2.Substring(str2.IndexOf('-') + 1);

                Console.WriteLine("IP = {0} Port = {1} ", MassiveIP[i], MassivePort[i]);
            }


            return Tuple.Create(MassiveIP,MassivePort);
        }
        //       номер сценария    кого оповестить 
        //        0 127.0.0.1  127.0.0.1  127.0.0.1  127.0.0.1  127.0.0.1  127.0.0.1  127.0.0.1  127.0.0.2 
    }
}

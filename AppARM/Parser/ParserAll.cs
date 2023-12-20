using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppARM.Parser
{
    public class ParserAll
    {
        byte[] Message = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 };  // правильный массив
        //byte[] data = new byte[8];
        byte[] test = new byte[8];
        int countByte = 0;

        //подсчет кол-ва "значение" в строке 
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
            // return index;
            if (index + 1 < _message.Length)
            {
                return true;
            }
            else return false;
        }
        //заполнение массива тестового
        public byte[] AddMassive(string _message)
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
                    test[i] = Convert.ToByte(str);
                    i++;
                    str = "";
                }
            }
            test[i] = Convert.ToByte(str);
            for (int i1 = 0; i1 < test.Length; i1++)
            {
                Console.WriteLine(test[i1]);
            }
            return test;
        }
    }
}

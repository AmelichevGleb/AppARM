using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Windows.Shapes;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Windows;
using AppARM.Scripts;
using static System.Net.WebRequestMethods;
using System.Windows.Controls;

namespace AppARM.TestXML
{
    public class Files
    {

        private string fileLogs = "logs.txt";
        private string fileApu = "Apu.txt";
        //private string fileName = "inf.txt";

        //Инициализация файла logs.txt и Apu.txt
        public void InitFile()
        {
            try
            {
                if (CheckFile(fileLogs) == false)
                {
                    CreateFile(fileLogs);
                }

                if (CheckFile(fileLogs) == false) { MessageBox.Show("Неудается создать файл Logs"); }
                else
                {
                    CreateFile(fileApu);
                }
            }
            catch (Exception ex)
            {
                ReadExeption(ex);
            }
        }

        //Проверка файла на наличие
        public bool CheckFile(string _name)
        {
            string path = Environment.CurrentDirectory + @"\" + _name;
            if (System.IO.File.Exists(path))
            {
                return true;
            }
            else
                return false;
        }

        //Запись ошибки в файл
        public void ReadExeption(Exception _ex)
        {
            Console.WriteLine("Exception: " + _ex.Message.ToString());
            ReadFile(_ex.ToString(), true);      
        }

        //Создание файла
        public bool CreateFile(string _name)
        {
            try
            {
                string path = Environment.CurrentDirectory + @"\" + _name;
                using (StreamWriter w = System.IO.File.AppendText(_name))
                    w.Close();
                return true;
            }
            catch (Exception ex)
            {
                ReadExeption(ex);
                return false;
            }
        }

        //Удаление файла
        public bool DeleteFile(string _name)
        {
            try
            {
                string path = Environment.CurrentDirectory + @"\" + _name;
                System.IO.File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ReadExeption(ex);
                return false;
            }
        }
       
        //Запись информации в файл
        public void ReadFile(string _name, string _text)
        {
            string path = Environment.CurrentDirectory + @"\" + _name;
            System.IO.File.AppendAllText(_name, _text + "\n");
        }

        //Запись ошибки в файл logs.txt
        public void ReadFile(string _text, bool state)
        {
            string path = Environment.CurrentDirectory + @"\" + fileLogs;
            Console.WriteLine(_text + DateTime.Now);
            System.IO.File.AppendAllText(fileLogs, "-" +" "+ Convert.ToString(DateTime.Now)+ " " + _text + "\n");
        }
        
        //вывод информации из файла
        public string ShowDatainFile(string _name)
        {
            string line = null;
            string path = Environment.CurrentDirectory + @"\" + _name;
            using (StreamReader myReader = new StreamReader(path))
            {
                line = myReader.ReadToEnd();
            }
            return line;
        }

        //проверка на пустоту в файле 
        public bool CheckNullFile(string _name)
        {
            string? line = null;
            string path = Environment.CurrentDirectory + @"\" + _name;
            StreamReader sr = new StreamReader(path);
            line = sr.ReadLine();
            if (line == null) { return true; }
            else return false;
        }
        
        //?????
        public void ParserApu()
        {
            Script scripts = new Script();
            string str = "1;192.168.1.1;8080";
            var s = scripts.ParserElement(str);
            Console.WriteLine(s.Item1 ," ", s.Item2);
            Console.WriteLine(s.Item2 + s.Item3);
            Console.WriteLine(s.Item3);
        }
    }
}

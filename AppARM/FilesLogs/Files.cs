using System;
using System.Text;
using System.IO;
using System.Windows;

namespace AppARM.FilesLogs

{
    public class Files
    {
        private StringBuilder message; 
        
        private string fileNameLogs = "logs.txt";
        private string fileNameException = "exception.txt";
       
        private string fileApu = "Apu.txt";
        //private string fileName = "inf.txt";

       
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
                ReadException(ex);
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
                ReadException(ex);
                return false;
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

        //Инициализация файла логи
        public void InitFileLogs()
        {
            try
            {
                if (CheckFile(fileNameLogs) == false)
                {
                    CreateFile(fileNameLogs);
                }             
            }
            catch (Exception ex)
            {
                var message = new StringBuilder();
                message.AppendFormat("Неудается создать файл {0} \n Ошибка {1}", fileNameLogs, Convert.ToString(ex));
                MessageBox.Show(Convert.ToString(message));
            }
        }

        //Инициализация файла ошибок
        public void InitFileException()
        {
            try
            {
                if (CheckFile(fileNameException) == false)
                {
                    CreateFile(fileNameException);
                }
            }
            catch (Exception ex)
            {
                var message = new StringBuilder();
                message.AppendFormat("Неудается создать файл {0} \n Ошибка {1}", fileNameException, Convert.ToString(ex));
                MessageBox.Show(Convert.ToString(message));
            }
        }

        //Запись информации в файл
        public void ReadFile(string _name, string _text)
        {
            string path = Environment.CurrentDirectory + @"\" + _name;
            System.IO.File.AppendAllText(_name, _text + "\n");
        }

        //Запись ошибки в файл
        public void ReadException(Exception _ex)
        {
            Console.WriteLine("Exception: " + _ex.Message.ToString());
            ReadFile(_ex.ToString(), true);
        }

        //Запись в файл 
        //если true идет запись в файл ошибок
        //если false идет запись в файл логовы
        public void ReadFile(string _text, bool state)
        {
            try
            {
                if (state == true)
                {
                    System.IO.File.AppendAllText(fileNameException, "-" + " " + Convert.ToString(DateTime.Now) + " " + _text + "\n");
                }
                else
                {
                    System.IO.File.AppendAllText(fileNameLogs, "-" + " " + Convert.ToString(DateTime.Now) + " " + _text + "\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Convert.ToString(ex));
            }
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

    }
}

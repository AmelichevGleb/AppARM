using AppARM.Structure;
using AppARM.FilesLogs;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppARM.Structure.ElementDataBase;
using static AppARM.Structure.ElementDataBaseAlarm;
using System.Windows;

namespace AppARM.PostgresSQL
{
    public class AlarmDB
    {
        //alarmDB


        /*  Cоздать таблицу 
         *  
         *  | id | ip устройства чс | номер сценария |   
         * 
         * 
         * 
         */
        private string server;
        private string port;
        private string user;
        private string password;
        private static string connectString;

        private NpgsqlConnection connect;
        private Files files = new Files();
        WorkElementDBAlarm workElement = new WorkElementDBAlarm();

        //подключение к базе данных +
        public AlarmDB(string _server, string _port, string _user, string _password)
        {
            server = _server;
            port = _port;
            user = _user;
            password = _password;
            connectString = "Server=" + server + ";Port=" + port + ";User Id=" + user + ";Password=" + password + ";";
            connect = new NpgsqlConnection(connectString);
        }

        //Создание таблицы Оповещения +
        public bool CreateTableAlarm(string _nametable)
        {

            string sqlStr = "CREATE TABLE if not exists " + _nametable +
                  "(  id serial,\r\n" +
                  "ip_device   text,\r\n" +
                  "script text,\r\n" +
                  "path   text,\r\n" +
                  "notify   text\r\n)";
            Console.WriteLine(sqlStr);


            try
            {
                connect.Open();
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, connect);
                sqlCommand.ExecuteNonQuery();
                connect.Close();
                return true;
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                MessageBox.Show("Ошибка подключения к базе данных 'Датчики ЧС'");
=======
                MessageBox.Show("Ошибка подключения к базе данных 'Оповещения'");
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
                files.ReadException(ex);
                connect.Close();
                return false;
            }
        }

        //Удаление таблицы + 
        public bool DeleteTable(string _nametable)
        {
            try
            {
                string sqlStr = "DROP TABLE " + _nametable;
                connect.Open();
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, connect);
                sqlCommand.ExecuteNonQuery();
                connect.Close();
                return true;
            }
            catch (Exception ex)
            {
                files.ReadException(ex);
                connect.Close();
                return false;
            }
        }

        //Подключение к таблице + 
        public bool ConnectDataBase()
        {
            try
            {
                connect.Open(); //Открываем соединение.
                connect.Close(); //Закрываем соединение.
                return true;
            }
            catch (Exception ex)
            {
                files.ReadException(ex);
                connect.Close();
                return false;
            }
        }
      
        //Добавление данных в таблицу +
        public bool InsertDataBase(string _nametable, string _ip_device, string _script, string _path, string _notify)
        {
            string sqlStr = "INSERT INTO " + _nametable + " (ip_device ,script,path ,notify) " +
                "VALUES(";
            //"(  id serial,\r\n" +
            //"ip_device   text,\r\n" +
            //"script text,\r\n" +
            //"path   text,\r\n" +
            //"notify   text\r\n)";

            try
            {
                connect.Open();
                sqlStr = sqlStr + _ip_device + "," + _script + "," + _path + "," + _notify + ")";
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, connect);
                Console.WriteLine(sqlStr);
                sqlCommand.ExecuteNonQuery();
                connect.Close();
                return true;
            }
            catch (Exception ex)
            {
                connect.Close();
                files.ReadException(ex);
                return false;
            }
        }

        //Выгрузка базы данных +
        public NpgsqlDataReader GetDataBase(string _nametable)
        {
            try
            {
                //"(  id serial,\r\n" +
                //"ip_device   text,\r\n" +
                //"script text,\r\n" +
                //"path   text,\r\n" +
                //"notify   text\r\n)";
                connect.Open();
                string sql = "SELECT id,ip_device,script,path,notify FROM " + _nametable;
                using var cmd = new NpgsqlCommand(sql, connect);
                Console.WriteLine(sql);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                connect.Close();
                return rdr;
            }
            catch (Exception ex)
            {

                files.ReadException(ex);
                connect.Close();
                return null;
            }
        }

        //Удаление записи в таблице +
        public bool DeleteValueDB(string _nametable, int _id)
        {
            string sqlStr = "Delete from " + _nametable +
                " where id = " + _id;
            try
            {
                connect.Open();
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, connect);
                sqlCommand.ExecuteNonQuery();
                Console.WriteLine(sqlStr);
                connect.Close();
                return true;
            }
            catch (Exception ex)
            {
                files.ReadException(ex);
                connect.Close();
                return false;
            }
        }

        //кол-во записей в БД + 
        public string CountDataInBD(string _nametable)
        {
            string count = null;
            //select count(*) from arm
            try
            {
                string sqlStr = "select count(*) from " + _nametable;
                connect.Open();
                using var cmd = new NpgsqlCommand(sqlStr, connect);
                Console.WriteLine(sqlStr);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("{0} - last id ", Convert.ToString(rdr.GetInt64(0)));
                    count = Convert.ToString(rdr.GetInt64(0));
                }
                connect.Close();
                return count;
            }
            catch (Exception _ex)
            {
                connect.Close();
                files.ReadException(_ex);
                return null;
            }
        }

        //получение последнего ID в базе данных +
        public int GetLastID(string _nametable)
        {
            /*
           SELECT * 
            FROM test2 
            ORDER BY id DESC, 
             id DESC 
            LIMIT 1;
           */

            int lastId = -1;
            try
            {
                string sqlStr = "SELECT * FROM " + _nametable + " ORDER BY id DESC , id DESC LIMIT 1";
                connect.Open();
                using var cmd = new NpgsqlCommand(sqlStr, connect);
                Console.WriteLine(sqlStr);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("{0} - last id ", rdr.GetInt32(0));
                    lastId = rdr.GetInt32(0);
                }
                connect.Close();
                return lastId;
            }
            catch (Exception _ex)
            {
                connect.Close();
                files.ReadException(_ex);
                return -1;
            }
        }

        //Очистка таблицы + 
        public bool ClearTable(string _nametable)
        {
            try
            {
                string sqlStr = "TRUNCATE " + _nametable + ";";
                //TRUNCATE name_table
                connect.Open();
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, connect);
                sqlCommand.ExecuteNonQuery();
                connect.Close();
                return true;
            }
            catch (Exception _ex)
            {
                connect.Close();
                files.ReadException(_ex);
                return false;
            }
        }

        //Обновление элемента в таблице
        public bool UpdateElementDataBase(string _nametable, string _id, string _ip_device, string _script, string _path, string _notify)
        {
            /*
                * update test2 
                * set text = 122 , value = 12
                * where id = 2 
            */

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string sqlStr = "Update " + _nametable + " set ";
            string sqlStrWhere = " where id = " + _id;
            if ((_ip_device == null) && (_script == null) && (_path == null) && (_notify == null))
            {
                Console.WriteLine("1");
                return false;
            }
            if ((_ip_device != null) && (_script == null) && (_path == null) && (_notify == null))
            {
                Console.WriteLine("2");
                sqlStr = sqlStr + "ip_device = " + _ip_device + sqlStrWhere;
            }
            if ((_ip_device == null) && (_script != null) && (_path == null) && (_notify == null))
            {
                Console.WriteLine();
                sqlStr = sqlStr + "script = " + _script + sqlStrWhere;
            }
            if ((_ip_device == null) && (_script == null) && (_path != null) && (_notify == null))
            {
                Console.WriteLine("3");
                sqlStr = sqlStr + "path = " + _path + sqlStrWhere;
            }
            if ((_ip_device == null) && (_script == null) && (_path == null) && (_notify != null))
            {
                Console.WriteLine("4");
                sqlStr = sqlStr + "notify = " + _path + sqlStrWhere;
            }

            if ((_ip_device != null) && (_script != null) && (_path != null) && (_notify != null))
            {
                sqlStr = sqlStr +
                    "ip_device = " + "'" + _ip_device + "'" + ',' +
                    "script = " + "'" + _script + "'" + ',' +
                    "path = " + "'" + _path + "'" + ',' +
                    "notify = " + "'" + _notify + "'"
                    + sqlStrWhere;
                Console.WriteLine("5");
            }
            try
            {
                connect.Open();
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, connect);
                sqlCommand.ExecuteNonQuery();
                Console.WriteLine(sqlStr);
                connect.Close();
                return true;
            }
            catch (Exception ex)
            {

                files.ReadException(ex);
                connect.Close();
                return false;
            }
        }

        public Tuple<string, string> GettingListOfDevices(string _nametable, string _ip)
        {
            //select script, notify from alarmdb
            //where ip_device = '127.0.0.1'
            

            try
            {
                string sqlStr = "select script,notify from " + _nametable;
                string sqlStrWhere = " where ip_device = " + "'" +_ip + "'";
                sqlStr = sqlStr + sqlStrWhere;
                string AllIp = "";
                string Number = "";
                
                connect.Open();
                using var cmd = new NpgsqlCommand(sqlStr, connect);
                Console.WriteLine(sqlStr);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("result {0} {1} ", rdr.GetString(0), rdr.GetString(1));
                    AllIp = rdr.GetString(1);
                    Number = rdr.GetString(0);
                }
               
                connect.Close();
                return Tuple.Create(AllIp, Number);
            }
            catch (Exception _ex)
            {
                connect.Close();
                files.ReadException(_ex);
                return Tuple.Create("","");
            }
        }



        //поиск ip метеостанции от датчика ЧС ???
        public Tuple<string, string> returnIp4CDevice(string _nametable, string _ip)
        {
            //Select ip_device from meteostation where description ='127.0.0.1'
            string ip = "";
            try
            {
                string sqlStr = "Select ip_device from " + _nametable + " where description =" + "'" + _ip + "'";
                connect.Open();
                using var cmd = new NpgsqlCommand(sqlStr, connect);
                Console.WriteLine(sqlStr);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("{0} -  id ", rdr.GetString(0));
                    ip = rdr.GetString(0);
                }
                connect.Close();
                return Tuple.Create(ip, "0");
            }
            catch (Exception _ex)
            {
                connect.Close();
                files.ReadException(_ex);
                return Tuple.Create("", "0");
            }
        }

        //Удаление таблицы ???
        public bool DropTable(string _nametable)
        {
            try
            {
                string sqlStr = "Drop table " + _nametable + ";";
                connect.Open();
                Console.WriteLine(sqlStr);
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, connect);
                sqlCommand.ExecuteNonQuery();
                connect.Close();
                return true;
            }
            catch (Exception _ex)
            {
                connect.Close();
                files.ReadException(_ex);
                return false;
            }
        }



    }
}
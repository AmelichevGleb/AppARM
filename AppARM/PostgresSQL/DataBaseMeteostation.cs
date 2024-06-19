using System;
using System.Collections.Generic;
using Npgsql;

using AppARM.Structure;
using AppARM.FilesLogs;
using AppARM.Weather;
using static AppARM.Structure.ElementDataBase;
using System.Windows;

namespace AppARM.PostgresSQL
{

    public class DataBaseMeteostation
    {
        private string server;
        private string port;
        private string user;
        private string password;
        private static string connectString;

        private NpgsqlConnection connect;
        private Files files = new Files();
        WorkElementDBMeteo workElement = new WorkElementDBMeteo();

        //подключение к базе данных
        public DataBaseMeteostation(string _server, string _port, string _user, string _password)
        {
                server = _server;
                port = _port;
                user = _user;
                password = _password;
                connectString = "Server=" + server + ";Port=" + port + ";User Id=" + user + ";Password=" + password + ";";
                connect = new NpgsqlConnection(connectString);
        }


        //Создание таблицы метеостанции
        public bool CreateTable(string _nametable)
        {

           string sqlStr = "CREATE TABLE if not exists " + _nametable +
                 "(  id serial,\r\n" +
                 "ip_device   text,\r\n" +
                 "port text,\r\n" +
                 "location   text,\r\n" +
                 "longitude   text,\r\n" +
                 "lagatitude   text,\r\n" +
<<<<<<< HEAD
                 "description  text\r\n)";
=======
                 "description  text,\r\n" +
                 "temperature text,\r\n" +
                 "windSpeed text,\r\n" +
                 "directionWind text \r\n)";
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
             
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
                MessageBox.Show("Ошибка подключения к базе данных 'Метеостанция'");
                files.ReadException(ex);
                connect.Close();
                return false;
            }
        }

        //Удаление таблицы
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

        //Подключение к таблице
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

        //поиск ip метеостанции от датчика ЧС
        public Tuple<string, string> returnIp4CDevice(string _nametable, string _ip)
        {
            //Select ip_device from meteostation where description ='127.0.0.1'
            string ip = "";
            string port = "";
            try
            {
                string sqlStr = "Select ip_device,port from " + _nametable + " where description =" + "'" + _ip+"'";
                connect.Open();
                using var cmd = new NpgsqlCommand(sqlStr, connect);
                Console.WriteLine(sqlStr);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("{0} -  id ", rdr.GetString(0));
                    Console.WriteLine("{0} -  Port ", rdr.GetString(1));
                    ip = rdr.GetString(0);
                    port = rdr.GetString(1);

                }
                connect.Close();
                return Tuple.Create(ip,port);
            }
            catch (Exception _ex)
            {
                connect.Close();
                files.ReadException(_ex);
                return Tuple.Create("","0");
            }
        }

<<<<<<< HEAD
        //поиск долготы по ip адресу 
        //select longitude from meteostation
        //where ip_device = '127.0.0.1'

        public string  returnLongitudeMeteostation( string _ip)
        {
            string longitude = "";
            try
            {
                string sqlStr = "select longitude from meteostation "  + " where ip_device = " + "'" + _ip + "'";
                connect.Open();
                using var cmd = new NpgsqlCommand(sqlStr, connect);
                Console.WriteLine(sqlStr);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("{0} -  longitude ", rdr.GetString(0));
                    longitude = rdr.GetString(0);
                   

                }
                connect.Close();
                return longitude;
            }
            catch (Exception _ex)
            {
                connect.Close();
                files.ReadException(_ex);
                return "";
            }
        }

        //Добавление данных в таблицу
        public bool InsertDataBase(string _nametable, string _ipDevice, string _port, string _location, string _longitude, string _lagatitude, string _description)
        {
            string sqlStr = "INSERT INTO " + _nametable + " (ip_device ,port,location ,longitude ,lagatitude,description) " +
=======
        //Добавление данных в таблицу
        public bool InsertDataBase(string _nametable, string _ipDevice, string _port, string _location, string _longitude, string _lagatitude, string _description, string _temperature, string _windspeed, string _directionwind)
        {
            string sqlStr = "INSERT INTO " + _nametable + " (ip_device ,port,location ,longitude ,lagatitude,description,temperature, windspeed,directionwind) " +
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
                "VALUES(";
            try
            {
                connect.Open();
<<<<<<< HEAD
                sqlStr = sqlStr + _ipDevice + "," + _port + "," + _location + "," + _longitude + "," + _lagatitude + "," + _description +  ")";
=======
                sqlStr = sqlStr + _ipDevice + "," + _port + "," + _location + "," + _longitude + "," + _lagatitude + "," + _description + "," + _temperature + "," + _windspeed + "," + _directionwind + ")";
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
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

        //Удаление записи в таблице
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

        //Выгрузка базы данных (с метеоданными)

        public NpgsqlDataReader GetDataBaseLong(string _nametable)
        {
            try
            {
                connect.Open();
                string sql = "SELECT * FROM " + _nametable;
                using var cmd = new NpgsqlCommand(sql, connect);
                Console.WriteLine(sql);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                connect.Close();
                return rdr;
            }
            catch (Exception ex)
            {
                List<ElementDataBase> element1 = new List<ElementDataBase>();
                files.ReadException(ex);
                connect.Close();
                return null;
            }
        }

        //Выгрузка базы данных (без метеоданных)
        public NpgsqlDataReader GetDataBaseShort(string _nametable)
        {
            try
            {
                connect.Open();
                string sql = "SELECT id,ip_device,port,location,longitude,lagatitude,description FROM " + _nametable;
                using var cmd = new NpgsqlCommand(sql, connect);
                Console.WriteLine(sql);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                connect.Close();
                return rdr;
            }
            catch (Exception ex)
            {
                List<ElementDataBase> element1 = new List<ElementDataBase>();
                files.ReadException(ex);
                connect.Close();
                return null;
            }
        }
        //кол-во записей в БД
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
        //Очистка таблицы
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

        //Удаление таблицы
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

        //получение последнего ID в базе данных
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

        //Обновление элемента в таблице
<<<<<<< HEAD
        public bool UpdateElementDataBase(string _nametable, string _id, string _ipDevice, string _port, string _location, string _longitude, string _lagatitude, string _description)
=======
        public bool UpdateElementDataBase(string _nametable, string _id, string _ipDevice, string _port, string _location, string _longitude, string _lagatitude, string _description, string _temperature, string _windspeed, string _directionwind)
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
        {
            /*
                * update test2 
                * set text = 122 , value = 12
                * where id = 2 
            */

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
<<<<<<< HEAD
           
            string sqlStr = "Update " + _nametable + " set ";
            string sqlStrWhere = " where id = " + _id;
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description == null))
=======
            Console.WriteLine(_temperature);
            string sqlStr = "Update " + _nametable + " set ";
            string sqlStrWhere = " where id = " + _id;
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description == null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            {
                Console.WriteLine("1");
                return false;
            }
<<<<<<< HEAD
            if ((_ipDevice != null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description == null))
=======
            if ((_ipDevice != null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description == null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            {
                Console.WriteLine("2");
                sqlStr = sqlStr + "ip_device = " + _ipDevice + sqlStrWhere;
            }
<<<<<<< HEAD
            if ((_ipDevice == null) && (_port != null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description == null))
=======
            if ((_ipDevice == null) && (_port != null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description == null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            {
                Console.WriteLine();
                sqlStr = sqlStr + "port = " + _port + sqlStrWhere;
            }
<<<<<<< HEAD
            if ((_ipDevice == null) && (_port == null) && (_location != null) && (_longitude == null) && (_lagatitude == null) && (_description == null))
=======
            if ((_ipDevice == null) && (_port == null) && (_location != null) && (_longitude == null) && (_lagatitude == null) && (_description == null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            {
                Console.WriteLine("3");
                sqlStr = sqlStr + "location = " + _location + sqlStrWhere;
            }
<<<<<<< HEAD
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude != null) && (_lagatitude == null) && (_description == null))
=======
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude != null) && (_lagatitude == null) && (_description == null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            {
                Console.WriteLine("4");
                sqlStr = sqlStr + "longitude = " + _longitude + sqlStrWhere;
            }
<<<<<<< HEAD
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude != null) && (_description == null))
=======
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude != null) && (_description == null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            {
                Console.WriteLine("5");
                sqlStr = sqlStr + "lagatitude = " + _lagatitude + sqlStrWhere;
            }
<<<<<<< HEAD
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description != null))
=======
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description != null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            {
                Console.WriteLine("6");
                sqlStr = sqlStr + "description = " + _description + sqlStrWhere;
            }
<<<<<<< HEAD

            if ((_ipDevice != null) && (_port != null) && (_location != null) && (_longitude != null) && (_lagatitude != null) && (_description != null))
=======
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description == null) & (_temperature != null) && (_windspeed != null) && (_directionwind != null))
            {
                sqlStr = sqlStr + "temperature = " + "'" + _temperature + "'," + " " + "windspeed = " + "'" + _windspeed + "'," + " " + "directionwind = " + "'" + _directionwind + "'" + " " + sqlStrWhere;
            }
            if ((_ipDevice != null) && (_port != null) && (_location != null) && (_longitude != null) && (_lagatitude != null) && (_description != null) & (_temperature != null) && (_windspeed != null) && (_directionwind != null))
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            {
                sqlStr = sqlStr +
                    "ip_device = " + "'" + _ipDevice + "'" + ',' +
                    "port = " + "'" + _port + "'" + ',' +
                    "location = " + "'" + _location + "'" + ',' +
                    "longitude = " + "'" + _longitude + "'" + ',' +
                    "lagatitude = " + "'" + _lagatitude + "'" + ',' +
                    "description = " + "'" + _description + "'"
                    + sqlStrWhere;
                Console.WriteLine("7");
                // "temperature = " + "'" + _temperature + "'" + ',' +
                // "windspeed = " + "'" + _windspeed + "'" + ',' +
                // "directionwind = " + "'" + _directionwind + "'"
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
    }
}
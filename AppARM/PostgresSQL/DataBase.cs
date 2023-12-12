using System;

using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using AppARM.Structure;
using AppARM.TestXML;

using Npgsql;
using static System.Net.Mime.MediaTypeNames;
using static AppARM.Structure.ElementDataBase;

namespace AppARM.PostgresSQL
{

    public class DataBase
    {

        private string server;
        private string port;
        private string user;
        private string password;
        private static string connStr;
  
        private NpgsqlConnection conn;
        private Files files = new Files();
        WorkElementDB workElement = new WorkElementDB();

        public DataBase(string _server, string _port, string _user, string _password)
        {
            server = _server;
            port = _port;
            user = _user;
            password = _password;
            connStr = "Server=" + server + ";Port=" + port + ";User Id=" + user + ";Password=" + password + ";";
            conn = new NpgsqlConnection(connStr);
        }
        
        //Создание таблицы Девайсов
        public bool CreateTableApy(string _nametable)
        {
            string sqlStr = "CREATE TABLE if not exists " + _nametable +
                "(  id serial,\r\n" +
                "ip_device   text,\r\n" +
                "port text,\r\n" +
                "location   text,\r\n" +
                "longitude   text,\r\n" +
                "lagatitude   text,\r\n" +
                "description  text,\r\n" +
                "temperature text,\r\n" +
                "windSpeed text,\r\n" +
                "directionWind text \r\n)";
            try
            {
                conn.Open();
                //string sqlStr = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='" + _nametable + "'";
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, conn);
                sqlCommand.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                files.ReadExeption(ex);
                conn.Close();
                return false;
            }
        }
        
        //Удаление таблицы
        public bool DeleteTable(string _nametable)
        {
            try
            {
                string sqlStr = "DROP TABLE " + _nametable;
                conn.Open();
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, conn);
                sqlCommand.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                files.ReadExeption(ex);
                conn.Close();
                return false;
            }
        }  
       
        //Подключение к таблице
        public bool ConnectDataBase()
        {
            try
            {
                conn.Open(); //Открываем соединение.
                conn.Close(); //Закрываем соединение.
                return true;
            }
            catch (Exception ex)
            {
                files.ReadExeption(ex);
                conn.Close();
                return false;
            }
        }

        //Добавление данных в таблицу
        public bool InsertDataBase(string _nametable, string _ipDevice,string _port, string _location,string _longitude , string _lagatitude,string _description , string _temperature, string _windspeed ,string _directionwind)
        {
            string sqlStr = "INSERT INTO " + _nametable  + " (ip_device ,port,location ,longitude ,lagatitude,description,temperature, windspeed,directionwind) " +
                "VALUES(";
            try
            {
                
                conn.Open();
                sqlStr = sqlStr + _ipDevice + ","+ _port + "," + _location + "," + _longitude + "," + _lagatitude + "," + _description+ "," + _temperature + "," + _windspeed + "," + _directionwind + ")";
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, conn);
                Console.WriteLine(sqlStr);
                sqlCommand.ExecuteNonQuery();
                 conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                conn.Close();
                files.ReadExeption(ex);
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
                conn.Open();
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, conn);
                sqlCommand.ExecuteNonQuery();
                Console.WriteLine(sqlStr);
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                files.ReadExeption(ex);
                conn.Close();
                return false;
            }
        }


        //Выгрузка базы данных (с метеоданными)

        public NpgsqlDataReader GetDataBaseLong(string _nametable)
        {
            try
            {
                conn.Open();
                string sql = "SELECT * FROM " + _nametable;
                using var cmd = new NpgsqlCommand(sql, conn);
                Console.WriteLine(sql);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                conn.Close();
                return rdr;
            }
            catch (Exception ex)
            {
                List<ElementDataBase> element1 = new List<ElementDataBase>();
                files.ReadExeption(ex);
                conn.Close();
                return null;
            }

        }

        //Выгрузка базы данных (без метеоданных)
        public NpgsqlDataReader GetDataBaseShort(string _nametable)
        {
            try
            {
                conn.Open();
                string sql = "SELECT id,ip_device,port,location,longitude,lagatitude,description FROM " + _nametable ;
                using var cmd = new NpgsqlCommand(sql, conn);
                Console.WriteLine(sql);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                conn.Close();
                return rdr;
            }
            catch (Exception ex)
            {
                List<ElementDataBase> element1 = new List<ElementDataBase>();
                files.ReadExeption(ex);
                conn.Close();
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
                conn.Open();
                using var cmd = new NpgsqlCommand(sqlStr, conn);
                Console.WriteLine(sqlStr);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("{0} - last id ",Convert.ToString( rdr.GetInt64(0)));
                    count = Convert.ToString(rdr.GetInt64(0));
                }
                conn.Close();
                return count;
            }
            catch (Exception _ex)
            {
                conn.Close();
                files.ReadExeption(_ex);
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
                conn.Open();
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, conn);
                sqlCommand.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception _ex)
            {
                conn.Close();
                files.ReadExeption(_ex);
                return false;
            }
        }
        
        //Удаление таблицы
        public bool DropTable(string _nametable)
        {
            try
            {
                string sqlStr = "Drop table " + _nametable + ";";
                conn.Open();
                Console.WriteLine(sqlStr);
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, conn);
                sqlCommand.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception _ex)
            {
                conn.Close();
                files.ReadExeption(_ex);
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
                string sqlStr = "SELECT * FROM "  + _nametable + " ORDER BY id DESC , id DESC LIMIT 1";
                conn.Open();
                using var cmd = new NpgsqlCommand(sqlStr, conn);
                Console.WriteLine(sqlStr);
                using NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("{0} - last id ", rdr.GetInt32(0));
                    lastId = rdr.GetInt32(0);
                }
                conn.Close();
                return lastId;
            }
            catch (Exception _ex)
            {
                conn.Close();
                files.ReadExeption(_ex);
                return -1;
            }
        }

        //Обновление элемента в таблице
        //(string _nametable, string _ipDevice,string _port, string _location,string _longitude , string _lagatitude,string _description , string _temperature, string _windspeed ,string _directionwind)
        public bool UpdateElementDataBase(string _nametable, string _id, string _ipDevice, string _port, string _location, string _longitude, string _lagatitude, string _description, string _temperature, string _windspeed, string _directionwind)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Console.WriteLine(_temperature);
            
            string sqlStr = "Update " + _nametable + " set ";
            string sqlStrWhere = " where id = " + _id;

            /*
             * update test2 
             * set text = 122 , value = 12
             * where id = 2 
             */
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description == null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
            {
                Console.WriteLine("1");
                return false;
            }
            if ((_ipDevice != null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description == null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
            {
                Console.WriteLine("2");
                sqlStr = sqlStr + "ip_device = " + _ipDevice + sqlStrWhere;
            }
            if ((_ipDevice == null) && (_port != null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description == null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
            {
                Console.WriteLine();
                sqlStr = sqlStr + "port = " + _port + sqlStrWhere;
            }
            if ((_ipDevice == null) && (_port == null) && (_location != null) && (_longitude == null) && (_lagatitude == null) && (_description == null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
            {
                Console.WriteLine("3");
                sqlStr = sqlStr + "location = " + _location + sqlStrWhere;
            }
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude != null) && (_lagatitude == null) && (_description == null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
            {
                Console.WriteLine("4");
                sqlStr = sqlStr + "longitude = " + _longitude + sqlStrWhere;
            }
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude != null) && (_description == null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
            {
                Console.WriteLine("5");
                sqlStr = sqlStr + "lagatitude = " + _lagatitude + sqlStrWhere;
            }
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description != null) & (_temperature == null) && (_windspeed == null) && (_directionwind == null))
            {
                Console.WriteLine("6");
                sqlStr = sqlStr + "description = " + _description + sqlStrWhere;
            }
            
            if ((_ipDevice == null) && (_port == null) && (_location == null) && (_longitude == null) && (_lagatitude == null) && (_description == null) & (_temperature != null) && (_windspeed != null) && (_directionwind != null))
            {
                sqlStr = sqlStr + "temperature = " + "'"+_temperature + "'," + " " + "windspeed = " + "'" + _windspeed + "'," + " " + "directionwind = " + "'" + _directionwind + "'" + " "  + sqlStrWhere; 
            }
          
            if ((_ipDevice != null) && (_port != null) && (_location != null) && (_longitude != null) && (_lagatitude != null) && (_description != null) & (_temperature != null) && (_windspeed != null) && (_directionwind != null))
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
                conn.Open();              
                NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlStr, conn);
                sqlCommand.ExecuteNonQuery();
                Console.WriteLine(sqlStr);
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {

                files.ReadExeption(ex);
                conn.Close();
                return false;
            }
        }
    }
}

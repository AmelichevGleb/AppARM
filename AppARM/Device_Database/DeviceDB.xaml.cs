using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Input;

using AppARM.WeatherSokol;
using AppARM.PostgresSQL;
using AppARM.Structure;
using static AppARM.Structure.ElementDataBase;
using AppARM.Device_Database;
using AppARM.Test;
using AppARM.Weather;

namespace AppARM.Device_Database
{
    /// <summary>
    /// Логика взаимодействия для DeviceDB.xaml
    /// </summary>

    //https://www.csharp-console-examples.com/wpf/wpf-entity-framework-select-insert-update-delete/


    public partial class DeviceDB : Window
    {


        List<StructList> deviceList = new List<StructList>(1); // список всех устройств из базы данных
        WorkElementDBMeteo wDB = new WorkElementDBMeteo();
        List<ElementDataBase> element = new List<ElementDataBase>();

        private DataBaseMeteostation db;

        private string tableNameMeteo = "Meteostation"; // Название таблицы для метеостанций 
        private string tableNameDanger = "DeviceDanger"; // Название таблицы для датчиков ЧС
        private string tableNameNull = "Null"; //Название для будущей таблицы

        private byte[] Message = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 };

        private double temperature;
        private double windSpeed;
        private int directionWind;

        public DeviceDB(DataBaseMeteostation _dataBase)
        {
            db = _dataBase;
            InitializeComponent();
            LoadMeteoStation();
            //LoadDangerDevice();
            DG_MeteoStation.ItemsSource = deviceList;
            DG_MeteoStation.Visibility = Visibility.Visible;
            DG_DeviceDanger.Visibility = Visibility.Collapsed;
        }

        //____________________________ОСНОВНЫЕ_КНОПКИ__________________________________________
       
        //Кнопка OK
        private void BC_Ok(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //Кнопка Cancel
        private void BC_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MI_MeteoStation_Click(object sender, RoutedEventArgs e)
        {
            DG_MeteoStation.Visibility = Visibility.Visible;
            DG_DeviceDanger.Visibility = Visibility.Collapsed;
        }

        private void MI_Danger_Click(object sender, RoutedEventArgs e)
        {
            DG_MeteoStation.Visibility = Visibility.Collapsed;
            DG_DeviceDanger.Visibility = Visibility.Visible;
        }

        //Пока пустое 
        private void MI_Apy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Будет позже");
        }
        //__________________________________________________________________________________________

        //____________________________ОСНОВНЫЕ_ФУНКЦИИ__________________________________________

        //загрузка и заполнение базы данных (Метеостанций)
        private void LoadMeteoStation()
        {
            //Загрузка таблицы с Метеостанциями
            var t = db.GetDataBaseShort(tableNameMeteo);
            try
            {
                if (t != null)
                {
                    while (t.Read())
                    {
                        deviceList.Add(new StructList(Convert.ToInt32(t.GetInt32(0)), Convert.ToString(t.GetString(1)),
                            Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5)),
                            Convert.ToString(t.GetString(6))));
                        Console.WriteLine("{0} {1} {2} {3} {4} {5}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5));
                    }
                }
                else
                {
                    db.CreateTable(tableNameMeteo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }
        //Загрузка таблицы с датчиков чс
        private void LoadDangerDevice()
        {
            //Загрузка таблицы с Метеостанциями
            var t = db.GetDataBaseShort(tableNameDanger);
            try
            {
                /*
                id serial NOT NULL,
                ip_device text,
                port text,
                location text,
                longitude text,
                lagatitude text,
                description text
                */
                if (t != null)
                {
                    while (t.Read())
                    {
                        deviceList.Add(new StructList(Convert.ToInt32(t.GetInt32(0)), Convert.ToString(t.GetString(1)),
                            Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5)),
                            Convert.ToString(t.GetString(6))));
                        Console.WriteLine("{0} {1} {2} {3} {4} {5}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5));
                    }
                }
                else
                {

                    db.CreateTable(tableNameDanger);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        private void GetWeather(string _ip, string port)
        {
            this.temperature = 0;
            this.windSpeed = 0;
            this.directionWind = 0;
            try
            {
                TcpClient tcpClient = new TcpClient();
                // Test.Text += "Проверка по " + ipAdress + ":" + port + '\n'; 
                tcpClient.Connect(_ip, Convert.ToInt32(port));
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(Message, 0, Message.Length);

                byte[] bytes = new byte[tcpClient.ReceiveBufferSize];
                int bytesRead = stream.Read(bytes, 0, tcpClient.ReceiveBufferSize);

                ByteWeather byteWeather = new ByteWeather(null, null, 0, null, null, null, bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], bytes[6], bytes[7], bytes[8],
                        bytes[9], bytes[10], bytes[11], bytes[12], bytes[13], bytes[14], bytes[15], bytes[16], bytes[17], bytes[18], bytes[19], bytes[20], bytes[21], bytes[22], bytes[23],
                        bytes[24], bytes[25], bytes[26], bytes[27], bytes[28], false);

                this.temperature = byteWeather.temperature;
                this.windSpeed = byteWeather.windSpeed;
                this.directionWind = byteWeather.directionWind;

                Thread.Sleep(10);

                tcpClient.Close();
            }
            catch (Exception ex)
            {

            }
        }

        //поиск индекса в списке
        public int FindIndexsList(List<StructList> deviceList, int id)
        {
            int index = deviceList.FindIndex(
                     delegate (StructList structList)
                     {
                         return structList.Id.Equals(id);
                     }
                 );
            Console.WriteLine("Индекс - {0}", index);
            return index;
        }

        //Вывести данные в списке
        private void Shows(List<StructList> deviceList)
        {
            foreach (var device in deviceList)
            {
                Console.WriteLine(Convert.ToString(device.Id), "  ", device.IP_device, "  ", device.Location, "  ", device.Longitude, "  ", device.Lagatitude, "  ", device.Description);
            }
        }

        //__________________________________________________________________________________________

        //____________________________ОСНОВНЫЕ_МЕТОДЫ_ОКНА__________________________________________

        private void DG_device_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        //обновление данных в таблице
        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            StructList p = e.Row.Item as StructList;
            MessageBox.Show($"{p.Id} {p.IP_device} {p.Port} {p.Location} {p.Longitude} {p.Lagatitude} {p.Description}");
            //вызов обновления базы данных 
            GetWeather(p.IP_device, p.Port);
            db.UpdateElementDataBase(tableNameMeteo, Convert.ToString(p.Id), p.IP_device, Convert.ToString(p.Port), p.Location, p.Longitude, p.Lagatitude, p.Description, Convert.ToString(temperature), Convert.ToString(windSpeed), Convert.ToString(directionWind));
            db.UpdateElementDataBase(tableNameMeteo, Convert.ToString(p.Id), null, null, null, null, null, null, Convert.ToString(temperature), Convert.ToString(windSpeed), Convert.ToString(directionWind));
        }

        //Получаем данные из таблицы
        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            StructList path = DG_MeteoStation.SelectedItem as StructList;
            //MessageBox.Show(" ID: " + path.Id + "\n Исполнитель: " + path.Name + "\n Альбом: " + path.Age
            //    + "\n Год: ");
        }


        //вывод информации о записи
        private void MenuItem_Click_Show(object sender, RoutedEventArgs e)
        {
            StructList path = DG_MeteoStation.SelectedItem as StructList;
            try
            {
                if (path != null)
                {
                    MessageBox.Show(" ID: " + path.Id + "\n Ip_device: " + path.IP_device + "\n location: " + path.Location + "\n longitode: " + path.Longitude
                        + "\n lagatitude: " + path.Lagatitude + "\n Description: " + path.Description + "\n null: ");
                }
            }
            finally { Console.WriteLine("Не выбрано поле для методы SHOW "); }
        }

        //вывод информации о записи
        private void MenuItem_Click_Add(object sender, RoutedEventArgs e)
        {
            db.InsertDataBase(tableNameMeteo, "'127.0.0.1'", "11000", "'Kaluga'", "54.5293", "36.2754", "'127.0.0.2'", "'test'", "'test'", "'test'");
            var id = db.GetLastID(tableNameMeteo);
            deviceList.Add(new StructList(id, "127.0.0.1", "11000", "Kaluga", "54.5293", "36.2754", "127.0.0.2"));
            DG_MeteoStation.ItemsSource = deviceList.ToList();
        }

        //удаление записи из таблицы
        private void MenuItem_Click_Delete(object sender, RoutedEventArgs e)
        {
            StructList path = DG_MeteoStation.SelectedItem as StructList;
            try
            {
                if (path != null)
                {
                    db.DeleteValueDB(tableNameMeteo, path.Id);
                    var t = FindIndexsList(deviceList, path.Id);
                    deviceList.RemoveAt(FindIndexsList(deviceList, path.Id));
                    MessageBox.Show(" ID: " + path.Id + "\n Ip_device: " + path.IP_device + "\n location: " + path.Location + "\n longitode: " + path.Longitude
                         + "\n lagatitude: " + path.Lagatitude + "\n Description: " + path.Description + "\n null: ");
                    DG_MeteoStation.ItemsSource = deviceList.ToList();
                    Console.WriteLine("--------------------------------------------------");
                    Shows(deviceList);
                }
            }
            finally
            {
                Console.WriteLine("Не выбрано поле для методы Delete ");
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_Show1(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_Add1(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_Delete1(object sender, RoutedEventArgs e)
        {

        }


        //__________________________________________________________________________________________
    }
}
using AppARM.PostgresSQL;
using AppARM.Structure;
using AppARM.WeatherSokol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static AppARM.Structure.ElementDataBase;
using AppARM.FilesLogs;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
<<<<<<< HEAD
using System.Net.Http;
=======
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180

namespace AppARM.Device_Database
{
    /// <summary>
    /// Логика взаимодействия для UserControlDataBase.xaml
    /// </summary>
    public partial class UserControlDataBase : UserControl
    {

        List<StructList> meteodeviceList = new List<StructList>(1); 
        List<StructList> alarmDeviceList = new List<StructList>(1);
        List<StructList> scenarioList = new List<StructList>(1);
        
        WorkElementDBMeteo wDB = new WorkElementDBMeteo();
        List<ElementDataBase> element = new List<ElementDataBase>();

        private DataBaseMeteostation dbMeteostation;
        private AlarmDB dbAlarm;
        private AdditionalDb dbScenario;

        private Files files = new Files();

        private string tableNameMeteo = "Meteostation"; // Название таблицы для метеостанций 
        private string tableNameDanger = "AlarmDB"; // Название таблицы для датчиков ЧС
        private string tableNameScenario = "Scenario"; //Название для будущей таблицы

        private byte[] Message = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x5A, 0xC5, 0xF1 };

        private double temperature;
        private double windSpeed;
        private int directionWind;
        public UserControlDataBase(DataBaseMeteostation _dataBase, AlarmDB _alarmDB , AdditionalDb _scenarioDb)
        {
            dbMeteostation = _dataBase;
            dbAlarm = _alarmDB;
            dbScenario = _scenarioDb;
            InitializeComponent();
            LoadMeteoStation();
            LoadDangerDevice();
            LoadScenario();
            DG_MeteoStation.ItemsSource = meteodeviceList;
            DG_DeviceDanger.ItemsSource = alarmDeviceList;
            DG_Scenario.ItemsSource = scenarioList;
            DG_MeteoStation.Visibility = Visibility.Collapsed;
            DG_DeviceDanger.Visibility = Visibility.Collapsed;
            DG_Scenario.Visibility= Visibility.Collapsed;
        }

        //____________________________ОСНОВНЫЕ_КНОПКИ__________________________________________

        //Кнопка OK
        private void BC_Ok(object sender, RoutedEventArgs e)
        {
            //this.Close();
        }
        //Кнопка Cancel
        private void BC_Cancel(object sender, RoutedEventArgs e)
        {
            //this.Close();
        }

        private void MI_MeteoStation_Click(object sender, RoutedEventArgs e)
        {
            DG_MeteoStation.Visibility = Visibility.Visible;
            DG_DeviceDanger.Visibility = Visibility.Collapsed;
            DG_Scenario.Visibility = Visibility.Collapsed;
            files.ReadFile("Открытие таблицы Метеостанции", false);
        }
        private void MI_Danger_Click(object sender, RoutedEventArgs e)
        {
            DG_MeteoStation.Visibility = Visibility.Collapsed;
            DG_DeviceDanger.Visibility = Visibility.Visible;
            DG_Scenario.Visibility = Visibility.Collapsed;
            files.ReadFile("Открытие таблицы датчики ЧС", false);
        }
        //Пока пустое 
        private void MI_Apy_Click(object sender, RoutedEventArgs e)
        {
            DG_MeteoStation.Visibility = Visibility.Collapsed;
            DG_DeviceDanger.Visibility = Visibility.Collapsed;
            DG_Scenario.Visibility = Visibility.Visible;
            files.ReadFile("Открытие таблицы Сценарии", false);
        }
        //__________________________________________________________________________________________

        //____________________________ОСНОВНЫЕ_ФУНКЦИИ__________________________________________
       
        //______________________________МЕТЕОСТАНЦИЯ________________________________________________

        //загрузка и заполнение базы данных (Метеостанций)
        private void LoadMeteoStation()
<<<<<<< HEAD
        { 
=======
        {

>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            var t = dbMeteostation.GetDataBaseShort(tableNameMeteo);
            try
            {
                if (t != null)
                {
                    while (t.Read())
                    {
                        meteodeviceList.Add(new StructList(Convert.ToInt32(t.GetInt32(0)), Convert.ToString(t.GetString(1)),
                            Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5)),
                            Convert.ToString(t.GetString(6))));
                        Console.WriteLine("{0} {1} {2} {3} {4} {5}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5));
                    }
                }
                else
                {
                    dbMeteostation.CreateTable(tableNameMeteo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        //Получаем данные из таблицы Метеостанция
        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //   StructList path = DG_MeteoStation.SelectedItem as StructList;
            //MessageBox.Show(" ID: " + path.Id + "\n Исполнитель: " + path.Name + "\n Альбом: " + path.Age
            //    + "\n Год: ");
        }

        //вывод информации о записи Метеостанция
        private void MenuItem_Click_Show(object sender, RoutedEventArgs e)
        {
            files.ReadFile("Вывод информации в таблице Метеостанция", false);
            StructList path = DG_MeteoStation.SelectedItem as StructList;
            try
            {
                if (path != null)
                {
                    MessageBox.Show(" ID: " + path.Id + "\n Ip_device: " + path.IP_device + "\n location: " + path.Location + "\n longitode: " + path.Longitude
                        + "\n lagatitude: " + path.Lagatitude + "\n Description: " + path.Description + "\n null: ");
                   
                }
                files.ReadFile(path.Id + " " + path.IP_device + " " + path.Location + " "
                       + path.Longitude + " " + path.Lagatitude + " " + path.Description, false);
            }
            finally { Console.WriteLine("Не выбрано поле для методы SHOW "); files.ReadFile("Не выбрано поле для методы SHOW", false); }
        }

        //вывод информации о записи Метеостанция
        private void MenuItem_Click_Add(object sender, RoutedEventArgs e)
        {
            files.ReadFile("Добавлена запись в таблицу Метеостанция", false);
<<<<<<< HEAD
            dbMeteostation.InsertDataBase(tableNameMeteo, "'127.0.0.1'", "11000", "'Kaluga'", "54.5293", "36.2754", "'127.0.0.2'");
=======
            dbMeteostation.InsertDataBase(tableNameMeteo, "'127.0.0.1'", "11000", "'Kaluga'", "54.5293", "36.2754", "'127.0.0.2'", "'test'", "'test'", "'test'");
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            var id = dbMeteostation.GetLastID(tableNameMeteo);
            meteodeviceList.Add(new StructList(id, "127.0.0.1", "11000", "Kaluga", "54.5293", "36.2754", "127.0.0.2"));
            DG_MeteoStation.ItemsSource = meteodeviceList.ToList();
            files.ReadFile("127.0.0.1 11000 kaluga 54.5293 36.2754 127.0.0.2", false);
        }

        //удаление записи из таблицы Метеостанция 
        private void MenuItem_Click_Delete(object sender, RoutedEventArgs e)
        {
            files.ReadFile("Удаление записи из таблицы Метеостанция", false);
            StructList path = DG_MeteoStation.SelectedItem as StructList;
            try
            {
                if (path != null)
                {
                    dbMeteostation.DeleteValueDB(tableNameMeteo, path.Id);
                    var t = FindIndexsList(meteodeviceList, path.Id);
                    meteodeviceList.RemoveAt(FindIndexsList(meteodeviceList, path.Id));
                    MessageBox.Show(" ID: " + path.Id + "\n Ip_device: " + path.IP_device + "\n location: " + path.Location + "\n longitode: " + path.Longitude
                         + "\n lagatitude: " + path.Lagatitude + "\n Description: " + path.Description + "\n null: ");
                    DG_MeteoStation.ItemsSource = meteodeviceList.ToList();
                    Shows(meteodeviceList);
                }
                files.ReadFile(path.Id + " " + path.IP_device + " " + path.Location + " "
                          + path.Longitude + " " + path.Lagatitude + " " + path.Description, false);
            }

            finally
            {
                Console.WriteLine("Не выбрано поле для методы Delete ");
                files.ReadFile("Не выбрано поле для методы Delete", false);
            }
        }

        //обновление данных в таблице Метеостанция 
        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            files.ReadFile("Обновление записи в таблице Метеостанция", false);
            StructList p = e.Row.Item as StructList;
            MessageBox.Show($"{p.Id} {p.IP_device} {p.Port} {p.Location} {p.Longitude} {p.Lagatitude} {p.Description}");
            //вызов обновления базы данных 
<<<<<<< HEAD
            //GetWeather(p.IP_device, p.Port);
            dbMeteostation.UpdateElementDataBase(tableNameMeteo, Convert.ToString(p.Id), p.IP_device, Convert.ToString(p.Port), p.Location, p.Longitude, p.Lagatitude, p.Description);
            //dbMeteostation.UpdateElementDataBase(tableNameMeteo, Convert.ToString(p.Id), null, null, null, null, null, null, Convert.ToString(temperature), Convert.ToString(windSpeed), Convert.ToString(directionWind));
=======
            GetWeather(p.IP_device, p.Port);
            dbMeteostation.UpdateElementDataBase(tableNameMeteo, Convert.ToString(p.Id), p.IP_device, Convert.ToString(p.Port), p.Location, p.Longitude, p.Lagatitude, p.Description, Convert.ToString(temperature), Convert.ToString(windSpeed), Convert.ToString(directionWind));
            dbMeteostation.UpdateElementDataBase(tableNameMeteo, Convert.ToString(p.Id), null, null, null, null, null, null, Convert.ToString(temperature), Convert.ToString(windSpeed), Convert.ToString(directionWind));
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
            files.ReadFile(p.Id + " " + p.IP_device + " " + p.Location + " "
                            + p.Longitude + " " + p.Lagatitude + " " + p.Description, false);
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
        //__________________________________________________________________________________________

        //______________________________Датчики_ЧС________________________________________________
        //Загрузка таблицы с датчиков чс
        private void LoadDangerDevice()
        {
            var t = dbAlarm.GetDataBase(tableNameDanger);
            try
            {
                if (t != null)
                {
                    while (t.Read())
                    {
                        alarmDeviceList.Add(new StructList(Convert.ToInt32(t.GetInt32(0)), Convert.ToString(t.GetString(1)),
                            Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4))));
                        Console.WriteLine("{0} {1} {2} {3} {4}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4));
                    }
                }
                else
                {
                    dbAlarm.CreateTableAlarm(tableNameDanger);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }
        //Вывод информации из строки таблицы Alarm
        private void MenuItem_Click_Show_AlarmDevice(object sender, RoutedEventArgs e)
        {
            files.ReadFile("Просмотр записи в таблице Датчики ЧС", false);
            StructList path = DG_DeviceDanger.SelectedItem as StructList;
            files.ReadFile(path.Id + " " + path.IP_device + " " + path.Script + " " + path.Path + " " + path.Notify, false);
            try
            {
                if (path != null)
                {
                    MessageBox.Show(" ID: " + path.Id + "\n Ip_device: " + path.IP_device + "\n Script: " + path.Script
                        + "\n Path: " + path.Path + "\n Notify: " + path.Notify);

                  
                }
               
            }
            finally { Console.WriteLine("Не выбрано поле для методы SHOW ");
                files.ReadFile("Не выбрано поле для методы SHOW", false);
            }
        }
        //добавление записи в таблицу Alarm
        private void MenuItem_Click_Add_AlarmDevice(object sender, RoutedEventArgs e)
        {
            files.ReadFile("Добавление записи в таблицу Датчики ЧС", false);
            dbAlarm.InsertDataBase(tableNameDanger, "'192.168.1.1'", "0", "'Path'", "'127.0.0.1;127.0.0.1'");
            var id = dbAlarm.GetLastID(tableNameDanger);
            alarmDeviceList.Add(new StructList(id, "192.168.1.1", "0", "Path", "127.0.0.1;127.0.0.1"));
            DG_DeviceDanger.ItemsSource = alarmDeviceList.ToList();
            files.ReadFile("192.168.1.1 0 Path 127.0.0.1;127.0.0.1", false);
        }
        //удаление записи из таблицы Alarm
        private void MenuItem_Click_Delete_AlarmDevice(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Удаление устройства");
            StructList path = DG_DeviceDanger.SelectedItem as StructList;
            try
            {
                files.ReadFile("удаление записи из таблицы Датчики ЧС", false);
                if (path != null)
                {
                    dbAlarm.DeleteValueDB(tableNameDanger, path.Id);
                    var t = FindIndexsList(alarmDeviceList, path.Id);
                    alarmDeviceList.RemoveAt(FindIndexsList(alarmDeviceList, path.Id));
                    MessageBox.Show(" ID: " + path.Id + "\n Ip_device: " + path.IP_device + "\n Script: " + path.Script
                        + "\n Path: " + path.Path + "\n Notify: " + path.Notify);
                    DG_DeviceDanger.ItemsSource = alarmDeviceList.ToList();
                    Console.WriteLine("--------------------------------------------------");
                    Shows(alarmDeviceList);
                    
                }
                files.ReadFile(path.Id + " " + path.IP_device + " " + path.Script + " " + path.Path + " " + path.Notify, false);

            }
            finally
            {
                Console.WriteLine("Не выбрано поле для методы Delete ");
                files.ReadFile("Не выбрано поле для методы Delete", false);
            }
        }
        //обновление записи в таблице Alarm
        private void dataGridAlarm_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            files.ReadFile("Обновление записи в таблице Датчики ЧС", false);
            StructList p = e.Row.Item as StructList;
            MessageBox.Show($"{p.Id} {p.IP_device} {p.Script} {p.Path} {p.Notify}");
            files.ReadFile(p.Id + " " + p.IP_device + " " + p.Script + " " + p.Path + " " + p.Notify, false);
            //вызов обновления базы данных 
            dbAlarm.UpdateElementDataBase(tableNameDanger, Convert.ToString(p.Id), p.IP_device, Convert.ToString(p.Script), p.Path, p.Notify);
         
        }

        //__________________________________________________________________________________________

        //_____________________________СЦЕНАРИИ_____________________________________________________

        //Загрузка таблицы с сценарии
        private void LoadScenario() {
            var t = dbScenario.GetDataBase(tableNameScenario);
            try
            {
                if (t != null)
                {
                    while (t.Read())
                    {
                        scenarioList.Add(new StructList(Convert.ToInt32(t.GetInt32(0)), Convert.ToString(t.GetString(1)),
                           Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3))));
                        Console.WriteLine("{0} {1} {2} {3}", t.GetInt32(0), t.GetString(1), t.GetString(2) , t.GetString(3));
                    }
                }
                else
                {
                    dbScenario.CreateTableAdditionalDB(tableNameScenario);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }
        //Вывод информации из строки таблицы Сценарии
        private void MenuItem_Click_Show_Scenario(object sender, RoutedEventArgs e)
        {
            files.ReadFile("Вывод информации по таблице Сценарии", false);

            StructList path = DG_Scenario.SelectedItem as StructList;
            try
            {
                if (path != null)
                {
                    MessageBox.Show(" ID: " + path.Id + "\n Ip_device: " + path.IP_device + "\n Порт: " + path.Port);
                   
                }
                files.ReadFile(path.Id + " " + path.IP_device + " " + path.Port, false);
            }

            finally { Console.WriteLine("Не выбрано поле для методы SHOW ");
                files.ReadFile("Не выбрано поле для методы SHOW", false);
            }
        }

        //добавление записи в таблицу Сценарии
        private void MenuItem_Click_Add_Scenario(object sender, RoutedEventArgs e)
        {
            files.ReadFile("Добавление информации в таблицу Сценарии", false);
            dbScenario.InsertDataBase(tableNameScenario, "'127.0.0.1'", "11000","1");
            var id = dbScenario.GetLastID(tableNameScenario);
            scenarioList.Add(new StructList(id, "127.0.0.1", "11000" , "1"));
            DG_Scenario.ItemsSource = scenarioList.ToList();
            files.ReadFile("127.0.0.1 11000 1", false);
        }
        //удаление записи из таблицы Сценарии
        private void MenuItem_Click_Delete_Scenario(object sender, RoutedEventArgs e) {
            StructList path = DG_Scenario.SelectedItem as StructList;
            files.ReadFile("Удалние информации из таблицы Сценарии", false);
            files.ReadFile(path.Id + " " + path.IP_device + " " + path.Port + " " + path.Script, false);
            try
            {
                if (path != null)
                {
                    dbScenario.DeleteValueDB(tableNameScenario, path.Id);
                    var t = FindIndexsList(scenarioList, path.Id);
                    scenarioList.RemoveAt(FindIndexsList(scenarioList, path.Id));
                    MessageBox.Show(" ID: " + path.Id + "\n Ip_device: " + path.IP_device + "\n Порт: " + path.Port);
                    DG_Scenario.ItemsSource = scenarioList.ToList();
                  
                    Shows(scenarioList);
                }
            }
            finally
            {
                files.ReadFile("Не выбрано поле для метода Delete", false);
                Console.WriteLine("Не выбрано поле для методы Delete ");
            }
        }
        //обновление записи в таблице Сценарии 
        private void DG_Scenario_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            files.ReadFile("Обновление записи в таблице Сценарии", false);
            StructList p = e.Row.Item as StructList;
            MessageBox.Show($"{p.Id} {p.IP_device} {p.Port} {p.Script} ");
            files.ReadFile(p.Id + " " + p.IP_device + " " + p.Port + " " + p.Script, false);
            //вызов обновления базы данных 
            dbScenario.UpdateElementDataBase(tableNameScenario, Convert.ToString(p.Id), p.IP_device,p.Port ,Convert.ToString(p.Script));

        }
        //__________________________________________________________________________________________


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

      
        //____________________________ОСНОВНЫЕ_МЕТОДЫ_ОКНА__________________________________________

        private void DG_device_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void DG_DeviceDanger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        
        }
        private void DG_Scenario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DG_MeteoStation_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void DG_DeviceDanger_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void bt_send_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                string ip = scenarioList[DG_Scenario.SelectedIndex].IP_device;
                string port = scenarioList[DG_Scenario.SelectedIndex].Port;
                string type = scenarioList[DG_Scenario.SelectedIndex].Script;
                files.ReadFile("Запуск сценария на " + ip+ " " + port, false);
                //MessageBox.Show(deviceList[dataGrid.SelectedIndex].Id + deviceList[dataGrid.SelectedIndex].Port);
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(IPAddress.Parse(ip), Convert.ToInt32(port));
                NetworkStream tcpStream = tcpClient.GetStream();
                //КИНУТЬ СЦЕНАРИЙ
<<<<<<< HEAD
   
                tcpStream.Write(IndividualScenario(type), 0, IndividualScenario(type).Length);
                byte[] bytes = new byte[tcpClient.ReceiveBufferSize];
                string returnData = Encoding.UTF8.GetString(bytes);
=======
                byte[] sendBytes = Encoding.UTF8.GetBytes(IndividualScenario(type));
                tcpStream.Write(sendBytes, 0, sendBytes.Length);
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
                tcpClient.Close();
                tcpStream.Close();


            }
            catch (Exception ex)
            {
                files.ReadFile("ошибка запуска сценария", false);
                MessageBox.Show(ex.Message.ToString());
            }
        }



        //__________________________________________________________________________________________
<<<<<<< HEAD
        byte[] sendMassive;
        private byte[] IndividualScenario(string _typeScenario)
=======

        private string IndividualScenario(string _typeScenario)
>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180
        {
            bool _type = false;
            string soundCheck = "internal";
            if (_type == true) { soundCheck = "external"; }
            var command = new StringBuilder();
            command.AppendFormat("<?xml version=\"1.0\" encoding=\"utf-8\" ?><command>\n<action>start</action>\n" +
               "<parameters>\n<scenario>{0}</scenario>\n<audio>{1}<audio/>\n</parameters>\n</command>", _typeScenario, soundCheck);
<<<<<<< HEAD

            Console.WriteLine(command);
          
            return SendReceive(Convert.ToString(command));

        }
        private byte[] SendReceive(string _command)
        {
            // перезапись 1-го массива установа первых 4-х байт
            byte[] massive1 = Encoding.Default.GetBytes(_command);
            byte[] massive2 = new byte[massive1.Length + 4];
            Int32 datasize = massive1.Length;

            Console.WriteLine(massive1.Length);
            for (int i = 3; i >= 0; i--)
            {
                massive2[i] = (byte)(datasize % 256);
                datasize = datasize / 256;
            }
            Console.WriteLine("byte Array: " + BitConverter.ToString(massive2));
            for (int j = 4; j < massive2.Length; j++)
            {
                massive2[j] = massive1[j - 4];
            }
            Console.WriteLine("byte Array: " + BitConverter.ToString(massive2));
            sendMassive = massive2;
            return sendMassive;
            
           
        }
=======
            return Convert.ToString(command);
        }

>>>>>>> 19377ed4ed662b43bcd00eaaaae1aa67e1138180


    }
}
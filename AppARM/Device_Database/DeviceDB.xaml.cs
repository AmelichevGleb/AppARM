using AppARM.PostgresSQL;
using AppARM.Structure;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Mono.Security.X509.X520;
using AppARM.Device_Database;
using AppARM.Test;
using AppARM.Structure;
using MessageBox = System.Windows.MessageBox;
using System.Reflection.Emit;
using static AppARM.Structure.ElementDataBase;

namespace AppARM.Device_Database
{
    /// <summary>
    /// Логика взаимодействия для DeviceDB.xaml
    /// </summary>

    //https://www.csharp-console-examples.com/wpf/wpf-entity-framework-select-insert-update-delete/


    public partial class DeviceDB : Window
    {
        

        List<StructList> deviceList = new List<StructList>(1); // список всех устройств из базы данных
        WorkElementDB wDB = new WorkElementDB();
        List<ElementDataBase> element = new List<ElementDataBase>();

        private DataBase db;
        private string tableName = "Arm"; // Название таблицы для PostgreSQL

        public DeviceDB(DataBase _dataBase)
        {
            db = _dataBase; 
            InitializeComponent();
            Load();
            DG_device.ItemsSource = deviceList;
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
        //__________________________________________________________________________________________

        //____________________________ОСНОВНЫЕ_ФУНКЦИИ__________________________________________

        //загрузка и заполнение базы данных
        private void Load()
        {
            var t = db.GetDataBase(tableName);
            if (t != null)
            {
                while (t.Read())
                {
                    deviceList.Add(new StructList(Convert.ToInt32(t.GetInt32(0)), Convert.ToString(t.GetString(1)),
                        Convert.ToString(t.GetString(2)), Convert.ToString(t.GetString(3)), Convert.ToString(t.GetString(4)), Convert.ToString(t.GetString(5))));
                    Console.WriteLine("{0} {1} {2} {3} {4} {5}", t.GetInt32(0), t.GetString(1), t.GetString(2), t.GetString(3), t.GetString(4), t.GetString(5));
                }
            }
            else
            {
                
                db.CreateTable(tableName);
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
            MessageBox.Show($"{p.Id} {p.IP_device} {p.Location} {p.Longitude} {p.Lagatitude} {p.Description}");
            //вызов обновления базы данных 
            db.UpdateElementDataBase(tableName, Convert.ToString(p.Id), p.IP_device, p.Location, p.Longitude,p.Lagatitude,p.Description);
           /* Console.WriteLine(Convert.ToString(p.Id)," ", Convert.ToString(p.Name), p.Age);
            if (flagfix)
            {
                int numRow = e.Row.GetIndex();
                int t1 = e.Column.DisplayIndex;
                Console.WriteLine("column {0}", t1);
                Console.WriteLine("Row {0}", numRow);
                flagfix = false;
                DG_device.CancelEdit();
                DG_device.CancelEdit();
                flagfix = true;
                DG_device.Items.Refresh();
            }
           */
        }

        //Получаем данные из таблицы
        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            StructList path = DG_device.SelectedItem as StructList;
            //MessageBox.Show(" ID: " + path.Id + "\n Исполнитель: " + path.Name + "\n Альбом: " + path.Age
            //    + "\n Год: ");
        }


        //вывод информации о записи
        private void MenuItem_Click_Show(object sender, RoutedEventArgs e)
        {
            StructList path = DG_device.SelectedItem as StructList;
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
        private void MenuItem_Click_Add(object sender,RoutedEventArgs e) 
        {
            db.InsertDataBase(tableName, "'127.0.0.1'", "'Kaluga'", "54.5293", "36.2754", "'Inform'");
            var id = db.GetLastID(tableName);
            deviceList.Add(new StructList(id, "127.0.0.1", "Kaluga", "54.5293", "36.2754", "Inform"));
            DG_device.ItemsSource = deviceList.ToList();
        }
          
        //удаление записи из таблицы
        private void MenuItem_Click_Delete(object sender, RoutedEventArgs e)
        {
            StructList path = DG_device.SelectedItem as StructList;
            try
            {
                if (path != null)
                {
                    db.DeleteValueDB(tableName, path.Id);
                    var t = FindIndexsList(deviceList, path.Id);
                    deviceList.RemoveAt(FindIndexsList(deviceList, path.Id));
                    MessageBox.Show(" ID: " + path.Id + "\n Ip_device: " + path.IP_device + "\n location: " + path.Location + "\n longitode: " + path.Longitude
                         + "\n lagatitude: " + path.Lagatitude + "\n Description: " + path.Description + "\n null: ");
                    DG_device.ItemsSource = deviceList.ToList();
                    Console.WriteLine("--------------------------------------------------");
                    Shows(deviceList);
                }
            }
            finally
            {
                Console.WriteLine("Не выбрано поле для методы Delete ");
            }
        }
        //__________________________________________________________________________________________
    }
}

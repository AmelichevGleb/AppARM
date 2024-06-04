using AppARM.Structure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


using AppARM.PostgresSQL;
using AppARM.Weather;

namespace AppARM.Device_Database
{
    /// <summary>
    /// Логика взаимодействия для TestTable.xaml
    /// </summary>
    public partial class TestTable : Window
    {




        AdditionalDb additional = new AdditionalDb("127.0.0.1","5432","postgres","111111");
        string testname = "addFunc";
        List<StructList> deviceList = new List<StructList>(1); // список всех устройств из базы данных


        List<ElementAdditionalDataBase> addList = new List<ElementAdditionalDataBase>(1); // список всех устройств из базы данных
        private void LoadAdditionalDB()
        {

            var t = additional.GetDataBase(testname);
            try
            {
                if (t != null)
                {
                    while (t.Read())
                    {
                      //  deviceList.Add(new StructList(Convert.ToInt32(t.GetInt32(0)), Convert.ToString(t.GetString(1)),
                     //      Convert.ToString(t.GetString(2))));
                        Console.WriteLine("{0} {1} {2} ", t.GetInt32(0), t.GetString(1), t.GetString(2));
                    }
                }
                else
                {
                    additional.CreateTableAdditionalDB(testname);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }



        public TestTable()
        {
            // 127.0.0.1
            // 5432
            // postgres
            // 111111

           
            InitializeComponent();
            LoadAdditionalDB();


            dataGrid.ItemsSource = deviceList;
        }
        private void bt_send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show(deviceList[dataGrid.SelectedIndex].Id  + deviceList[dataGrid.SelectedIndex].Port);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        //вывод информации о записи
        private void MenuItem_Click_Show(object sender, RoutedEventArgs e)
        {        
           
                StructList path = dataGrid.SelectedItem as StructList;
                try
                {
                    if (path != null)
                    {
                        MessageBox.Show(" ID: " + path.Id + "\n Ip_device: " + path.IP_device + "\n Порт: " + path.Port);
                    }
                }
                finally { Console.WriteLine("Не выбрано поле для методы SHOW "); }
        }

        //обновление данных в таблице
        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            
            StructList p = e.Row.Item as StructList;
            MessageBox.Show($"{p.Id} {p.IP_device} {p.Port}");
            //вызов обновления базы данных 

            //additional.UpdateElementDataBase(testname, Convert.ToString(p.Id), p.IP_device, Convert.ToString(p.Port));
        }
        //вывод информации о записи
        private void MenuItem_Click_Add(object sender, RoutedEventArgs e)
        {
            additional.InsertDataBase(testname, "'127.0.0.1'", "11000", "1");
            var id = additional.GetLastID(testname);
           // deviceList.Add(new StructList(id, "127.0.0.1", "11000"));
            dataGrid.ItemsSource = deviceList.ToList();
        }

        //удаление записи из таблицы
        private void MenuItem_Click_Delete(object sender, RoutedEventArgs e)
        {

            StructList path = dataGrid.SelectedItem as StructList;
            try
            {
                if (path != null)
                {
                    additional.DeleteValueDB(testname, path.Id);
                    var t = FindIndexsList(deviceList, path.Id);
                    deviceList.RemoveAt(FindIndexsList(deviceList, path.Id));
                    MessageBox.Show(" ID: " + path.Id + "\n Ip_device: " + path.IP_device + "\n location: " + path.Location + "\n longitode: " + path.Longitude
                         + "\n lagatitude: " + path.Lagatitude + "\n Description: " + path.Description + "\n null: ");
                    dataGrid.ItemsSource = deviceList.ToList();
                    Console.WriteLine("--------------------------------------------------");
                    Shows(deviceList);
                }
            }
            finally
            {
                Console.WriteLine("Не выбрано поле для методы Delete ");
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
                Console.WriteLine(Convert.ToString(device.Id), "  ", device.IP_device, "  ", device.Port);
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

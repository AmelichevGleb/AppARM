using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

using AppARM.FilesLogs;
using AppARM.ServerListren;

namespace AppARM
{
    /// <summary>
    /// Логика взаимодействия для UserControlSetting.xaml
    /// </summary>
    public partial class UserControlSetting : UserControl
    {
        public ServerForListen _server;
        private Thread thread;
        private Files files = new Files();

        private string password;
        public UserControlSetting(Thread _thread)
        {
            InitializeComponent();
            thread = _thread;
            TB_Server.Text = Convert.ToString(Properties.Settings.Default.IP_adress);
            TB_Port.Text = Convert.ToString(Properties.Settings.Default.Port);
            TB_Login.Text = Convert.ToString(Properties.Settings.Default.Login_BD);
            TB_PasswordMask.Text = Convert.ToString(Properties.Settings.Default.Password_BD);
            TB_IP_for_device.Text = Convert.ToString(Properties.Settings.Default.IP_ServerListen);
            TB_Port_for_device.Text = Convert.ToString(Properties.Settings.Default.Port_ServerListen);
            TB_IP_Server_Device.Text = Convert.ToString(Properties.Settings.Default.Ip_Server_Device);
            TB_Port_Server_Device.Text = Convert.ToString(Properties.Settings.Default.Port_Server_Device);
           
            TB_IP_server_maps.Text = Convert.ToString(Properties.Settings.Default.IP_Server_maps);
            TB_Port_server_maps.Text = Convert.ToString(Properties.Settings.Default.Port_Server_maps);
            Properties.Settings.Default.Save();
            //_server = startListen;
           
        }

        //____________________________ОСНОВНЫЕ_КНОПКИ__________________________________________

        //Кнопка OK сохраняет конфигурации и закрывает приложение
        private void BC_Ok(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Port = Convert.ToInt32(TB_Port.Text);
            Properties.Settings.Default.IP_adress = Convert.ToString(TB_Server.Text);
            Properties.Settings.Default.Login_BD = Convert.ToString(TB_Login.Text);
            Properties.Settings.Default.Password_BD = Convert.ToString(TB_PasswordMask.Text);
            Properties.Settings.Default.IP_ServerListen = Convert.ToString(TB_IP_for_device.Text);
            Properties.Settings.Default.Port_ServerListen = Convert.ToString(TB_Port_for_device.Text);
            Properties.Settings.Default.IP_Server_maps = Convert.ToString(TB_IP_server_maps.Text);
            Properties.Settings.Default.Port_Server_maps = Convert.ToString(TB_Port_server_maps.Text);
            Properties.Settings.Default.Ip_Server_Device = Convert.ToString(TB_IP_Server_Device.Text);
            Properties.Settings.Default.Port_Server_Device = Convert.ToString(TB_Port_Server_Device.Text);
            Properties.Settings.Default.Save();
            files.ReadFile("Внесение изменений в настройки", false);
            MessageBox.Show("Внесение изменений в настройки\nПри необходимости перезапустите сервер");
            //this.Close();
        }
        //Кнопка Cancel закрывает окно
        private void BC_Cancel(object sender, RoutedEventArgs e)
        {

        }

        //CheckBOX показывает пароль или скрывает
        private void CB_ShowPassword_Checked(object sender, RoutedEventArgs e)
        {

            if ((CB_ShowPassword.IsChecked != true))
            {
                TB_PasswordMask.Visibility = Visibility.Hidden;
                PB_Password.Visibility = Visibility.Visible;
                TB_PasswordMask.Text = password;
            }
            else
            {
                PB_Password.Visibility = Visibility.Hidden;
                TB_PasswordMask.Visibility = Visibility.Visible;
                PB_Password.Password = password;
            }
        }

        //____________________________TEXTBOX__________________________________________

        //Изменение пароля в маске
        private void TB_PasswordMask_TextChanged(object sender, EventArgs e)
        {
            password = TB_PasswordMask.Text.ToString();
            PB_Password.Password = password;
        }

        //Изменение пароля без маски
        private void PB_Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password = PB_Password.Password.ToString();
            TB_PasswordMask.Text = password;
        }
    }
}

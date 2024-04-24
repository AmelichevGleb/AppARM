using System;
using System.Windows;

namespace AppARM.SettingApp
{
    /// <summary>
    /// Логика взаимодействия для SettingForm.xaml
    /// </summary>
    public partial class SettingForm : Window
    {
        private string password; 
        
        public SettingForm()
        {
            InitializeComponent();
            TB_Server.Text = Convert.ToString(Properties.Settings.Default.IP_adress);
            TB_Port.Text = Convert.ToString(Properties.Settings.Default.Port);
            TB_Login.Text = Convert.ToString(Properties.Settings.Default.Login_BD);
            TB_PasswordMask.Text = Convert.ToString(Properties.Settings.Default.Password_BD);
            TB_IP_for_device.Text = Convert.ToString(Properties.Settings.Default.IP_ServerListen);
            TB_Port_for_device.Text = Convert.ToString(Properties.Settings.Default.Port_ServerListen);
            TB_IP_server_maps.Text = Convert.ToString(Properties.Settings.Default.IP_Server_maps);
            TB_Port_server_maps.Text = Convert.ToString(Properties.Settings.Default.Port_Server_maps);
            Properties.Settings.Default.Save();
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
            Properties.Settings.Default.Save();
            this.Close();
        }
        //Кнопка Cancel закрывает окно
        private void BC_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
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

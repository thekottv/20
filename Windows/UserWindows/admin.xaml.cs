using System;
using _20.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Drawing;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Threading;

namespace _20.Windows.UserWindows
{
    /// <summary>
    /// Логика взаимодействия для laborant.xaml
    /// </summary>
    public partial class admin : Window
    {
        public static class CurrentUser
        {
            public static string UserName = LoginWindow.Globals.UserName;
            public static string UserRole = LoginWindow.Globals.userinfo.role.role1;
            public static BitmapImage UserAva = ImageConverter.ByteArrayToImage(LoginWindow.Globals.userinfo.avatar);

            public static int UserID = LoginWindow.Globals.userinfo.ID;
            public static string UserLastseen = LoginWindow.Globals.userinfo.lastseen.ToString();
            public static string UserIP = LoginWindow.Globals.userinfo.ip;
        }


        public admin()
        {
            InitializeComponent();
            Thread.Sleep(300);
            DGridUsers.ItemsSource = lab20Entities.GetContext().user.ToList(); // поля таблицы выебываются
            TB_Name.Text = CurrentUser.UserName;
            TB_Role.Text = CurrentUser.UserRole;
            IMG_Ava.ImageSource = CurrentUser.UserAva;
        }

        private void BTN_Logout_Click(object sender, RoutedEventArgs e) => Logout();

        private void Logout()
        {
            LoginWindow.Globals.userinfo.lastseen = DateTime.Now;
            LoginWindow.Globals.userinfo.ip = ImageConverter.CurrentIp();
            lab20Entities.GetContext().user.AddOrUpdate(LoginWindow.Globals.userinfo);
            lab20Entities.GetContext().SaveChangesAsync();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e) //remove
        {
            MessageBox.Show("Done! You are awesome✨");
        }

        private void DGridUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

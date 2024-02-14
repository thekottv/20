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
using System.Net;

namespace _20.Windows.UserWindows
{
    /// <summary>
    /// Логика взаимодействия для laborant.xaml
    /// </summary>
    public partial class laborant : Window
    {
        public static class CurrentUser
        {
            public static string UserName = LoginWindow.Globals.UserName;
            public static string UserRole = LoginWindow.Globals.userinfo.role.role1;
            public static BitmapImage UserAva = ImageConverter.ByteArrayToImage(LoginWindow.Globals.userinfo.avatar);
        }


        public laborant()
        {
            InitializeComponent();
            TB_Name.Text = CurrentUser.UserName;
            TB_Role.Text = CurrentUser.UserRole;
            IMG_Ava.ImageSource = CurrentUser.UserAva;

            InitializeScripts();
            Thread.Sleep(300);
            DGridProbirki.ItemsSource = lab20Entities.GetContext().order.ToList();
        }
        private void More_Click(object sender, RoutedEventArgs e)
        {
            var CurrentOrder = DGridProbirki.SelectedItem as order;
            NewOrder infoWindow = new NewOrder((CurrentOrder) as order);
            infoWindow.Show();    //.ShowDialog(); после реализации редактирования в окне

            DGridProbirki.Items.Refresh();
        }

        CancellationTokenSource cts = new CancellationTokenSource();
        private async void InitializeScripts() // эта хуйня работает коряво, и окно подвисает каждый Dispatcher.Invoke
        {

            try
            {
                await Task.Run(() => Timer(), cts.Token);
            }
            catch (OperationCanceledException) {; } //ловим специально выплевываемую для завершения потока ошибку (А она, походу, не всплывает, и эту конструкцию бы нахуй удалить)
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
            cts.Dispose(); // должен завершать поток, но по каким-то причинам не работает
        }

        private void Timer()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //await Task.Run(() => Warning(sw));
            while (sw.Elapsed.TotalSeconds < 360)
            {
                Dispatcher.Invoke(() => TB_SessionTime.Text = $"Время сессии: {sw.Elapsed.Hours.ToString("00")}:{sw.Elapsed.Minutes.ToString("00")}:{sw.Elapsed.Seconds.ToString("00")}");
                Thread.Sleep(1000);
                Warning(sw);
            }
            MessageBox.Show("Время сессии истекло.");
            Dispatcher.Invoke(() => Logout());
        }

        private void Warning(Stopwatch sw)
        {
            //while (sw.Elapsed.TotalSeconds < 20)
            //    ;
            //MessageBox.Show("Время Вашей сессии подходит к концу.");
            if (sw.Elapsed.TotalSeconds >= 298 & sw.Elapsed.TotalSeconds < 301)
            {
                MessageBox.Show("Время Вашей сессии подходит к концу.");
                Dispatcher.Invoke(() => TB_SessionWarning.Visibility = Visibility.Visible);
            }
        }

        private void BTN_NewProbirka_click(object sender, RoutedEventArgs e)
        {
            NewOrder newOrder = new NewOrder(null);
            newOrder.ShowDialog();
            DGridProbirki.ItemsSource = lab20Entities.GetContext().order.ToList();
            DGridProbirki.Items.Refresh();
        }

        private void DGridProbirki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

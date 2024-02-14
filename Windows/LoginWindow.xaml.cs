using _20.Model;
using _20.Windows;
using _20.Windows.UserWindows;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
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
using System.Windows.Shapes;

namespace _20
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public int timervalue = 0;
        int counter = 0;
        public LoginWindow()
        {
            InitializeComponent();
            LoginCaptcha.Visibility = Visibility.Hidden;
            TB_login.Focus();
        }

        public static class Globals //Глобальные переменные для разграничения 
        {
            public static int UserRole;
            public static string UserName;
            public static user userinfo { get; set; }
        }

        private async void Login_button(object sender, RoutedEventArgs e)
        {
            var CurrentUser = AppData.db.user.FirstOrDefault(u => u.login == TB_login.Text && u.password == PB_password.Password); //Разграничение с проверкой логина и пароля
            if (CurrentUser != null)
            {
                Globals.UserRole = CurrentUser.role.ID;
                Globals.userinfo = CurrentUser;
                Globals.UserName = CurrentUser.name;
                TB_login.IsEnabled = false;
                PB_password.IsEnabled = false;

                if (LoginAvaliability() == true)
                {
                    await Task.Run(() => UpdateUserLastLogin(CurrentUser));
                    InterfaceInit(CurrentUser);
                }
                else
                {
                    MessageBox.Show("Вход в аккаунт временно невозможен. Повторите попытку позже");
                    InputUnblock();
                }
            }
            else
            {
                counter++;
                MessageBox.Show("Неправильное имя пользователя или пароль.");
                await CaptchaApproving();
                PB_password.Password = string.Empty;

                if (counter > 1)
                await Task.Run(() => Timer());
            }
        }

        private bool LoginAvaliability()
        {
            DateTime lastenter = (DateTime)Globals.userinfo.lastseen;
            long diff = DateTime.Now.Ticks - lastenter.Ticks; //разница в тиках. например, 200000000 тиков это 20 секунд лмфао
            bool rolex = (Globals.userinfo.role.ID != 1) && (Globals.userinfo.role.ID != 2);

            if (diff >= 200000000 || rolex) // ролекс нужен для проверки прав, ибо бух и админ не имеют рестрикшна на вход 
                return true;
            else
                return false;
        }

        private void InterfaceInit(user CurrentUser)
        {
            switch (CurrentUser.role.ID)
            {
                case 1: // Лаборант
                    laborant laborant = new laborant();
                    laborant.Show();
                    this.Close();
                    break;
                case 2: // Лаборант-исслеователь
                    laborant_explorer laborant_Explorer = new laborant_explorer();
                    laborant_Explorer.Show();
                    this.Close();
                    break;
                case 3: // Бухгалтер
                    buh buh = new buh();
                    buh.Show();
                    this.Close();
                    break;
                case 4: // Админ
                    admin admin = new admin();
                    admin.Show();
                    this.Close();
                    break;

                default:
                    MessageBox.Show("Временно отсутствует функционал для Вашей роли. Обратитесь к администратору системы.");
                    break;
            }
        }

        private void UpdateUserLastLogin(user CurrentUser)
        {
            CurrentUser.lastseen = DateTime.Now;
            CurrentUser.ip = ImageConverter.CurrentIp();

            //ava updater after login
            //System.Drawing.Image img = System.Drawing.Image.FromFile("D:/Шарага/Палыч/Палыч новое на удаленку/!done shit/20/assets/imgs/Бухгалтер.jpeg");
            //CurrentUser.avatar = ImageConverter.ImageToByteArray(img);
            lab20Entities.GetContext().user.AddOrUpdate(CurrentUser);
            lab20Entities.GetContext().SaveChangesAsync();
        }

        private void Timer()
        {
            Dispatcher.Invoke(() => InputBlock());

            timervalue = 10;
            while (timervalue > 0)
            {
                timervalue--;
                Dispatcher.Invoke(() => TB_RetryTime.Text = $"Повторите попытку через {timervalue} секунд."); // использование Dispatcher.Invoke необходимо для того, чтобы функция из другого потока могла манипулировать с элементом из первого.
                Thread.Sleep(1000);
            }
            Dispatcher.Invoke(() => TB_RetryTime.Text = String.Empty);
            Dispatcher.Invoke(() => InputUnblock());
        }

        private void InputBlock()
        {
            TB_login.IsEnabled = false;
            PB_password.IsEnabled = false;
            BTN_login.IsEnabled = false;
        }

        private void InputUnblock()
        {
            TB_login.IsEnabled = true;
            PB_password.IsEnabled = true;
            BTN_login.IsEnabled = true;
        }

        private void CaptchaTest()
        {
            InputBlock();

            LoginCaptcha.CreateCaptcha();
            LoginCaptcha.Visibility = Visibility.Visible;
        }

        private void CaptchaApproveWaiting() //Ожидание изменения значения переменной (потому что я тупой)
        {
            do
            {
                ;
            }
            while (LoginCaptcha.CaptchaApproved == false);
        }

        async Task CaptchaApproving()
        {
            CaptchaTest();

            await Task.Run(() => CaptchaApproveWaiting());


            LoginCaptcha.Visibility = Visibility.Hidden;
            
            InputUnblock();
        }

        private void TB_login_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TB_login.Text == "")
            {
                LB_login.Visibility = Visibility.Visible;
            }
            else
            {
                LB_login.Visibility = Visibility.Hidden;
            }
        }

        private void PB_password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            TB_password_unmask.Text = PB_password.Password;

            if (PB_password.Password == "")
            {
                LB_password.Visibility = Visibility.Visible;
                BTN_pword_show.Visibility = Visibility.Hidden;
            }
            else
            {
                LB_password.Visibility = Visibility.Hidden;
                BTN_pword_show.Visibility = Visibility.Visible;
            }
        }

        private void pword_show_Click(object sender, RoutedEventArgs e)
        {
            BTN_pword_show.Visibility = Visibility.Hidden;
            TB_password_unmask.Visibility = Visibility.Visible;
            BTN_pword_hide.Visibility = Visibility.Visible;
        }

        private void pword_hide_Click(object sender, RoutedEventArgs e) => pword_hide();

        private void pword_hide_MouseLeave(object sender, MouseEventArgs e) => pword_hide();

        private void pword_hide()
        {
            BTN_pword_hide.Visibility = Visibility.Hidden;
            BTN_pword_show.Visibility = Visibility.Visible;
            TB_password_unmask.Visibility = Visibility.Hidden;
        }
    }
}

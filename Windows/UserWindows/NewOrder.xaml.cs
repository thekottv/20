using _20.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BarcodeStandard;
using System.Drawing;
using System.Runtime.InteropServices;
using SkiaSharp;
using System.Drawing.Printing;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft;
using Newtonsoft.Json.Linq;

namespace _20.Windows.UserWindows
{
    /// <summary>
    /// Логика взаимодействия для NewOrder.xaml
    /// </summary>
    public partial class NewOrder : Window
    {
        public order _currentorder = new order();
        private Parser parser = new Parser();
        public NewOrder(order selectedOrder)
        {
            InitializeComponent();
            CBX_Status.ItemsSource = AppData.db.order_status.ToList();
            LB_Services.ItemsSource = AppData.db.service.ToList();

            Dictionary<int, int> currentServices = parser.GetServicesDic_FromDB();

            if (selectedOrder != null)
            {
                Globals.orderServices = parser.GetServicesList_FromOrder(selectedOrder.service_ids);
                _currentorder = selectedOrder;
                CBX_Status.SelectedIndex = _currentorder.status_id - 1;
                DTP_Date.Value = _currentorder.dateOfOrder;
            }
            else
            {
                _currentorder.dateOfOrder = DateTime.Now;
                CBX_Status.SelectedIndex = 0; // "В ожидании" по дефолту
            }

            LB_BAR_Tip.Content = ReturnActualBarcode();
            TBX_BAR.Focus();
            DataContext = _currentorder;
        }

        public static class Globals // Необходим для корректной работы чекбоксов.
        {
            public static List<int> orderServices;
        }
        //ТЕКУЩИЙ СЕРВИС - получение
        //Получить string значение services_ids из текущего order, если он передан
        //Распарсить его в список?
        //Получить список сервисов, соотнести id сервиса с code сервиса и сгенерировать словарь [code:id]
        //Соотнести код из списка с айдишнмком сервиса
        //использовать айдишники сервисов для расставления галочек используемых сервисов

        //НОВЫЙ СЕРВИС
        //Получить список сервисов, соотнести id сервиса с code сервиса и сгенерировать словарь [code:id]
        //использовать айдишники сервисов для расставления галочек используемых сервисов

        //отправка
        //Использовать расставленные галочки на используемых сервисах для определения необходимых сервисов
        //Используя полученнй ранее словарь забилдить строку services_ids для текущего order

        private void BTN_GenerateBar_Click(object sender, RoutedEventArgs e)
        {
            TBX_BAR.Text = ReturnActualBarcode();
        }

        private string ReturnActualBarcode()
        {
            string datepart = DateTime.Today.Day.ToString("00") +
                  DateTime.Today.Month.ToString("00") +
                  DateTime.Today.Year.ToString("00");
            string idpart = (lab20Entities.GetContext().order.ToList().Last().ID + 1).ToString("000000"); // Определение в скобках позволит получить ID последнего айтема в таблице, увеличенный на 1.
            string contatenatedbar = datepart + idpart;

            return contatenatedbar;
        }

        private void TBX_BAR_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TBX_BAR.Text == "")
            {
                LB_BAR_Tip.Visibility = Visibility.Visible;
            }
            else
            {
                LB_BAR_Tip.Visibility = Visibility.Hidden;
            }
        }

        private static bool IsTextAllowed(string Text, string AllowedRegex) // Handling wrong symbols in textboxes to prevent type conflicts
        {
            try
            {
                var regex = new Regex(AllowedRegex);
                return !regex.IsMatch(Text);
            }
            catch
            {
                return true;
            }
        }

        private void TBX_BAR_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text, "^[0-9]+$"); //symbol restriction
            e.Handled = TBX_BAR.Text.Length > 13; //length restriction
        }

        private BitmapImage GenerateBarcode(string digicode)
        {
            BarcodeStandard.Barcode barcode = new BarcodeStandard.Barcode()
            {
                IncludeLabel = false, //true
                Alignment = AlignmentPositions.Center,
                Width = 370,
                Height = 45,
                BackColor = SkiaSharp.SKColor.Parse("#fafafa"),
                ForeColor = SkiaSharp.SKColor.Parse("#222222"),
            };
            var img = barcode.Encode(BarcodeStandard.Type.Code39, digicode).Encode().AsStream(); // Магия превращения SKImage в Stream
            BitmapImage barcodeBmp = ImageConverter.StreamToImage(img);                         // И потом в BitmapImage
            return barcodeBmp;
        }

        private void BTN_LockBar_Click(object sender, RoutedEventArgs e)
        {
            BTN_LockBar.Visibility = Visibility.Hidden;
            BTN_UnLockBar.Visibility = Visibility.Visible;
            BTN_GenerateBar.IsEnabled = false;
            TBX_BAR.IsEnabled = false;
            BRD_Bar.Visibility = Visibility.Visible;
            IMG_Bar.ImageSource = GenerateBarcode(TBX_BAR.Text);
            LB_Bar_Code.Content = TBX_BAR.Text;
            LB_Bar_Code.Visibility = Visibility.Visible;
        }

        private void BTN_UnLockBar_Click(object sender, RoutedEventArgs e)
        {
            BTN_UnLockBar.Visibility = Visibility.Hidden;
            BTN_LockBar.Visibility = Visibility.Visible;
            BTN_GenerateBar.IsEnabled = true;
            TBX_BAR.IsEnabled = true;
            BRD_Bar.Visibility = Visibility.Hidden;
            LB_Bar_Code.Visibility = Visibility.Hidden;
        }

        private void BTN_PrintBar_Click(object sender, EventArgs e) => PrintToPDF();

        private void PrintToPDF() //part 1
        {
            PrintDocument pd = new PrintDocument();

            pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";
            pd.PrinterSettings.PrintToFile = true;
            pd.PrintPage += PrintPage;
            pd.Print();
        }
        private void PrintPage(object o, PrintPageEventArgs e) //part 2
        {
            BarcodeStandard.Barcode barcode = new BarcodeStandard.Barcode()
            {
                IncludeLabel = true,
                Alignment = AlignmentPositions.Center,
                Width = 300,
                Height = 100,
                BackColor = SkiaSharp.SKColors.White,
                ForeColor = SkiaSharp.SKColors.Black,
            };
            var img = barcode.Encode(BarcodeStandard.Type.Code39, TBX_BAR.Text).Encode().AsStream();

            System.Drawing.Image img2 = System.Drawing.Image.FromStream(img);
            System.Drawing.PointF loc = new System.Drawing.PointF(100, 100);
            e.Graphics.DrawImage(img2, loc);
        }

        private void CHKBX_SelectedService_Checked(object sender, RoutedEventArgs e)
        {
            var CurrentService = LB_Services.SelectedItem as service;
            if (CurrentService != null)
            {
                if (CurrentService != null && Globals.orderServices.Contains(CurrentService.code))    //Если галка на сервисе отмечена и сервис до этого был в заказе - ничего не делать
                {
                    ;
                }
                else if (CurrentService != null)                                                     //Если галка на сервисе отмечена, но сервис до этого не был в заказе - добавить сервис
                {
                    Globals.orderServices.Add(CurrentService.code);
                }
            }
        }

        private void CHKBX_SelectedService_Unchecked(object sender, RoutedEventArgs e)
        {
            var CurrentService = LB_Services.SelectedItem as service;
            if (CurrentService != null)
            {
                if (Globals.orderServices.Contains(CurrentService.code))    //Если галка на сервисе не отмечена, но сервис до этого был в заказе - убрать сервис из заказа
                {
                    Globals.orderServices.RemoveAt(NewOrder.Globals.orderServices.IndexOf(CurrentService.code));
                }
                else                                                        //Если галка на сервисе не отмечена, и сервис до этого отсутствовал в заказе - ничего не делать
                {
                    ;
                }
            }
        }
    }
}

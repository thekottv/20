using _20.Model;
using System;
using System.Collections.Generic;
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

namespace _20.Windows.UserWindows
{
    /// <summary>
    /// Логика взаимодействия для ProbirkiInfoWindow.xaml
    /// </summary>
    public partial class ProbirkiInfoWindow : Window
    {
        private order _currentorder = new order();
        public ProbirkiInfoWindow(order selectedOrder)
        {
            InitializeComponent();
        }
    }
}

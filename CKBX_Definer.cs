using _20.Windows.UserWindows;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Linq;

namespace _20
{
    public class CKBX_Definer : IValueConverter
    {
        List<int> sLi = NewOrder.Globals.orderServices; //сервисы из текущего заказа
        int currentvalue;

        // В конвертере bool -> Brush  в качестве value передается status (состояние определенного сервиса, список с которыми передан в сурс листбокса),
        //      который является либо true, либо false
        //В данном случае (конвертер int -> bool) в качестве value передается code (код определенного сервиса, список с корыми передан в сурс листбокса),
        //      который является кодом текущего (для конкретного чекбокса) сервиса
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            currentvalue = (int)value;
            if (sLi.Contains((int)value) is bool Contains)
            {
                return Contains ? true : false ;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)  //Никогда не будет использовано из-за установленного типа привязки OneTime
        {
            throw new NotImplementedException();
        }
    }
}

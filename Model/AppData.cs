using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20.Model
{
    public static class AppData//Связь с БД на которую потом ссылаемся почти в каждом окне приложения
    {
        public static lab20Entities db = new lab20Entities();
    }
}

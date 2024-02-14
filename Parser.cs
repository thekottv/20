using _20.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _20
{
    public class Parser
    {
        public Dictionary<int, int> GetServicesDic_FromDB()  //Словарь сервисов с таблицы  //Получить список сервисов, соотнести id сервиса с code сервиса и сгенерировать словарь [code:id]
        {
            List<_20.Model.service> gottenServs = AppData.db.service.ToList();
            Dictionary<int, int> parsedServs = new Dictionary<int, int>();

            foreach (var service in gottenServs)
            {
                parsedServs.Add(service.code, service.ID);
            }

            return parsedServs;
        }

        public string GetServicesString_ForDB(List<int> servisi) //Строка сервисов для таблицы
        {
            int last = servisi.Last();

            string buitString = string.Empty;

            buitString += "[";
            foreach (int code in servisi)
            {
                buitString += $"{{\"code\":{code}}}";
                if (code != last)
                    buitString += ",";
            }
            buitString += "]";

            return buitString;
        }

        public List<int> GetServicesList_FromOrder(string services)  //Лист сервисов из заказа
        {
            List<int> buildList = new List<int>();
            var parsed = JArray.Parse(services);

            foreach (JObject root in parsed)
            {
                foreach (KeyValuePair<String, JToken> app in root)
                {
                    buildList.Add(Int32.Parse(app.Value.ToString().TrimStart('{', '}')));
                }
            }

            return buildList;
        }
    }
}

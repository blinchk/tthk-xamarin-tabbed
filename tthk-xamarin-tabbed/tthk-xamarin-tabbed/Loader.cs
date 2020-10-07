using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace tthk_xamarin_tabbed
{
    
    public class Loader
    {
        private string loadedJson;
        private List<Holiday> holidays;
        private JArray holidaysArray;
        private IList<JToken> holidaysTokens;

        public Loader()
        {
            loadedJson = new WebClient().DownloadString("https://xn--riigiphad-v9a.ee/?output=json");
            holidaysArray = JArray.Parse(loadedJson);
        }
        
        internal List<Holiday> GetHolidays()
        {
            holidays = new List<Holiday>();
            foreach (var token in holidaysArray)
            {
                Holiday holiday = token.ToObject<Holiday>();
                holidays.Add(holiday);
            }
            return holidays;
        }
    }
}
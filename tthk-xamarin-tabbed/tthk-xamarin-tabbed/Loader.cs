using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

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
        
        internal List<Holiday> GetMonthHolidays(int season, int month)
        {
            holidays = GetHolidays();
            int[,] monthBySeason = new int[4,3] { {3,4,5} , {6,7,8} , {9, 10, 11} , {12,1,2}};
            List<Holiday> selectedHolidays = new List<Holiday>();
            string selectedYear = Preferences.Get("year", DateTime.Now.Year.ToString());
            foreach (var holiday in holidays)
            {
                if (holiday.Date.Month == monthBySeason[season, month] && holiday.Date.Year.ToString() == selectedYear)
                {
                    selectedHolidays.Add(holiday);
                }
            }

            return selectedHolidays;
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
using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;

namespace tthk_xamarin_tabbed
{
    
    public class Loader
    {
        private readonly string loadedJson;
        private JArray holidaysArray;

        public Loader()
        {
            loadedJson = new WebClient().DownloadString("https://xn--riigiphad-v9a.ee/?output=json");
            holidaysArray = JArray.Parse(loadedJson);
        }
        
        internal List<Holiday> GetMonthHolidays(int season, int month)
        {
            List<Holiday> holidays = GetHolidays();
            int[,] monthBySeason = new int[,] { {3,4,5} , {6,7,8} , {9, 10, 11} , {12,1,2}};
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
            List<Holiday> holidays = new List<Holiday>();
            foreach (var token in holidaysArray)
            {
                Holiday holiday = token.ToObject<Holiday>();
                holidays.Add(holiday);
            }
            return holidays;
        }

        
    }
}
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace tthk_xamarin_tabbed
{
    public partial class MainPage : TabbedPage
    {
        private string[] MonthNames = new string[]
        {
            "Jaanuar", "Veebruar", "Märts", "Aprill", "Mai", "Juuni", "Juuli", "August", "September", "Oktoober",
            "November", "Detsember"
        };
        
        public MainPage()
        {
            Title = "Riigipühad";
            ItemsSource = MonthNames;
            ItemTemplate = new DataTemplate(() => new MonthPageModel());
        }
        

        public static List<Holiday> GetHolidays()
        {
            List<Holiday> holidays = new List<Holiday>();
            Loader loader = new Loader();
            holidays = loader.GetHolidays();
            return holidays;
        }
    }
}
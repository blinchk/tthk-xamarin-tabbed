using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace tthk_xamarin_tabbed
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonthPageModel : ContentPage
    {
        string[] MonthNames = new string[] {
            "Jaanuar", "Veebruar", "Märts", "Aprill", "Mai", "Juuni", "Juuli", "August", "September", "Oktoober",
            "November"
        };
        public IEnumerable<int> MonthNumbers = Enumerable.Range(1, 12);
        public ObservableCollection<HolidayGrouping<int, Holiday>> HolidayGroups { get; set; }
        private void GroupHolidays(IEnumerable<Holiday> holidayList)
        {
            var holidays = holidayList.GroupBy(h => h.Date.Year).Select(h => new HolidayGrouping<int, Holiday>(h.Key, h));
            HolidayGroups = new ObservableCollection<HolidayGrouping<int, Holiday>>(holidays);
            this.BindingContext = this;
        }

        public MonthPageModel()g
        {
            ListView listView = new ListView
            {
                HasUnevenRows = true,
                IsGroupingEnabled = true,
                GroupHeaderTemplate = new DataTemplate(() =>
                {
                    Label groupHeaderLabel = new Label()
                    {
                        FontSize = 20,
                        TextColor = Color.White
                    };
                    groupHeaderLabel.SetBinding(Label.TextProperty, "");
                    return new ViewCell
                    {
                        View = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            BackgroundColor = Color.Orange,
                            Children = {groupHeaderLabel}
                        }
                    };
                }),

                ItemTemplate = new DataTemplate(() =>
                {
                    Label titleLabel = new Label { FontSize=18 };
                    titleLabel.SetBinding(Label.TextProperty, "Title");
                    
                    Label dateLabel = new Label();
                    dateLabel.SetBinding(Label.TextProperty, "Date");
                    
                    Label kindLabel = new Label();
                    kindLabel.SetBinding(Label.TextProperty, "Kind");
                    
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Vertical,
                            Children = { titleLabel, dateLabel, kindLabel}
                        }
                    };
                })
            };
            GroupHolidays(MainPage.GetHolidays());
            listView.SetBinding(ListView.ItemsSourceProperty, "HolidayGroups");
            listView.SetBinding(ListView.GroupHeaderTemplateProperty, "Name");
            Content = listView;
        }
    }
}
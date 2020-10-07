using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace tthk_xamarin_tabbed
{
    public partial class MainPage : TabbedPage
    {
        private const int SEASONS_COUNT = 4;
        private const int MONTHS_COUNT = 3;
        private List<TabbedPage> seasonsTabbedPages;
        private ContentPage[,] monthsContentPages;
        private ContentPage March, Aprill, May, 
            June, July, August, 
            September, October, November, 
            December, January, February;
        private TabbedPage springTabbedPage, summerTabbedPage, fallTabbedPage, winterTabbedPage;
        public MainPage()
        {
            ToolbarItem yearChoose = new ToolbarItem()
            {
                Text = "Vali aasta"
            };
            yearChoose.Clicked += YearChooseClicked;
            ToolbarItems.Add(yearChoose);
            Title = "Pühad " + Preferences.Get("year", DateTime.Now.Year.ToString());
            string[] seasonNames = new string[SEASONS_COUNT] { "Kevad", "Suvi", "Sügis", "Talv" };
            string[,] monthsNames = new string[SEASONS_COUNT, MONTHS_COUNT] { 
                {"Märts", "Aprill", "Mai"}, 
                {"Juuni", "Juuli", "August"}, 
                {"September", "Oktoober", "November"}, 
                {"Detsember", "Jaanuar", "Veebruar"}
            }; 
            monthsContentPages = new ContentPage[SEASONS_COUNT, MONTHS_COUNT] {
                {March, Aprill, May}, 
                {June, July, August}, 
                {September, October, November},
                {December, January, February}
            };
            seasonsTabbedPages = new List<TabbedPage>() 
                {springTabbedPage, 
                summerTabbedPage,
                fallTabbedPage, 
                winterTabbedPage};
            Loader loader = new Loader();
            for (int i = 0; i < SEASONS_COUNT; i++)
            {
                seasonsTabbedPages[i] = new TabbedPage()
                {
                    Title = seasonNames[i]
                };
                for (int j = 0; j < MONTHS_COUNT; j++)
                {
                    var holidays = loader.GetMonthHolidays(i, j);
                    string monthName = monthsNames[i, j];
                    if (holidays.Count == 0)
                    {
                        monthsContentPages[i, j] = new ContentPage()
                        {
                            Title = monthName,
                            Content = new StackLayout()
                            {
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.FillAndExpand,
                                Children =
                                {
                                    new Image()
                                    {
                                        Source = "error.gif",
                                        IsAnimationPlaying = true,
                                    },
                                    new Label()
                                    {
                                        Text = "Kahjuks, sellel kuul puuduvad mingi pühad.",
                                        FontSize = 25,
                                        Padding = new Thickness(10, 0),
                                        VerticalTextAlignment = TextAlignment.Center,
                                        HorizontalTextAlignment = TextAlignment.Center,
                                    }
                                }
                            }
                        };
                    }
                    else
                    {
                        monthsContentPages[i, j] = new ContentPage()
                        {
                            Title = monthName,
                            Content = new ListView(ListViewCachingStrategy.RecycleElementAndDataTemplate) // ListView haves caching
                            {
                                HasUnevenRows = true,
                                ItemsSource = holidays,
                                SelectionMode = ListViewSelectionMode.None,
                                ItemTemplate = new DataTemplate(() =>
                                {
                                    Label dayLabel = new Label()
                                    {
                                        FontSize = 18,
                                        VerticalOptions = LayoutOptions.FillAndExpand
                                    };
                                    dayLabel.SetBinding(Label.TextProperty, "Day");

                                    Label titleLabel = new Label()
                                    {
                                        Padding = new Thickness(15, 0),
                                        FontAttributes = FontAttributes.Bold,
                                        VerticalOptions = LayoutOptions.FillAndExpand
                                    };
                                    titleLabel.SetBinding(Label.TextProperty, "Title");
                                    StackLayout dateWithHolidayLayout = new StackLayout()
                                    {
                                        Children = {dayLabel, titleLabel},
                                        Orientation = StackOrientation.Horizontal
                                    };
                                    Label kindLabel = new Label()
                                    {
                                        FontAttributes = FontAttributes.Italic
                                    };
                                    kindLabel.SetBinding(Label.TextProperty, "Kind");
                                    return new ViewCell
                                    {
                                        View = new StackLayout()
                                        {
                                            Padding = new Thickness(20,5),
                                            Orientation = StackOrientation.Vertical,
                                            Children = {dateWithHolidayLayout, kindLabel}
                                        }
                                    };
                                })
                            }
                        };
                    }
                    seasonsTabbedPages[i].Children.Add(monthsContentPages[i, j]);
                }

                Children.Add(seasonsTabbedPages[i]);
            }
        }

        private async void YearChooseClicked(object sender, EventArgs e)
        {
            int currentYear = DateTime.Now.Year;
            string[] availableHolidaysYears = new string[5];
            for (int i = 0; i < 5; i++)
            {
                int year = DateTime.Now.Year + i;
                availableHolidaysYears[i] = year.ToString();
            }
            string selectedYear = await DisplayActionSheet("Vali aasta", "Katkesta", null, availableHolidaysYears);
            if (selectedYear == "Katkesta")
            {
                selectedYear = Preferences.Get("year", DateTime.Now.Year.ToString());
            }
            Preferences.Set("year", selectedYear);
            (Application.Current).MainPage = new NavigationPage(new MainPage());
        }


        public List<Holiday> GetHolidays()
        {
            Loader loader = new Loader();
            List<Holiday> holidays = loader.GetHolidays();
            return holidays;
        }
    }
}
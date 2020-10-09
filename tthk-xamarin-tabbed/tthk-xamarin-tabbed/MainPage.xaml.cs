using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Runtime.CompilerServices;
using System.Linq;
using Newtonsoft.Json.Schema.Generation;

namespace tthk_xamarin_tabbed
{
    public partial class MainPage
    {
        private const int SeasonsCount = 4;
        private const int MonthsCount = 3;
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
            string[] seasonNames = new string[] { "Kevad", "Suvi", "Sügis", "Talv" };
            string[,] monthsNames = new string[,] { 
                {"Märts", "Aprill", "Mai"}, 
                {"Juuni", "Juuli", "August"}, 
                {"September", "Oktoober", "November"}, 
                {"Detsember", "Jaanuar", "Veebruar"}
            }; 
            monthsContentPages = new[,] {
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
            for (int i = 0; i < SeasonsCount; i++)
            {
                seasonsTabbedPages[i] = new TabbedPage()
                {
                    Title = seasonNames[i]
                };
                for (int j = 0; j < MonthsCount; j++)
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
                        ListView holidaysListView = new ListView(ListViewCachingStrategy.RecycleElementAndDataTemplate) // ListView haves caching
                        {
                            HasUnevenRows = true,
                            ItemsSource = holidays,
                            SelectionMode = ListViewSelectionMode.None,
                            ItemTemplate = new DataTemplate(() =>
                            {
                                Label dayLabel = new Label()
                                {
                                    FontSize = 23,
                                    VerticalOptions = LayoutOptions.FillAndExpand,
                                    VerticalTextAlignment = TextAlignment.Center
                                };
                                dayLabel.SetBinding(Label.TextProperty, "Day");

                                Label titleLabel = new Label()
                                {
                                    Padding = new Thickness(15, 0),
                                    FontAttributes = FontAttributes.Bold,
                                    VerticalOptions = LayoutOptions.FillAndExpand,
                                    VerticalTextAlignment = TextAlignment.Center
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
                                        Padding = new Thickness(20, 5),
                                        Orientation = StackOrientation.Vertical,
                                        Children = {dateWithHolidayLayout, kindLabel}
                                    }
                                };
                            })
                        };
                        holidaysListView.ItemTapped += HolidaysListViewOnItemTapped;
                        monthsContentPages[i, j] = new ContentPage()
                        {
                            Title = monthName,
                            Content = holidaysListView
                        };
                    }
                    seasonsTabbedPages[i].Children.Add(monthsContentPages[i, j]);
                }

                Children.Add(seasonsTabbedPages[i]);
            }
        }

        public string FirstLetterToUpper(string str)
        {
            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);
            else
                return str;
        }

        private async void HolidaysListViewOnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var content = e.Item as Holiday;
                if (content != null)
                {
                    string text;
                    text = $"{content.Date.Day}.{content.Date.Month}.{content.Date.Year} tähistatakse {content.Title}, mis on {content.Kind}.";
                    ShareText(text);
                }
            }
            ((ListView)sender).SelectedItem = null;
        }

        private async void YearChooseClicked(object sender, EventArgs e)
        {
            int currentYear = DateTime.Now.Year;
            string[] availableHolidaysYears = new string[5];
            for (int i = 0; i < 5; i++)
            {
                int year = currentYear + i;
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

        public async void ShareText(string text)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Saatada püha"
            });
        }
    }
}
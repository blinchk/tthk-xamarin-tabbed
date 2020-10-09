using System;
using System.Linq;

namespace tthk_xamarin_tabbed
{
    public class Holiday
    {
        private string title;

        public DateTime Date { get; set; }
        public int Day
        {
            get => Date.Day;
        }
        public string Title { 
            get { return title.First().ToString().ToUpper() + title.Substring(1); }
            set { title = value;  }
        }
        public string Notes { get; set; }
        public string Kind { get; set; }
    }
}
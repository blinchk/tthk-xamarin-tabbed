﻿using System;

namespace tthk_xamarin_tabbed
{
    public class Holiday
    {
        public DateTime Date { get; set; }
        public int Day
        {
            get => Date.Day;
        }
        public string Title { get; set; }
        public string Notes { get; set; }
        public string Kind { get; set; }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace tthk_xamarin_tabbed
{
    public class HolidayGrouping<K, T> : ObservableCollection<T>
    {
        public K Name { get; private set; }

        public HolidayGrouping(K name, IEnumerable<T> items)
        {
            Name = name;
            foreach (T item in items)
                Items.Add(item);
        }
    }
}
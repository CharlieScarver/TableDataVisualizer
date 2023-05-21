using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListVisualizer.Model
{
    public class TableResult // <T> where T : IList, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public TableResult()
        {
            Columns = new ObservableCollection<ItemColumn>();
            Items = new ObservableCollection<ObservableCollection<string>>();
        }

        public ObservableCollection<ItemColumn> Columns { get; set; }

        public ObservableCollection<ObservableCollection<string>> Items { get; set; }

    }
}

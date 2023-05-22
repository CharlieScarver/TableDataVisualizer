using System.Collections.ObjectModel;

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

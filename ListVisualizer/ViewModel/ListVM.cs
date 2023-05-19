using ListVisualizer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ListVisualizer.ViewModel
{
    public class ListVM : ObservableObject
    {
        private string _databaseName = "Data Source=.\\SQLEXPRESS;Initial Catalog=PearReviewDb;Integrated Security=True;TrustServerCertificate=true;"; // string.Empty;
        private string _tableName = "dbo.Courses"; // string.Empty;

        private readonly ObservableCollection<ObservableCollection<string>> _listItems = new ObservableCollection<ObservableCollection<string>>();
        private readonly ObservableCollection<string> _itemColumns = new ObservableCollection<string>();
        private readonly List<int> _shownColumns = new List<int>();

        private DataGrid dataGrid;

        public string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; NotifyPropertyChanged("DatabaseName"); }
        }

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; NotifyPropertyChanged("TableName"); }
        }

        public ObservableCollection<ObservableCollection<string>> ListItems
        {
            get { return _listItems; }
        }

        public ObservableCollection<string> ItemColumns
        {
            get { return _itemColumns; }
        }

        public ICommand CmdConnectToDb
        {
            get { return new CommandCreator(ConnectToDb); }
        }

        public ICommand CmdToggleCheckbox
        {
            get { return new CommandCreator(ToggleCheckbox); }
        }

        public List<int> ShownColumns => _shownColumns;

        private void ConnectToDb(object gridParam)
        {
            // Extract to Model
            using IDbConnection connection = new SqlConnection(DatabaseName);
            string sqlquery = $"SELECT * FROM {TableName}";
            IDbCommand command = new SqlCommand
            {
                Connection = (SqlConnection)connection
            };
            connection.Open();
            command.CommandText = sqlquery;
            IDataReader reader = command.ExecuteReader();
            // ---

            bool notEndOfResult;
            notEndOfResult = reader.Read();

            ItemColumns.Clear();
            ListItems.Clear();
            bool readColNames = true;

            // Latest
            DataGrid grid = (DataGrid)gridParam;
            if (dataGrid != grid) {
                dataGrid = grid;
            }
            grid.Columns.Clear();
            // ---

            while (notEndOfResult)
            {
                ObservableCollection<string> values = new ObservableCollection<string>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string colName = reader.GetName(i);
                    string value = reader.GetValue(i).ToString() ?? "";
                    if (readColNames)
                    {
                        ItemColumns.Add(colName);
                        ShownColumns.Add(i);
                    }
                    values.Add(value);
                }

                readColNames = false;
                ListItems.Add(values);
                notEndOfResult = reader.Read();
            }

            // Latest
            ListItems.CollectionChanged += DataGrid_CollectionChanged;
            grid.ItemsSource = ListItems;
            // ---

            RebuildDataGrid();
        }

        private void DataGrid_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            dataGrid.Columns.Clear();
            dataGrid.Items.Refresh();
        }

        private void RebuildDataGrid()
        {
            // Rebuild grid - we expect a new column configuration
            dataGrid.Columns.Clear();

            for (int i = 0; i < _itemColumns.Count; i++)
            {
                string colName = _itemColumns[i];

                if (ShownColumns.IndexOf(i) != -1)
                {
                    DataGridTextColumn column = new DataGridTextColumn();
                    column.Header = colName;
                    column.Binding = new Binding($"[{i}]");
                    dataGrid.Columns.Add(column);
                }
            }

            dataGrid.Items.Refresh();
        }

        private void ToggleCheckbox(object parameter)
        {
            int index = ItemColumns.IndexOf(parameter.ToString() ?? "");
            if (index != -1)
            {
                // Update shown columns
                if (ShownColumns.IndexOf(index) == -1)
                {
                    ShownColumns.Add(index);
                }
                else
                {
                    ShownColumns.Remove(index);
                }

                // Rebuild grid
                RebuildDataGrid();
            }
        }
    }
}

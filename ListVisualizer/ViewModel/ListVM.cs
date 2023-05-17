using ListVisualizer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ListVisualizer.ViewModel
{
    public class ListVM : ObservableObject
    {
        private string _databaseName = "Data Source=.\\SQLEXPRESS;Initial Catalog=PearReviewDb;Integrated Security=True;TrustServerCertificate=true;"; // string.Empty;
        private string _tableName = "dbo.Courses"; // string.Empty;

        private readonly ObservableCollection<ObservableCollection<string>> _listItems = new ObservableCollection<ObservableCollection<string>>();
        private readonly ObservableCollection<string> _itemProperties = new ObservableCollection<string>();

        private readonly ObservableCollection<ExpandoObject> _dynamicItems = new ObservableCollection<ExpandoObject>();

        private ObservableCollection<IListItem> _classItems = new ObservableCollection<IListItem>();

        private TableData _tableDataItems;
        private DataTable _dataTable = new DataTable();

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

        public ObservableCollection<string> ItemPropeties
        {
            get { return _itemProperties; }
        }

        public TableData TableDataItems
        {
            get { return _tableDataItems; }
            set { _tableDataItems = value; NotifyPropertyChanged("TableDataItems"); }
        }

        public IEnumerable<DataRow> DataTableItems
        {
            get { return _dataTable.AsEnumerable(); }
        }

        public ObservableCollection<ExpandoObject> DynamicItems
        {
            get { return _dynamicItems; }
        }

        public ObservableCollection<IListItem> ClassItems
        {
            get { return _classItems; }
        }

        public ICommand CmdConnectToDb
        {
            get { return new CommandCreator(ConnectToDb); }
        }

        public ICommand CmdToggleCheckbox
        {
            get { return new CommandCreator(ToggleCheckbox); }
        }

        public ICommand CmdSetItems
        {
            get { return new CommandCreator(SetItems); }
        }

        private void ConnectToDb(object gridParam)
        {
            using IDbConnection connection = new SqlConnection(DatabaseName);
            string sqlquery = $"SELECT * FROM {TableName}";
            IDbCommand command = new SqlCommand
            {
                Connection = (SqlConnection)connection
            };
            connection.Open();
            command.CommandText = sqlquery;
            IDataReader reader = command.ExecuteReader();

            bool notEndOfResult;
            notEndOfResult = reader.Read();

            ItemPropeties.Clear();
            ListItems.Clear();
            bool readColNames = true;

            //DataGrid grid = (DataGrid)gridParam;
            DataTable table = _dataTable; //new DataTable();
            table.Columns.Clear();
            table.Rows.Clear();
            // grid.Columns.Clear();

            while (notEndOfResult)
            {
                ObservableCollection<string> values = new ObservableCollection<string>();
                string[] arrValues = new string[reader.FieldCount];
                dynamic row = new ExpandoObject();
                DataRow tableRow = table.NewRow();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string colName = reader.GetName(i);
                    string value = reader.GetValue(i).ToString() ?? "";
                    if (readColNames)
                    {
                        ItemPropeties.Add(colName);
                        table.Columns.Add(colName, typeof(string));
                        //grid.Columns.Add(new DataGridTextColumn() { Header = colName });
                    }
                    values.Add(value);
                    arrValues[i] = value;
                    //row[colName] = value;
                }

                tableRow.ItemArray = arrValues;

                readColNames = false;
                ListItems.Add(values);
                DynamicItems.Add(row);
                table.Rows.Add(tableRow);
                notEndOfResult = reader.Read();
            }

            NotifyPropertyChanged("DataTableItems"); // !! Maybe put in setter?
            //grid.ItemsSource = table.AsEnumerable();
            CreateTableData();
        }

        private void ToggleCheckbox(object parameter)
        {

        }

        private void CreateTableData()
        {
            List<TableDataRow> rows = new List<TableDataRow>();
            foreach (var item in ListItems)
            {
                TableDataRow tr = new TableDataRow(item.ToList<string>());
                rows.Add(tr);
            }
            TableData table = new TableData(ItemPropeties.ToList<string>(), rows);
        }

        private void SetItems(object itemsParam)
        {
            IEnumerable<IListItem> items = itemsParam as IEnumerable<IListItem>;
            ClassItems.Clear();
            foreach (var item in items)
            {
                ClassItems.Add(item);
            }
        }
    }
}

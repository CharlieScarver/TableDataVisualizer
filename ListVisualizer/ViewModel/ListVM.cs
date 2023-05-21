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
using System.Windows.Documents;
using System.Windows.Input;

namespace ListVisualizer.ViewModel
{
    public class ListVM : ObservableObject
    {
        private string _databaseName = "Data Source=.\\SQLEXPRESS;Initial Catalog=PearReviewDb;Integrated Security=True;TrustServerCertificate=true;"; // string.Empty;
        private string _tableName = "dbo.Courses"; // string.Empty;

        private DataGrid dataGrid;
        private DatabaseContext dbContext;
        private TableResult tableItems;

        public ListVM()
        {
            dbContext = new DatabaseContext(DatabaseName);
            dataGrid = new DataGrid();

            TableItems = new TableResult();
        }

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

        public TableResult TableItems
        {
            get { return tableItems; }
            set { tableItems = value; NotifyPropertyChanged("TableItems"); }
        }

        // Commands

        public ICommand CmdFillDataGrid
        {
            get { return new CommandCreator(FillDataGrid); }
        }

        public ICommand CmdToggleCheckbox
        {
            get { return new CommandCreator(ToggleCheckbox); }
        }

        // Internal methods

        private void FillDataGrid(object gridParam)
        {
            TableItems = dbContext.FetchTableEntries(TableName);

            // Get a reference to the DataGrid
            DataGrid grid = (DataGrid)gridParam;
            if (dataGrid != grid) {
                dataGrid = grid;
            }

            // Update
            grid.ItemsSource = TableItems.Items;

            RebuildDataGrid();
        }

        private void RebuildDataGrid()
        {
            // Rebuild grid - we expect a new column configuration
            dataGrid.Columns.Clear();

            for (int i = 0; i < TableItems.Columns.Count; i++)
            {
                ItemColumn col = TableItems.Columns[i];

                if (col.IsVisible)
                {
                    DataGridTextColumn column = new DataGridTextColumn();
                    column.Header = col.Name;
                    column.Binding = new Binding($"[{i}]");
                    column.IsReadOnly = true;
                    dataGrid.Columns.Add(column);
                }
            }

            dataGrid.Items.Refresh();
        }

        private void ToggleCheckbox(object itemColumn)
        {
            // Rebuild grid
            RebuildDataGrid();
        }
    }
}

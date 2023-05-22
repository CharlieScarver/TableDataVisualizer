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
        private const string TABLES_TABLE_NAME = "INFORMATION_SCHEMA.TABLES";
        private const string TABLES_CONDITION = "TABLE_TYPE = 'BASE TABLE'";
        private const string TABLES_COLUMNS = "TABLE_NAME";

        private readonly AppConfiguration config;

        private readonly DatabaseContext dbContext;
        private DataGrid dataGrid;

        private TableResult availableTables;
        private TableResult tableItems;

        public ListVM()
        {
            config = AppConfiguration.GetAppConfiguration();

            dbContext = new DatabaseContext();
            dataGrid = new DataGrid();

            AvailableTables = new TableResult();
            TableItems = new TableResult();
        }

        public string TableName
        {
            get { return config.TableName; }
            set { config.TableName = value; NotifyPropertyChanged("TableName"); }
        }

        public TableResult TableItems
        {
            get { return tableItems; }
            set { tableItems = value; NotifyPropertyChanged("TableItems"); }
        }

        public TableResult AvailableTables
        {
            get { return availableTables; }
            set { availableTables = value; NotifyPropertyChanged("AvailableTables"); }
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

        public ICommand CmdFetchTables
        {
            get { return new CommandCreator(FetchTables); }
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

        private void FetchTables(object _)
        {
            AvailableTables = dbContext.FetchTableEntries(TABLES_TABLE_NAME, TABLES_CONDITION, TABLES_COLUMNS);
        }
    }
}

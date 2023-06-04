using ListVisualizer.Model;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ListVisualizer.ViewModel
{
    public class ListViewModel : ObservableObject
    {
        private const string TABLES_TABLE_NAME = "INFORMATION_SCHEMA.TABLES";
        private const string TABLES_CONDITION = "TABLE_TYPE = 'BASE TABLE'";
        private const string TABLES_COLUMNS = "TABLE_NAME";

        private readonly AppConfiguration config;

        private readonly DatabaseContext dbContext;
        private DataGrid dataGrid;

        private TableResult availableTables;
        private TableResult tableItems;

        private bool isTableSelected = false;

        public ListViewModel()
        {
            config = AppConfiguration.GetAppConfiguration();

            dbContext = new DatabaseContext();
            dataGrid = new DataGrid();

            AvailableTables = new TableResult();
            TableItems = new TableResult();

            if (config.TableName != null && config.TableName != string.Empty)
            {
                IsTableSelected = true;
            }
        }

        public string TableName
        {
            get { return config.TableName; }
            set {
                config.TableName = value;
                if (config.TableName != null && config.TableName != string.Empty)
                {
                    IsTableSelected = true;
                }
                NotifyPropertyChanged(nameof(TableName));
            }
        }

        public TableResult TableItems
        {
            get { return tableItems; }
            set { tableItems = value; NotifyPropertyChanged(nameof(TableItems)); }
        }

        public TableResult AvailableTables
        {
            get { return availableTables; }
            set { availableTables = value; NotifyPropertyChanged(nameof(AvailableTables)); }
        }

        public bool IsTableSelected
        {
            get => isTableSelected;
            set { isTableSelected = value; NotifyPropertyChanged(nameof(IsTableSelected)); }
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

        protected virtual void FillDataGrid(object gridParam)
        {
            if (config.ConnectionString == null || config.ConnectionString == "") { return; }
            if (TableName == null || TableName == "") { return; }

            try
            {
                TableItems = dbContext.FetchTableEntries(TableName);
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }

            // Get a reference to the DataGrid
            DataGrid grid = (DataGrid)gridParam;
            if (dataGrid != grid)
            {
                dataGrid = grid;
            }

            // Update
            grid.ItemsSource = TableItems.Items;

            RebuildDataGrid();
        }

        protected virtual void RebuildDataGrid()
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

        protected virtual void ToggleCheckbox(object itemColumn)
        {
            // Rebuild grid
            RebuildDataGrid();
        }

        protected virtual void FetchTables(object _)
        {
            if (config.ConnectionString == null || config.ConnectionString == "") { return; }

            IsTableSelected = false;

            try
            {
                AvailableTables = dbContext.FetchTableEntries(TABLES_TABLE_NAME, TABLES_CONDITION, TABLES_COLUMNS);
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }

            if (config.DeveloperMode)
            {
                AvailableTables.Columns.RemoveAt(0);
                for (int i = 0; i < AvailableTables.Items.Count; i++)
                {
                    AvailableTables.Items[i].RemoveAt(0);
                }
            }
        }
    }
}

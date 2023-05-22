using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace ListVisualizer.Model
{
    public class DatabaseContext
    {
        public DatabaseContext()
        {
        }

        public string ConnectionString { get => AppConfiguration.GetAppConfiguration().ConnectionString; }

        public TableResult FetchTableEntries(string tableName, string sqlCondition = "", string columnNames = "")
        {
            AppConfiguration config = AppConfiguration.GetAppConfiguration();

            using IDbConnection connection = new SqlConnection(config.ConnectionString);
            string sqlQuery = $"SELECT * FROM {tableName}";

            if (columnNames != null && columnNames != "")
            {
                sqlQuery = $"SELECT {columnNames} FROM {tableName}";
            }

            if (sqlCondition != null && sqlCondition != "")
            {
                sqlQuery = $"{sqlQuery} WHERE {sqlCondition}";
            }

            IDbCommand command = new SqlCommand
            {
                Connection = (SqlConnection)connection
            };
            connection.Open();
            command.CommandText = sqlQuery;
            IDataReader reader = command.ExecuteReader();

            bool notEndOfResult;
            notEndOfResult = reader.Read();

            TableResult result = new TableResult();
            bool readColNames = true;
            int rowNumber = 1;

            if (config.DeveloperMode)
            {
                result.Columns.Add(new ItemColumn("SID", true));
            }

            // For each result row:
            while (notEndOfResult)
            {
                ObservableCollection<string> row = new ObservableCollection<string>();
                if (config.DeveloperMode)
                {
                    row.Add(rowNumber.ToString());
                }

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    // Columns
                    if (readColNames)
                    {
                        string colName = reader.GetName(i);
                        ItemColumn col = new ItemColumn(colName, true);
                        result.Columns.Add(col);
                    }
                    // Row values
                    string value = reader.GetValue(i).ToString() ?? "";
                    row.Add(value);
                }

                readColNames = false;
                result.Items.Add(row);
                notEndOfResult = reader.Read();
                rowNumber++;
            }

            return result;
        }
    }
}

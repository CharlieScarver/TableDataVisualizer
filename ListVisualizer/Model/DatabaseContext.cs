using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ListVisualizer.Model
{
    public class DatabaseContext
    {
        public DatabaseContext()
        {
            ConnectionString = string.Empty;
        }

        public DatabaseContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; protected set; }

        public TableResult FetchTableEntries(string tableName, string sqlCondition = "", string columnNames = "")
        {
            using IDbConnection connection = new SqlConnection(ConnectionString);
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

            // For each result row:
            while (notEndOfResult)
            {
                ObservableCollection<string> row = new ObservableCollection<string>();

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
            }

            return result;
        }
    }
}

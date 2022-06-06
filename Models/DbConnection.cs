using System;
using System.Data.SQLite;

namespace CRM.Models
{
    internal static class DbConnection
    {
        internal static SQLiteCommand Open()
        {
            string connectionString = $"Data Source={AppDomain.CurrentDomain.BaseDirectory}db.sqlite;Version=3;";

            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            var cmd = new SQLiteCommand(connection);

            return cmd;
        }
    }
}

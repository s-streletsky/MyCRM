using System.Data.SQLite;

namespace CRM.Models
{
    internal static class DbConnection
    {
        private const string connectionString = @"Data Source=C:\SQLiteStudio\crm_db;Version=3;";
        public static SQLiteCommand Open()
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            var cmd = new SQLiteCommand(connection);

            return cmd;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class StockRepository : IRepository<StockItem>
    {
        private const string connectionString = @"Data Source=C:\SQLiteStudio\crm_db;Version=3;";

        public StockItem Add(StockItem item)
        {
            throw new NotImplementedException();
        }

        public void Delete(StockItem item)
        {
            throw new NotImplementedException();
        }

        public StockItem Get(StockItem item)
        {
            throw new NotImplementedException();
        }

        public void GetAll(Database db)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var cmd = new SQLiteCommand(connection);

                cmd.CommandText = "SELECT * FROM products";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var id = sqlReader.GetInt32(0);
                    var name = sqlReader.GetString(1);
                    var manufacturer = sqlReader.GetInt32(2);
                    var description = sqlReader.GetString(3);
                    var currency = (Currency)sqlReader.GetInt32(4);
                    var purchasePrice = sqlReader.GetFloat(5);
                    var retailPrice = sqlReader.GetFloat(6);
                    var quantity = sqlReader.GetFloat(7);

                    db.StockItems.Add(new StockItem(id, name, manufacturer, description, currency, purchasePrice, retailPrice, quantity));
                }
            }
        }

        public StockItem Update(StockItem item)
        {
            throw new NotImplementedException();
        }
    }
}

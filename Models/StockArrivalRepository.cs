using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class StockArrivalRepository : IRepository<StockArrival>
    {
        public StockArrival Add(StockArrival stockArrival)
        {
            using (var cmd = DbConnection.Open())
            {
                string addStockArrival = "INSERT INTO stock_arrivals (date, stock_item_id, quantity) VALUES "
                    + $"('{stockArrival.Date}', {stockArrival.StockItem.Id}, {stockArrival.Quantity})";

                cmd.CommandText = addStockArrival;
                cmd.ExecuteNonQuery();

                string getItemId = "SELECT id FROM stock_arrivals WHERE date=" + $"'{stockArrival.Date}'";
                cmd.CommandText = getItemId;
                stockArrival.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return stockArrival;
            }
        }

        public void Delete(StockArrival stockArrival)
        {
            using (var cmd = DbConnection.Open())
            {
                string deleteStockArrival = "DELETE FROM stock_arrivals WHERE id=" + $"{stockArrival.Id}";

                cmd.CommandText = deleteStockArrival;
                cmd.ExecuteNonQuery();
            }
        }

        public StockArrival Get(StockArrival item)
        {
            throw new NotImplementedException();
        }

        public void GetAll(Database db)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "SELECT * FROM stock_arrivals ORDER BY date DESC";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var id = sqlReader.GetInt32(0);
                    var date = DateTime.Parse(sqlReader.GetString(1));
                    var stockItem = db.StockItems.First(x => x.Id == sqlReader.GetInt32(2));
                    var quantity = sqlReader.GetFloat(3);

                    db.StockArrivals.Add(new StockArrival(id, date, stockItem, quantity));
                }
            }
        }

        public StockArrival Update(StockArrival stockArrival)
        {
            using (var cmd = DbConnection.Open())
            {
                string updateStockArrival = "UPDATE stock_arrivals SET quantity=" + $"'{stockArrival.Quantity}' " +
                                            "WHERE id=" + $"'{stockArrival.Id}'";

                cmd.CommandText = updateStockArrival;
                cmd.ExecuteNonQuery();

                return stockArrival;
            }
        }
    }
}

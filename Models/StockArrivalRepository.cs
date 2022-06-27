using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                cmd.CommandText = @"INSERT INTO StockArrivals (date, stock_item_id, quantity)
                    VALUES (@date, @stock_item_id, @quantity)";
                cmd.Parameters.AddWithValue("@date", stockArrival.Date);
                cmd.Parameters.AddWithValue("@stock_item_id", stockArrival.StockItem.Id);
                cmd.Parameters.AddWithValue("@quantity", stockArrival.Quantity);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT id FROM StockArrivals WHERE date = @date";
                cmd.Parameters.AddWithValue("@date", stockArrival.Date);
                cmd.Prepare();
                stockArrival.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return stockArrival;
            }
        }

        public void Delete(StockArrival stockArrival)
        {
            using (var cmd = DbConnection.Open())
            {
                string deleteStockArrival = "DELETE FROM StockArrivals WHERE id=" + $"{stockArrival.Id}";

                cmd.CommandText = deleteStockArrival;
                cmd.ExecuteNonQuery();
            }
        }

        public StockArrival Get(StockArrival item)
        {
            throw new NotImplementedException();
        }

        public void GetAll(ObservableCollection<StockArrival> dbStockArrivals, IEnumerable<StockItem> dbStockItems)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "SELECT * FROM StockArrivals ORDER BY date DESC";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var id = sqlReader.GetInt32(0);
                    var date = DateTime.Parse(sqlReader.GetString(1));
                    var stockItem = dbStockItems.First(x => x.Id == sqlReader.GetInt32(2));
                    var quantity = sqlReader.GetFloat(3);

                    dbStockArrivals.Add(new StockArrival(id, date, stockItem, quantity));
                }
            }
        }

        public bool TryDelete(StockArrival item)
        {
            throw new NotImplementedException();
        }

        public StockArrival Update(StockArrival stockArrival)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = @"UPDATE StockArrivals
                    SET quantity=@quantity
                    WHERE id=@id";

                cmd.Parameters.AddWithValue("@quantity", stockArrival.Quantity);
                cmd.Parameters.AddWithValue("@id", stockArrival.Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                return stockArrival;
            }
        }
    }
}

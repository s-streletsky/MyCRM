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
        public StockArrival Add(StockArrival item)
        {
            using (var cmd = DbConnection.Open())
            {
                string addArrival = "INSERT INTO stock_arrivals (date, stock_item_id, quantity) VALUES "
                    + $"('{item.Date}', {item.StockItem.Id}, {item.Quantity})";

                cmd.CommandText = addArrival;
                cmd.ExecuteNonQuery();

                string getItemId = "SELECT id FROM stock_arrivals WHERE date=" + $"'{item.Date}'";
                cmd.CommandText = getItemId;
                item.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return item;
            }
        }

        public void Delete(StockArrival item)
        {
            throw new NotImplementedException();
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

        public StockArrival Update(StockArrival item)
        {
            throw new NotImplementedException();
        }
    }
}

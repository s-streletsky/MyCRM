using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class StockItemRepository : IRepository<StockItem>
    {
        public StockItem Add(StockItem item)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "INSERT INTO StockItems (name, manufacturer_id, description, currency_id, purchase_price, retail_price, quantity) VALUES "
                    + $"(@name, @manufacturer_id, @description, @currency_id, @purchase_price, @retail_price, @quantity)";

                cmd.Parameters.AddWithValue("@name", item.Name);
                cmd.Parameters.AddWithValue("@manufacturer_id", item.Manufacturer.Id);
                cmd.Parameters.AddWithValue("@description", item.Description);
                cmd.Parameters.AddWithValue("@currency_id", (int)item.Currency);
                cmd.Parameters.AddWithValue("@purchase_price", item.PurchasePrice);
                cmd.Parameters.AddWithValue("@retail_price", item.RetailPrice);
                cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT id FROM StockItems WHERE name=@name";
                cmd.Parameters.AddWithValue("@name", item.Name);
                cmd.Prepare();
                item.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return item;
            }
        }

        public void Delete(StockItem item)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "DELETE FROM StockItems WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", item.Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }

        public StockItem Get(StockItem item)
        {
            throw new NotImplementedException();
        }

        public void GetAll(Database db)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "SELECT * FROM StockItems ORDER BY id DESC";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var id = sqlReader.GetInt32(0);
                    var name = sqlReader.GetString(1);
                    var manufacturer = db.Manufacturers.First(x => x.Id == sqlReader.GetInt32(2));
                    var description = sqlReader.GetString(3);
                    var currency = (Currency)sqlReader.GetInt32(4);
                    var purchasePrice = sqlReader.GetFloat(5);
                    var retailPrice = sqlReader.GetFloat(6);
                    var quantity = sqlReader.GetFloat(7);

                    db.StockItems.Add(new StockItem(id, name, manufacturer, description, currency, purchasePrice, retailPrice, quantity));
                }

                foreach (var item in db.StockItems)
                {
                    UpdateQuantity(item);
                }
            }
        }

        public bool TryDelete(StockItem item)
        {
            throw new NotImplementedException();
        }

        public StockItem Update(StockItem item)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "UPDATE StockItems SET name=@name, " +
                                                         "manufacturer_id=@manufacturer_id, " +
                                                         "description=@description, " +
                                                         "currency_id=@currency_id, " +
                                                         "purchase_price=@purchase_price, " +
                                                         "retail_price=@retail_price " +
                                     "WHERE id=@id";

                cmd.Parameters.AddWithValue("@name", item.Name);
                cmd.Parameters.AddWithValue("@manufacturer_id", item.Manufacturer.Id);
                cmd.Parameters.AddWithValue("@description", item.Description);
                cmd.Parameters.AddWithValue("@currency_id", (int)item.Currency);
                cmd.Parameters.AddWithValue("@purchase_price", item.PurchasePrice);
                cmd.Parameters.AddWithValue("@retail_price", item.RetailPrice);
                cmd.Parameters.AddWithValue("@id", item.Id);
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                return item;
            }
        }

        public void UpdateQuantity(StockItem item)
        {
            var stockList = new List<float>();
            var ordersList = new List<float>();

            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "SELECT * FROM StockArrivals WHERE stock_item_id=@id";
                cmd.Parameters.AddWithValue("@id", item.Id);
                cmd.Prepare();
                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var quantity = sqlReader.GetFloat(3);

                    stockList.Add(quantity);
                }

                sqlReader.Close();

                var stockSum = stockList.Sum();

                cmd.CommandText = "SELECT * FROM OrdersItems WHERE stock_item_id=@id";
                cmd.Parameters.AddWithValue("@id", item.Id);
                cmd.Prepare();
                SQLiteDataReader sqlReader2 = cmd.ExecuteReader();

                while (sqlReader2.Read())
                {
                    var quantity = sqlReader2.GetFloat(3);

                    ordersList.Add(quantity);
                }

                sqlReader2.Close();

                var ordersSum = ordersList.Sum();

                item.Quantity = stockSum - ordersSum;

                cmd.CommandText = "UPDATE StockItems SET quantity=@quantity WHERE id=@id";
                cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                cmd.Parameters.AddWithValue("@id", item.Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
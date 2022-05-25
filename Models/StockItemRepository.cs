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
                string addItem = "INSERT INTO stock_items (name, manufacturer_id, description, currency_id, purchase_price, retail_price, quantity) VALUES "
                    + $"('{item.Name}', '{item.Manufacturer.Id}', '{item.Description}', '{(int)item.Currency}', '{item.PurchasePrice}', '{item.RetailPrice}', '{item.Quantity}')";

                cmd.CommandText = addItem;
                cmd.ExecuteNonQuery();

                string getItemId = "SELECT id FROM stock_items WHERE name=" + $"'{item.Name}'";
                cmd.CommandText = getItemId;
                item.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return item;
            }
        }

        public void Delete(StockItem item)
        {
            using (var cmd = DbConnection.Open())
            {
                string deleteItem = "DELETE FROM stock_items WHERE id=" + $"{item.Id}";

                cmd.CommandText = deleteItem;
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
                cmd.CommandText = "SELECT * FROM stock_items";

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
                string updateItem = "UPDATE stock_items SET name=" + $"'{item.Name}', " +
                                                         "manufacturer_id=" + $"{item.Manufacturer.Id}, " +
                                                         "description=" + $"'{item.Description}', " +
                                                         "currency_id=" + $"{(int)item.Currency}, " +
                                                         "purchase_price=" + $"{item.PurchasePrice}, " +
                                                         "retail_price=" + $"'{item.RetailPrice}' " +
                                     "WHERE id=" + $"{item.Id}";

                cmd.CommandText = updateItem;
                cmd.ExecuteNonQuery();

                return item;
            }
        }

        public void UpdateQuantity(StockItem item)
        {
            var quantityList = new List<float>();

            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "SELECT * FROM stock_arrivals WHERE stock_item_id=" + $"{item.Id}";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var quantity = sqlReader.GetFloat(3);

                    quantityList.Add(quantity);
                }

                sqlReader.Close();

                var sum = quantityList.Sum();
                
                item.Quantity = sum;

                string updateQuantity = "UPDATE stock_items SET quantity=" + $"{sum} " + "WHERE id=" + $"{item.Id}";

                cmd.CommandText = updateQuantity;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
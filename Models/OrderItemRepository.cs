using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class OrderItemRepository : IRepository<OrderItem>
    {
        public OrderItem Add(OrderItem item)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "INSERT INTO OrdersItems (order_id, stock_item_id, quantity, price, discount, total, profit, expenses, exchange_rate) VALUES "
                                + $"(@order_id, @stock_item_id, @quantity, @price, @discount, @total, @profit, @expenses, @exchange_rate)";
                cmd.Parameters.AddWithValue("@order_id", item.Order.Id);
                cmd.Parameters.AddWithValue("@stock_item_id", item.StockItem.Id);
                cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                cmd.Parameters.AddWithValue("@price", item.Price);
                cmd.Parameters.AddWithValue("@discount", item.Discount);
                cmd.Parameters.AddWithValue("@total", item.Total);
                cmd.Parameters.AddWithValue("@profit", item.Profit);
                cmd.Parameters.AddWithValue("@expenses", item.Expenses);
                cmd.Parameters.AddWithValue("@exchange_rate", item.ExchangeRate);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                string getOrderId = "SELECT id FROM OrdersItems WHERE id = (SELECT MAX(id) FROM OrdersItems)";
                cmd.CommandText = getOrderId;
                item.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return item;
            }
        }

        public void Delete(OrderItem item)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "DELETE FROM OrdersItems WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", item.Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }

        public OrderItem Get(OrderItem item)
        {
            //using (var cmd = DbConnection.Open())
            //{
            //    cmd.CommandText = "SELECT * FROM clients";

            //    SQLiteDataReader sqlReader = cmd.ExecuteReader();

            //    while (sqlReader.Read())
            //    {
            //        var id = sqlReader.GetInt32(0);
            //        var created = DateTime.Parse(sqlReader.GetString(1));
            //        var name = sqlReader.GetString(2);
            //        var nickname = sqlReader.GetString(3);
            //        var phone = sqlReader.GetString(4);
            //        var email = sqlReader.GetString(5);
            //        var country = (Country)sqlReader.GetInt32(6);
            //        var city = sqlReader.GetString(7);
            //        var address = sqlReader.GetString(8);
            //        var shippingMethod = (ShippingMethod)sqlReader.GetInt32(9);
            //        var postalCode = sqlReader.GetString(10);
            //        var notes = sqlReader.GetString(11);

            //        db.Clients.Add(new Client(id, created, name, nickname, phone, email, country, city, address, shippingMethod, postalCode, notes));
            //    }
            //}
            return item;
        }

        public void GetAll(Database db)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "SELECT * FROM OrdersItems ORDER BY id DESC";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var id = sqlReader.GetInt32(0);

                    var orderId = sqlReader.GetInt32(1);
                    Order order = null;

                    foreach (var item in db.Orders)
                    {
                        if (item.Id == orderId)
                        {
                            order = item;
                            break;
                        }
                    }

                    var stockItemId = sqlReader.GetInt32(2);
                    StockItem stockItem = null;

                    foreach (var item in db.StockItems)
                    {
                        if (item.Id == stockItemId)
                        {
                            stockItem = item;
                            break;
                        }
                    }

                    var quantity = sqlReader.GetFloat(3);
                    var price = sqlReader.GetFloat(4);
                    var discount = sqlReader.GetFloat(5);
                    var total = sqlReader.GetFloat(6);
                    var profit = sqlReader.GetFloat(7);
                    var expenses = sqlReader.GetFloat(8);
                    var exchange_rate = sqlReader.GetFloat(9);

                    db.OrdersItems.Add(new OrderItem(id, order, stockItem, quantity, price, discount, total, profit, expenses, exchange_rate));
                }
            }
        }

        public bool TryDelete(OrderItem item)
        {
            //using (var cmd = DbConnection.Open())
            //{
            //    string exists = "SELECT 1 FROM OrdersItems WHERE id=" + $"{item.Id} LIMIT 1";

            //    cmd.CommandText = exists;
            //    var result = cmd.ExecuteScalar();

            //    if (result != null) return false;
            //    //{
            //    //    return false;
            //    //}
            //    //else
            //    //{
            //    //    return true;
            //    //}
            //    else return true;
            //}
            return false;
        }

        public OrderItem Update(OrderItem item)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "UPDATE OrdersItems SET quantity=@quantity, " +
                                                         "discount=@discount, " +
                                                         "total=@total, " +
                                                         "discount=@discount, " +
                                                         "expenses=@expenses, " +
                                                         "profit=@profit " +
                                                   "WHERE id=@id";

                cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                cmd.Parameters.AddWithValue("@discount", item.Discount);
                cmd.Parameters.AddWithValue("@total", item.Total);
                cmd.Parameters.AddWithValue("@expenses", item.Expenses);
                cmd.Parameters.AddWithValue("@profit", item.Profit);
                cmd.Parameters.AddWithValue("@id", item.Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                return item;
            }
        }
    }
}

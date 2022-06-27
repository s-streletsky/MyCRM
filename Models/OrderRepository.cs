using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class OrderRepository : IRepository<Order>
    {
        public Order Add(Order order)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "INSERT INTO Orders (date, client_id, status_id, notes) VALUES "
                                + $"(@date, @client_id, @status, '')";
                cmd.Parameters.AddWithValue("@date", order.Date);
                cmd.Parameters.AddWithValue("@client_id", order.Client.Id);
                cmd.Parameters.AddWithValue("@status", (int)order.Status);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT id FROM Orders WHERE date=@date";
                cmd.Parameters.AddWithValue("@date", order.Date);
                cmd.Prepare();
                order.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return order;
            }
        }

        public void Delete(Order order)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "DELETE FROM Orders WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", order.Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }

        public Order Get(Order item)
        {
            throw new NotImplementedException();
        }

        public void GetAll (ObservableCollection<Order> dbOrders, IEnumerable<Client> dbClients)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText =
                    @"SELECT Orders.id, date, client_id, status_id, IFNULL(SUM(OrdersItems.total), 0) AS total, IFNULL(SUM(OrdersItems.expenses), 0) AS expenses, IFNULL(SUM(OrdersItems.profit), 0) AS profit, notes
                    FROM Orders
                    LEFT JOIN OrdersItems ON OrdersItems.order_id = Orders.id
                    GROUP BY Orders.id
                    ORDER BY Orders.id DESC";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var id = sqlReader.GetInt32(0);
                    var date = DateTime.Parse(sqlReader.GetString(1));
                    var client = dbClients.First(x => x.Id == sqlReader.GetInt32(2));
                    var status = (OrderStatus)sqlReader.GetInt32(3);
                    var total = sqlReader.GetFloat(4);
                    var expenses = sqlReader.GetFloat(5);
                    var profit = sqlReader.GetFloat(6);
                    var notes = sqlReader.GetString(7);
                    

                    dbOrders.Add(new Order(id, date, client, status, total, expenses, profit, notes));
                }
            }
        }

        public bool TryDelete(Order item)
        {
            throw new NotImplementedException();
        }

        public Order Update(Order order)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = @"UPDATE Orders 
                    SET client_id = @client_id,
                    status_id = @status_id,
                    notes = @notes
                    WHERE id = @id";

                cmd.Parameters.AddWithValue("@client_id", order.Client.Id);
                cmd.Parameters.AddWithValue("@status_id", (int)order.Status);
                cmd.Parameters.AddWithValue("@notes", order.Notes);
                cmd.Parameters.AddWithValue("@id", order.Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                return order;
            }
        }
    }
}

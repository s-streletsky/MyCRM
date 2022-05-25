using System;
using System.Collections.Generic;
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
                string addOrder = "INSERT INTO Orders (created, client_id, status_id, total, notes) VALUES "
                                + $"('{order.Created}', {order.Client.Id}, {(int)order.Status}, {0}, '{""}')";

                cmd.CommandText = addOrder;
                cmd.ExecuteNonQuery();

                string getOrderId = "SELECT id FROM Orders WHERE created=" + $"'{order.Created}'";
                cmd.CommandText = getOrderId;
                order.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return order;
            }
        }

        public void Delete(Order item)
        {
            throw new NotImplementedException();
        }

        public Order Get(Order item)
        {
            throw new NotImplementedException();
        }

        public void GetAll (Database db)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "SELECT * FROM Orders";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var id = sqlReader.GetInt32(0);
                    var created = DateTime.Parse(sqlReader.GetString(1));
                    var client = db.Clients.First(x => x.Id == sqlReader.GetInt32(2));
                    var status = (OrderStatus)sqlReader.GetInt32(3);
                    var total = sqlReader.GetFloat(4);
                    var notes = sqlReader.GetString(5);

                    db.Orders.Add(new Order(id, created, client, status, total, notes));
                }
            }
        }

        public bool TryDelete(Order item)
        {
            throw new NotImplementedException();
        }

        public Order Update(Order item)
        {
            throw new NotImplementedException();
        }
    }
}

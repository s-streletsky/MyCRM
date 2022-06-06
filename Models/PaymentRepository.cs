using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class PaymentRepository : IRepository<Payment>
    {
        public Payment Add(Payment payment)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "INSERT INTO Payments (date, client_id, order_id, amount, notes) VALUES "
                                + $"(@date, @client_id, @order_id, @amount, @notes)";
                cmd.Parameters.AddWithValue("@date", payment.Date);
                cmd.Parameters.AddWithValue("@client_id", payment.Client.Id);
                cmd.Parameters.AddWithValue("@order_id", payment.Order.Id);
                cmd.Parameters.AddWithValue("@amount", payment.Amount);
                cmd.Parameters.AddWithValue("@notes", payment.Notes);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT id FROM Payments WHERE id = (SELECT MAX(id) FROM Payments)";
                payment.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return payment;
            }
        }

        public void Delete(Payment item)
        {
            throw new NotImplementedException();
        }

        public Payment Get(Payment item)
        {
            throw new NotImplementedException();
        }

        public void GetOrderPayments(Database db, List<Payment> list, Order order)
        {
            if (order != null)
            {
                using (var cmd = DbConnection.Open())
                {
                    cmd.CommandText = "SELECT * FROM Payments WHERE order_id=@id";
                    cmd.Parameters.AddWithValue("@id", order.Id);
                    cmd.Prepare();

                    SQLiteDataReader sqlReader = cmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        var id = sqlReader.GetInt32(0);
                        var date = DateTime.Parse(sqlReader.GetString(1));
                        var client = db.Clients.First(x => x.Id == sqlReader.GetInt32(2));
                        var ord = db.Orders.First(x => x.Id == sqlReader.GetInt32(3));
                        var amount = sqlReader.GetFloat(4);
                        var notes = sqlReader.GetString(5);

                        list.Add(new Payment(id, date, client, ord, amount, notes));
                    }
                }
            }           
        }
        public void GetAll(ObservableCollection<Payment> payments, ObservableCollection<Client> clients, ObservableCollection<Order> orders)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "SELECT * FROM Payments ORDER BY id DESC";
                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var id = sqlReader.GetInt32(0);
                    var date = DateTime.Parse(sqlReader.GetString(1));
                    var client = clients.First(x => x.Id == sqlReader.GetInt32(2));
                    var order = orders.First(x => x.Id == sqlReader.GetInt32(3));
                    var amount = sqlReader.GetFloat(4);
                    var notes = sqlReader.GetString(5);

                    payments.Add(new Payment(id, date,client, order,amount, notes));
                }
            }
        }

        public bool TryDelete(Payment item)
        {
            throw new NotImplementedException();
        }

        public Payment Update(Payment item)
        {
            throw new NotImplementedException();
        }
    }
}

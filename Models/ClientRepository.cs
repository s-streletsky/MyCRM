using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace CRM.Models
{
    internal class ClientRepository : IRepository<Client>
    {
        public Client Add(Client client)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = @"INSERT INTO Clients (date, name, nickname, phone, email, country, city, address, shipping_method_id, postal_code, notes)
                    VALUES (@date, @name, @nickname, @phone, @email, @country, @city, @address, @shipping_method, @postal_code, @notes)";                                     
                cmd.Parameters.AddWithValue("@date", client.Date);
                cmd.Parameters.AddWithValue("@name", client.Name);
                cmd.Parameters.AddWithValue("@nickname", client.Nickname);
                cmd.Parameters.AddWithValue("@phone", client.Phone);
                cmd.Parameters.AddWithValue("@email", client.Email);
                cmd.Parameters.AddWithValue("@country", (int)client.Country);
                cmd.Parameters.AddWithValue("@city", client.City);
                cmd.Parameters.AddWithValue("@address", client.Address);
                cmd.Parameters.AddWithValue("@shipping_method", (int)client.ShippingMethod);
                cmd.Parameters.AddWithValue("@postal_code", client.PostalCode);
                cmd.Parameters.AddWithValue("@notes", client.Notes);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT id FROM Clients WHERE date=@date";
                cmd.Parameters.AddWithValue("@date", client.Date);
                cmd.Prepare();
                client.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return client;
            }
        }
        public void Delete(Client client)
        {
            using (var cmd = DbConnection.Open())
            {
                string deleteClient = "DELETE FROM Clients WHERE id=" + $"{client.Id}";

                cmd.CommandText = deleteClient;
                cmd.ExecuteNonQuery();
            }
        }
        public Client Get(Client client)
        {
            throw new NotImplementedException();
        }
        public float GetBalance(Client client)
        {
            using (var cmd = DbConnection.Open())
            {
                //cmd.CommandText = @"SELECT (SUM(oi.total)-p.paid) balance
                //    FROM OrdersItems oi
                //    INNER JOIN Orders o
                //    ON o.id = oi.order_id
                //    INNER JOIN(
                //        SELECT client_id, SUM(amount) AS paid
                //        FROM Payments
                //        GROUP BY client_id
                //        ) p
                //    ON p.client_id = o.client_id
                //    WHERE o.client_id = @id
                //    GROUP BY o.client_id";
                //cmd.Parameters.AddWithValue("@id", client.Id);
                //cmd.Prepare();

                cmd.CommandText = @"SELECT IFNULL(SUM(oi.total), 0)
                    FROM Orders o
                    LEFT JOIN OrdersItems oi
                    ON oi.order_id = o.id
                    WHERE o.client_id = @id
                    GROUP BY o.client_id";
                cmd.Parameters.AddWithValue("@id", client.Id);
                cmd.Prepare();
                var ordersItemsTotal = Convert.ToSingle(cmd.ExecuteScalar());

                cmd.CommandText = @"SELECT IFNULL(SUM(amount), 0)
                    FROM Payments
                    WHERE client_id = @id";
                cmd.Parameters.AddWithValue("@id", client.Id);
                cmd.Prepare();
                var paymentsTotal = Convert.ToSingle(cmd.ExecuteScalar());

                float balance = paymentsTotal - ordersItemsTotal;

                return balance;
            }
        }
        public void GetAll(ObservableCollection<Client> dbClients)
        {
            using (var cmd = DbConnection.Open())    
            {
                cmd.CommandText = "SELECT * FROM Clients ORDER BY id DESC";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var id = sqlReader.GetInt32(0);
                    var date = DateTime.Parse(sqlReader.GetString(1));
                    var name = sqlReader.GetString(2);
                    var nickname = sqlReader.GetString(3);
                    var phone = sqlReader.GetString(4);
                    var email = sqlReader.GetString(5);
                    var country = (Country)sqlReader.GetInt32(6);
                    var city = sqlReader.GetString(7);
                    var address = sqlReader.GetString(8);
                    var shippingMethod = (ShippingMethod)sqlReader.GetInt32(9);
                    var postalCode = sqlReader.GetString(10);
                    var notes = sqlReader.GetString(11);

                    dbClients.Add(new Client(id, date, name, nickname, phone, email, country, city, address, shippingMethod, postalCode, notes));
                }
            }
        }
        public bool TryDelete(Client client)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "SELECT 1 FROM Orders WHERE client_id=" + $"{client.Id} LIMIT 1";
                var result = cmd.ExecuteScalar();

                if (result != null) return false;
                else return true;
            }
        }
        public Client Update(Client client)
        {
            using (var cmd = DbConnection.Open())
            {
                string updateClient = "UPDATE Clients " +
                    "SET name=" + $"'{client.Name}', " +
                    "nickname=" + $"'{client.Nickname}', " +
                    "phone=" + $"'{client.Phone}', " +
                    "email=" + $"'{client.Email}', " +
                    "country=" + $"{(int)client.Country}, " +
                    "city=" + $"'{client.City}', " +
                    "address=" + $"'{client.Address}', " +
                    "shipping_method_id=" + $"{(int)client.ShippingMethod}, " +
                    "postal_code=" + $"'{client.PostalCode}', " +
                    "notes=" + $"'{client.Notes}' " +
                    "WHERE id=" + $"'{client.Id}'";
                                                        
                cmd.CommandText = updateClient;
                cmd.ExecuteNonQuery();

                return client;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class ClientRepository : IRepository<Client>
    {
        public Client Add(Client item)
        {
            var client = item;

            using (var cmd = DbConnection.Open())
            {
                string addNewClient = "INSERT INTO clients (created, name, nickname, phone, email, country, city, address, shipping_method_id, postal_code, notes) VALUES "
                                     + $"('{client.Created}', '{client.Name}', '{client.Nickname}', '{client.Phone}', '{client.Email}', {(int)client.Country}, '{client.City}', '{client.Address}', {(int)client.ShippingMethod}, '{client.PostalCode}', '{client.Notes}')";

                cmd.CommandText = addNewClient;
                cmd.ExecuteNonQuery();

                string getClientId = "SELECT id FROM clients WHERE created=" + $"'{client.Created}'";
                cmd.CommandText = getClientId;
                client.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return client;
            }
        }

        public void Delete(Client item)
        {
            var client = item;

            using (var cmd = DbConnection.Open())
            {
                string deleteClient = "DELETE FROM clients WHERE id=" + $"{client.Id}";

                cmd.CommandText = deleteClient;
                cmd.ExecuteNonQuery();
            }
        }

        public Client Get(Client item)
        {
            throw new NotImplementedException();
        }

        public void GetAll(Database db)
        {
            using (var cmd = DbConnection.Open())    
            {
                cmd.CommandText = "SELECT * FROM clients";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var id = sqlReader.GetInt32(0);
                    var created = DateTime.Parse(sqlReader.GetString(1));
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

                    db.Clients.Add(new Client(id, created, name, nickname, phone, email, country, city, address, shippingMethod, postalCode, notes));
                }
            }
        }

        public Client Update(Client item)
        {
            var client = item;

            using (var cmd = DbConnection.Open())
            {
                string updateClient = "UPDATE clients SET name=" + $"'{client.Name}'," +
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

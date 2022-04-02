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
        private const string connectionString = @"Data Source=C:\SQLiteStudio\crm_db;Version=3;";

        public Client Add(Client item)
        {
            var client = item;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var cmd = new SQLiteCommand(connection);

                string addNewClient = "INSERT INTO clients (created, name, nickname, phone, email, country, city, address, shipping_method_id, postal_code, notes) VALUES "
                                     + $"('{client.Created}', '{client.Name}', '{client.Nickname}', '{client.Phone}', '{client.Email}', {(int)client.Country}, '{client.City}', '{client.Address}', {(int)client.ShippingMethod}, '{client.PostalCode}', '{client.Notes}')";

                cmd.CommandText = addNewClient;
                cmd.ExecuteNonQuery();

                string getClientId = "SELECT id FROM clients WHERE created=" + $"'{client.Created}'";
                cmd.CommandText = getClientId;
                client.Id = Convert.ToInt32(cmd.ExecuteScalar());
                //client.Id = (int)cmd.ExecuteScalar();

                return client;
            }
        }

        public void Delete(Client item)
        {
            var client = item;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var cmd = new SQLiteCommand(connection);

                string deleteClient = "DELETE FROM clients WHERE id=" + $"{client.Id}";

                cmd.CommandText = deleteClient;
                cmd.ExecuteNonQuery();
            }
        }

        public Client Get(Client item)
        {
            throw new NotImplementedException();
        }

        public Client Update(Client item)
        {
            var client = item;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var cmd = new SQLiteCommand(connection);

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

                //string getClientId = "SELECT id FROM clients WHERE created=" + $"'{client.Created}'";
                //cmd.CommandText = getClientId;
                //client.Id = Convert.ToInt32(cmd.ExecuteScalar());
                //client.Id = (int)cmd.ExecuteScalar();

                return client;
            }
        }
    }
}

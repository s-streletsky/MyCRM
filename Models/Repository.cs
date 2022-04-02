using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.SQLite;

namespace CRM.Models
{
    //abstract class Repo<T>
    //{
    //    public Repo()
    //    {
    //        //CRUD
    //        //CREATE
    //        //UPDATE
    //        //DELETE
    //    }

    //    public abstract T Get(int id);
    //    public abstract IEnumerable<T> GetAll();
    //    public abstract T Add(T item);
    //    public abstract int Add(T item);
    //    public abstract T Update(T item);
    //    public abstract void Delete(T item);
    //}

    //class ClientRepo : IRepository<Client>
    //{
    //    public Client Add(Client id)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    class Repository
    {
        private const string DbPath = "D:\\Prog\\CRM\\Db.txt";
        private const string connectionString = @"Data Source=C:\SQLiteStudio\crm_db;Version=3;";
        private JsonSerializer serializer;

        public Repository()
        {
            this.serializer = new JsonSerializer();
        }

        public async void Save(Database db)
        {
            using (StreamWriter sw = new StreamWriter(DbPath, true))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, db);
            }
        }

        public void SaveClient(Client client)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var cmd = new SQLiteCommand(connection);

                string commandText = "INSERT INTO clients (created, name, nickname, phone, email, country, city, address, shipping_method_id, postal_code, notes) VALUES " 
                                                + $"('{client.Created}', '{client.Name}', '{client.Nickname}', '{client.Phone}', '{client.Email}', {(int)client.Country}, '{client.City}', '{client.Address}', {(int)client.ShippingMethod}, '{client.PostalCode}', '{client.Notes}')";

                cmd.CommandText = commandText;                   

                cmd.ExecuteNonQuery();

                // prepared statemen

                // UPSERT
                // var id = db.insert
                // client.id = id;

                //while (sqlReader.Read())
                //{
                //    var id = sqlReader.GetInt32(0);
                //    var created = sqlReader.GetString(1);
                //    var name = sqlReader.GetString(2);
                //    var nickname = sqlReader.GetString(3);
                //    var phone = sqlReader.GetString(4);
                //    var email = sqlReader.GetString(5);
                //    var country = (Country)sqlReader.GetInt32(6);
                //    var city = sqlReader.GetString(7);
                //    var address = sqlReader.GetString(8);
                //    var shippingMethod = (ShippingMethod)sqlReader.GetInt32(9);
                //    var postalCode = sqlReader.GetString(10);
                //    var notes = sqlReader.GetString(11);

                //    db.Clients.Add(new Client(id, name, nickname, phone, email, country, city, address, shippingMethod, postalCode));
                //}
            }
        }

        //public Database Load()
        //{
        //    Database db = null;

        //    using (StreamReader file = File.OpenText(DbPath))
        //    {
        //        JsonSerializer serializer = new JsonSerializer();
        //        db = (Database)serializer.Deserialize(file, typeof(Database));
        //    }

        //    return db;            
        //}

        public Database Load()
        {
            Database db = new Database();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var cmd = new SQLiteCommand(connection);

                cmd.CommandText = @"SELECT * 
                                    FROM clients";

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

            return db;
        }

        //public bool TryLoad(string filePath, out List<Country> loadedList)
        //{
        //    loadedList = null;

        //    if (!File.Exists(filePath))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            loadedList = Load(filePath);
        //        }
        //        catch (Exception ex)
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}
        
        //public List<Country> Load(string filePath)
        //{
        //    using (FileStream fs = new FileStream(filePath, FileMode.Open))
        //    {
        //        return (List<Country>)serializer.Deserialize(fs);
        //    }
        //}

        //public void Update()
        //{
        //}
    }
}

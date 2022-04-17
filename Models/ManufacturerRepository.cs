﻿using System;
using System.Data.SQLite;

namespace CRM.Models
{
    internal class ManufacturerRepository : IRepository<Manufacturer>
    {
        public Manufacturer Add(Manufacturer item)
        {
            var manufacturer = item;

            using (var cmd = DbConnection.Open())
            {
                string addNewManufacturer = "INSERT INTO manufacturers (name) VALUES " + $"('{manufacturer.Name}')";

                cmd.CommandText = addNewManufacturer;
                cmd.ExecuteNonQuery();

                string getManufacturerId = "SELECT id FROM manufacturers WHERE name=" + $"'{manufacturer.Name}'";
                cmd.CommandText = getManufacturerId;
                manufacturer.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return manufacturer;
            }
        }

        public void Delete(Manufacturer item)
        {
            throw new NotImplementedException();
        }

        public Manufacturer Get(Manufacturer item)
        {
            throw new NotImplementedException();
        }

        public void GetAll(Database db)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "SELECT * FROM manufacturers";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var id = sqlReader.GetInt32(0);
                    var name = sqlReader.GetString(1);

                    db.Manufacturers.Add(new Manufacturer(id, name));
                }
            }
        }

        public Manufacturer Update(Manufacturer item)
        {
            throw new NotImplementedException();
        }
    }
}
﻿using System;
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
            throw new NotImplementedException();
        }

        public void Delete(OrderItem item)
        {
            throw new NotImplementedException();
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

        public OrderItem Update(OrderItem item)
        {
            throw new NotImplementedException();
        }
    }
}

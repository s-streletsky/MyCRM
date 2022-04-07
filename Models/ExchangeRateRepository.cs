using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class ExchangeRateRepository : IRepository<ExchangeRate>
    {
        private const string connectionString = @"Data Source=C:\SQLiteStudio\crm_db;Version=3;";

        public ExchangeRate Add(ExchangeRate item)
        {
            var newExRate = item;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var cmd = new SQLiteCommand(connection);

                string addNewExRate = "INSERT INTO exchange_rates (date, currency_id, exchange_rate) VALUES "
                                     + $"('{newExRate.Date}', '{(int)newExRate.Currency}', '{newExRate.Rate}')";

                cmd.CommandText = addNewExRate;
                cmd.ExecuteNonQuery();

                //string getClientId = "SELECT id FROM clients WHERE created=" + $"'{newExRate.Created}'";
                //cmd.CommandText = getClientId;
                //newExRate.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return newExRate;
            }
        }

        public void Delete(ExchangeRate item)
        {
            throw new NotImplementedException();
        }

        public ExchangeRate Get(ExchangeRate item)
        {
            throw new NotImplementedException();
        }
        public void GetAll(Database db)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var cmd = new SQLiteCommand(connection);

                cmd.CommandText = @"SELECT * FROM exchange_rates ORDER BY date DESC";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var date = DateTime.Parse(sqlReader.GetString(0));
                    var currency = (Currency)sqlReader.GetInt32(1);
                    var rate = sqlReader.GetDecimal(2);

                    db.ExchangeRates.Add(new ExchangeRate(date, currency, rate));
                }
            }
        }

        public ExchangeRate Update(ExchangeRate item)
        {
            throw new NotImplementedException();
        }
    }
}

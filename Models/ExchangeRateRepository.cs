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
        public ExchangeRate Add(ExchangeRate rate)
        {
            using (var cmd = DbConnection.Open())
            {
                string addNewExRate = "INSERT INTO exchange_rates (date, currency_id, value) VALUES "
                                     + $"('{rate.Date}', {(int)rate.Currency}, {rate.Value})";

                cmd.CommandText = addNewExRate;
                cmd.ExecuteNonQuery();

                return rate;
            }
        }

        public void Delete(ExchangeRate rate)
        {
            using (var cmd = DbConnection.Open())
            {
                string deleteRate = "DELETE FROM exchange_rates WHERE date=" + $"'{rate.Date}'";

                cmd.CommandText = deleteRate;
                cmd.ExecuteNonQuery();
            }
        }

        public ExchangeRate Get(ExchangeRate rate)
        {
            throw new NotImplementedException();
        }
        public void GetAll(Database db)
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText = "SELECT * FROM exchange_rates ORDER BY date DESC";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var date = DateTime.Parse(sqlReader.GetString(0));
                    var currency = (Currency)sqlReader.GetInt32(1);
                    var value = sqlReader.GetFloat(2);

                    db.ExchangeRates.Add(new ExchangeRate(date, currency, value));
                }
            }
        }

        public ExchangeRate Update(ExchangeRate rate)
        {
            throw new NotImplementedException();
        }
    }
}

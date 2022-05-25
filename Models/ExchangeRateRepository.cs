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
                string addNewExRate = "INSERT INTO ExchangeRates (date, currency_id, value) VALUES "
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
                string deleteRate = "DELETE FROM ExchangeRates WHERE date=" + $"'{rate.Date}'";

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
                cmd.CommandText = "SELECT * FROM ExchangeRates ORDER BY id DESC";

                SQLiteDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    var id = sqlReader.GetInt32(0);
                    var date = DateTime.Parse(sqlReader.GetString(1));
                    var currency = (Currency)sqlReader.GetInt32(2);
                    var value = sqlReader.GetFloat(3);

                    db.ExchangeRates.Add(new ExchangeRate(id, date, currency, value));
                }
            }
        }

        public bool TryDelete(ExchangeRate item)
        {
            throw new NotImplementedException();
        }

        public ExchangeRate Update(ExchangeRate rate)
        {
            throw new NotImplementedException();
        }
    }
}

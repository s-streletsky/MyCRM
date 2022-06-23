using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CRM.Models
{

    internal class Database
    {
        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
        public ObservableCollection<OrderItem> OrdersItems { get; set; } = new ObservableCollection<OrderItem>();
        public ObservableCollection<StockItem> StockItems { get; set; } = new ObservableCollection<StockItem>();
        public ObservableCollection<StockArrival> StockArrivals { get; set; } = new ObservableCollection<StockArrival>();
        public ObservableCollection<ExchangeRate> ExchangeRates { get; set; } = new ObservableCollection<ExchangeRate>();
        public ObservableCollection<Manufacturer> Manufacturers { get; set; } = new ObservableCollection<Manufacturer>();
        public ObservableCollection<Payment> Payments { get; set; } = new ObservableCollection<Payment>();

        internal void CreateEmptyDatabase()
        {
            using (var cmd = DbConnection.Open())
            {
                cmd.CommandText =
                    @"PRAGMA foreign_keys = off;
                    BEGIN TRANSACTION;

                    -- Table: Clients
                    CREATE TABLE Clients (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, date TEXT NOT NULL, name TEXT NOT NULL, nickname TEXT, phone TEXT, email TEXT, country INTEGER, city TEXT, address TEXT, shipping_method_id INTEGER REFERENCES ShippingMethods (id), postal_code TEXT, notes TEXT);                    

                    -- Table: Currencies
                    CREATE TABLE Currencies (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, code TEXT);
                    INSERT INTO Currencies (id, code) VALUES (1, 'EUR');
                    INSERT INTO Currencies (id, code) VALUES (2, 'USD');
                    INSERT INTO Currencies (id, code) VALUES (3, 'UAH');

                    -- Table: ExchangeRates
                    CREATE TABLE ExchangeRates (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, date TEXT, currency_id INTEGER REFERENCES currencies (id) NOT NULL, value REAL);
                    INSERT INTO ExchangeRates (id, date, currency_id, value) VALUES (1, @current_date, 3, 1.0);

                    -- Table: Manufacturers
                    CREATE TABLE Manufacturers (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL);                    

                    -- Table: Orders
                    CREATE TABLE Orders (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, date TEXT NOT NULL, client_id INTEGER REFERENCES clients (id), status_id INTEGER REFERENCES OrderStatuses (id), notes TEXT);

                    -- Table: OrdersItems
                    CREATE TABLE OrdersItems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, order_id INTEGER REFERENCES orders (id) NOT NULL, stock_item_id INTEGER REFERENCES StockItems (id) NOT NULL, quantity REAL, price REAL, discount REAL, total REAL, profit REAL, expenses REAL, exchange_rate REAL);

                    -- Table: OrderStatuses
                    CREATE TABLE OrderStatuses (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL);
                    INSERT INTO OrderStatuses (id, name) VALUES (1, 'Готов');
                    INSERT INTO OrderStatuses (id, name) VALUES (2, 'К отправке');
                    INSERT INTO OrderStatuses (id, name) VALUES (3, 'Оплачен полностью');
                    INSERT INTO OrderStatuses (id, name) VALUES (4, 'НОВЫЙ');
                    INSERT INTO OrderStatuses (id, name) VALUES (5, 'Выставлен счёт');
                    INSERT INTO OrderStatuses (id, name) VALUES (6, 'Оплачен частично');
                    INSERT INTO OrderStatuses (id, name) VALUES (7, 'Отправлен');

                    -- Table: Payments
                    CREATE TABLE Payments (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, date TEXT, client_id INTEGER REFERENCES clients (id) NOT NULL, order_id INTEGER REFERENCES Orders (id) NOT NULL, amount REAL NOT NULL, notes TEXT);

                    -- Table: ShippingMethods
                    CREATE TABLE ShippingMethods (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL);
                    INSERT INTO ShippingMethods (id, name) VALUES (0, '-- не указан --');
                    INSERT INTO ShippingMethods (id, name) VALUES (1, 'Новая почта');
                    INSERT INTO ShippingMethods (id, name) VALUES (2, 'Укрпочта');
                    INSERT INTO ShippingMethods (id, name) VALUES (3, 'Самовывоз');

                    -- Table: StockArrivals
                    CREATE TABLE StockArrivals (id INTEGER PRIMARY KEY NOT NULL, date TEXT, stock_item_id INTEGER REFERENCES StockItems (id), quantity REAL);

                    -- Table: StockItems
                    CREATE TABLE StockItems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL, manufacturer_id INTEGER REFERENCES manufacturers (id), description TEXT, currency_id INTEGER REFERENCES currencies (id), purchase_price REAL NOT NULL, retail_price REAL NOT NULL, quantity REAL NOT NULL);

                    COMMIT TRANSACTION;
                    PRAGMA foreign_keys = on;";

                cmd.Parameters.AddWithValue("@current_date", DateTime.Now);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

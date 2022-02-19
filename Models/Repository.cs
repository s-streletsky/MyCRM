using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CRM.Models
{
    class Repository
    {
        const string DbPath = "D:\\Prog\\CRM\\Db.txt";
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

        public Database Load()
        {
            Database db = null;

            using (StreamReader file = File.OpenText(DbPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                db = (Database)serializer.Deserialize(file, typeof(Database));
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

using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TaskProject.Models;

namespace TaskProject.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        List<Item> items;

        public MockDataStore()
        {
            object locker = new object(); // class level private field
                                          // rest of class code
            string dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "database.db3");
			Console.WriteLine("beep" + dbPath);
            // dbPath contains a valid file path for the database file to be stored
            lock (locker)
            {
                var db = new SQLiteConnection(dbPath);

                db.CreateTable<Item>();
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            object locker = new object(); // class level private field
                                          // rest of class code
            string dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "database.db3");
            // dbPath contains a valid file path for the database file to be stored
            lock (locker)
            {
                var db = new SQLiteConnection(dbPath);

                db.CreateTable<Item>();

                Item oldItem = new Item();

                oldItem = db.FindWithQuery<Item>("SELECT * FROM Items WHERE id= ?",id);

                db.Delete(oldItem);

                
            }

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
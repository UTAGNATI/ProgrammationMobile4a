using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Models;
using TaskProject.Views;
using Xamarin.Forms;

namespace TaskProject.ViewModels
{
    class CompletedViewModel : BaseViewModel
    {
		public ObservableCollection<Item> CompletedItems { get; set; }
		public Command LoadItemsCommand { get; set; }

		public CompletedViewModel()
		{
			Title = "Completed";
			CompletedItems = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());


            MessagingCenter.Subscribe<ItemsPage, Item>(this, "DoneItem", async (obj, item) =>
            {
                var newItem = item as Item; 
				CompletedItems.Add(newItem);

                //await DataStore.DeleteItemAsync(1);
            });

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
                
                Console.WriteLine("Reading data from CompletedViewModel\n");
                var table = db.Table<Item>();
                foreach (var s in table)
                {
					if (s.Completed == true) {
						CompletedItems.Add(s);
					} 
                }
            }
		}

		public async Task ExecuteDeleteCommand(Item item)
        {
            await DataStore.DeleteItemAsync(item.Id);

            CompletedItems.Remove(item);

        }

		async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                CompletedItems.Clear();

                object locker = new object(); // class level private field
                                              // rest of class code
                string dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    "database.db3");
                // dbPath contains a valid file path for the database file to be stored
                lock (locker)
                {
                    var db = new SQLiteConnection(dbPath);

                    db.CreateTable<Item>(); //checks if database exists and cretaes a new one if it doesn't

                    Console.WriteLine("Reading data from ExecuteLoadItemCommand in itemsviewmodel\n");
                    var table = db.Table<Item>();
                    foreach (var s in table)
                    {
                        if (s.Completed == true) {
							CompletedItems.Add(s);	//add all not completed items in the data base to the observableCollection
						} 
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
	}
}

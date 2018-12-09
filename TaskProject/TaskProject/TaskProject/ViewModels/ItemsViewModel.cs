using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using TaskProject.Models;
using TaskProject.Views;
using System.IO;
using SQLite;

namespace TaskProject.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }


        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());


            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item; 
				Items.Add(newItem);

                //await DataStore.DeleteItemAsync(1);
            });

			MessagingCenter.Subscribe<ItemsPage, Item>(this, "DoneItem", async (obj, item) =>
            {
				Console.WriteLine("avant remove");
                var newItem = item as Item; 
				Items.Remove(newItem);
				Console.WriteLine("apres remove");

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
                
                Console.WriteLine("Reading data from ItemsViewModel\n");
                var table = db.Table<Item>();
                foreach (var s in table)
                {
					if (s.Completed == false) {
						Items.Add(s);
					} 
                }
            }
        }

        public async Task ExecuteDeleteCommand(Item item)
        {
            await DataStore.DeleteItemAsync(item.Id);

            Items.Remove(item);

        }


        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();

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
                        if (s.Completed == false) {
							Items.Add(s);	//add all not completed items in the data base to the observableCollection
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


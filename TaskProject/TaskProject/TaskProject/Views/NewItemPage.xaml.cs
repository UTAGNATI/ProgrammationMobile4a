using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TaskProject.Models;
using System.IO;
using SQLite;
using System.Diagnostics;

namespace TaskProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            object locker = new object(); // class level private field
                                          // rest of class code
            string dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "database.db3");
			
            lock (locker)
            {
                var db = new SQLiteConnection(dbPath);
                var newItem = new Item();

                db.CreateTable<Item>();

                newItem.Text = this.Item.Text;
                newItem.Company = this.Item.Company;
                newItem.Description = this.Item.Description;
                newItem.DeadLine = this.Item.DeadLine;
                newItem.NbOfParticipants = this.Item.NbOfParticipants;
                newItem.Participants = this.Item.Participants;
                newItem.Priority = this.Item.Priority;
				newItem.Completed = false;

                db.Insert(newItem); //insert a new element in the table Items
				MessagingCenter.Send(this, "AddItem", newItem);

                Debug.WriteLine("This item has been added : " + newItem.Text + " " + newItem.Description + " " + newItem.Completed);
            }
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
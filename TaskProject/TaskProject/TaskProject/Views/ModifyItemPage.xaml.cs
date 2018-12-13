using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TaskProject.Models;
using System.IO;
using SQLite;
using System.Diagnostics;
using TaskProject.ViewModels;

namespace TaskProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyItemPage : ContentPage
    {
        public Item Item { get; set; }
        ItemDetailViewModel viewModel;

        public ModifyItemPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
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
                Console.WriteLine("error1");
                var newItem = db.FindWithQuery<Item>("SELECT * FROM Items WHERE id= ?", this.viewModel.Item.Id);

                this.Item = this.viewModel.Item;
                
                db.CreateTable<Item>();

                newItem.Text = this.viewModel.Item.Text;
                newItem.Company = this.Item.Company;
                newItem.Description = this.Item.Description;
                newItem.StartingDate = this.Item.StartingDate;
                newItem.DeadLine = this.Item.DeadLine;
                newItem.NbOfParticipants = this.Item.NbOfParticipants;
                newItem.Participants = this.Item.Participants;
                newItem.Priority = this.Item.Priority;

                db.Update(newItem); //Update an element in the table Items
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
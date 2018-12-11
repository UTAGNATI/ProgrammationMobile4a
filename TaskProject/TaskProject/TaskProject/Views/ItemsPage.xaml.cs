using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TaskProject.Models;
using TaskProject.Views;
using TaskProject.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using TaskProject.Services;

namespace TaskProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

			if (viewModel.Items.Count == 0) {
				//viewModel.LoadItemsCommand.Execute(null);
			}
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            var item = sender as Button;
            var selectedItem = item.CommandParameter as Item;

            if (selectedItem == null)
                return;

			Console.WriteLine("item to validate: " + selectedItem.Id + "\n");

			
            object locker = new object(); // class level private field
                                          // rest of class code
            string dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "database.db3");
			
            lock (locker)
            {
                var db = new SQLiteConnection(dbPath);

				selectedItem.Completed = true;

				db.Update(selectedItem);
				MessagingCenter.Send(this, "DoneItem", selectedItem);

                Debug.WriteLine("This item has been completed : " + selectedItem.Text + " " + selectedItem.Description + " " + selectedItem.Completed);
            }

        
            ItemsListView.SelectedItem = null;
        }
    }
}
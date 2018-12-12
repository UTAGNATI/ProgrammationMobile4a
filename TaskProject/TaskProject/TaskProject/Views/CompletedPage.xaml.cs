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
    public partial class CompletedPage : ContentPage
    {
		CompletedViewModel viewModel;
        
        public CompletedPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CompletedViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            CompletedListView.SelectedItem = null;
        }

		protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.CompletedItems.Count == 0)
            {
                //ne rien faire
            }
            else {
                viewModel.LoadItemsCommand.Execute(null);
            }
                
        }

		async void Button_Clicked(object sender, EventArgs e)
        {
            var item = sender as Button;
            var selectedItem = item.CommandParameter as Item;

            if (selectedItem == null)
                return;

			Console.WriteLine("item to delete: " + selectedItem.Id + "\n");

			await viewModel.ExecuteDeleteCommand(selectedItem);
		
        }
    }
}

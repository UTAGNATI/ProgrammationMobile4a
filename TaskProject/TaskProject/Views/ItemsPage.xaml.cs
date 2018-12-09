using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TaskProject.Models;
using TaskProject.Views;
using TaskProject.ViewModels;
using TaskProject.Storage;
using Microsoft.EntityFrameworkCore;

namespace TaskProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        public static List<Item> Items { get; set; }
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

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            var item = sender as Button;
            var selectedItem = item.CommandParameter as Item;

            if (selectedItem == null)
                return;

            DisplayAlert(selectedItem.Text, selectedItem.Description, "hello");

            using (AppDbContext context = new AppDbContext())
            {
                Items = await context.Items.Where(x => x.Text.StartsWith("Fi")).OrderBy(x => x.Text).ToListAsync();
                Console.WriteLine(Items);
            }


                // Manually deselect item.
                ItemsListView.SelectedItem = null;
        }
    }
}
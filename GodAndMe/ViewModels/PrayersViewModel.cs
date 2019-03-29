using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GodAndMe.Models;
using GodAndMe.Views;
using Xamarin.Forms;

namespace GodAndMe.ViewModels
{
    public class PrayersViewModel : BaseViewModel
    {
        public ObservableCollection<Prayers> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public PrayersViewModel()
        {
            Items = new ObservableCollection<Prayers>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<PrayersPageNew, Prayers>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Prayers;
                if (Items.Count > 0 && Items.Any(x => x.Id == item.Id))
                {
                    Prayers oldItem = Items.First(x => x.Id == item.Id);
                    Items.Remove(oldItem);
                    Items.Add(newItem);
                    await PrayersDataStore.UpdateItemAsync(newItem);
                }
                else
                {
                    if (!Items.Any(x => x.Id == newItem.Id))
                    {
                        Items.Add(newItem);
                    }
                    await PrayersDataStore.AddItemAsync(newItem);
                }
                Items.OrderBy((arg) => arg.Title);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<PrayersPageNew, Prayers>(this, "UpdateItem", async (obj, item) =>
            {
                await PrayersDataStore.UpdateItemAsync(item);
                Items.OrderBy((arg) => arg.Title);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<PrayersPage, Prayers>(this, "DeleteItem", async (obj, item) =>
            {
                if (Items.Any(x => x.Id == item.Id))
                {
                    await PrayersDataStore.DeleteItemAsync(item.Id);
                    Items.OrderBy((arg) => arg.Title);
                    await ExecuteLoadItemsCommand();
                }
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await PrayersDataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
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
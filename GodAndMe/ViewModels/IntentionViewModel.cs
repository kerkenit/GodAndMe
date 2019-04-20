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
    public class IntentionViewModel : BaseViewModel
    {
        public ObservableCollection<Intention> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public IntentionViewModel()
        {
            Items = new ObservableCollection<Intention>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<IntentionPageNew, Intention>(this, "AddItem", async (obj, item) =>
            {
                if (!string.IsNullOrWhiteSpace(item.Description) || !string.IsNullOrWhiteSpace(item.Person))
                {
                    if (Items.Count > 0 && Items.Any(x => x.Id == item.Id))
                    {
                        Intention oldItem = Items.First(x => x.Id == item.Id);
                        Items[Items.IndexOf(oldItem)] = item;
                        await IntentionDataStore.UpdateItemAsync(item);
                    }
                    else
                    {
                        if (!Items.Any(x => x.Id == item.Id))
                        {
                            Items.Add(item);
                        }
                        await IntentionDataStore.AddItemAsync(item);
                    }
                }
                if (string.IsNullOrWhiteSpace(item.Description) && string.IsNullOrWhiteSpace(item.Person) && item.Start == null)
                {
                    Items.Remove(item);
                    await IntentionDataStore.DeleteItemAsync(item.Id);
                }
                Items.OrderBy((arg) => arg.Completed ? DateTime.MinValue : arg.Start == null ? DateTime.Today : arg.Start);
                //await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<IntentionPage, Intention>(this, "UpdateItem", async (obj, item) =>
            {
                if (!string.IsNullOrWhiteSpace(item.Description) || !string.IsNullOrWhiteSpace(item.Person))
                {
                    if (Items.Count > 0 && Items.Any(x => x.Id == item.Id))
                    {
                        Intention oldItem = Items.First(x => x.Id == item.Id);
                        Items[Items.IndexOf(oldItem)] = item;
                    }
                    await IntentionDataStore.UpdateItemAsync(item);
                }
                if (string.IsNullOrWhiteSpace(item.Description) && string.IsNullOrWhiteSpace(item.Person) && item.Start == null)
                {
                    Items.Remove(item);
                    await IntentionDataStore.DeleteItemAsync(item.Id);
                }
                Items.OrderBy((arg) => arg.Completed ? DateTime.MinValue : arg.Start == null ? DateTime.Today : arg.Start);
                //await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<IntentionPage, Intention>(this, "DeleteItem", async (obj, item) =>
            {
                if (Items.Any(x => x.Id == item.Id))
                {
                    Items.Remove(item);
                    await IntentionDataStore.DeleteItemAsync(item.Id);
                    Items.OrderBy((arg) => arg.Completed ? DateTime.MinValue : arg.Start == null ? DateTime.Today : arg.Start);
                    //await ExecuteLoadItemsCommand();
                }
            });

            MessagingCenter.Subscribe<IntentionPageNew, Intention>(this, "GetItem", async (obj, item) =>
            {
                if (Items.Any((arg) => arg.Id == item.Id))
                {
                    Items.Remove(item);
                }
                Items.Add(await IntentionDataStore.GetItemAsync(item.Id));
                Items.OrderBy((arg) => arg.Completed ? DateTime.MinValue : arg.Start == null ? DateTime.Today : arg.Start);
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
                var items = await IntentionDataStore.GetItemsAsync(true);
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
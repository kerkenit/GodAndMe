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
        public Command LoadIntentionsCommand { get; set; }

        public IntentionViewModel()
        {
            Items = new ObservableCollection<Intention>();
            LoadIntentionsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<IntentionPageNew, Intention>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Intention;
                if (Items.Count > 0 && Items.Any(x => x.Id == item.Id))
                {
                    Intention oldItem = Items.First(x => x.Id == item.Id);
                    Items.Remove(oldItem);
                    Items.Add(newItem);
                    await IntentionDataStore.UpdateItemAsync(newItem);
                }
                else
                {
                    if (!Items.Any(x => x.Id == newItem.Id))
                    {
                        Items.Add(newItem);
                    }
                    await IntentionDataStore.AddItemAsync(newItem);
                }
                Items.OrderBy((arg) => arg.Completed ? DateTime.MinValue : arg.Start == null ? DateTime.Today : arg.Start);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<IntentionPage, Intention>(this, "UpdateItem", async (obj, item) =>
            {
                await IntentionDataStore.UpdateItemAsync(item);
                Items.OrderBy((arg) => arg.Completed ? DateTime.MinValue : arg.Start == null ? DateTime.Today : arg.Start);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<IntentionPage, Intention>(this, "DeleteItem", async (obj, item) =>
            {
                var oldItem = item as Intention;
                Items.Remove(oldItem);
                await IntentionDataStore.DeleteItemAsync(oldItem.Id);
                Items.OrderBy((arg) => arg.Completed ? DateTime.MinValue : arg.Start == null ? DateTime.Today : arg.Start);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<IntentionPageNew, Intention>(this, "GetItem", async (obj, item) =>
            {
                var oldItem = item as Intention;
                if (Items.Any((arg) => arg.Id == oldItem.Id))
                {
                    Items.Remove(oldItem);
                }
                Items.Add(await IntentionDataStore.GetItemAsync(oldItem.Id));
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
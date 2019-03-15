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
    public class SinsViewModel : BaseViewModel
    {
        public ObservableCollection<Sins> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public SinsViewModel()
        {
            Items = new ObservableCollection<Sins>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<SinsPageNew, Sins>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Sins;
                if (Items.Count > 0 && Items.Any(x => x.Id == item.Id))
                {
                    Sins oldItem = Items.First(x => x.Id == item.Id);
                    Items.Remove(oldItem);
                    Items.Add(newItem);
                    await SinsDataStore.UpdateItemAsync(newItem);
                }
                else
                {
                    if (!Items.Any(x => x.Id == newItem.Id))
                    {
                        Items.Add(newItem);
                    }
                    await SinsDataStore.AddItemAsync(newItem);
                }
                Items.OrderBy((arg) => arg.Start);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<SinsPageNew, Sins>(this, "UpdateItem", async (obj, item) =>
            {
                await SinsDataStore.UpdateItemAsync(item);
                Items.OrderBy((arg) => arg.Start);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<SinsPage, Sins>(this, "DeleteItem", async (obj, item) =>
            {
                var oldItem = item as Sins;
                //Items.Remove(oldItem);
                await SinsDataStore.DeleteItemAsync(oldItem.Id);
                Items.OrderBy((arg) => arg.Start);
                await ExecuteLoadItemsCommand();
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
                var items = await SinsDataStore.GetItemsAsync(true);
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
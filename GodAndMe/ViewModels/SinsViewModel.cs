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
                if (Items.Count > 0 && Items.Any(x => x.Id == item.Id))
                {
                    Sins oldItem = Items.First(x => x.Id == item.Id);
                    Items.Remove(oldItem);
                    Items.Add(item);
                    await SinsDataStore.UpdateItemAsync(item);
                }
                else
                {
                    if (!Items.Any(x => x.Id == item.Id))
                    {
                        Items.Add(item);
                    }
                    await SinsDataStore.AddItemAsync(item);
                }
                Items.OrderBy((arg) => arg.Committed);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<SinsPageNew, Sins>(this, "UpdateItem", async (obj, item) =>
            {
                if (Items.Count > 0 && Items.Any(x => x.Id == item.Id))
                {
                    Sins oldItem = Items.First(x => x.Id == item.Id);
                    Items[Items.IndexOf(oldItem)] = item;
                }
                await SinsDataStore.UpdateItemAsync(item);
                Items.OrderBy((arg) => arg.Committed);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<SinsPage, Sins>(this, "UpdateItem", async (obj, item) =>
            {
                if (Items.Count > 0 && Items.Any(x => x.Id == item.Id))
                {
                    Sins oldItem = Items.First(x => x.Id == item.Id);
                    Items[Items.IndexOf(oldItem)] = item;
                }
                await SinsDataStore.UpdateItemAsync(item);
                Items.OrderBy((arg) => arg.Committed);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<SinsPage, Sins>(this, "DeleteItem", async (obj, item) =>
            {
                if (Items.Any(x => x.Id == item.Id))
                {
                    Items.Remove(item);
                    await SinsDataStore.DeleteItemAsync(item.Id);
                    Items.OrderBy((arg) => arg.Committed);
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
                if (App.unlocked_YN)
                {
                    var items = await SinsDataStore.GetItemsAsync(true);
                    foreach (var item in items)
                    {
                        Items.Add(item);
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
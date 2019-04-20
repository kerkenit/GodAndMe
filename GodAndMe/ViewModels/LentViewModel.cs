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
    public class LentViewModel : BaseViewModel
    {
        public ObservableCollection<Lent> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public LentViewModel()
        {
            Items = new ObservableCollection<Lent>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<LentPageNew, Lent>(this, "AddItem", async (obj, item) =>
            {
                if (Items.Count > 0 && Items.Any(x => x.Id == item.Id))
                {
                    Lent oldLent = Items.First(x => x.Id == item.Id);
                    Items.Remove(oldLent);
                    Items.Add(item);
                    await LentDataStore.UpdateItemAsync(item);
                }
                else
                {
                    if (!Items.Any(x => x.Id == item.Id))
                    {
                        Items.Add(item);
                    }
                    await LentDataStore.AddItemAsync(item);
                }
                Items.OrderByDescending((arg) => arg.Start);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<LentPage, Lent>(this, "AddItem", async (obj, item) =>
            {
                if (Items.Count > 0 && Items.Any(x => x.Id == item.Id))
                {
                    Lent oldLent = Items.First(x => x.Id == item.Id);
                    Items.Remove(oldLent);
                    Items.Add(item);
                    await LentDataStore.UpdateItemAsync(item);
                }
                else
                {
                    if (!Items.Any(x => x.Id == item.Id))
                    {
                        Items.Add(item);
                    }
                    await LentDataStore.AddItemAsync(item);
                }
                Items.OrderByDescending((arg) => arg.Start);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<LentPage, Lent>(this, "UpdateItem", async (obj, item) =>
            {
                if (Items.Count > 0 && Items.Any(x => x.Id == item.Id))
                {
                    Lent oldItem = Items.First(x => x.Id == item.Id);
                    Items[Items.IndexOf(oldItem)] = item;
                }
                await LentDataStore.UpdateItemAsync(item);
                Items.OrderByDescending((arg) => arg.Start);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<LentPage, Lent>(this, "DeleteItem", async (obj, item) =>
            {
                if (Items.Any(x => x.Id == item.Id))
                {
                    Items.Remove(item);
                    await LentDataStore.DeleteItemAsync(item.Id);
                    Items.OrderByDescending((arg) => arg.Start);
                    await ExecuteLoadItemsCommand();
                }
            });

            MessagingCenter.Subscribe<LentPageNew, Lent>(this, "GetItem", async (obj, item) =>
            {
                if (Items.Any((arg) => arg.Id == item.Id))
                {
                    Items.Remove(item);
                }
                Items.Add(await LentDataStore.GetItemAsync(item.Id));
                Items.OrderByDescending((arg) => arg.Start);
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
                var items = await LentDataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
                Items.OrderByDescending((arg) => arg.Start);
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
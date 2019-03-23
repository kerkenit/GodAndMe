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
        public ObservableCollection<Lent> Lent { get; set; }
        public Command LoadLentCommand { get; set; }

        public LentViewModel()
        {
            Lent = new ObservableCollection<Lent>();
            LoadLentCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<LentPageNew, Lent>(this, "AddItem", async (obj, item) =>
            {
                if (Lent.Count > 0 && Lent.Any(x => x.Id == item.Id))
                {
                    Lent oldLent = Lent.First(x => x.Id == item.Id);
                    Lent.Remove(oldLent);
                    Lent.Add(item);
                    await LentDataStore.UpdateItemAsync(item);
                }
                else
                {
                    if (!Lent.Any(x => x.Id == item.Id))
                    {
                        Lent.Add(item);
                    }
                    await LentDataStore.AddItemAsync(item);
                }
                Lent.OrderBy((arg) => (arg.Start == null ? DateTime.Today : arg.Start));
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<LentPage, Lent>(this, "AddItem", async (obj, item) =>
            {
                if (Lent.Count > 0 && Lent.Any(x => x.Id == item.Id))
                {
                    Lent oldLent = Lent.First(x => x.Id == item.Id);
                    Lent.Remove(oldLent);
                    Lent.Add(item);
                    await LentDataStore.UpdateItemAsync(item);
                }
                else
                {
                    if (!Lent.Any(x => x.Id == item.Id))
                    {
                        Lent.Add(item);
                    }
                    await LentDataStore.AddItemAsync(item);
                }
                Lent.OrderBy((arg) => (arg.Start == null ? DateTime.Today : arg.Start));
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<LentPage, Lent>(this, "UpdateItem", async (obj, item) =>
            {
                await LentDataStore.UpdateItemAsync(item);
                Lent.OrderBy((arg) => (arg.Start == null ? DateTime.Today : arg.Start));
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<LentPage, Lent>(this, "DeleteItem", async (obj, item) =>
            {
                Lent.Remove(item);
                await LentDataStore.DeleteItemAsync(item.Id);
                Lent.OrderBy((arg) => (arg.Start == null ? DateTime.Today : arg.Start));
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<LentPageNew, Lent>(this, "GetItem", async (obj, item) =>
            {
                if (Lent.Any((arg) => arg.Id == item.Id))
                {
                    Lent.Remove(item);
                }
                Lent.Add(await LentDataStore.GetItemAsync(item.Id));
                Lent.OrderBy((arg) => (arg.Start == null ? DateTime.Today : arg.Start));
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Lent.Clear();
                var items = await LentDataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Lent.Add(item);
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
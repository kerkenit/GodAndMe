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
            LoadLentCommand = new Command(async () => await ExecuteLoadLentCommand());

            MessagingCenter.Subscribe<LentPageNew, Lent>(this, "AddItem", async (obj, item) =>
            {
                var newLent = item as Lent;
                if (Lent.Count > 0 && Lent.Any(x => x.Id == item.Id))
                {
                    Lent oldLent = Lent.First(x => x.Id == item.Id);
                    Lent.Remove(oldLent);
                    Lent.Add(newLent);
                    await LentDataStore.UpdateItemAsync(newLent);
                }
                else
                {
                    if (!Lent.Any(x => x.Id == newLent.Id))
                    {
                        Lent.Add(newLent);
                    }
                    await LentDataStore.AddItemAsync(newLent);
                }
                Lent.OrderBy((arg) => (arg.Start == null ? DateTime.Today : arg.Start));
            });

            MessagingCenter.Subscribe<LentPage, Lent>(this, "UpdateItem", async (obj, item) =>
            {
                await LentDataStore.UpdateItemAsync(item);
                Lent.OrderBy((arg) => (arg.Start == null ? DateTime.Today : arg.Start));
            });

            MessagingCenter.Subscribe<LentPage, Lent>(this, "DeleteItem", async (obj, item) =>
            {
                Lent oldLent = item as Lent;
                Lent.Remove(oldLent);
                await LentDataStore.DeleteItemAsync(oldLent.Id);
                Lent.OrderBy((arg) => (arg.Start == null ? DateTime.Today : arg.Start));

            });

            MessagingCenter.Subscribe<LentPageNew, Lent>(this, "GetItem", async (obj, item) =>
            {
                Lent oldLent = item as Lent;
                if (Lent.Any((arg) => arg.Id == oldLent.Id))
                {
                    Lent.Remove(oldLent);
                }
                Lent.Add(await LentDataStore.GetItemAsync(oldLent.Id));
                Lent.OrderBy((arg) => (arg.Start == null ? DateTime.Today : arg.Start));
            });
        }

        async Task ExecuteLoadLentCommand()
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
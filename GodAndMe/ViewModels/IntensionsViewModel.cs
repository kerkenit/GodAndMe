using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;

using GodAndMe.Models;
using GodAndMe.Views;

namespace GodAndMe.ViewModels
{
    public class IntentionsViewModel : BaseViewModel
    {
        public ObservableCollection<Intention> Intentions { get; set; }
        public Command LoadIntentionsCommand { get; set; }

        public IntentionsViewModel()
        {
            Intentions = new ObservableCollection<Intention>();
            LoadIntentionsCommand = new Command(async () => await ExecuteLoadIntentionsCommand());

            MessagingCenter.Subscribe<NewIntentionPage, Intention>(this, "AddItem", async (obj, item) =>
            {
                var newIntention = item as Intention;
                if (Intentions.Count > 0 && Intentions.Any(x => x.Id == item.Id))
                {
                    Intention oldIntention = Intentions.First(x => x.Id == item.Id);
                    Intentions.Remove(oldIntention);
                    Intentions.Add(newIntention);
                    await IntentionDataStore.UpdateItemAsync(newIntention);
                }
                else
                {
                    if (!Intentions.Any(x => x.Id == newIntention.Id))
                    {
                        Intentions.Add(newIntention);
                    }
                    await IntentionDataStore.AddItemAsync(newIntention);
                }
            });

            MessagingCenter.Subscribe<IntentionsPage, Intention>(this, "DeleteItem", async (obj, item) =>
            {
                Intention oldIntention = item as Intention;
                Intentions.Remove(oldIntention);
                await IntentionDataStore.DeleteItemAsync(oldIntention.Id);
            });
        }

        async Task ExecuteLoadIntentionsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Intentions.Clear();
                var items = await IntentionDataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Intentions.Add(item);
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
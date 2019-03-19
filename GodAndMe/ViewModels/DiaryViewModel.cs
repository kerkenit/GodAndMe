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
    public class DiaryViewModel : BaseViewModel
    {
        public ObservableCollection<Diary> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public DiaryViewModel()
        {
            Items = new ObservableCollection<Diary>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<DiaryPageNew, Diary>(this, "AddItem", async (obj, item) =>
            {
                if (Items.Count > 0 && Items.Any(x => x.Id == item.Id))
                {
                    Diary oldItem = Items.First(x => x.Id == item.Id);
                    Items.Remove(oldItem);
                    Items.Add(item);
                    await DiaryDataStore.UpdateItemAsync(item);
                }
                else
                {
                    if (!Items.Any(x => x.Id == item.Id))
                    {
                        Items.Add(item);
                    }
                    await DiaryDataStore.AddItemAsync(item);
                }
                Items.OrderBy((arg) => arg.Start);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<DiaryPageNew, Diary>(this, "UpdateItem", async (obj, item) =>
            {
                await DiaryDataStore.UpdateItemAsync(item);
                Items.OrderBy((arg) => arg.Start);
                await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<DiaryPage, Diary>(this, "DeleteItem", async (obj, item) =>
            {
                //Items.Remove(item);
                await DiaryDataStore.DeleteItemAsync(item.Id);
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
                var items = await DiaryDataStore.GetItemsAsync(true);
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
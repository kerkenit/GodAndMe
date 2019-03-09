using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GodAndMe.Interface;
using GodAndMe.Models;
using SQLite;
using Xamarin.Forms;

namespace GodAndMe.Services
{
    public class LentDataStore : IDataStore<Lent>
    {
        List<Lent> items;
        SQLiteConnection db;
        public LentDataStore()
        {
            items = new List<Lent>();
            db = DependencyService.Get<IDatabaseConnection>().DbConnection();
            db.CreateTable<Lent>();
            items = db.Table<Lent>().ToList();
            items = items.OrderBy((arg) => (arg.Start == null ? DateTime.Today : arg.Start)).ToList<Lent>();
#if DEBUG
            if (false && items.Count == 0)
            {
                List<Lent> LentItems = new List<Lent>
                {
                    //new Lent {
                    //    Id = Guid.NewGuid().ToString(),
                    //   Text = "Sexueel misbruik",
                    //    Description="Om verlichting van de Heilige Geest bij de afgelopen synode",
                    //    Person="Paus Fransiscus",
                    //    Completed=true,
                    //    Start= new DateTime(2019,2,25)
                    //},
                    //new Lent {
                    //    Id = Guid.NewGuid().ToString(),
                    //    //Text = "Vriendin",
                    //    Description="Dat zij Uw wil mogen doen",
                    //    Person="Tom",
                    //    Completed=false,
                    //    Start= new DateTime(2019,2,17)
                    //},
                    // new Lent {
                    //    Id = Guid.NewGuid().ToString(),
                    //    //Text = "ID kaart",
                    //    Description="Dat het goed mag gaan met het aanvragen van de ID kaart",
                    //    Person="Mónica Ruiz",
                    //    Completed=false,
                    //    Start= new DateTime(2019,3,11)
                    //},
                    //new Lent {
                    //    Id = Guid.NewGuid().ToString(),
                    //    //Text = "Retraite",
                    //    Description="Dat er veel jongeren mogen komen",
                    //    Person="Mónica Ruiz",
                    //    Completed=false,
                    //    Start= new DateTime(2019,3,1)
                    //},
                    //new Lent {
                    //    Id = Guid.NewGuid().ToString(),
                    //    //Text = "Pijn",
                    //    Description="Dat ze kracht naar kruis mag krijgen",
                    //    Person="Mama",
                    //    Completed=false,
                    //  Start= null
                    //},
                };
                db.InsertAll(LentItems);
                foreach (var item in LentItems)
                {
                    items.Add(item);
                }
            }
#endif
        }

        public async Task<bool> AddItemAsync(Lent item)
        {
            if (item != null)
            {
                if (items.Any(x => x.Id == item.Id))
                {
                    return await UpdateItemAsync(item);
                }
                else
                {
                    db.Insert(item); // after creating the newStock object
                    items.Add(item);
                }
                return await Task.FromResult(true);

            }
            return await Task.FromResult(false);
        }

        public async Task<bool> UpdateItemAsync(Lent item)
        {
            var oldItem = items.FirstOrDefault((Lent arg) => arg.Id == item.Id);
            item.Id = oldItem.Id;
            items.Remove(oldItem);
            items.Add(item);
            db.Update(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.FirstOrDefault((Lent arg) => arg.Id == id);
            items.Remove(oldItem);
            db.Delete(oldItem);
            return await Task.FromResult(true);
        }

        public async Task<Lent> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Lent>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                items = db.Table<Lent>().ToList();
            }
            return await Task.FromResult(items.OrderBy((arg) => (arg.Start == null ? DateTime.Today : arg.Start)));
        }
    }
}
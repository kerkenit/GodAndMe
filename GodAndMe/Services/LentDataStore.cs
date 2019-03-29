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
            if (CommonFunctions.SCREENSHOT)
            {
                db.DeleteAll<Lent>();
                db.InsertAll(new List<Lent> {
                    new Lent {
                        Id = Guid.NewGuid().ToString(),
                        Text = CommonFunctions.i18n("SCREENSHOT_Lent_1_Text"),
                        MoneyFrom = ((double)new Random().Next(150, 300)) / 100,
                        MoneyTo = ((double)new Random().Next(0, 100)) / 100,
                        Start = DateTime.Today.AddDays(-4).AddHours(9).AddMinutes(1)
                    },
                    new Lent {
                        Id = Guid.NewGuid().ToString(),
                        Text = CommonFunctions.i18n("SCREENSHOT_Lent_2_Text"),
                        MoneyFrom = ((double)new Random().Next(200, 400)) / 100,
                        MoneyTo = 0.00,
                        Start = DateTime.Today.AddDays(-4).AddHours(9).AddMinutes(2)
                    },
                    new Lent {
                        Id = Guid.NewGuid().ToString(),
                        Text = CommonFunctions.i18n("SCREENSHOT_Lent_3_Text"),
                        MoneyFrom = ((double)new Random().Next(50, 300)) / 100,
                        MoneyTo = 0.00,
                        Start = DateTime.Today.AddDays(-4).AddHours(9).AddMinutes(3)
                    },
                });

                items = db.Table<Lent>().ToList();
            }
#if DEBUG
            else if (true && items.Count == 0)
            {
                db.InsertAll(new List<Lent>
                {
                    new Lent {
                        Id = Guid.NewGuid().ToString(),
                        Text = "Jus de orange i.p.v. bananen sap",
                        MoneyFrom = 2.23,
                        MoneyTo = 2.03,
                        Start = new DateTime(2019,3,9,15,0,1)
                    },
                    new Lent {
                        Id = Guid.NewGuid().ToString(),
                        Text = "Spa fruit",
                        MoneyFrom = 1.03,
                        MoneyTo = 0.00,
                        Start = new DateTime(2019,3,9,15,0,2)
                    },
                    new Lent {
                        Id = Guid.NewGuid().ToString(),
                        Text = "Eieren",
                        MoneyFrom = 1.89,
                        MoneyTo = 0.00,
                        Start = new DateTime(2019,3,9,15,0,3)
                    },
                    new Lent {
                        Id = Guid.NewGuid().ToString(),
                        Text = "Kaas van de Jumbo i.p.v. de markt",
                        MoneyFrom = 9.00,
                        MoneyTo = 5.43,
                        Start = new DateTime(2019,3,9,15,0,4)
                    },
                });
            }
#endif

            items = items.OrderBy((arg) => arg.Start).ToList();
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
            if (items.Any(x => x.Id == id))
            {
                var oldItem = items.FirstOrDefault((Lent arg) => arg.Id == id);
                items.Remove(oldItem);
                db.Delete(oldItem);
            }
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
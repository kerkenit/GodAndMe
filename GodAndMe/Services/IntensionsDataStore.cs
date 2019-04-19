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
    public class IntentionsDataStore : IDataStore<Intention>
    {
        List<Intention> items;
        SQLiteConnection db;
        public IntentionsDataStore()
        {
            items = new List<Intention>();
            db = DependencyService.Get<IDatabaseConnection>().DbConnection();
            db.CreateTable<Intention>();
            items = db.Table<Intention>().ToList();

            if (CommonFunctions.SCREENSHOT)
            {
#pragma warning disable CS0162 // Unreachable code detected
                db.DeleteAll<Intention>();
                db.InsertAll(new List<Intention> {
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Intention_1_Description"),
                        Person = CommonFunctions.i18n("SCREENSHOT_Intention_1_Person"),
                        Completed = true,
                        Start = new DateTime(2019,2,25)
                    },
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Intention_2_Description"),
                        Person = CommonFunctions.i18n("SCREENSHOT_Intention_2_Person"),
                        Completed = false,
                        Start = DateTime.Today.AddDays(-9)
                    },
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Intention_3_Description"),
                        Person = CommonFunctions.i18n("SCREENSHOT_Intention_3_Person"),
                        Completed = false,
                        Start = null
                    }
                });
                items = db.Table<Intention>().ToList();

#pragma warning restore CS0162 // Unreachable code detected
            }
#if DEBUG
            else if (true && items.Count == 0)
            {
                db.InsertAll(new List<Intention> {
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Intention_1_Description"),
                        Person = CommonFunctions.i18n("SCREENSHOT_Intention_1_Person"),
                        Completed = true,
                        Start = new DateTime(2019,2,25)
                    },
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Intention_2_Description"),
                        Person = CommonFunctions.i18n("SCREENSHOT_Intention_2_Person"),
                        Completed = false,
                        Start = DateTime.Today.AddDays(-9)
                    },
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Intention_3_Description"),
                        Person = CommonFunctions.i18n("SCREENSHOT_Intention_3_Person"),
                        Completed = false,
                        Start = null
                    }
                });
                items = db.Table<Intention>().ToList();

            }
#endif
            items = items.OrderBy((arg) => arg.Completed ? DateTime.MinValue : arg.Start == null ? DateTime.Today : arg.Start).ToList();

        }

        public async Task<bool> AddItemAsync(Intention item)
        {
            if (item != null)
            {
                if (!string.IsNullOrWhiteSpace(item.Description) || !string.IsNullOrWhiteSpace(item.Person))
                {
                    if (items.Any(x => x.Id == item.Id))
                    {
                        return await UpdateItemAsync(item);
                    }
                    else
                    {
                        db.Insert(item);
                        items.Add(item);
                    }
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> UpdateItemAsync(Intention item)
        {
            var oldItem = items.FirstOrDefault((Intention arg) => arg.Id == item.Id);
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
                var oldItem = items.FirstOrDefault((Intention arg) => arg.Id == id);

                items.Remove(oldItem);
                db.Delete(oldItem);
            }
            return await Task.FromResult(true);
        }

        public async Task<Intention> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Intention>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                items = db.Table<Intention>().ToList();
            }
            return await Task.FromResult(items.OrderBy((arg) => arg.Completed ? DateTime.MinValue : arg.Start == null ? DateTime.Today : arg.Start));
        }
    }
}
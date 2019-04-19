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
    public class PrayersDataStore : IDataStore<Prayers>
    {
        List<Prayers> items;
        SQLiteConnection db;

        public PrayersDataStore()
        {
            items = new List<Prayers>();
            db = DependencyService.Get<IDatabaseConnection>().DbConnection();
            db.CreateTable<Prayers>();
            items = db.Table<Prayers>().ToList();

            if (CommonFunctions.SCREENSHOT)
            {
#pragma warning disable CS0162 // Unreachable code detected
                db.DeleteAll<Prayers>();
                db.InsertAll(new List<Prayers> {
                    new Prayers {
                        Id = Guid.NewGuid().ToString(),
                        Title = CommonFunctions.i18n("SCREENSHOT_Prayers_1_Title"),
                        Description = CommonFunctions.i18n("SCREENSHOT_Prayers_1_Description"),
                    },
                    new Prayers {
                        Id = Guid.NewGuid().ToString(),
                        Title = CommonFunctions.i18n("SCREENSHOT_Prayers_2_Title"),
                        Description = CommonFunctions.i18n("SCREENSHOT_Prayers_2_Description"),
                    },
                    new Prayers {
                        Id = Guid.NewGuid().ToString(),
                        Title = CommonFunctions.i18n("SCREENSHOT_Prayers_3_Title"),
                        Description = CommonFunctions.i18n("SCREENSHOT_Prayers_3_Description"),
                    },
                });

                items = db.Table<Prayers>().ToList();
#pragma warning restore CS0162 // Unreachable code detected
            }
#if DEBUG
            else if (true && items.Count == 0)
            {
                db.InsertAll(new List<Prayers> {
                    new Prayers {
                        Id = Guid.NewGuid().ToString(),
                        Title = CommonFunctions.i18n("SCREENSHOT_Prayers_1_Title"),
                        Description = CommonFunctions.i18n("SCREENSHOT_Prayers_1_Description"),
                    },
                    new Prayers {
                        Id = Guid.NewGuid().ToString(),
                        Title = CommonFunctions.i18n("SCREENSHOT_Prayers_2_Title"),
                        Description = CommonFunctions.i18n("SCREENSHOT_Prayers_2_Description"),
                    },
                    new Prayers {
                        Id = Guid.NewGuid().ToString(),
                        Title = CommonFunctions.i18n("SCREENSHOT_Prayers_3_Title"),
                        Description = CommonFunctions.i18n("SCREENSHOT_Prayers_3_Description"),
                    },
                });

                items = db.Table<Prayers>().ToList();
            }
#endif

            items = items.OrderBy((arg) => arg.Title).ToList();
        }

        public async Task<bool> AddItemAsync(Prayers item)
        {
            if (item != null)
            {
                if (!string.IsNullOrEmpty(item.Description))
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
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Prayers item)
        {
            var oldItem = items.FirstOrDefault((Prayers arg) => arg.Id == item.Id);
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
                var oldItem = items.FirstOrDefault((Prayers arg) => arg.Id == id);
                if (oldItem != null)
                {
                    items.Remove(oldItem);
                    db.Delete(oldItem);
                }
            }
            return await Task.FromResult(true);
        }

        public async Task<Prayers> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Prayers>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                items = db.Table<Prayers>().ToList();
            }
            return await Task.FromResult(items.OrderBy((arg) => arg.Title));
        }
    }
}
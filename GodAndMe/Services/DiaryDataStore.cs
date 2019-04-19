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
    public class DiaryDataStore : IDataStore<Diary>
    {
        List<Diary> items;
        SQLiteConnection db;

        public DiaryDataStore()
        {
            items = new List<Diary>();
            db = DependencyService.Get<IDatabaseConnection>().DbConnection();
            db.CreateTable<Diary>();
            items = db.Table<Diary>().ToList();

            if (CommonFunctions.SCREENSHOT)
            {
#pragma warning disable CS0162 // Unreachable code detected
                db.DeleteAll<Diary>();
                db.InsertAll(new List<Diary> {
                    new Diary {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Diary_1_Description"),
                        Start = DateTime.Today.AddDays(-1)
                    },
                    new Diary {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Diary_2_Description"),
                        Start = DateTime.Today.AddDays(-2)
                    },
                    new Diary {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Diary_3_Description"),
                        Start = DateTime.Today.AddDays(-3)
                    },
                });
                items = db.Table<Diary>().ToList();
#pragma warning restore CS0162 // Unreachable code detected
            }
#if DEBUG
            else if (true && items.Count == 0)
            {
#pragma warning disable CS0162 // Unreachable code detected
                db.InsertAll(new List<Diary> {
                    new Diary {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Diary_1_Description"),
                        Start = DateTime.Today.AddDays(-1)
                    },
                    new Diary {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Diary_2_Description"),
                        Start = DateTime.Today.AddDays(-2)
                    },
                    new Diary {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Diary_3_Description"),
                        Start = DateTime.Today.AddDays(-3)
                    },
                });
                items = db.Table<Diary>().ToList();
#pragma warning restore CS0162 // Unreachable code detected
            }
#endif

            items = items.OrderByDescending((arg) => arg.Start).ToList();
        }

        public async Task<bool> AddItemAsync(Diary item)
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

        public async Task<bool> UpdateItemAsync(Diary item)
        {
            var oldItem = items.FirstOrDefault((Diary arg) => arg.Id == item.Id);
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
                var oldItem = items.FirstOrDefault((Diary arg) => arg.Id == id);
                if (oldItem != null)
                {
                    items.Remove(oldItem);
                    db.Delete(oldItem);
                }
            }
            return await Task.FromResult(true);
        }

        public async Task<Diary> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Diary>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                items = db.Table<Diary>().ToList();
            }
            return await Task.FromResult(items.OrderByDescending((arg) => arg.Start));
        }
    }
}
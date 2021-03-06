﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GodAndMe.Interface;
using GodAndMe.Models;
using SQLite;
using Xamarin.Forms;

namespace GodAndMe.Services
{
    public class SinsDataStore : IDataStore<Sins>
    {
        List<Sins> items;
        SQLiteConnection db;

        public SinsDataStore()
        {
            items = new List<Sins>();
            db = DependencyService.Get<IDatabaseConnection>().DbConnection();
            db.CreateTable<Sins>();
#if DEBUG
            items = db.Table<Sins>().ToList();
#else
            items = db.Table<Sins>().Where((arg) => !arg.Confessed).ToList();
#endif

            if (CommonFunctions.SCREENSHOT)
            {
#pragma warning disable CS0162 // Unreachable code detected
                db.DeleteAll<Sins>();
                db.InsertAll(new List<Sins> {
                    new Sins {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Sins_1_Description"),
                        Committed= DateTime.Today.AddDays(-1)
                    },
                    new Sins {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Sins_2_Description"),
                        Committed = DateTime.Today.AddDays(-2)
                    },
                    new Sins {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Sins_3_Description"),
                        Committed = DateTime.Today.AddDays(-3)
                    },
                });
                items = db.Table<Sins>().Where((arg) => !arg.Confessed).ToList();
#pragma warning restore CS0162 // Unreachable code detected
            }
#if DEBUG
            else if (items.Count == 0)
            {
                db.InsertAll(new List<Sins> {
                    new Sins {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Sins_1_Description"),
                        Committed= DateTime.Today.AddDays(-1)
                    },
                    new Sins {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Sins_2_Description"),
                        Committed = DateTime.Today.AddDays(-2)
                    },
                    new Sins {
                        Id = Guid.NewGuid().ToString(),
                        Description = CommonFunctions.i18n("SCREENSHOT_Sins_3_Description"),
                        Committed = DateTime.Today.AddDays(-3)
                    },
                });
                items = db.Table<Sins>().Where((arg) => !arg.Confessed).ToList();
            }
#endif

            items = items.OrderBy((arg) => arg.Committed).ToList();
        }

        public async Task<bool> AddItemAsync(Sins item)
        {
            if (item != null)
            {
                if (true || !string.IsNullOrEmpty(item.Description))
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

        public async Task<bool> UpdateItemAsync(Sins item)
        {
            var oldItem = items.FirstOrDefault((Sins arg) => arg.Id == item.Id);
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
                var oldItem = items.FirstOrDefault((Sins arg) => arg.Id == id);
                if (oldItem != null)
                {
                    items.Remove(oldItem);
                    db.Delete(oldItem);
                }
            }
            return await Task.FromResult(true);
        }

        public async Task<Sins> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Sins>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
#if DEBUG
                items = db.Table<Sins>().ToList();
#else
           items = db.Table<Sins>().Where((arg) => !arg.Confessed).ToList();
#endif

            }
            return await Task.FromResult(items.OrderBy((arg) => arg.Committed));
        }
    }
}
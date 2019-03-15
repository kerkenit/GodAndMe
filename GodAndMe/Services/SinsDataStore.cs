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
            items = db.Table<Sins>().ToList();
            items = items.OrderBy((arg) => arg.Start).ToList();
#if DEBUG
            if (CommonFunctions.SCREENSHOT)
            {
                db.DeleteAll<Sins>();
                switch (System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower())
                {
                    case "nl":
                        db.InsertAll(new List<Sins> {
                            new Sins {
                                Id = Guid.NewGuid().ToString(),
                                Description="Boos geworden",
                                Start= DateTime.Today.AddDays(-1)
                            },
                            new Sins {
                                Id = Guid.NewGuid().ToString(),
                                Description="Te lang in bed gelegen",
                                Start= DateTime.Today.AddDays(-2)
                            },

                            new Sins {
                                Id = Guid.NewGuid().ToString(),
                                Description="Niet goed voor de schepping gezorgd",
                                Start= DateTime.Today.AddDays(-3)
                            },
                        });
                        break;
                    case "en":
                        db.InsertAll(new List<Sins> {
                           new Sins {
                                Id = Guid.NewGuid().ToString(),
                                Description="Become angry",
                                Start= DateTime.Today.AddDays(-1)
                            },
                            new Sins {
                                Id = Guid.NewGuid().ToString(),
                                Description="Lying in bed too long",
                                Start= DateTime.Today.AddDays(-2)
                            },

                            new Sins {
                                Id = Guid.NewGuid().ToString(),
                                Description="Not well taken care of God's creation",
                                Start= DateTime.Today.AddDays(-3)
                            },
                        });
                        break;
                    case "es":
                        db.InsertAll(new List<Sins> {
                            new Sins {
                                Id = Guid.NewGuid().ToString(),
                                Description="Enfadarse",
                                Start= DateTime.Today.AddDays(-1)
                            },
                            new Sins {
                                Id = Guid.NewGuid().ToString(),
                                Description="Acostado en la cama demasiado tiempo",
                                Start= DateTime.Today.AddDays(-2)
                            },

                            new Sins {
                                Id = Guid.NewGuid().ToString(),
                                Description="Not well taken care of creation",
                                Start= DateTime.Today.AddDays(-3)
                            },
                        });
                        break;
                }

            }

            else if (true && items.Count == 0)
            {
                //db.InsertAll(new List<Sins> { new Sins { Id = Guid.NewGuid().ToString(), Start=new DateTime(2019,3,12, 8,50,0), Description="Bewuster het Onze Vader bidden en mij dat diep op mij in laten werken. Dus niet snel opriedelen, maar met het hart bidden." },
                //    //new Sins { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                //    //new Sins { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                //    //new Sins { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                //    //new Sins { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                //    //new Sins { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." },
                //});
            }
#endif
        }

        public async Task<bool> AddItemAsync(Sins item)
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
            var oldItem = items.FirstOrDefault((Sins arg) => arg.Id == id);
            if (oldItem != null)
            {
                items.Remove(oldItem);
                db.Delete(oldItem);
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
                items = db.Table<Sins>().ToList();
            }
            return await Task.FromResult(items.OrderBy((arg) => arg.Start));
        }
    }
}
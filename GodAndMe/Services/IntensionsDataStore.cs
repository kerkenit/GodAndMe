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
            items = items.OrderBy((arg) => arg.Completed ? DateTime.MinValue : arg.Start == null ? DateTime.Today : arg.Start).ToList();


            if (CommonFunctions.SCREENSHOT)
            {
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
            }
#if DEBUG
            else if (true && items.Count == 0)
            {
                db.InsertAll(new List<Intention> {
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description="Om verlichting van de Heilige Geest bij de afgelopen synode over het sexueel misbruik bij minderjarige",
                        Person="Paus Fransiscus",
                        Completed=true,
                        Start= new DateTime(2019,2,25)
                    },
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description="Dat zij en Mariska Uw wil mogen doen",
                        Person="Tom van 't Klooster",
                        Completed=false,
                        Start= new DateTime(2019,2,17)
                    },
                     new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description="Dat het goed mag gaan met het aanvragen van de ID kaart",
                        Person="Mónica Ruiz",
                        Completed=false,
                        Start= new DateTime(2019,3,11)
                    },
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description="Dat er veel jongeren mogen komen",
                        Person="Mónica Ruiz",
                        Completed=true,
                        Start= new DateTime(2019,3,1)
                    },
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description="Dat ze kracht naar kruis mag krijgen en het lijden mag aanvaarden",
                        Person="Mama",
                        Completed=false,
                        Start= null
                    },
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description="Voor staibiliteit bij de problemen met haar bloeddruk",
                        Person="Maria Ruiz",
                        Completed=true,
                        Start= new DateTime(2019,3,7)
                    },
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description="Dat hij een fijne vakantie mag hebben",
                        Person="Tom van 't Klooster",
                        Completed=false,
                        Start= new DateTime(2019,3,8)
                    },
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description="Dat hij een fijne vakantie mag hebben",
                        Person="Tom van 't Klooster",
                        Completed=false,
                        Start= new DateTime(2019,3,8)
                    },
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description="Dat het goed mag gaan bij haar tijdenlijk werk",
                        Person="Mónica Ruiz",
                        Completed=false,
                        Start= null
                    },
                    new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description="Dat het goed mag met haar bloeddruk",
                        Person="Marlène Thodé",
                        Completed=false,
                        Start= null
                    },
                     new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description="Dat hij Uw zegen mag krijgen terwijl hij zich voorbereid op zijn priesterwijding",
                        Person="Marco Figliola",
                        Completed=false,
                        Start= new DateTime(2019,6,15)
                    },
                     new Intention {
                        Id = Guid.NewGuid().ToString(),
                        Description="Dat ze behouden terug mag komen",
                        Person="Corina",
                        Completed=false,
                        Start= new DateTime(2019,4,10)
                    },
                });

            }
#endif
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
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
            items = items.OrderBy((arg) => arg.Start).ToList();
#if DEBUG
            if (CommonFunctions.SCREENSHOT)
            {
                db.DeleteAll<Diary>();
                switch (System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower())
                {
                    case "nl":
                        db.InsertAll(new List<Diary> {
                            new Diary {
                                Id = Guid.NewGuid().ToString(),
                                Description="Bewuster het Onze Vader bidden en dieper op mij laten inwerken. Dus niet snel bidden maar met het hart bidden.",
                                Start= DateTime.Today.AddDays(-1)
                            },
                            new Diary {
                                Id = Guid.NewGuid().ToString(),
                                Description="De Heer is mijn herder, mij zal niets ontbreken",
                                Start= DateTime.Today.AddDays(-2)
                            },

                            new Diary {
                                Id = Guid.NewGuid().ToString(),
                                Description="Als een hert dat verlangt naar water,\r\r\nzo verlangt mijn ziel naar U.\r\r\nU alleen kunt mijn hart vervullen,\r\r\nmijn aanbidding is voor U.\r\r\nU alleen bent mijn Kracht, mijn Schild.\r\nAan U alleen geef ik mij geheel.\r\nU alleen kunt mijn hart vervullen,\r\nmijn aanbidding is voor U.",
                                Start= DateTime.Today.AddDays(-3)
                            },
                        });
                        break;
                    case "en":
                        db.InsertAll(new List<Diary> {
                            new Diary {
                                Id = Guid.NewGuid().ToString(),
                                Description="Bewuster het Onze Vader bidden en dieper op mij laten inwerken. Dus niet snel bidden maar met het hart bidden.",
                                Start= DateTime.Today.AddDays(-1)
                            },
                            new Diary {
                                Id = Guid.NewGuid().ToString(),
                                Description="The Lord is my shepherd, I lack nothing.",
                                Start= DateTime.Today.AddDays(-2)
                            },

                            new Diary {
                                Id = Guid.NewGuid().ToString(),
                                Description="As the deer panteth for the water\r\nSo my soul longeth after thee\r\nThou alone are my heart's desire\r\nAnd I long to worship thee\r\n\r\nThou alone are my strength, my shield\r\nTo Thee alone may my spirit yield\r\nThou alone are my heart's desire\r\nAnd I long to worship thee\r\n\r\nThou art my friend and you are my brother\r\nEven though Thou are a king\r\nI love Thee more than any other\r\nSo much more than anything",
                                Start= DateTime.Today.AddDays(-3)
                            },
                        });
                        break;
                    case "es":
                        db.InsertAll(new List<Diary> {
                            new Diary {
                                Id = Guid.NewGuid().ToString(),
                                Description="Reza más conscientemente la oración del Señor y deja que me afecte. Entonces no ores rápido, sino ora con tu corazón.",
                                Start= DateTime.Today.AddDays(-1)
                            },
                            new Diary {
                                Id = Guid.NewGuid().ToString(),
                                Description="El Señor es mi pastor, nada me falta;",
                                Start= DateTime.Today.AddDays(-2)
                            },

                            new Diary {
                                Id = Guid.NewGuid().ToString(),
                                Description="Como el ciervo busca por las aguas,\r\nAsí clama mi alma por ti Señor\r\nDía y noche yo tengo sed de ti\r\n\r\nY sólo a ti buscaré\r\nLlename, llename Señor\r\nDame más, más de tu amor\r\nYo tengo sed sólo de ti",
                                Start= DateTime.Today.AddDays(-3)
                            },
                        });
                        break;
                }

            }

            else if (true && items.Count == 0)
            {
                db.InsertAll(new List<Diary> { new Diary { Id = Guid.NewGuid().ToString(), Start=new DateTime(2019,3,12, 8,50,0), Description="Bewuster het Onze Vader bidden en mij dat diep op mij in laten werken. Dus niet snel opriedelen, maar met het hart bidden." },
                    //new Diary { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                    //new Diary { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                    //new Diary { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                    //new Diary { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                    //new Diary { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." },
                });
            }
#endif
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
            var oldItem = items.FirstOrDefault((Diary arg) => arg.Id == id);
            if (oldItem != null)
            {
                items.Remove(oldItem);
                db.Delete(oldItem);
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
            return await Task.FromResult(items.OrderBy((arg) => arg.Start));
        }
    }
}
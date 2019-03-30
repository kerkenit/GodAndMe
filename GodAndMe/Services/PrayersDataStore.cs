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
            items = items.OrderBy((arg) => arg.Title).ToList();

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
#pragma warning restore CS0162 // Unreachable code detected
            }
#if DEBUG
            else if (true && items.Count == 0)
            {
                db.InsertAll(new List<Prayers> {
                            new Prayers {
                                Id = Guid.NewGuid().ToString(),
                                Title="Noveengebed tot O.L. Vrouw van de Wonderdadige Medaille",
                                Description="Onbevlekte Maagd Maria - Moeder van Jezus Christus en Moeder van ons - wij hebben het grootste vertrouwen in uw voorspraak. U kunt van uw Zoon alles verkrijgen, wat goed is voor ons.\n\nWij danken U, dat U ons daaraan herinnert - door de medaille, waarop onze verlossing is afgebeeld. U houdt van ons, wij vertrouwen op U. Wil voor ons uw kinderen de genade verkrijgen, waarom wij nederig vragen.\n\n(Hier maakt men zijn/haar intenties).\n\nMoeder Maria - verkrijg voor ons niet alleen tijdelijke gunsten - maar vooral dat wij bereid mogen zijn tot gebed en offer. Zo alleen zullen wij in staat zijn - tot ware liefde voor uw Zoon - en tot oprechte liefde voor de evenmens. Dan zullen wij het geluk van de hemel verkrijgen - dat U reeds bezit als onze Koningin en Moeder. Amen.\n\nSchietgebed:\nO Maria, zonder zonden ontvangen, bid voor ons, die onze toevlucht tot U nemen. (3x)",
                            },
                            new Prayers {
                                Id = Guid.NewGuid().ToString(),
                                Title="Heilige Aartsengel Michaël",
                                Description="Heilige Aartsengel Michaël, verdedig ons in de strijd; wees onze bescherming tegen de boosheid en de listen van de duivel. Wij smeken ootmoedig dat God hem Zijn macht doe gevoelen. En gij, vorst van de hemelse legerscharen, drijf Satan en andere boze geesten, die tot verderf van de zielen over de wereld rondgaan, door de goddelijke kracht in de hel terug. Amen.",
                            },

                            new Prayers {
                                Id = Guid.NewGuid().ToString(),
                                Title="Kom Heilige Geest",
                                Description="Kom, Heilige Geest, vervul de harten van uw gelovigen, en ontsteek in hen het vuur van uw liefde.\nV. Zend uw Geest uit en alles zal herschapen worden.\nA. En Gij zult het aanschijn van de aarde vernieuwen.\n\nLaat ons bidden\nGod Gij hebt de harten van de gelovigen door de verlichting van de Heilige Geest onderwezen: geef dat wij door die Heilige Geest de ware wijsheid mogen bezitten, en ons altijd over zijn vertroosting mogen verblijden. Door Christus onze Heer.\n\nAmen.",
                            },
                        });
            }
#endif
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
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
#if DEBUG
            if (CommonFunctions.SCREENSHOT)
            {
                db.DeleteAll<Prayers>();
                switch (System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower())
                {
                    case "nl":
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
                        break;
                    case "en":
                        db.InsertAll(new List<Prayers> {
                           new Prayers {
                                Id = Guid.NewGuid().ToString(),
                                Title="Novena to Saint Joseph",
                                Description="Remember, O most pure spouse of the Blessed Virgin Mary,\nmy sweet protector St. Joseph\nthat no one ever had recourse to thy protection\nor implored thy aid without obtaining relief.\nConfiding therefore in thy goodness,\nI come before thee, and humbly supplicate thee.\nOh, despise not my petitions,\nfoster-father of the Redeemer,\nbut graciously receive them.\nAmen.",
                            },
                            new Prayers {
                                Id = Guid.NewGuid().ToString(),
                                Title="Prayer to Saint Michael",
                                Description="Saint Michael the Archangel,\ndefend us in battle,\nbe our protection against the wickedness and snares of the devil;\nmay God rebuke him, we humbly pray;\nand do thou, O Prince of the heavenly host,\nby the power of God, cast into hell\nSatan and all the evil spirits\nwho prowl through the world seeking the ruin of souls.\nAmen",
                            },

                            new Prayers {
                                Id = Guid.NewGuid().ToString(),
                                Title="Come, Holy Spirit",
                                Description="Come, Holy Spirit, fill the hearts of Thy faithful and enkindle in them the fire of Thy love.\nSend forth Thy Spirit and they shall be created.\nAnd Thou shalt renew the face of the earth.\nLet us pray.\nO God, Who didst instruct the hearts of the faithful by the light of the Holy Spirit, grant us in the same Spirit to be truly wise, and ever to rejoice in His consolation.\nThrough Christ, our Lord.\nAmen.",
                            },
                        });
                        break;
                    case "es":
                        db.InsertAll(new List<Prayers> {
                             new Prayers {
                                Id = Guid.NewGuid().ToString(),
                                Title="Oración a san Miguel arcángel",
                                Description="San Miguel arcángel, defiéndenos en batalla,\nsé nuestro amparo contra las maldades y acechanzas del diablo,\nque Dios le reprenda, es nuestra humilde súplica;\ny tú, Príncipe de las huestes celestiales,\npor el poder de Dios,\narroja al Infierno a Satanás y a los demás espíritus malignos,\nque rondan por el mundo buscando la ruina de las almas.\nAmén.",
                            },

                            new Prayers {
                                Id = Guid.NewGuid().ToString(),
                                Title="Ven Espíritu Santo",
                                Description="Ven Espíritu Santo,\nVenid a llenar los corazones de todos tus fieles,\nY enciende en ellos,\nEl verdadero fuego de tu amor,\nEnvía Señor tu Espíritu,\nY que renueve toda la faz de la tierra.\n\nORACIÓN\n\n¡Oh mi amado Dios!\nQue sumergiste los corazones de todos tus fieles,\nCon la divina luz del Espíritu Santo,\nAyúdanos que guiados por el mismo Espíritu,\nSigamos tu rectitud y disfrutemos siempre,\nDe tu consuelo y de tu amor.\nPor Nuestro Señor Jesucristo,\n\nAmén.",
                            },
                            new Prayers {
                                Id = Guid.NewGuid().ToString(),
                                Title="Novena a San Joseph",
                                Description="Oh gloriosísimo Padre de Jesús, Esposo de María. Patriarca y Protector de la Santa Iglesia, a quien el Padre Eterno confió el cuidado de gobernar, regir y defender en la tierra la Sagrada Familia; protégenos también a nosotros, que pertenecemos, como fieles católicos. a la santa familia de tu Hijo que es la Iglesia, y alcánzanos los bienes necesarios de esta vida, y sobre todo los auxilios espirituales para la vida eterna. Alcánzanos especialmente estas tres gracias, la de no cometer jamás ningún pecado mortal, principalmente contra la castidad; la de un sincero amor y devoción a Jesús y María, y la de una buena muerte, recibiendo bien los últimos Sacramentos. Concédenos además la gracia especial que te pedimos cada uno en esta novena.",
                            },

                        });
                        break;
                }

            }

            else if (true && items.Count == 0)
            {
                //db.InsertAll(new List<Prayers> { new Prayers { Id = Guid.NewGuid().ToString(), Start=new DateTime(2019,3,12, 8,50,0), Description="Bewuster het Onze Vader bidden en mij dat diep op mij in laten werken. Dus niet snel opriedelen, maar met het hart bidden." },
                //    //new Prayers { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                //    //new Prayers { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                //    //new Prayers { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                //    //new Prayers { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                //    //new Prayers { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." },
                //});
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
            var oldItem = items.FirstOrDefault((Prayers arg) => arg.Id == id);
            if (oldItem != null)
            {
                items.Remove(oldItem);
                db.Delete(oldItem);
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
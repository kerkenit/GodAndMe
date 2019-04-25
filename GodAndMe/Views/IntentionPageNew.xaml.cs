using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntentionPageNew : ContentPage
    {
        public IntentionDetailViewModel viewModel;
        IAddressBookInformation ContactPersons = DependencyService.Get<IAddressBookInformation>();
        public Intention Item { get; set; }
        string ChooseOther = "- " + CommonFunctions.i18n("ChooseOther") + " -";
        string ChooseRecent = "- " + CommonFunctions.i18n("ChooseRecent") + " -";

        public IntentionPageNew(string title, Intention item = null)
        {

            InitializeComponent();

#if __IOS__
            ToolbarItem btCancel = new ToolbarItem()
            {
                Text = CommonFunctions.i18n("Cancel"),
                IsDestructive = true,
                Priority = -1
            };

            btCancel.Clicked += async (object sender, EventArgs e) =>
            {
                await Navigation.PopToRootAsync();
            };
            this.ToolbarItems.Add(btCancel);
#endif

            if (item != null)
            {
                Item = item;
            }
            else
            {
                Item = new Intention
                {
                    Id = Guid.NewGuid().ToString(),
                    Start = null
                };
            }

            viewModel = new IntentionDetailViewModel(Item)
            {
                Title = title
            };


            if (ContactPersons.IsAuthorized().Result)
            {
                FillContactList();
            }

            btPerson.Clicked += (object sender, EventArgs e) =>
            {
                if (!ContactPersons.IsAuthorized().Result && ContactPersons.RequestAccess().Result)
                {
                    FillContactList();
                }
                else if (ddlPerson.ItemsSource == null || ddlPerson.ItemsSource.Count == 0)
                {
                    FillContactList();
                }
                if (ddlPerson.ItemsSource != null && ddlPerson.ItemsSource.Count > 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        tbPerson.IsVisible = false;
                        ddlPerson.IsVisible = true;
                        if (!string.IsNullOrWhiteSpace(tbPerson.Text))
                        {
                            ddlPerson.SelectedItem = tbPerson.Text;
                        }
                        ddlPerson.Unfocused += (object sender1, FocusEventArgs e1) =>
                        {
                            ddlPerson.IsVisible = false;
                            tbPerson.IsVisible = true;
                            if (ddlPerson.SelectedItem != null && ddlPerson.SelectedItem.ToString() != ChooseOther && ddlPerson.SelectedItem.ToString() != ChooseRecent)
                            {
                                tbPerson.Text = ddlPerson.SelectedItem.ToString();
                            }
                            else
                            {
                                tbPerson.Text = string.Empty;
                                ddlPerson.Unfocus();
                            }
                        };
                        ddlPerson.Focus();
                    });
                }
#if __ANDROID__
                Task.Factory.StartNew(() =>
                {
                    Task.Delay(App.DELAY).Wait();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ddlPerson.Focus();
                    });
                });
#endif
            };
            BindingContext = viewModel;
        }

        void FillContactList()
        {
            List<string> persons = ContactPersons.GetContacts().Result;

            IEnumerable<Intention> existingItems = viewModel.IntentionDataStore.GetItemsAsync(true).Result;

            var recentPersons = from intension in existingItems
                                group intension by intension.Person into Intensions
                                select new
                                {
                                    Name = Intensions.Key,
                                    Count = Intensions.Count(),
                                };
            persons.Insert(0, ChooseOther);

            bool anyRecentPersons = false;
            foreach (var person in recentPersons.OrderBy((arg) => arg.Count))
            {
                if (persons.Any(x => x == person.Name))
                {
                    persons.Remove(person.Name);
                    persons.Insert(0, person.Name);
                    anyRecentPersons = true;
                }
            }
            if (anyRecentPersons)
            {
                persons.Insert(0, ChooseRecent);
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                ddlPerson.ItemsSource = persons;
            });
        }

        void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                tbDescription.Focus();
            });
        }

        void OnTextChanged(Object sender, TextChangedEventArgs e)
        {
            tbDescription.InvalidateLayout();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            Item = viewModel.Item;
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}
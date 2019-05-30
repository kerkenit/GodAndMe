using System;
using GodAndMe.Extensions;
using GodAndMe.Interface;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntentionDetailPage : ContentPage
    {
        IntentionDetailViewModel viewModel;
        public Intention Item { get; set; }

        public IntentionDetailPage(IntentionDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public IntentionDetailPage()
        {
            InitializeComponent();

            Item = new Intention
            {
                //Text = "",
                Description = "",
                Id = Guid.NewGuid().ToString(),
                Start = null
            };

            viewModel = new IntentionDetailViewModel(Item);
            BindingContext = viewModel;
        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {
            if ((viewModel != null) && (viewModel.Item != null) && (viewModel.IntentionDataStore != null))
            {
                Item = await viewModel.IntentionDataStore.GetItemAsync(viewModel.Item.Id);
            }

            await Navigation.PushAsync(new IntentionPageNew(CommonFunctions.i18n("EditIntention"), Item));
        }

        async void OnShare(object sender, EventArgs e)
        {
            var mi = ((Button)sender);
            if ((viewModel != null) && (viewModel.Item != null) && (viewModel.IntentionDataStore != null))
            {
                Item = await viewModel.IntentionDataStore.GetItemAsync(mi.CommandParameter.ToString());
            }
            try
            {
                string url = ("https://godandme.app/share/" + CryptFile.Encrypt_Legacy(JsonConvert.SerializeObject(new string[] { Item.Person, Item.Description, Item.Start == null ? string.Empty : ((DateTime)Item.Start).ToString("yyyy-MM-dd") })) + "?lang=" + CommonFunctions.Culture.TwoLetterISOLanguageName).Trim();
                var share = DependencyService.Get<IShare>();
                await share.Show(
                    string.Format(CommonFunctions.i18n("WouldYouPrayForX"), Item.Person),
                    Item.Description,
                    url,
                    Environment.NewLine + Environment.NewLine + CommonFunctions.i18n("DownloadApp")
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
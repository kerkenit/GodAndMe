using System;
using System.Linq;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryPageNew : ContentPage
    {
        public DiaryDetailViewModel viewModel;
        public Diary Item { get; set; }
        private DiaryType[] diaryTypes;

        public DiaryPageNew(string title, DiaryType[] types, Diary item = null)
        {
            InitializeComponent();
            diaryTypes = types;

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

            Title = title;
            if (item != null)
            {
                Item = item;
                ddlType.Items.Add(CommonFunctions.i18n("God"));
                ddlType.Items.Add(CommonFunctions.i18n("DiscernmentOfSpirits"));
                ddlType.Items.Add(CommonFunctions.i18n("Personal"));
                ddlType.Items.Add(CommonFunctions.i18n("MomentsOfHappiness"));
                ddlType.SelectedItem = CommonFunctions.i18n(Enum.GetName(typeof(DiaryType), Item.DiaryType));
            }
            else
            {
                Item = new Diary
                {
                    Id = Guid.NewGuid().ToString(),
                    Start = DateTime.Now
                };
                if (types.Contains(DiaryType.God))
                {
                    ddlType.Items.Add(CommonFunctions.i18n("God"));
                }
                if (types.Contains(DiaryType.DiscernmentOfSpirits))
                {
                    ddlType.Items.Add(CommonFunctions.i18n("DiscernmentOfSpirits"));
                }
                if (types.Contains(DiaryType.Personal))
                {
                    ddlType.Items.Add(CommonFunctions.i18n("Personal"));
                }
                if (types.Contains(DiaryType.MomentsOfHappiness))
                {
                    ddlType.Items.Add(CommonFunctions.i18n("MomentsOfHappiness"));
                }

                ddlType.SelectedIndex = 0;
            }
            ddlType.SelectedIndexChanged += ddlType_SelectedIndexChanged;
            viewModel = new DiaryDetailViewModel(Item);
            BindingContext = viewModel;




            Device.BeginInvokeOnMainThread(() =>
            {
                tbDescription.Focus();
            });
        }

        private void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DiscernmentOfSpirits = CommonFunctions.i18n("DiscernmentOfSpirits_Q1") + Environment.NewLine + Environment.NewLine + CommonFunctions.i18n("DiscernmentOfSpirits_Q3") + Environment.NewLine + Environment.NewLine + CommonFunctions.i18n("DiscernmentOfSpirits_Q3") + Environment.NewLine + Environment.NewLine;
            switch (ddlType.SelectedIndex)
            {
                case 1: //Discernment of Spirits
                    if (string.IsNullOrWhiteSpace(tbDescription.Text))
                    {
                        tbDescription.Text = DiscernmentOfSpirits;
                    }
                    break;
                default:
                    if (!string.IsNullOrWhiteSpace(tbDescription.Text) && tbDescription.Text == DiscernmentOfSpirits)
                    {
                        tbDescription.Text = string.Empty;
                    }
                    break;
            }
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

            if (ddlType.SelectedItem.ToString() == CommonFunctions.i18n("DiscernmentOfSpirits"))
            {
                Item.DiaryType = (int)DiaryType.DiscernmentOfSpirits;
            }
            else if (ddlType.SelectedItem.ToString() == CommonFunctions.i18n("Personal"))
            {
                Item.DiaryType = (int)DiaryType.Personal;
            }
            else if (ddlType.SelectedItem.ToString() == CommonFunctions.i18n("MomentsOfHappiness"))
            {
                Item.DiaryType = (int)DiaryType.MomentsOfHappiness;
            }
            else
            {
                Item.DiaryType = (int)DiaryType.God;
            }

            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}
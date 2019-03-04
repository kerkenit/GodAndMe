using System;

using GodAndMe.Models;

namespace GodAndMe.ViewModels
{
    public class IntentionsDetailViewModel : BaseViewModel
    {
        public Intention Item { get; set; }
        public IntentionsDetailViewModel(Intention item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}

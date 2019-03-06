using GodAndMe.Models;

namespace GodAndMe.ViewModels
{
    public class IntentionDetailViewModel : BaseViewModel
    {
        public Intention Item { get; set; }
        public IntentionDetailViewModel(Intention item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}

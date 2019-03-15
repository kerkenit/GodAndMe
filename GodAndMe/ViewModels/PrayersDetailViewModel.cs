using GodAndMe.Models;

namespace GodAndMe.ViewModels
{
    public class PrayersDetailViewModel : BaseViewModel
    {
        public Prayers Item { get; set; }
        public PrayersDetailViewModel(Prayers item = null)
        {
            //Title = item?.Text;
            Item = item;
        }
    }
}
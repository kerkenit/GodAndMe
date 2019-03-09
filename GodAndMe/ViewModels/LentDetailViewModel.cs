using GodAndMe.Models;

namespace GodAndMe.ViewModels
{
    public class LentDetailViewModel : BaseViewModel
    {
        public Lent Item { get; set; }
        public LentDetailViewModel(Lent item)
        {
            Item = item;
        }
    }
}

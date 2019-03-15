using GodAndMe.Models;

namespace GodAndMe.ViewModels
{
    public class SinsDetailViewModel : BaseViewModel
    {
        public Sins Item { get; set; }
        public SinsDetailViewModel(Sins item = null)
        {
            //Title = item?.Text;
            Item = item;
        }
    }
}
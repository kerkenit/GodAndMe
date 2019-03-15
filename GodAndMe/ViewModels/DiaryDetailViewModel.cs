using GodAndMe.Models;

namespace GodAndMe.ViewModels
{
    public class DiaryDetailViewModel : BaseViewModel
    {
        public Diary Item { get; set; }
        public DiaryDetailViewModel(Diary item = null)
        {
            //Title = item?.Text;
            Item = item;
        }
    }
}

using Xamarin.Forms;

namespace GodAndMe
{
    public class MyEditor : Editor
    {
        public void InvalidateLayout()
        {
            this.InvalidateMeasure();
        }
    }
}

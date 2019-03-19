using System;
using System.Threading.Tasks;

namespace GodAndMe.Interface
{
    public enum ContactSort
    {
        Last = 1,
        First = 2
    };

    public interface ISettings
    {
        bool GetTouchID();
        void SetTouchID(bool value);

        string GetYourName();
        void SetYourName(string value);

        int GetContactSorting();
        void SetContactSorting(int value);
    }
}

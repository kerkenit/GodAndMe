using System.Collections.Generic;
using System.Threading.Tasks;

namespace GodAndMe
{
    public interface IAddressBookInformation
    {
        List<string> GetContacts();
        Task<bool> RequestAccess();
    }
}
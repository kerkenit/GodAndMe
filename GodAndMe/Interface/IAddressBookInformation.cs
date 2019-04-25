using System.Collections.Generic;
using System.Threading.Tasks;

namespace GodAndMe
{
    public interface IAddressBookInformation
    {
        Task<List<string>> GetContacts();
        Task<bool> RequestAccess();
        Task<bool> IsAuthorized();
    }
}
using System.Threading.Tasks;

namespace GodAndMe.DependencyServices
{
    public interface ITouchID
    {
        Task<bool> AuthenticateUserIDWithTouchID();
        bool CanAuthenticateUserIDWithTouchID();
    }
}
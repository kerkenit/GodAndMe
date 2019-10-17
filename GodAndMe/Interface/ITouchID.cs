using System.Threading.Tasks;

namespace GodAndMe.DependencyServices
{
    public enum LocalAuthType
    {
        None,
        Passcode,
        TouchId,
        FaceId
    }

    public interface ITouchID
    {
        Task<bool> AuthenticateUserIDWithTouchID();
        bool CanAuthenticateUserIDWithTouchID();
        LocalAuthType GetLocalAuthType();
    }
}
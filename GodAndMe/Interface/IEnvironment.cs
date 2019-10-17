using System.Threading.Tasks;

namespace GodAndMe
{
    public interface IEnvironment
    {
        Theme GetOperatingSystemTheme();
        //Task<Theme> GetOperatingSystemTheme();
    }

    public enum Theme { Light, Dark }
}

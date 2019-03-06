using Foundation;
using GodAndMe.DependencyServices;
using GodAndMe.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(VersionAndBuild_iOS))]
namespace GodAndMe.iOS.DependencyServices
{
    public class VersionAndBuild_iOS : IAppVersionAndBuild
    {
        public string GetVersionNumber()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();
        }
        public string GetBuildNumber()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();
        }
    }
}
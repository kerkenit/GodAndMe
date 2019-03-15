using Android.Content;
using Android.Content.PM;
using GodAndMe.DependencyServices;
using GodAndMe.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(VersionAndBuild_Android))]
namespace GodAndMe.Droid.DependencyServices
{
    public class VersionAndBuild_Android : IAppVersionAndBuild
    {

        /// <summary>
        /// Gets the current Application Context.
        /// </summary>
        /// <value>The activity.</value>

        PackageInfo _appInfo;
        public VersionAndBuild_Android()
        {
            Context context = global::Android.App.Application.Context;
            _appInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
        }
        public string GetVersionNumber()
        {
            return _appInfo.VersionName;
        }
        public string GetBuildNumber()
        {
            return _appInfo.VersionCode.ToString();
        }
    }
}
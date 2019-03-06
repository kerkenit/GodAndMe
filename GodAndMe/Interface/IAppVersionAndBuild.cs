namespace GodAndMe.DependencyServices
{
    public interface IAppVersionAndBuild
    {
        string GetVersionNumber();
        string GetBuildNumber();
    }
}
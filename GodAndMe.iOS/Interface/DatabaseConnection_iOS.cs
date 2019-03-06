using System.IO;
using GodAndMe.Interface;
using LocalDataAccess.iOS;
using SQLite;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection))]
namespace LocalDataAccess.iOS
{
    public class DatabaseConnection : IDatabaseConnection
    {
        public SQLiteConnection DbConnection()
        {
            string dbName = "GodAndMe.db3";
            string libraryFolder = Foundation.NSFileManager.DefaultManager.GetUrlForUbiquityContainer(null).AbsoluteString;
            string path = Path.Combine(libraryFolder, dbName);
            return new SQLiteConnection(path);
        }
    }
}
using SQLite;
using LocalDataAccess.Droid;
using System.IO;
using GodAndMe.Interface;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection))]
namespace LocalDataAccess.Droid
{
    public class DatabaseConnection : IDatabaseConnection
    {
        public SQLiteConnection DbConnection()
        {
            string dbName = "GodAndMe.db3";
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
            return new SQLiteConnection(path, true);
        }
    }
}
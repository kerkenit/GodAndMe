using System;
using System.IO;
using Foundation;
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
            string dbName = "GodAndMe.db3", path = null, libraryFolder = null;
            try
            {
                string docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                libraryFolder = Path.Combine(docsPath, "..", "Library");
                path = Path.Combine(libraryFolder, dbName);
            }
            catch (NullReferenceException)
            {
                try
                {
                    libraryFolder = NSFileManager.DefaultManager.GetUrlForUbiquityContainer(null).AbsoluteString;
                    path = Path.Combine(libraryFolder, dbName);

                }
                catch (NullReferenceException)
                {
                    try
                    {
                        libraryFolder = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.LibraryDirectory, NSSearchPathDomain.User)[0].AbsoluteString;
                        path = Path.Combine(libraryFolder, dbName);
                    }
                    catch (NullReferenceException)
                    {

                    }
                }
            }
            if (path != null)
            {
                return new SQLiteConnection(path);
            }
            return new SQLiteConnection(dbName);
        }
    }
}
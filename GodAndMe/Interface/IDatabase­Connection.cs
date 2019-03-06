using SQLite;

namespace GodAndMe.Interface
{
    public interface IDatabaseConnection
    {
        SQLiteConnection DbConnection();
    }
}
using System;
using System.Threading.Tasks;

namespace GodAndMe.Interface
{
    public interface IShare
    {
        Task Show(string title, string message, string filePath);
    }
}

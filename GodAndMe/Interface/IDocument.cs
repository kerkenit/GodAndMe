using System;
using System.Threading.Tasks;

namespace GodAndMe.Interface
{
    public interface IDocument
    {
        Task<string> GetFile();
    }
}

using System;
using GodAndMe.Interface;

namespace GodAndMe.iOS.Interface
{
    public class FileStore : IFileStore
    {
        public string GetFilePath()
        {
            return "image.png";
        }
    }
}
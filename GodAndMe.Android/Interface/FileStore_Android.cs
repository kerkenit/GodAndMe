using System;
using System.IO;
using GodAndMe.Interface;
using Environment = Android.OS.Environment;
namespace GodAndMe.Droid.Interface
{
    public class FileStore : IFileStore
    {
        public string GetFilePath()
        {
            return Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath, "image.png");
        }
    }
}

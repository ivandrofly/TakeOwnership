using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TakeOwnership.Helpers
{
    public static class FileUtils
    {
        /// <summary>
        /// True if current paremeter is file, false if it's directory...
        /// </summary>
        public static bool IsFile(string fs) => !((File.GetAttributes(fs) & FileAttributes.Directory) == FileAttributes.Directory);
    }
}

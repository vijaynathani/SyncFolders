using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFolderOverNetwork
{
    class DirectoryCheck
    {
        public static string GetCurrentDirectoryName() => TrimName(Directory.GetCurrentDirectory());
        public static string TrimName(string path) => path[(path.LastIndexOf('\\') + 1)..][(path.LastIndexOf('/') + 1)..];
        private static bool AreDirsSame(string dir1, string dir2)
        {
            return TrimName(dir1) == TrimName(dir2);
        }

        public static bool AreDirectoryNamesMatching(Stream stream)
        {
            var myDir = Directory.GetCurrentDirectory();
            Transfer.Send(myDir, stream);
            var remoteDir = (string) Transfer.Receive(stream);
            ConvertNames.Obj = new ConvertNames(myDir, remoteDir);
            if (AreDirsSame(myDir,remoteDir)) return true;
            Console.Error.WriteLine("Current directory name {0}, does not match remote directory name {1}", TrimName(myDir), TrimName(remoteDir));
            return false;
        }

        public static bool Ignore(string directory) => directory.EndsWith(".git"); //.git dirs tend to give permission errors.//To copy .git dirs also, return false here.
    }
}

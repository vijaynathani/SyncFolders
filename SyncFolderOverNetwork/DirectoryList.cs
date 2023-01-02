using System.Collections.Generic;
using System.IO;

namespace SyncFolderOverNetwork
{
    internal class DirectoryList
    {
        private readonly HashSet<string> _names = new();
        private const string CurrentDir = ".";
        private readonly bool convertPath;

        private DirectoryList(bool convertPath) => this.convertPath = convertPath;

        public static ISet<string> GetEntireDirectoryTreeFolderNames(bool convertPathNameToRemote = false)
        {
            var fs = new DirectoryList(convertPathNameToRemote);
            fs.AddChildDirectories(CurrentDir);
            fs._names.TrimExcess();
            return fs._names;
        }

        private void AddChildDirectories(string dir)
        {
            if (DirectoryCheck.Ignore(dir)) return;
            foreach (var d in Directory.GetDirectories(dir))
            {
                _names.Add(convertPath?ConvertNames.Obj.ConvertLocalToRemote(d):d);
                AddChildDirectories(d);
            }
        }
    }
}
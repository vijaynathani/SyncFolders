using System.Collections.Generic;
using System.IO;

namespace SyncFolderOverNetwork
{
    internal class FileList
    {
        private const string CurrentDir = ".";
        private readonly HashSet<FileDetails> _files = new HashSet<FileDetails>();
        private readonly bool convertPath;
        private FileList(bool convertPath) => this.convertPath = convertPath;

        public static ISet<FileDetails> GetEntireDirectoryTreeFileNames(bool convertPathNameToRemote = false)
        {
            var fs = new FileList(convertPathNameToRemote);
            fs.AddChildFiles(CurrentDir);
            fs._files.TrimExcess();
            return fs._files;
        }

        private void AddChildFiles(string dir)
        {
            if (DirectoryCheck.Ignore(dir)) return;
            foreach (var f in Directory.GetFiles(dir))
                _files.Add(Create(f));
            foreach (var d in Directory.GetDirectories(dir))
                AddChildFiles(d);
        }
        private FileDetails Create(string filename) => 
            convertPath ? FileDetails.GetWithNameConvertedToRemote(filename) : FileDetails.Get(filename);

    }
}
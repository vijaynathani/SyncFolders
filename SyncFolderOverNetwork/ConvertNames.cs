using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncFolderOverNetwork
{
    internal class ConvertNames
    {
        public static ConvertNames Obj;
        private readonly char source, remote;
        public ConvertNames(string myPath, string remotePath) {
            source = FindSlashDirection(myPath);
            remote = FindSlashDirection(remotePath);
        }

        private char FindSlashDirection(string path) => path.Contains('/') ? '/' : '\\';
        public string ConvertLocalToRemote(string path) => source == remote? path : path.Replace(source, remote);
        public string ConvertRemoteToLocal(string path) => source == remote? path : path.Replace(remote, source);
        public ISet<string> ConvertLocalToRemote(ISet<string> paths)
        {
            if (source == remote) return paths;
            var r = new HashSet<string>();
            foreach (var path in paths)
                r.Add(ConvertLocalToRemote(path));
            r.TrimExcess();
            return r;
        }
        public ISet<FileDetails> ConvertLocalToRemote(ISet<FileDetails> files)
        {
            if (source == remote) return files;
            var r = new HashSet<FileDetails>();
            foreach (var f in files)
                r.Add(new FileDetails(ConvertLocalToRemote(f.NameOfFile),f.LastModified, f.Size));
            r.TrimExcess();
            return r;
        }
    }
}

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
        private char FindSlashDirection(string path) => path.StartsWith('/') ? '/' : '\\';
        public string ConvertLocalToRemote(string path) => ArePathsSameKind? path : path.Replace(source, remote);
        public string ConvertRemoteToLocal(string path) => ArePathsSameKind? path : path.Replace(remote, source);
        public bool ArePathsSameKind => source == remote;
    }
}

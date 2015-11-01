using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace SyncFolderOverNetwork
{
    internal class DestinationFolder
    {
        private readonly string _sourceIP;
        private BinaryReader _br;
        private Stream _stream;

        public DestinationFolder(string sourceIP)
        {
            _sourceIP = sourceIP;
        }

        public void Start()
        {
            var client = new TcpClient(_sourceIP, SourceFolder.Port);
            _stream = new BufferedStream(client.GetStream());
            _br = new BinaryReader(_stream);
            try
            {
                StartTranfer();
            }
            finally
            {
                Transfer.Send(SourceFolder.EndConnection, _stream);
                _br.Close();
                client.Close();
            }
        }

        private void StartTranfer()
        {
            if (!DirectoryCheck.AreDirectoryNamesMatching(_stream)) return;
            SyncFolders();
            SyncFiles();
        }

        private void SyncFiles()
        {
            Console.WriteLine("Receiving List of files.");
            var destinationFiles = (ISet<FileDetails>) Transfer.Receive(_stream);
            Console.WriteLine("Deleting extra files.");
            foreach (FileDetails f in new FileSync(destinationFiles).DeleteExtraFiles().GetFilesToBeTransferred())
            {
                Console.WriteLine("Receiving {0}", f);
                Transfer.ReceiveFile(f, _stream, _br);
            }
        }

        private void SyncFolders()
        {
            Console.WriteLine("Receiving Folder List.");
            var desitnationFolders = (ISet<string>) Transfer.Receive(_stream);
            Console.WriteLine("Deleting/Creating folders.");
            new FolderSync(desitnationFolders).MakeTreeEqual();
        }
    }
}
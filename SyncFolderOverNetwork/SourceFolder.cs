using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SyncFolderOverNetwork
{
    internal class SourceFolder
    {
        public const int Port = 8901;

        public const string EndConnection =
            "**End of Communiction with Client. All Files Transferred. Program by Vijay Nathani.**";

        private readonly string _ipAddress;
        private BinaryWriter _bw;
        private Socket _socket;
        private Stream _stream;

        public SourceFolder(string ipAddress)
        {
            _ipAddress = ipAddress.Trim();
        }

        public void Start()
        {
            WaitForClientToConnect();
            try
            {
                TransferInfo();
            }
            finally
            {
                _bw.Close();
            }
        }

        private void WaitForClientToConnect()
        {
            IPAddress ipAddress = IPAddress.Parse(_ipAddress);
            var listener = new TcpListener(ipAddress, Port);
            listener.Start();
            _socket = listener.AcceptSocket();
            _stream = new BufferedStream(new NetworkStream(_socket));
            _bw = new BinaryWriter(_stream);
            Console.WriteLine("Destination machine connected.");
            listener.Stop();
        }

        private void TransferInfo()
        {
            if (!DirectoryCheck.AreDirectoryNamesMatching(_stream))
            {
                Transfer.Receive(_stream); //Wait for EndConnection message
                return;
            }
            SendFolderNames();
            SendFileNames();
            SendSpecificFileContents();
        }

        private void SendSpecificFileContents()
        {
            Console.WriteLine("Waiting for fileName that has to be transferred.");
            while (true)
            {
                var fileName = ConvertNames.Obj.ConvertRemoteToLocal((string)Transfer.Receive(_stream));
                if (fileName == EndConnection) return;
                new SendFile(fileName, _bw).Start();
            }
        }

        private void SendFileNames()
        {
            Console.WriteLine("Sending List of all Files.");
            SendObject(FileList.GetEntireDirectoryTreeFileNames(true));
        }

        private void SendFolderNames()
        {
            Console.WriteLine("Sending Folder List.");
            SendObject(DirectoryList.GetEntireDirectoryTreeFolderNames(true));
        }

        private void SendObject(Object o)
        {
            Transfer.Send(o, _stream);
        }
    }
}
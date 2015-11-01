using System;

namespace SyncFolderOverNetwork
{
    internal class Program
    {
        private const string CopyRight = "Software by Vijay Nathani. Details at http://goo.gl/KnXctI";

        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("Usage: <<Program>> {-s|-d} ip-address-of-source");
                Environment.Exit(1);
            }
            Console.WriteLine(CopyRight);
            var mesg = "Starting {0} in current directory: " + DirectoryCheck.GetCurrentDirectoryName();
            switch (args[0])
            {
                case "-s":
                    Console.WriteLine(mesg, "source");
                    new SourceFolder(args[1]).Start();
                    break;
                case "-d":
                    Console.WriteLine(mesg, "destination");
                    new DestinationFolder(args[1]).Start();
                    break;
                default:
                    Console.Error.WriteLine("The first parameter should '-s' for source or '-d' for destination");
                    break;
            }
        }
    }
}
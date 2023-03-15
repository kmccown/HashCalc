using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HashCalc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            compareDirectories();
        }



        //simply output the sha256 hash of a file
        public static void checkHash()
        {

            string test = "";
            string path = @"file\path";
            FileInfo file = new FileInfo(path);
            FileStream filestream = file.Open(FileMode.Open);
            using (SHA256 mySHA256 = SHA256.Create())
            {
                test = Convert.ToBase64String(mySHA256.ComputeHash(filestream), Base64FormattingOptions.InsertLineBreaks);
            }
            Console.WriteLine(test);
            Console.ReadLine();
        }

        //compare all sha256 hashes of files in two directories
        //this has been made to work with a filestructure of 2-folder deep directories
        //compares each file in each folder in each directory to the other directory based on them having the same name
        //writes any filenames where the hashes don't match to the errorSheet
        public static void compareDirectories()
        {

            string dir1 = @"directorypath1";
            string dir2 = @"directorypath2";
            string test1 = "";
            string test2 = "";

            string errorSheet = "start\n";

            var direct1 = new DirectoryInfo(dir1);
            var direct2 = new DirectoryInfo(dir2);


            FileStream filestream = null;
            FileStream filestream2 = null;


            foreach (var dir in direct1.GetDirectories())
            {
                foreach (var dir0 in direct2.GetDirectories())
                {
                    foreach (FileInfo file in dir.GetFiles())
                    {
                        foreach (FileInfo file2 in dir0.GetFiles())
                        {
                            if (dir0.Name == dir.Name)
                            {
                                if (file.Name == file2.Name)
                                {
                                    filestream = file.Open(FileMode.Open);
                                    filestream2 = file2.Open(FileMode.Open);
                                    using (SHA256 mySHA256 = SHA256.Create())
                                    {
                                        test1 = Convert.ToBase64String(mySHA256.ComputeHash(filestream), Base64FormattingOptions.InsertLineBreaks);
                                        test2 = Convert.ToBase64String(mySHA256.ComputeHash(filestream2), Base64FormattingOptions.InsertLineBreaks);
                                    }
                                    if (test1 != test2)
                                    {
                                        errorSheet += dir.Name;
                                        errorSheet += "\n";
                                        errorSheet += file.Name;
                                        errorSheet += "\n";
                                    }
                                    filestream.Close();
                                    filestream2.Close();
                                }
                            }
                        }
                    }
                }
            }
            

            errorSheet += "end\n";
            Console.WriteLine(errorSheet);
            Console.ReadLine();
        }
    }
}
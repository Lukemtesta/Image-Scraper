using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace DatasetCreator
{
    class Program
    {
        /// <summary>
        /// Separates an image database into a training and test set.
        /// 
        /// TODO: Write a C# command line parser
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if(args.Count() < 4)
            {
                Console.WriteLine("Please provide <path to training data> <path to test data>");
                return;
            }

            var dir_train = Directory.GetDirectories(args[0]);
            var dir_test = Directory.GetDirectories(args[1]);

            CreateDataset(dir_train, args[2]);
            CreateDataset(dir_test, args[3]);
        }

        static void CreateDataset(string[] rootdir, string filename)
        {
            // Tag filestream for collection before end of method scope
            using (FileStream fs = File.Create(filename))
            {
                for (int i = 0; i < rootdir.Count(); ++i)
                {
                    Directory.GetFiles(rootdir[i]).ToList().ForEach(name =>
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes(name + " " + i + "\n");
                        fs.Write(info, 0, info.Length);
                    });
                }
            }
        }
    }
}

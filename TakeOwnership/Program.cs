using CommandLine;
using System;
using System.IO;
using TakeOwnership.Helpers;
using TakeOwnership.Models;

namespace TakeOwnership
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Option>(args)
                .WithParsed<Option>(opt =>
                {

                    // neither file or directory is set to be processed
                    if (!(opt.File || opt.Directory))
                    {
                        WriteErrorMessage("One of the option must be enabled (file/directory)");
                    }

                    // invalid path
                    if (!Path.IsPathRooted(opt.Target))
                    {
                        WriteErrorMessage("Invalid <<Target>> path entered!");
                    }

                    if (opt.IsRecursive)
                    {
                        foreach (var fs in Directory.GetFileSystemEntries(opt.Target))
                        {
                            if (FileUtils.IsFile(fs) && opt.File == false)
                            {
                                // todo: display warning message if version is set to true in option
                                continue;
                            }

                            // directory
                            if (!FileUtils.IsFile(fs) && opt.Directory == false)
                            {
                                // todo: display warning message if version is set to true in option
                                continue;
                            }

                            // update ownership

                        }
                    }

                });
        }


        private static void WriteErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void WriteWarningMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void WriteSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}

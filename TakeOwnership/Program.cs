using CommandLine;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
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

#if DEBUG
                DisplayObjectInfo(opt);
#endif

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

                // use option provied owner otherwise use current user as the new object / filesystem owner
                string newOwner = string.IsNullOrEmpty(opt.Owner) ? $"{Environment.UserDomainName}/{Environment.UserName}" : opt.Owner;


                    // NOTE: This doesn't seem to work for now (the domain-name needs to pre prepended with user name)
                    //var identity = new NTAccount(Environment.UserName);

                    // the new object owner
                    var identity = new NTAccount(Environment.UserDomainName, string.IsNullOrWhiteSpace(opt.Owner) ? Environment.UserName : opt.Owner);

                    Console.WriteLine($"New file owner: {identity.Value}");

                    // TODO: Refactor
                    if (opt.IsRecursive)
                    {
                        var siType = typeof(System.Security.Principal.SecurityIdentifier);
                        foreach (var fs in Directory.GetFileSystemEntries(opt.Target))
                        {
                            bool isFile = FileUtils.IsFile(fs);
                            if (isFile && opt.File == false)
                            {
                                // todo: display warning message if version is set to true in option
                                continue;
                            }

                            // directory
                            if (!isFile && opt.Directory == false)
                            {
                                // todo: display warning message if version is set to true in option
                                continue;
                            }

                            FileSystemSecurity fsSecurity = default;
                            try
                            {
                                if (isFile)
                                {
                                    fsSecurity = File.GetAccessControl(fs);

                                    var owner = fsSecurity.GetOwner(siType);

                                    // trying to set same owner
                                    if (owner.Value.Equals(identity.Value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }

                                    Console.WriteLine($"Old-owner: {owner.Value}");


                                    fsSecurity.PurgeAccessRules(owner);
                                    WriteSuccessMessage("all access rule purge for old-owner");

                                    //Console.WriteLine(fileSecurity.GetOwner(typeof(System.Security.Principal.SecurityIdentifier)))
                                    fsSecurity.SetOwner(identity);
                                    WriteSuccessMessage("New owner set with success");

                                    // update file access control
                                    File.SetAccessControl(fs, (FileSecurity)fsSecurity);
                                }
                                else
                                {

                                    fsSecurity = Directory.GetAccessControl(fs);

                                    var owner = fsSecurity.GetOwner(siType);
                                    Console.WriteLine($"Old-owner: {owner.Value}");

                                    // purge access rule for old owner
                                    fsSecurity.PurgeAccessRules(owner);
                                    WriteSuccessMessage("all access rule purge for old-owner");

                                    fsSecurity.SetOwner(identity);
                                    WriteSuccessMessage("New owner set with success");

                                    // update directory acces control
                                    Directory.SetAccessControl(fs, (DirectorySecurity)fsSecurity);
                                }

                                WriteSuccessMessage($"{Path.GetFileName(fs)}");
                            }
                            catch (Exception ex)
                            {
                                WriteErrorMessage(ex.Message);
                                WriteWarningMessage($"Fails processing: {Path.GetFileName(fs)}");

                                break;
                            }
                        }
                    }

                });
        }

        private static bool ShouldSetOwner(string oldOwner, string newOwner)
        {
            if (oldOwner.Equals(newOwner, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
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

        private static void DisplayObjectInfo(object obj)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                Console.WriteLine($"Prop: {prop.Name}, Value: {prop.GetValue(obj)?.ToString()}");
            }
        }
    }
}


// command: TakeOwnership.exe -t e:/source -f true -d true -r true
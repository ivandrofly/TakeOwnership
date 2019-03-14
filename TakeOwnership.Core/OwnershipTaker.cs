using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using TakeOwnership.Helpers;

namespace TakeOwnership
{
    using Core.Helpers;

    public class OwnershipTaker
    {
        public void TakeOwnerShip(string path, TakeOwnershipContext context)
        {
            if (!(context.Options.File || context.Options.Directory))
            {
                return;
            }

            var securityId = typeof(SecurityIdentifier);

            foreach (var directory in Directory.GetDirectories(path))
            {
                // child directories must be processed aswell
                if (context.Options.IsRecursive)
                {
                    Task.Run(() =>
                    {
                        TakeOwnerShip(directory, context);
                    }).ConfigureAwait(false);
                }

                // allow set directory ownership
                if (context.Options.Directory)
                {
                    // get dir ownership
                    var dirAccessCtrl = Directory.GetAccessControl(directory);

                    if (context.Options.PurgeAllOtherAccess)
                    {
                        dirAccessCtrl.PurgeAllAccess<SecurityIdentifier>();
                    }

                    // set new owner
                    dirAccessCtrl.SetOwner(context.Owner);

                    try
                    {
                        // set access rule async
                        Directory.SetAccessControl(directory, dirAccessCtrl);
                        Debug.WriteLine($"Access set to {Path.GetFileName(directory)}");
                    }
                    catch (UnauthorizedAccessException uae)
                    {
                        Debug.WriteLine(uae.Message);
#if !DEBUG
                        throw uae;  
#endif
                    }
                }
            }

            // take file ownership
            if (context.Options.File)
            {
                // get all file in current directory
                foreach (string file in Directory.GetFiles(path, "*.*"))
                {
                    var fileAccessCtrl = File.GetAccessControl(file);

                    // remove all other owners of type SecurityIdentifier access
                    if (context.Options.PurgeAllOtherAccess)
                    {
                        fileAccessCtrl.PurgeAllAccess<SecurityIdentifier>();
                    }

                    fileAccessCtrl.SetOwner(context.Owner);

                    // set access rule async
                    try
                    {
                        File.SetAccessControl(file, fileAccessCtrl);
                        Debug.WriteLine($"Access set to {Path.GetFileName(file)}");
                    }
                    catch (UnauthorizedAccessException uae)
                    {
                        // notify the debug listener
                        Debug.WriteLine(uae.Message);
#if !DEBUG
                        throw uae;
#endif
                    }
                }
            }

        }

        public IEnumerable<FileSystemAccessRule> GetUsersWithPermission(string path)
        {
            FileSystemSecurity fsSecurity = default;

            if (FileUtils.IsFile(path))
            {
                fsSecurity = File.GetAccessControl(path);
            }
            else
            {
                fsSecurity = Directory.GetAccessControl(path);
            }
            //var ntAccountType = typeof(NTAccount);
            foreach (FileSystemAccessRule fsAccessRule in fsSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier)))
            {
                yield return fsAccessRule;
            }
        }
    }
}
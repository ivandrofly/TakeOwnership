using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;

namespace TakeOwnership
{
    public class OwnershipTaker
    {
        public void TakeOwnerShip(string path, TakeOwnershipContext context)
        {
            // TODO: Handle stackoverflow in case of very deep directories

            if (!(context.Options.File || context.Options.Directory))
            {
                //return Task.FromCanceled(CancellationToken.None);
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

                    // purge all access from previous owner
                    var oldOwner = dirAccessCtrl.GetOwner(securityId);
                    dirAccessCtrl.PurgeAccessRules(oldOwner);

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
                        //throw uae;
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

                    var oldOwner = fileAccessCtrl.GetOwner(securityId);

                    // purge old-owner access
                    fileAccessCtrl.PurgeAccessRules(oldOwner);
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
                        //throw uae;
                    }

                }
            }

        }

    }
}

// NOTE: RUN AS ADMINISTRATOR
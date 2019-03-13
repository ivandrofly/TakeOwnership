using CommandLine;
using System;
using System.Security.Principal;
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
                var ownershipTake = new OwnershipTaker();
                //var identity = new NTAccount(Environment.UserName); // doesn't seems to work
                var identity = new NTAccount(Environment.UserDomainName, string.IsNullOrWhiteSpace(opt.Owner) ? Environment.UserName : opt.Owner);
                Console.WriteLine($"Setting new file owner to: {identity.Value}");
                var context = new TakeOwnershipContext
                {
                    Options = opt,
                    Owner = identity,
                };

                var ownerShipTaker = new OwnershipTaker();
                ownerShipTaker.TakeOwnerShip(opt.Target, context);
                Console.WriteLine("Done...");

            });
        }
    }
}


// command: TakeOwnership.exe -t e:/source -f true -d true -r true

// TODO: 
// - Separate console with library
// - add unit-test
// - upload to nuget
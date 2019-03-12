using CommandLine;
using System;
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

                });
        }
    }
}

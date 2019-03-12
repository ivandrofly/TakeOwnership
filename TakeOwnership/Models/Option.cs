using CommandLine;

namespace TakeOwnership.Models
{
    class Option
    {
        [Option('t', "target", Required = true, HelpText = "The target directory to be processed")]
        /// <summary>
        /// Target path
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// True if files should be processed
        /// </summary>
        [Option('f', "file", HelpText = "True if files should be processed")]
        public bool File { get; set; }

        /// <summary>
        /// True if files should be processed
        /// </summary>
        [Option('d', "directory", HelpText = "True if files should be processed")]
        public bool Directory { get; set; }

        /// <summary>
        /// True if all sub-directories should be processed
        /// </summary>
        [Option('r', "recursive", HelpText = "True if all sub-directories should be processed")]
        public bool IsRecursive { get; set; }


        /// <summary>
        /// True if app should be more verbose
        /// </summary>
        [Option('v', "verbose", HelpText = "True if app should be more verbose")]
        public bool Verbose { get; set; }

        /// <summary>
        /// Name for the new owner (e.g: domain-name\username)
        /// </summary>
        [Option('o', "owner", Required = false, HelpText = "New owner name")]
        public string Owner { get; set; }
    }
}

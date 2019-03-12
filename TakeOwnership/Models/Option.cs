using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace TakeOwnership.Models
{
    class Option
    {
        [Option('t', "target", Required = true, HelpText ="The target directory to be processed")]
        /// <summary>
        /// Target path
        /// </summary>
        public string Target { get; set; }

        [Option('f', "file", HelpText = "True if files should be processed")]
        /// <summary>
        /// True if files should be processed
        /// </summary>
        public bool File { get; set; }

        [Option('f', "file", HelpText = "True if files should be processed")]
        /// <summary>
        /// True if files should be processed
        /// </summary>
        public bool Directory { get; set; }

        [Option('r', "recursive", HelpText = "True if all sub-directories should be processed")]

        /// <summary>
        /// True if all sub-directories should be processed
        /// </summary>
        public bool IsRecursive { get; set; }
    }
}

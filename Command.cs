using System;
using System.Globalization;

namespace GZipTest {
	abstract class Command : ICommand {
        /// <summary>
        /// Command text.
        /// </summary>
		public abstract string Text { get; }
        
        /// <summary>
        /// Command can be accepted.
        /// </summary>
        /// <param name="text">Command text.</param>
        /// <returns>Can accept command.</returns>
		public bool Accept(string text) {
			return String.CompareOrdinal(text, Text) == 0; 
		}

        /// <summary>
        /// Execute command.
        /// </summary>
        /// <param name="source">Source file name.</param>
        /// <param name="result">Output file name.</param>
		public abstract void Execute(string source, string result);
	}
}

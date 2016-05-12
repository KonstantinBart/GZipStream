using System.IO;
using System.Text;
using System.Collections.Generic;
using System;
using System.Security;

namespace GZipTest {
    /// <summary>
    /// Validation.
    /// </summary>
	public static class Validation {

        /// <summary>
        /// Validate program arguments.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void ValidateArguments(string[] args) {

            if (args == null)
                throw new ArgumentException("Arguments are not set");

			//Check parameters count
			if (args.Length != 3) {
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Please enter 3 parameters:").
					AppendLine("- for compression: GZipTest.exe compress [source file name] [archive file name]").
					AppendLine("- for decompression: GZipTest.exe decompress [archive file name] [decompressed file name]");
                throw new ArgumentException(sb.ToString());
			}

			//Check commands.
			bool isCommandValid = false;
			var commands = new List<Command>() { new CompressCommand(), new DecompressCommand() };
			foreach (Command command in commands) {
				if (command.Accept(args[0])) {
					isCommandValid = true;
				}
			}
            if (!isCommandValid)
                throw new ArgumentException("Please use \"compress\" and \"decompress\" commands only as the first parameter.");

			//Check source file name.
            try
            {
                FileInfo fileFrom = new FileInfo(args[1]);
                if (!fileFrom.Exists)
                {
                    throw new ArgumentException("Please enter correct source file name.");
                }
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("Please enter correct source file name.", ex);
            }
            catch (SecurityException ex)
            {
                throw new ArgumentException("Please enter correct source file name.", ex);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Please enter correct source file name.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new ArgumentException("Please enter correct source file name.", ex);
            }
            catch (PathTooLongException ex)
            {
                throw new ArgumentException("Please enter correct source file name.", ex);
            }
            catch (NotSupportedException ex)
            {
                throw new ArgumentException("Please enter correct source file name.", ex);
            }
		}

	}
}

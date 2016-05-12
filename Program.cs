using System;
using System.Collections.Generic;
using System.Globalization;

[assembly: CLSCompliant(true)]
namespace GZipTest
{
	partial class Program
	{
		static int Main(string[] args)
		{
			//Add Ctrl-C event handler.
			_handler += new EventHandler(Handler);
			NativeMethods.SetConsoleCtrlHandler(_handler, true);

			try {
				Program.Start(args);
			} catch (Exception ex) {
				Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Error: {0}", ex.Message));
				return 1;
			}
			return 0;
		}

		static void Start(string[] args)
		{
            //Validate arguments.
            Validation.ValidateArguments(args);

			//Command pattern.
			var commands = new List<Command>() { new CompressCommand(), new DecompressCommand() };
			foreach (Command command in commands)
			{
				if (command.Accept(args[0]))
				{
                    Console.Clear();
                    Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}ion started", command.Text));
					command.Execute(args[1], args[2]);
                    Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}ion completed", command.Text));
				}
			}
		}

	}
}

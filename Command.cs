using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace GZipTest {
	abstract class Command : ICommand {
		public abstract string Text { get; }
		public bool Accept(string text) {
			return String.Compare(text, Text, false, CultureInfo.InvariantCulture) == 0; 
		}
		public abstract int Execute(string source, string result);
	}
}

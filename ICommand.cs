using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest {
	public interface ICommand {
		int Execute(string source, string result);
	}
}

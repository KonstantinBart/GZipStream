using System.IO;
using System.Text;

namespace GZipTest {
	public static class Validation {
		public static bool IsNotValid(string[] args, out string errors) {
			errors = string.Empty;
			
			//Check parameters count
			if (args.Length != 3) {
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Please enter 3 parameters:").
					AppendLine("- for compression: GZipTest.exe compress [source file name] [archive file name]").
					AppendLine("- for decompression: GZipTest.exe decompress [archive file name] [decompressed file name]");
				errors = sb.ToString();
				return false;
			}

			//Check file names
			if (!IsFileNameCorrect(args[1])) {
				errors = "Please enter correct source file name";
				return false;
			}
			if (!IsFileNameCorrect(args[2])) {
				errors = "Please enter correct result file name";
				return false;
			}
						
			return true;
		}

		private static bool IsFileNameCorrect(string fileName) {
			bool result = true;
			try {
				FileInfo fileFrom = new FileInfo(fileName);
				if (!fileFrom.Exists) {
					result = false;
				}
			} catch {
				result = false;
			}
			return result;
		}
	}
}

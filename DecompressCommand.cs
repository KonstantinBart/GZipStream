using System.IO;
using System.IO.Compression;

namespace GZipTest {
	class DecompressCommand : Command {
		const string _text = "decompress";

		public override void Execute(string source, string result) {
			using (FileStream compressedStream = new FileStream(source, FileMode.Open, FileAccess.Read)) {
				using (var gZipStream = new GZipStream(compressedStream, CompressionMode.Decompress)) {
					using (FileStream uncompressedStream = new FileStream(result, FileMode.Create, FileAccess.Write)) {
						GZipHelper.Zip(gZipStream, uncompressedStream);
					}
				}
			}
		}

		public override string Text {
			get {
				return _text;
			}
		}
	}
}

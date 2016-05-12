using System.IO;
using System.IO.Compression;

namespace GZipTest {
	class CompressCommand : Command {
		const string _text = "compress";

		public override void Execute(string source, string result) {
			using (var uncompressedStream = new FileStream(source, FileMode.Open, FileAccess.Read)) {
				using (var compressedStream = new FileStream(result, FileMode.Create, FileAccess.Write)) {
					using (var gZipStream = new GZipStream(compressedStream, CompressionMode.Compress)) {
						GZipHelper.Zip(uncompressedStream, gZipStream);
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

using System;
using System.IO;
using System.IO.Compression;

namespace GZipTest {
	class DecompressCommand : Command {
		const string _text = "decompress";
		const int bufferSize = 5;

		public override int Execute(string source, string result) {
			try {
				using (var fsInput = new FileStream(source, FileMode.Open, FileAccess.Read)) {
					using (var fsOutput = new FileStream(result, FileMode.Create, FileAccess.Write)) {
						using (var gzipStream = new GZipStream(fsInput, CompressionMode.Decompress)) {
							var buffer = new Byte[bufferSize];
							int h;
							while ((h = gzipStream.Read(buffer, 0, buffer.Length)) > 0) {
								fsOutput.Write(buffer, 0, h);
							}
						}
					}
				}
			} catch {
				return 1;
			}
			return 0;
		}

		public override string Text {
			get {
				return _text;
			}
		}
	}
}

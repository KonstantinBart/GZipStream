using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace GZipTest {
	class Program {
		static int Main(string[] args) {
			int result = 1;

			//string errors = string.Empty;
			//if (Validation.IsNotValid(args, out errors)) {
			//    Console.WriteLine(errors);
			//    return 1;
			//}

			//TODO: Remove init
			args = new string[3];
			args[0] = "compress";
			switch (args[0]) {
				case "compress":
					args[1] = @"C:\Kostya\temp\" + "00028.mts";
					//args[2] = @"C:\Kostya\temp\" + "arc.gz";
					args[2] = @"C:\Kostya\temp\" + "arc.mts";
					break;
				case "decompress":
					args[1] = @"C:\Kostya\temp\" + "arc.gz";
					args[2] = @"C:\Kostya\temp\" + "result.avi";
					break;
				default:
					throw new NotSupportedException();
			}

			//Command pattern
			var commands = new List<Command>() { new CompressCommand(), new DecompressCommand() };
			foreach (Command command in commands) {
				if (command.Accept(args[0])) {
					return command.Execute(args[1], args[2]);
				}
			}
			Console.ReadLine();
			return result;
		}

		//const int bufferSize = 5;

		//private static void Compress(string source, string arc) {
		//    string sourceFile = @"c:\temp\" + source;
		//    string destFile = @"c:\temp\" + arc;
		//    Compress(sourceFile, destFile, bufferSize);
		//}

		//private static void Decompress(string arc, string source) {
		//    string arcFile = @"c:\temp\" + arc;
		//    string decompressedFile = @"c:\temp\" + source;
		//    Decompress(arcFile, decompressedFile, bufferSize);
		//}

		//public static void Compress(String fileSource, String fileDestination, int buffsize) {
		//    using (var fsInput = new FileStream(fileSource, FileMode.Open, FileAccess.Read)) {
		//        using (var fsOutput = new FileStream(fileDestination, FileMode.Create, FileAccess.Write)) {
		//            using (var gzipStream = new GZipStream(fsOutput, CompressionMode.Compress)) {
		//                var buffer = new Byte[buffsize];
		//                int h;
		//                while ((h = fsInput.Read(buffer, 0, buffer.Length)) > 0) {
		//                    gzipStream.Write(buffer, 0, h);
		//                }
		//            }
		//        }
		//    }
		//}

		//public static void Decompress(String fileSource, String fileDestination, int buffsize) {
		//    using (var fsInput = new FileStream(fileSource, FileMode.Open, FileAccess.Read)) {
		//        using (var fsOutput = new FileStream(fileDestination, FileMode.Create, FileAccess.Write)) {
		//            using (var gzipStream = new GZipStream(fsInput, CompressionMode.Decompress)) {
		//                var buffer = new Byte[buffsize];
		//                int h;
		//                while ((h = gzipStream.Read(buffer, 0, buffer.Length)) > 0) {
		//                    fsOutput.Write(buffer, 0, h);
		//                }
		//            }
		//        }
		//    }
		//}

		//public static void Compress(DirectoryInfo directorySelected) {
		//    //foreach (FileInfo fileToCompress in directorySelected.GetFiles()) {
		//        using (FileStream originalFileStream = fileToCompress.OpenRead()) {
		//            if ((File.GetAttributes(fileToCompress.FullName) &
		//               FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz") {
		//                using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz")) {
		//                    using (GZipStream compressionStream = new GZipStream(compressedFileStream,
		//                       CompressionMode.Compress)) {
		//                        originalFileStream.CopyTo(compressionStream);

		//                    }
		//                }
		//                FileInfo info = new FileInfo(directoryPath + "\\" + fileToCompress.Name + ".gz");
		//                Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
		//                fileToCompress.Name, fileToCompress.Length.ToString(), info.Length.ToString());
		//            }

		//        }

		//}

		//public static void Decompress(FileInfo fileToDecompress) {
		//    using (FileStream originalFileStream = fileToDecompress.OpenRead()) {
		//        string currentFileName = fileToDecompress.FullName;
		//        string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

		//        using (FileStream decompressedFileStream = File.Create(newFileName)) {
		//            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress)) {
		//                decompressionStream.CopyTo(decompressedFileStream);
		//                Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
		//            }
		//        }
		//    }
		//}
	}
}

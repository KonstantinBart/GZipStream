using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace GZipTest
{
	public class Compressor
	{
		public byte[] CompressedBytes { get; set; }
		public long BeforeCompressionBytes { get; set; }
		public long AfterCompressionBytes { get; set; }

		public void CompressFile(string fileName)
		{
			using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				var uncompressedBytes = new byte[fileStream.Length];
				fileStream.Read(uncompressedBytes, 0, (int)fileStream.Length);
				CompressedBytes = CompressGzip(uncompressedBytes);
				BeforeCompressionBytes = fileStream.Length;
				AfterCompressionBytes = CompressedBytes.Length;
				fileStream.Close();
			}
		}

		/// <summary>
		/// Take a simple stream of uncompressed bytes and compress them
		/// </summary>
		/// <param name="uncompressedBytes"></param>
		/// <returns></returns>
		public byte[] CompressGzip(byte[] uncompressedBytes)
		{
			using (var memory = new MemoryStream())
			{
				using
					(var gZipStream =
						new GZipStream(memory, CompressionMode.Compress, true))
				{
					gZipStream.Write
						(uncompressedBytes, 0, uncompressedBytes.Length);
				}
				return memory.ToArray();
			}
		}
	}
}

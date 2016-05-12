using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GZipTest
{
	public sealed class GZipHelper
	{
		static object _locker = new object();
        static readonly Semaphore _semaphore = new Semaphore(0, Int32.MaxValue);
        static readonly Queue<byte[]> _queue = new Queue<byte[]>();
        static Exception _innerException;
        const int _bufferLength = 4 * 1024;

		/// <summary>
		/// Buffer length.
		/// </summary>
		public static int BufferLength
		{
			get { return _bufferLength; }
		}

        private GZipHelper() { }

        /// <summary>
        /// GZip proccessing.
        /// </summary>
        /// <param name="sourceStream">Reading stream.</param>
        /// <param name="outputStream">Writing stream.</param>
		internal static void Zip(Stream sourceStream, Stream outputStream) {
			Thread writeThread = new Thread(Write);
			writeThread.Start(outputStream);
			try {
				while (true) {
					var buffer = new byte[BufferLength];
					int bytesCount = sourceStream.Read(buffer, 0, BufferLength);
					if (bytesCount == 0) {
						break;
					}
					if (bytesCount < BufferLength) {
						buffer = buffer.Take(bytesCount);
					}
					EnqueueBlock(buffer);
				}
			} finally {
				EnqueueBlock(null);
				writeThread.Join();
			}
			if (_innerException != null)
				throw _innerException;
		}

		static void EnqueueBlock(byte[] buffer) {
			lock (_locker) {
				_queue.Enqueue(buffer);
				_semaphore.Release();
			}
		}

        static void Write(object parameter) {
            Stream outputStream = parameter as Stream;
            try {
                while (true) {
                    _semaphore.WaitOne();
                    byte[] buffer;
                    lock (_locker)
                        buffer = _queue.Dequeue();
                    if (buffer == null)
                        break;
                    outputStream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex) {
                _innerException = ex;
            }
        }
				
	}

}

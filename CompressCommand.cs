using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Collections.Generic;

namespace GZipTest
{
	class CompressCommand : Command
	{
		const string _text = "compress";
		
		const int _blockSize = 512 * 1024;
		public int BlockLength
		{
			get { return _blockSize; }
		}

		static object locker = new object();

		public override int Execute(string source, string result)
		{
			try
			{
				Compress(source, result);
			}
			catch
			{
				return 1;
			}
			return 0;
		}

		ProducerConsumer<byte[]> q = new ProducerConsumer<byte[]>();

		private void Compress(string source, string result)
		{
			long fileLength = new FileInfo(source).Length;
			Byte[] block = null;
			long currentPosition = 0;
			List<Thread> readThreads = new List<Thread>();
			while (currentPosition < fileLength)
			{
				long bytesToEnd = fileLength - currentPosition;
				long bufferSize = bytesToEnd <= BlockLength ? bytesToEnd : BlockLength;

				Thread thread = new Thread(() => { block = ReadBlock(source, bufferSize, currentPosition); });
				readThreads.Add(thread);
				currentPosition += bufferSize;
			}

			foreach (var thread in readThreads)
			{
				thread.Start();
				while (thread.IsAlive)
				    continue;
				//q.Enqueue(block);
			}

			q.Stop();

			foreach (var t in readThreads)
				t.Join();

			//thread.Start();
			//while (thread.IsAlive)
			//    continue;



			//CommandArguments arguments = new CommandArguments(0, 1, source);
			
			////using (var fsOutput = new FileStream(result, FileMode.Create, FileAccess.Write))
			////{
			//    while (arguments.CurrentPosition < arguments.FileSize)
			//    {
			//        BlockLoader loader = new BlockLoader(arguments);
			//        Thread readThread = new Thread(() => { block = loader.ReadBlock(); });
			//        readThread.Start();
			//        while (readThread.IsAlive)
			//            continue;
			//        //TODO: Add correct key handler with removing all threads!
			//        if (Console.KeyAvailable)
			//        {
			//            break;
			//        }

			//        //fsOutput.Seek(arguments.CurrentPosition - arguments.BlockSize, SeekOrigin.Begin);
			//        //fsOutput.Write(block, 0, block.Length);
			//    };
			////}
		}

		public static byte[] ReadBlock(string source, long bufferSize, long currentPosition)
		{
			lock (locker)
			{
				byte[] buffer = new byte[bufferSize];
				using (var fsInput = new FileStream(source, FileMode.Open, FileAccess.Read))
				{
					{
						fsInput.Seek(currentPosition, SeekOrigin.Begin);
						fsInput.Read(buffer, 0, buffer.Length);
					}
				}
				return buffer;
			}
		}
			


		//    //using (var fsInput = new FileStream(source, FileMode.Open, FileAccess.Read)) {
		//    //    using (var fsOutput = new FileStream(result, FileMode.Create, FileAccess.Write)) {
		//    //        using (var gzipStream = new GZipStream(fsOutput, CompressionMode.Compress)) {
		//    //            var buffer = new Byte[bufferSize];
		//    //            int h;
		//    //            while ((h = fsInput.Read(buffer, 0, buffer.Length)) > 0) {
		//    //                gzipStream.Write(buffer, 0, h);
		//    //            }
		//    //        }
		//    //    }
		//    //}
		//}

		public override string Text
		{
			get
			{
				return _text;
			}
		}

	}

	public class ProducerConsumer<T> where T : class
	{
		object mutex = new object();
		Queue<T> queue = new Queue<T>();
		bool isDead = false;

		public void Enqueue(T task)
		{
			if (task == null)
				throw new ArgumentNullException("task");
			lock (mutex)
			{
				if (isDead)
					throw new InvalidOperationException("Queue already stopped");
				queue.Enqueue(task);
				Monitor.Pulse(mutex);
			}
		}

		public T Dequeue()
		{
			lock (mutex)
			{
				while (queue.Count == 0 && !isDead)
					Monitor.Wait(mutex);

				if (queue.Count == 0)
					return null;

				return queue.Dequeue();
			}
		}

		public void Stop()
		{
			lock (mutex)
			{
				isDead = true;
				Monitor.PulseAll(mutex);
			}
		}
	}

	public class BlockLoader
	{
		CommandArguments _arguments;
		static object locker = new object();

		public CommandArguments Arguments
		{
			get { return _arguments; }
			set { _arguments = value; }
		}

		public BlockLoader(CommandArguments arguments)
		{
			_arguments = arguments;
		}

		public byte[] ReadBlock()
		{
			lock (locker)
			{
				long bytesToEnd = Arguments.FileSize - Arguments.CurrentPosition;
				long bufferSize = bytesToEnd <= Arguments.BlockSize ? bytesToEnd : Arguments.BlockSize;
				byte[] buffer = new byte[bufferSize];
				using (var fsInput = new FileStream(Arguments.FileName, FileMode.Open, FileAccess.Read))
				{
					{
						var ere = fsInput.Seek(Arguments.CurrentPosition, SeekOrigin.Begin);
						fsInput.Read(buffer, 0, buffer.Length);
						Arguments.CurrentPosition += buffer.Length;
						Arguments.BlockNumber++;
					}
				}
				return buffer;
			}
		}
	}

	public class CommandArguments
	{
		const int _blockSize = 32 * 1024;
		public int BlockSize
		{
			get { return _blockSize; }
		}

		long _currentPosition;
		public long CurrentPosition
		{
			get { return _currentPosition; }
			set { _currentPosition = value; }
		}

		long _blockNumber;
		public long BlockNumber
		{
			get { return _blockNumber; }
			set { _blockNumber = value; }
		}

		readonly string _fileName;
		public string FileName
		{
			get { return _fileName; }
		}

		long _fileSize;

		public long FileSize
		{
			get { return _fileSize; }
		}


		public CommandArguments(long currentPosition, long blockNumber, string fileName)
		{
			_currentPosition = currentPosition;
			_blockNumber = blockNumber;
			_fileName = fileName;
			_fileSize = new FileInfo(fileName).Length;
		}
	}
}

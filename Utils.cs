using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace GZipTest
{
	partial class Program
	{
        static EventHandler _handler;

		private static bool Handler(CtrlType sig)
		{
			try {
				Process.GetCurrentProcess().Kill();
			} catch (Win32Exception ex) {
                ShowError(ex);
				return false;
			}
            catch (NotSupportedException ex)
            {
                ShowError(ex);
                return false;
            }
            catch (InvalidOperationException ex)
            {
                ShowError(ex);
                return false;
            }
			return true;
		}

        private static void ShowError(Exception ex)
        {
            Console.WriteLine(string.Format(CultureInfo.InvariantCulture,
                "Impossible stop application immediately, because {0}", ex.Message));
        }

	}

    delegate bool EventHandler(CtrlType sig);

    enum CtrlType
    {
        CTRL_C_EVENT = 0,
        CTRL_BREAK_EVENT = 1,
        CTRL_CLOSE_EVENT = 2,
        CTRL_LOGOFF_EVENT = 5,
        CTRL_SHUTDOWN_EVENT = 6
    }

    internal static class NativeMethods
    {
        [DllImport("Kernel32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetConsoleCtrlHandler(EventHandler handler, [MarshalAs(UnmanagedType.Bool)] bool add);
    }


	/// <summary>
	/// Extension for byte array.
	/// </summary>
	public static class ByteArrayExtension {
		/// <summary>
		/// Take realization.
		/// </summary>
		/// <param name="buffer">Source byte array.</param>
		/// <param name="bytesCount">Number of bytes from beginning of array.</param>
		/// <returns>Byte array with define number of bytes from beginning.</returns>
		public static byte[] Take(this Byte[] buffer, int bytesCount) {
			byte[] result = new byte[bytesCount];
			Array.Copy(buffer, result, bytesCount);
			return result;
		}
	}
}

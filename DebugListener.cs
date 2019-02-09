using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.IO;

namespace mAgicTvPlugIn
{
	class DebugListener : TraceListener
	{
		private TextWriter w;

		public DebugListener(string filePath)
		{
			w = new StreamWriter(filePath, true, Encoding.UTF8);
		}

		public override void Close()
		{
			w.Close();
		}

		public override void Write(string message)
		{
			string now = DateTime.Now.ToString("s");
			w.Write(string.Format("[{0}] {1}", now, message));
		}

		public override void WriteLine(string message)
		{
			Write(message + "\r\n");
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


namespace perceptron
{
	public static class Utils
	{
		public static void WriteLine(string format, params object[] args)
		{
			string str = string.Format(format, args);
			Trace.WriteLine(str);
			Console.WriteLine(str);
		}

		public static void Write(string format, params object[] args)
		{
			string str = string.Format(format, args);
			Trace.Write(str);
			Console.Write(str);			
		}
	}
}

using System;
using System.Diagnostics;

namespace Structura.Shared.Utilities.Tests
{
	public class PerformanceTester
	{
		public TimeSpan Elapsed { get; }

		public PerformanceTester(Action method)
		{
			var sw = new Stopwatch();
			sw.Start();
			method();
			sw.Stop();
			Elapsed = sw.Elapsed;
			Debug.WriteLine($"Method completed in {Elapsed.Milliseconds} ms.");
		}
	}
}

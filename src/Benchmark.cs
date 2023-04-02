using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Example
{
	internal class Benchmark
	{
		private readonly Database database;

		public Benchmark(Database database)
		{
			this.database = database;
		}

		internal void WriteToFile(Action draw)
		{
			StringBuilder sb = new();
			sb.AppendLine("Mode,Quadcount,BatchCount,Microseconds");
			void Line(DrawingMode mode, int myQuadCount, int myBatchCount)
			{
				database.DrawingMode.Set(mode);
				database.QuadCount.Set(myQuadCount);
				database.BatchCount.Set(myBatchCount);
				var time = (int)Math.Round(AvgTime(draw, 100).TotalMicroseconds);
				sb.AppendLine($"{mode},{myQuadCount},{myBatchCount},{time}");
				Console.WriteLine($"{mode},{myQuadCount},{myBatchCount},{time}");
			}
			void Lines(DrawingMode mode, int maxQuadCount, int maxBatchCount)
			{
				for (int myBatchCount = 1; myBatchCount <= maxBatchCount; myBatchCount *= 2)
				{
					for (int quads = 10; quads <= maxQuadCount; quads *= 2)
					{
						Line(mode, quads, myBatchCount);
					}
				}
			}
			Lines(DrawingMode.Immediate, 100000, 1);
			Lines(DrawingMode.DynamicArrayCopy, 10000000, 100000);
			Lines(DrawingMode.StaticVBO, 10000000, 100000);
			var dir = Path.GetDirectoryName(Zenseless.Patterns.PathTools.GetCurrentProcessPath()) ?? string.Empty;
			var fileName = Path.Combine(dir, "test.csv");
			File.WriteAllText(fileName, sb.ToString());
		}

		private static TimeSpan AvgTime(Action action, int repeat)
		{
			for (int i = 0; i < 5; ++i) action(); // warm-up
			Stopwatch timer = Stopwatch.StartNew();
			for (int i = 0; i < repeat; ++i) action();
			return timer.Elapsed / repeat;
		}
	}
}

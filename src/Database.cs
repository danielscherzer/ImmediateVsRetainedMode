using System;

namespace Example;

internal class Database
{
	public Database()
	{
		int Clamp(int batchCount) => Math.Clamp(batchCount, 1, QuadCount);
		QuadCount.Subscribe(value => BatchCount.Set(Clamp(BatchCount)));
	}

	public Observable<int> QuadCount { get; } = new(10000);
	public Observable<DrawingMode> DrawingMode { get; } = new(Example.DrawingMode.Immediate);
	public Observable<int> BatchCount { get; } = new(1);
}

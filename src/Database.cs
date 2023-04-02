using OpenTK.Mathematics;
using System;

namespace Example;

internal class Database
{
	public Database()
	{
		int Clamp(int batchCount) => Math.Clamp(batchCount, 1, QuadCount);
		QuadCount.Subscribe(value => QuadPoints.Set(Helper.CreateRandomQuads(value)));
		QuadCount.Subscribe(value => BatchCount.Set(Clamp(BatchCount)));
	}

	public Observable<int> QuadCount { get; } = new();
	public Observable<Vector2[]> QuadPoints { get; } = new();
	public Observable<DrawingMode> DrawingMode { get; } = new(Example.DrawingMode.Immediate);
	public Observable<int> BatchCount { get; } = new(1);
}

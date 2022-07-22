using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using Zenseless.Patterns;

namespace Example.Drawables
{
	internal class Batched : Disposable, IDrawable
	{
		public Batched(Vector2[] points, Func<IEnumerable<Vector2>, IDrawable> batchCreator, int batchCount = 1)
		{
			var batchSize = points.Length / batchCount;
			Vector2[] batch = new Vector2[batchSize];
			for (int i = 0; i < points.Length; i += batchSize)
			{
				Array.Copy(points, i, batch, 0, batchSize);
				_batches.Add(batchCreator(batch));
			}
		}

		public void Draw()
		{
			foreach (var batch in _batches)
			{
				batch.Draw();
			}
		}

		protected override void DisposeResources()
		{
			foreach (var batch in _batches)
			{
				if (batch is IDisposable disposable) disposable.Dispose();
			}
			_batches.Clear();
		}

		private readonly List<IDrawable> _batches = new();
	}
}

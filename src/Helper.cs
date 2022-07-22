using Example.Drawables;
using OpenTK.Mathematics;
using System;
using Zenseless.Patterns;

namespace Example
{
	internal static class Helper
	{
		public static Vector2[] CreateRandomQuads(int count)
		{
			Vector2[] points = new Vector2[count * 4];
			const float size = 0.02f;
			var rnd = new Random();
			float rnd1() => rnd.NextFloat(-1f, 1f); // net6 NextSingle has same speed
			for (int i = 0; i < count * 4; i += 4)
			{
				var v = new Vector2(rnd1(), rnd1());
				points[i] = v;
				points[i + 1] = v + new Vector2(size, 0f);
				points[i + 2] = v + new Vector2(size);
				points[i + 3] = v + new Vector2(0f, size);
			}
			return points;
		}

		internal static void UpdateDrawable(ref IDrawable? drawable, DrawingMode drawingMode, Vector2[] quadPoints, int batchCount)
		{
			if (drawable is IDisposable disposable) disposable.Dispose();
			drawable = drawingMode switch
			{
				DrawingMode.Immediate => new Immediate(quadPoints),
				DrawingMode.DynamicCopy => new DynamicVA(quadPoints),
				DrawingMode.BatchedRetained => new Batched(quadPoints, p => new StaticVBO(p), batchCount),
				_ => throw new ArgumentOutOfRangeException(nameof(drawingMode)),
			};
		}
	}
}
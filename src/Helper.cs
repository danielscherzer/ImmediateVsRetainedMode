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
			float size = 1f / MathF.Sqrt(count);
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

		internal static IDrawable CreateDrawable(DrawingMode drawingMode, Vector2[] quadPoints, int batchCount) => drawingMode switch
		{
			DrawingMode.Immediate => new Immediate(quadPoints),
			DrawingMode.DynamicArrayCopy => new Batched(quadPoints, p => new DynamicVA(p), batchCount),
			DrawingMode.StaticVBO => new Batched(quadPoints, p => new StaticVBO(p), batchCount),
			_ => throw new ArgumentOutOfRangeException(nameof(drawingMode)),
		};
	}
}
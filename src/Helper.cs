using Example.Drawables;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

internal static class Helper
{
	public static List<Vector2> CreateRandomQuads(int count)
	{
		List<Vector2> points = new();
		const float size = 0.02f;
		var rnd = new Random();
		float rnd1() => (float)rnd.NextDouble() * 2f - 1f;

		for (int i = 0; i < count; ++i)
		{
			var v = new Vector2(rnd1(), rnd1());
			points.Add(v);
			points.Add(v + new Vector2(size, 0f));
			points.Add(v + new Vector2(size));
			points.Add(v + new Vector2(0f, size));
		}
		return points;
	}

	internal static void ClearScreen()
	{
		GL.Clear(ClearBufferMask.ColorBufferBit);
	}

	internal static void UpdateDrawable(ref IDrawable? drawable, DrawingMode drawingMode, List<Vector2> quadPoints)
	{
		if (drawable is IDisposable disposable) disposable.Dispose();
		drawable = drawingMode switch
		{
			DrawingMode.Immediate => new ImmediateQuads(quadPoints),
			DrawingMode.NaiveRetained => new RetainedQuads(quadPoints),
			DrawingMode.BatchedRetained => new RetainedObjectGL(PrimitiveType.Quads, quadPoints),
			DrawingMode.BatchedDynamicCopy => new RetainedDynamicGL(PrimitiveType.Quads, quadPoints),
			_ => throw new ArgumentOutOfRangeException(nameof(drawingMode)),
		};
	}
}
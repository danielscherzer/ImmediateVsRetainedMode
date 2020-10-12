using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Example.Drawables
{
	internal class RetainedQuads : IDisposable, IDrawable
	{
		public RetainedQuads(IReadOnlyList<Vector2> points)
		{
			foreach (var quad in _retainedQuads) quad.Dispose();
			_retainedQuads.Clear();
			for (int i = 0; i < points.Count; i += 4)
			{
				var quad = new Vector2[] { points[i], points[i + 1], points[i + 2], points[i + 3], };
				_retainedQuads.Add(new RetainedObjectGL(PrimitiveType.Quads, quad));
			}
		}

		public void Draw()
		{
			foreach (var quad in _retainedQuads)
			{
				quad.Draw();
			}
		}

		public void Dispose()
		{
			// for a more correct implementation of Dispose please look MS documentation
			foreach (var quad in _retainedQuads) quad.Dispose();
			_retainedQuads.Clear();
		}

		private readonly List<RetainedObjectGL> _retainedQuads = new List<RetainedObjectGL>();
	}
}

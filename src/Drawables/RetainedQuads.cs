using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;
using Zenseless.Patterns;

namespace Example.Drawables
{
	internal class RetainedQuads : Disposable, IDrawable
	{
		public RetainedQuads(IReadOnlyList<Vector2> points)
		{
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

		protected override void DisposeResources()
		{
			DisposeAllFields(this);
			foreach (var quad in _retainedQuads) quad.Dispose();
			_retainedQuads.Clear();
		}

		private readonly List<RetainedObjectGL> _retainedQuads = new();
	}
}

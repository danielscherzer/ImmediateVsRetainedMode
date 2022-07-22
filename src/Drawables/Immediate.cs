using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Example.Drawables
{
	internal class Immediate : IDrawable
	{
		public Immediate(IEnumerable<Vector2> points)
		{
			_points = points;
		}

		public void Draw()
		{
			//draw quads
			GL.Begin(PrimitiveType.Quads);
			foreach (var point in _points)
			{
				GL.Vertex2(point);
			}
			GL.End();
		}

		private readonly IEnumerable<Vector2> _points;
	}
}

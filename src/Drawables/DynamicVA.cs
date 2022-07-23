using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Linq;

namespace Example.Drawables
{
	internal class DynamicVA : IDrawable
	{
		private readonly Vector2[] _array;

		public DynamicVA(Vector2[] points)
		{
			_array = points.ToArray();
		}

		public void Draw()
		{
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, _array);
			GL.EnableVertexAttribArray(0);
			GL.DrawArrays(PrimitiveType.Quads, 0, _array.Length); // draw with vertex array data
			GL.DisableVertexAttribArray(0);
		}
	}
}
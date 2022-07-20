using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Linq;

namespace Example.Drawables
{
	internal class RetainedDynamicGL : IDrawable
	{
		private readonly PrimitiveType _type;
		private readonly Vector2[] _array;

		public RetainedDynamicGL(PrimitiveType type, IEnumerable<Vector2> points)
		{
			_type = type;
			_array = points.ToArray(); //create an array (data is guarantied to be consecutive in memory
		}

		public void Draw()
		{
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, _array);
			GL.EnableVertexAttribArray(0);
			GL.DrawArrays(_type, 0, _array.Length); // draw with vertex array data
			GL.DisableVertexAttribArray(0);
		}
	}
}
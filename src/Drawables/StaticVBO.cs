using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Linq;
using Zenseless.Patterns;

namespace Example.Drawables
{
	internal class StaticVBO : Disposable, IDrawable
	{
		private readonly int _buffer;
		private readonly int _count;
		private readonly int _vertexArray;

		public StaticVBO(IEnumerable<Vector2> points)
		{
			var array = points.ToArray(); //create an array (data is guarantied to be consecutive in memory
			_count = array.Length;

			_vertexArray = GL.GenVertexArray(); // create a vertex array object for interpreting our buffer data (circle points)
			_buffer = GL.GenBuffer();
			GL.BindVertexArray(_vertexArray); // activate vertex array; from now on state is stored;

			int byteSize = Vector2.SizeInBytes * array.Length; // calculate size in bytes of point array
			GL.BindBuffer(BufferTarget.ArrayBuffer, _buffer); // activate buffer
			GL.BufferData(BufferTarget.ArrayBuffer, byteSize, array, BufferUsageHint.StaticDraw); //copy data over

			GL.EnableVertexAttribArray(0); // activate this vertex attribute for the active vertex array
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0); // specify what our buffer contains
			GL.BindVertexArray(0); // deactivate vertex array; state storing is stopped;
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0); // deactivate buffer; just to be on the cautious side;
		}

		public void Draw()
		{
			GL.BindVertexArray(_vertexArray); // activate vertex array
			GL.DrawArrays(PrimitiveType.Quads, 0, _count); // draw with vertex array data
														   //GL.BindVertexArray(0); // deactivate vertex array would be safer but also slower
		}

		protected override void DisposeResources()
		{
			GL.DeleteBuffer(_buffer);
			GL.DeleteVertexArray(_vertexArray);
		}
	}
}
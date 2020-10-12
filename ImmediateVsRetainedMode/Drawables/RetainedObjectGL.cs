using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Example.Drawables
{
	internal class RetainedObjectGL : IDisposable, IDrawable
	{
		private readonly int _buffer;
		private readonly int _count;
		private readonly PrimitiveType _type;
		private readonly int _vertexArray;

		public RetainedObjectGL(PrimitiveType type, IEnumerable<Vector2> points)
		{
			_type = type;

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

		public void Dispose()
		{
			// for a more correct implementation of Dispose please look MS documentation
			GL.DeleteBuffer(_buffer);
			GL.DeleteVertexArray(_vertexArray);
		}

		public void Draw()
		{
			GL.BindVertexArray(_vertexArray); // activate vertex array
			GL.DrawArrays(_type, 0, _count); // draw with vertex array data
			//GL.BindVertexArray(0); // deactivate vertex array would be safer but also slower
		}
	}
}
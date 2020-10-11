using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace Example
{
	internal class Quads
	{
		/// <summary>
		/// Create an instance of <seealso cref="Quads"/>
		/// </summary>
		/// <param name="corners">number of corners</param>
		public Quads(int count)
		{
			Count = count;
		}

		public int Count
		{
			get => _count;
			set
			{
				_count = value;
				UpdatePoints();
			}
		}

		private void UpdatePoints()
		{
			const float size = 0.002f;
			var rnd = new Random();
			float rnd1() => (float)rnd.NextDouble() * 2f - 1f;
			_points.Clear();

			for (int i = 0; i < Count; ++i)
			{ 
				var quad = new RectangleF(rnd1(), rnd1(), size, size);
				_points.Add(new Vector2(quad.Left, quad.Bottom));
				_points.Add(new Vector2(quad.Right, quad.Bottom));
				_points.Add(new Vector2(quad.Right, quad.Top));
				_points.Add(new Vector2(quad.Left, quad.Top));
			}

			_batch?.Dispose();
			_batch = new RetainedObjectGL(PrimitiveType.Quads, _points);

			foreach (var quad in _retainedQuads) quad.Dispose();
			_retainedQuads.Clear();
			for (int i = 0; i < _points.Count; i += 4)
			{
				var quad = new Vector2[] { _points[i], _points[i+1], _points[i+2], _points[i+3], };
				_retainedQuads.Add(new RetainedObjectGL(PrimitiveType.Quads, quad));
			}
		}

		public void DrawRetained()
		{
			foreach(var quad in _retainedQuads)
			{
				quad.Draw();
			}
		}

		public void DrawRetainedBatched()
		{
			_batch.Draw();
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

		private int _count;
		private RetainedObjectGL _batch;
		private readonly List<RetainedObjectGL> _retainedQuads = new List<RetainedObjectGL>();
		private readonly List<Vector2> _points = new List<Vector2>();
	}


}

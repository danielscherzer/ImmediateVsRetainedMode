using Example.Drawables;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Example
{
	internal class View
	{
		private IDrawable ImmediateMode(IReadOnlyList<Vector2> points) => new ImmediateQuads(points);
		private IDrawable RetainedMode(IReadOnlyList<Vector2> points) => new RetainedQuads(points);
		private IDrawable RetainedModeBatched(IReadOnlyList<Vector2> points) => new RetainedObjectGL(OpenTK.Graphics.OpenGL4.PrimitiveType.Quads, points);

		private Func<IReadOnlyList<Vector2>, IDrawable> currentCreator;
		private IDrawable drawable;

		public View(IReadOnlyList<Vector2> points)
		{
			SetAndUpdate(points, ImmediateMode);
		}

		public override string ToString() => currentCreator.Method.Name;

		internal void Update(IReadOnlyList<Vector2> points)
		{
			if (drawable is IDisposable disposable) disposable.Dispose();
			drawable = currentCreator(points);
		}

		internal void SetImmediate(IReadOnlyList<Vector2> points) => SetAndUpdate(points, ImmediateMode);
		internal void SetRetained(IReadOnlyList<Vector2> points) => SetAndUpdate(points, RetainedMode);
		internal void SetRetainedBatched(IReadOnlyList<Vector2> points) => SetAndUpdate(points, RetainedModeBatched);

		internal void Draw()
		{
			//clear screen - what happens without?
			GL.Clear(ClearBufferMask.ColorBufferBit);

			drawable.Draw();
		}

		private void SetAndUpdate(IReadOnlyList<Vector2> points, Func<IReadOnlyList<Vector2>, IDrawable> creator)
		{
			currentCreator = creator;
			Update(points);
		}
	}
}
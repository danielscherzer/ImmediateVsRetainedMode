using Example.Drawables;
using OpenTK.Graphics.OpenGL;
using System;

namespace Example
{
	internal class DrawSystem
	{
		private readonly Database database;
		IDrawable? drawable = null;

		public DrawSystem(Database database)
		{
			this.database = database;
			void Update() => Helper.UpdateDrawable(ref drawable, database.DrawingMode, database.QuadPoints, database.BatchCount);
			database.QuadPoints.Subscribe(_ => Update());
			database.DrawingMode.Subscribe(_ => Update());
			database.BatchCount.Subscribe(_ => Update());

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
		}

		public void Draw()
		{
			GL.Color4(1f, 1f, 1f, MathF.Max(1f / 256f, 10000f / database.QuadCount));
			GL.Clear(ClearBufferMask.ColorBufferBit);
			drawable?.Draw();
		}

		internal static void ResizeWindow(int width, int height) => GL.Viewport(0, 0, width, height);
	}
}

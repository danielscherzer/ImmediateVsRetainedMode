using Example.Drawables;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;

namespace Example
{
	internal class DrawSystem
	{
		public DrawSystem(Database database)
		{
			quadPoints = Helper.CreateRandomQuads(database.QuadCount.Value);
			drawable = Helper.CreateDrawable(database.DrawingMode, quadPoints, database.BatchCount);

			void Update()
			{
				if (drawable is IDisposable disposable) disposable.Dispose();
				drawable = Helper.CreateDrawable(database.DrawingMode, quadPoints, database.BatchCount);
			}
			database.DrawingMode.Subscribe(_ => Update());
			database.BatchCount.Subscribe(_ => Update());
			void Recalc()
			{
				quadPoints = Helper.CreateRandomQuads(database.QuadCount.Value);
				Update();
			}
			database.QuadCount.Subscribe(_ => Recalc());
		}

		public void Draw()
		{
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);
			GL.Enable(EnableCap.Blend);
			GL.Color4(1f, 1f, 1f, 0.5f);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			drawable.Draw();
		}

		internal static void ResizeWindow(int width, int height) => GL.Viewport(0, 0, width, height);

		private IDrawable drawable;
		private Vector2[] quadPoints;
	}
}

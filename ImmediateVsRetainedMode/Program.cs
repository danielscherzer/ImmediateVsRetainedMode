using Example.Drawables;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;

namespace Example
{
	internal class Program
	{
		private static void Main()
		{
			var quads = new Quads(10000);
			var window = new GameWindow(1024, 1024);
			IDrawable createImmediate() => new ImmediateQuads(quads.Points);
			IDrawable createRetained() => new RetainedQuads(quads.Points);
			IDrawable createRetainedBatched() => new RetainedObjectGL(OpenTK.Graphics.OpenGL4.PrimitiveType.Quads, quads.Points);
			
			Func<IDrawable> currentCreator = createImmediate;
			IDrawable drawable = currentCreator(); //after window creation because we use OpenGL

			quads.PropertyChanged += (_, a) =>
			{
				if(nameof(Quads.Points) == a.PropertyName) drawable = currentCreator();
			};
			window.KeyDown += (_, a) =>
			{
				switch (a.Key)
				{
					case Key.Escape: window.Close(); break;
					case Key.Enter: quads.Count *= 2; break; //TODO: notify and recreate
					case Key.Number1: currentCreator = createImmediate; drawable = currentCreator(); break;
					case Key.Number2: currentCreator = createRetained; drawable = currentCreator(); break;
					case Key.Number3: currentCreator = createRetainedBatched; drawable = currentCreator(); break;
				}
			};

			void Draw(double time)
			{
				Console.WriteLine($"Quads={quads.Count} {drawable.GetType().Name} {1000 * time}ms");

				//clear screen - what happens without?
				GL.Clear(ClearBufferMask.ColorBufferBit);

				drawable.Draw();

				window.SwapBuffers(); // buffer swap needed for double buffering
			}

			window.RenderFrame += (s, a) => Draw(a.Time);
			window.Run();

		}
	}
}

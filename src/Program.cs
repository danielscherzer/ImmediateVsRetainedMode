using OpenTK;
using OpenTK.Input;
using System;

namespace Example
{
	internal class Program
	{
		private static void Main()
		{
			var model = new Model(10000);
			var window = new GameWindow(1024, 1024);
			var view = new View(model.Points);

			model.PropertyChanged += (_, a) =>
			{
				if (nameof(Model.Points) == a.PropertyName) 
				{ 
					view.Update(model.Points); 
				}
			};

			window.KeyDown += (_, a) =>
			{
				switch (a.Key)
				{
					case Key.Escape: window.Close(); break;
					case Key.PageUp: model.Count *= 2; break;
					case Key.PageDown: model.Count /= 2; break;
					case Key.Number1: view.SetImmediate(model.Points); break;
					case Key.Number2: view.SetRetained(model.Points); break;
					case Key.Number3: view.SetRetainedBatched(model.Points); break;
				}
			};

			void Draw(double time)
			{
				Console.WriteLine($"Quads={model.Count} {view} {1000 * time}ms");

				view.Draw();

				window.SwapBuffers(); // buffer swap needed for double buffering
			}

			window.RenderFrame += (s, a) => Draw(a.Time);
			window.Run();

		}
	}
}

using Example;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Diagnostics;

namespace ImmediateVsRetainedMode
{
	class Program
	{
		enum State { Immediate, Retained, RetainedBatched };

		private static void Main()
		{
			var window = new GameWindow(1024, 1024);
			var quads = new Quads(10000); //after window creation because we use OpenGL
			//window.KeyDown += (_, a) =>
			//{ 
			//	switch(a.Key)
			//	{
			//		case Key.Number1: break;
			//		case Key.Number2: break;
			//		case Key.Number3: break;
			//	}
			//};

			void Draw(double time)
			{
				var kbState = Keyboard.GetState();
				if (kbState.IsKeyDown(Key.Escape)) window.Close();
				if (kbState.IsKeyDown(Key.Enter)) quads.Count *= 2;
				var state = kbState.IsKeyDown(Key.R) ? State.Retained : kbState.IsKeyDown(Key.B) ? State.RetainedBatched : State.Immediate;

				Console.WriteLine($"Quads={quads.Count} {state} {1000 * time}ms");
				
				//clear screen - what happens without?
				GL.Clear(ClearBufferMask.ColorBufferBit);

				switch(state)
				{
					case State.Immediate: quads.Draw(); break;
					case State.Retained: quads.DrawRetained(); break;
					case State.RetainedBatched: quads.DrawRetainedBatched(); break;
				}
				window.SwapBuffers(); // buffer swap needed for double buffering
			}

			window.RenderFrame += (s, a) => Draw(a.Time);
			window.Run();

		}
	}
}

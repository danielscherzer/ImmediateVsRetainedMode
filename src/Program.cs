using Example;
using Example.Drawables;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

var window = new GameWindow(GameWindowSettings.Default, new NativeWindowSettings { Profile = OpenTK.Windowing.Common.ContextProfile.Compatability });

Observable<int> quadCount = new(10000);
Observable<List<Vector2>> quadPoints = new(new List<Vector2>());
Observable<DrawingMode> drawingMode = new(DrawingMode.Immediate);

quadCount.OnChange += () => quadPoints.Value = Helper.CreateRandomQuads(quadCount.Value);
quadPoints.OnChange += () => drawingMode.NotifyChange();

IDrawable? drawable = null;
drawingMode.OnChange += () => Helper.UpdateDrawable(ref drawable, drawingMode.Value, quadPoints.Value);

window.KeyDown += args =>
{
	switch (args.Key)
	{
		case Keys.Escape: window.Close(); break;
		case Keys.PageUp: quadCount.Value *= 2; break;
		case Keys.PageDown: quadCount.Value /= 2; break;
		case Keys.D1: drawingMode.Value = DrawingMode.Immediate; break;
		case Keys.D2: drawingMode.Value = DrawingMode.NaiveRetained; break;
		case Keys.D3: drawingMode.Value = DrawingMode.BatchedRetained; break;
	}
};

window.RenderFrame += args => Console.WriteLine($"Quads={quadCount.Value} {drawingMode.Value} {1000 * args.Time}ms");
window.RenderFrame += _ => Helper.ClearScreen();
window.RenderFrame += _ => drawable?.Draw();
window.RenderFrame += _ => window.SwapBuffers();

quadCount.Value = 10000;
drawingMode.Value = DrawingMode.Immediate;

window.Run();

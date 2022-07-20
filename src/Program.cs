using Example;
using Example.Drawables;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

GameWindow window = new (GameWindowSettings.Default, new NativeWindowSettings { Profile = OpenTK.Windowing.Common.ContextProfile.Compatability });

Observable<int> quadCount = new();
Observable<List<Vector2>> quadPoints = new();
Observable<DrawingMode> drawingMode = new();

quadCount.OnChange += value => quadPoints.Set(Helper.CreateRandomQuads(value));

IDrawable? drawable = null;
quadPoints.OnChange += _ => Helper.UpdateDrawable(ref drawable, drawingMode, quadPoints);
drawingMode.OnChange += _ => Helper.UpdateDrawable(ref drawable, drawingMode, quadPoints);

double sum = 0.0;
int count = 0;
drawingMode.OnChange += _ => { count = 0; sum = 0.0; };
quadPoints.OnChange += _ => { count = 0; sum = 0.0; };

window.KeyDown += args =>
{
	switch (args.Key)
	{
		case Keys.Escape: window.Close(); break;
		case Keys.PageUp: quadCount.Set(quadCount * 2); break;
		case Keys.PageDown: quadCount.Set(quadCount / 2); break;
		case Keys.D1: drawingMode.Set(DrawingMode.Immediate); break;
		case Keys.D2: drawingMode.Set(DrawingMode.NaiveRetained); break;
		case Keys.D3: drawingMode.Set(DrawingMode.BatchedDynamicCopy); break;
		case Keys.D4: drawingMode.Set(DrawingMode.BatchedRetained); break;
	}
};

window.RenderFrame += args =>
{
	sum += args.Time;
	count++;
	Console.WriteLine($"Quads={(int)quadCount} {(DrawingMode)drawingMode} {1000 * args.Time:F2}ms avg={1000 * sum/count:F2}ms");
};
window.RenderFrame += _ => Helper.ClearScreen();
window.RenderFrame += _ => drawable?.Draw();
window.RenderFrame += _ => window.SwapBuffers();

quadCount.Set(10000);
drawingMode.Set(DrawingMode.Immediate);

window.Run();

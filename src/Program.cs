using Example;
using Example.Drawables;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

GameWindow window = new (GameWindowSettings.Default, new NativeWindowSettings { Profile = OpenTK.Windowing.Common.ContextProfile.Compatability });
window.VSync = OpenTK.Windowing.Common.VSyncMode.Off;

Observable<int> quadCount = new();
Observable<Vector2[]> quadPoints = new();
Observable<DrawingMode> drawingMode = new(DrawingMode.Immediate);
Observable<int> batchCount = new(1);
int Clamp(int batchCount) => Math.Clamp(batchCount, 1, quadCount);
var a = quadCount.Subscribe(value => quadPoints.Set(Helper.CreateRandomQuads(value)));
//quadCount.OnChange += value => quadPoints.Set(Helper.CreateRandomQuads(value));
var b = quadCount.Subscribe(value => batchCount.Set(Clamp(batchCount)));
//quadCount.OnChange += value => batchCount.Set(Clamp(batchCount));

IDrawable? drawable = null;
void Update() => Helper.UpdateDrawable(ref drawable, drawingMode, quadPoints, batchCount);
quadPoints.OnChange += _ => Update();
drawingMode.OnChange += _ => Update();
batchCount.OnChange += _ => Update();

window.KeyDown += args =>
{
	switch (args.Key)
	{
		case Keys.Escape: window.Close(); break;
		case Keys.PageUp: quadCount.Set(quadCount * 2); break;
		case Keys.PageDown: quadCount.Set(Math.Max(1, quadCount / 2)); break;
		case Keys.D1: drawingMode.Set(DrawingMode.Immediate); break;
		case Keys.D2: drawingMode.Set(DrawingMode.DynamicCopy); break;
		case Keys.D3: drawingMode.Set(DrawingMode.BatchedRetained); break;
		case Keys.Up: batchCount.Set(Clamp(batchCount * 2)); break;
		case Keys.Down: batchCount.Set(Clamp(batchCount / 2)); break;
	}
};
//TODO: Ignore first frame after change because it takes longer
double sum = 0.0;
int count = 0;
batchCount.OnChange += _ => { count = 0; sum = 0.0; };
drawingMode.OnChange += _ => { count = 0; sum = 0.0; };
quadPoints.OnChange += _ => { count = 0; sum = 0.0; };

window.RenderFrame += args =>
{
	sum += args.Time;
	count++;
	var message = $"Quads:{(int)quadCount} {(DrawingMode)drawingMode}";
	if (drawingMode != DrawingMode.Immediate) message += $" Batches:{(int)batchCount}";
	message += $" {1000 * args.Time:F2}ms avg:{1000 * sum / count:F2}ms";
	Console.WriteLine(message);
};

GL.Enable(EnableCap.Blend);
GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
window.RenderFrame += _ =>
{
	GL.Color4(1f, 1f, 1f, 10000f/quadCount);
	GL.Clear(ClearBufferMask.ColorBufferBit);
};
window.RenderFrame += _ => drawable?.Draw();
window.RenderFrame += _ => window.SwapBuffers();
window.Resize += args => GL.Viewport(0, 0, args.Width, args.Height);

quadCount.Set(10000);
window.Run();
a.Dispose();
b.Dispose();
Console.WriteLine();
﻿using Example;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

GameWindow window = new(GameWindowSettings.Default, new NativeWindowSettings() { Profile = ContextProfile.Compatability }); ;
window.VSync = VSyncMode.Off; // For correct benchmarks
var monitor = Monitors.GetMonitorFromWindow(window);
window.Size = new Vector2i(monitor.HorizontalResolution, monitor.VerticalResolution) / 2; // set window to halve monitor size

Database database = new();
DrawSystem drawSystem = new(database);
GuiSystem guiSystem = new(window, database);
Benchmark benchmark = new(database);

guiSystem.OnBenchmark += () => benchmark.WriteToFile(() => { drawSystem.Draw(); window.SwapBuffers(); }); // draw without gui for benchmarking

window.RenderFrame += args => drawSystem.Draw();
window.RenderFrame += args => guiSystem.Draw(args.Time);
window.RenderFrame += _ => window.SwapBuffers();

window.Resize += args => DrawSystem.ResizeWindow(args.Width, args.Height);

window.Run();

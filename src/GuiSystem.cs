using Example;
using ImGuiNET;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Linq;
using Zenseless.OpenTK.GUI;

internal class GuiSystem
{
	public GuiSystem(GameWindow window, Database database)
	{
		this.window = window;
		this.database = database;

		void Reset() { count = -5; sum = 0.0; }
		database.BatchCount.Subscribe(_ => Reset());
		database.DrawingMode.Subscribe(_ => Reset());
		database.QuadCount.Subscribe(_ => Reset());

		gui = new(window, 1f);
		gui.LoadFontDroidSans(24f);

		int Clamp(int batchCount) => Math.Clamp(batchCount, 1, database.QuadCount);
		window.KeyDown += args =>
		{
			switch (args.Key)
			{
				case Keys.Escape: window.Close(); break;
				case KeyUpQuadCount: database.QuadCount.Set(database.QuadCount * 2); break;
				case KeyDownQuadCount: database.QuadCount.Set(Math.Max(1, database.QuadCount / 2)); break;
				case KeyUpBatchCount: database.BatchCount.Set(Clamp(database.BatchCount * 2)); break;
				case KeyDownBatchCount: database.BatchCount.Set(Clamp(database.BatchCount / 2)); break;
				case KeyBenchmark: OnBenchmark?.Invoke(); break;
			}
		};
	}

	public event Action? OnBenchmark;

	public void Draw(double time)
	{
		count++;
		if (count > 0)
		{
			sum += time;
		}
		ImGui.NewFrame();
		var viewport = ImGui.GetMainViewport();
		ImGui.SetNextWindowPos(viewport.Pos);
		ImGui.Begin("Information", ImGuiWindowFlags.AlwaysAutoResize);
		ImGui.Text($"Time:{1000 * time:F2}ms");
		ImGui.Text($"Time avg:{1000 * sum / count:F2}ms");
		var mode = (int)(DrawingMode)database.DrawingMode;
		if (ImGui.ListBox("Draw mode", ref mode, modes, modes.Length))
		{
			database.DrawingMode.Set((DrawingMode)mode);
		}
		IntSlider("Quad count", database.QuadCount, 10000, 1000000, HintKeyText(KeyDownQuadCount, KeyUpQuadCount));
		if (database.DrawingMode != DrawingMode.Immediate)
		{
			IntSlider("Batch count", database.BatchCount, 1, Math.Min(10000, database.QuadCount), HintKeyText(KeyDownBatchCount, KeyUpBatchCount));
		}
		if (ImGui.Button("Run benchmark"))
		{
			OnBenchmark?.Invoke();
		}
		if (ImGui.IsItemHovered())
		{
			ImGui.SetTooltip(HintKeyText(KeyBenchmark));
		}
		ImGui.End();
		gui.Render(window.ClientSize);
	}

	private readonly string[] modes = Enum.GetNames(typeof(DrawingMode));
	private readonly GameWindow window;
	private readonly Database database;
	private readonly ImGuiFacade gui;
	private const Keys KeyUpQuadCount = Keys.PageUp;
	private const Keys KeyDownQuadCount = Keys.PageDown;
	private const Keys KeyUpBatchCount = Keys.Up;
	private const Keys KeyDownBatchCount = Keys.Down;
	private const Keys KeyBenchmark = Keys.B;
	//Ignore first frames after reset because it may take longer to build render batch structures
	private double sum = 0.0;
	private int count = -5;

	private static string HintKeyText(params Keys[] keys)
	{
		string plural = (keys.Length > 1 ? "s" : "");
		return $"Shortcut{plural} {string.Join(",", keys.Select(k => $"({k})"))} key{plural}";
	}

	private static void IntSlider(string label, Observable<int> observable, int min, int max, string hint)
	{
		var value = observable.Value;
		if (ImGui.SliderInt(label, ref value, min, max, "%d", ImGuiSliderFlags.Logarithmic))
		{
			observable.Set(value);
		}
		if (ImGui.IsItemHovered())
		{
			ImGui.SetTooltip(hint);
		}
	}
}
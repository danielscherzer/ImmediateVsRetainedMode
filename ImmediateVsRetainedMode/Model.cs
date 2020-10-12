using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Example
{
	internal class Model : INotifyPropertyChanged
	{
		public Model(int count)
		{
			Count = count;
		}

		public int Count
		{
			get => _count;
			set
			{
				_count = value;
				UpdatePoints();
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Points)));
			}
		}

		public IReadOnlyList<Vector2> Points => _points;

		public event PropertyChangedEventHandler PropertyChanged;

		private void UpdatePoints()
		{
			const float size = 0.002f;
			var rnd = new Random();
			float rnd1() => (float)rnd.NextDouble() * 2f - 1f;
			_points.Clear();

			for (int i = 0; i < Count; ++i)
			{
				var quad = new RectangleF(rnd1(), rnd1(), size, size);
				_points.Add(new Vector2(quad.Left, quad.Bottom));
				_points.Add(new Vector2(quad.Right, quad.Bottom));
				_points.Add(new Vector2(quad.Right, quad.Top));
				_points.Add(new Vector2(quad.Left, quad.Top));
			}
		}

		private int _count;
		private readonly List<Vector2> _points = new List<Vector2>();
	}


}

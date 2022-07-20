using System;

namespace Example
{
	internal class Observable<TType>
	{
		public bool HasValue => value != null;

		public event Action<TType>? OnChange;

		public void Set(TType value)
		{
			this.value = value;
			OnChange?.Invoke(value);
		}

		public static implicit operator TType(Observable<TType> observable) => observable.value ?? throw new ArgumentException("Observable value not set");

		private TType? value;
	}
}

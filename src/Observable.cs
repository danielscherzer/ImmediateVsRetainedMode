using System;

namespace Example
{
	internal class Observable<TType>
	{
		public Observable(TType value) => this.value = value;

		public event Action? OnChange;

		public TType Value
		{
			get => value;
			set
			{
				this.value = value;
				NotifyChange();
			}
		}

		public void NotifyChange() => OnChange?.Invoke();

		private TType value;
	}
}

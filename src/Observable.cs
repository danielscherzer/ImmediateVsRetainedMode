using System;
using System.Collections.Generic;

namespace Example
{
	internal class Observable<TType>
	{
		public Observable() { }

		public Observable(TType value) => this.value = value;

		//public void DependsOn<TOther>(Observable<TOther> observable) => observable.OnChange += _ => OnChange?.Invoke(this);

		public bool HasValue => value != null;

		public event Action<TType>? OnChange;

		public void Set(TType value)
		{
			this.value = value;
			OnChange?.Invoke(value);
			foreach(var sub in subscriptions) sub.Invoke(value);
		}

		public IDisposable Subscribe(Action<TType> subscription) => new Subscription(subscriptions, subscription);

		public static implicit operator TType(Observable<TType> observable) => observable.value ?? throw new ArgumentException("Observable value not set");

		private TType? value;
		private readonly HashSet<Action<TType>> subscriptions = new();

		private sealed class Subscription : IDisposable
		{
			private readonly HashSet<Action<TType>> subscriptions;
			private readonly Action<TType> subscription;

			public Subscription(HashSet<Action<TType>> subscriptions, Action<TType> subscription)
			{
				this.subscriptions = subscriptions;
				this.subscription = subscription;
				this.subscriptions.Add(subscription);
			}

			public void Dispose()
			{
				subscriptions.Remove(subscription);
			}
		}
	}
}

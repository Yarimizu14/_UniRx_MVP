using UnityEngine;
using System.Collections;

namespace UniRx.Triggers
{
    public static partial class ObservableTriggerExtensions
	{
		/// <summary>Update is called every frame, if the MonoBehaviour is enabled.</summary>
		public static IObservable<KeyCode> KeyDownAsObservable(this Component component, KeyCode targetKey)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<KeyCode>();
			return GetOrAddComponent<KeyDownTrigger> (component.gameObject).KeyDownAsObservable (targetKey);
		}
	}

	public class KeyDownTrigger : ObservableTriggerBase
	{
		Subject<KeyCode> keyInput;

		private KeyCode _targetKey;

		public IObservable<KeyCode> KeyDownAsObservable(KeyCode targetKey)
		{
			_targetKey = targetKey;

			return keyInput ?? (keyInput = new Subject<KeyCode>());
		}

		public IObservable<KeyCode> KeyDownAsObservable()
		{
			return keyInput ?? (keyInput = new Subject<KeyCode>());
		}

		// Update is called once per frame
		void Update ()
		{
			if (Input.GetKeyDown(_targetKey) && keyInput != null)
			{
				keyInput.OnNext (_targetKey);
			}
		}

		protected override void RaiseOnCompletedOnDestroy ()
		{
			if (keyInput != null)
			{
				keyInput.OnCompleted ();
			}
		}
	}
}

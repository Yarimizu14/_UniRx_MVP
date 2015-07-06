using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class CharacterModel
{
	/// <summary>
	/// Movement intention.
	/// </summary>
	public enum MovementIntention
	{
		Left,
		Right,
		Stop
	}

	/// <summary>
	/// The initial speed.
	/// </summary>
	private Vector3 _initialSpeed = new Vector3(3f, 0f, 0f);
	public ReactiveProperty<Vector3> speed;

	public ReactiveProperty<MovementIntention> intention;

	public CharacterModel()
	{
		speed = new ReactiveProperty<Vector3>(_initialSpeed);
		intention = new ReactiveProperty<MovementIntention>(MovementIntention.Stop);
	}
}

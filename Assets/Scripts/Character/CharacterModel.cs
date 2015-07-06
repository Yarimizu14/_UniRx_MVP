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
	/// Jump intention.
	/// </summary>
	public enum JumpIntention
	{
		Jump,
		None
	}

	/// <summary>
	/// The initial speed.
	/// </summary>
	private Vector3 _initialSpeed = new Vector3(3f, 0f, 0f);
	public ReactiveProperty<Vector3> speed;

	public ReactiveProperty<MovementIntention> moveIntention;

	/// <summary>
	/// The is on the ground.
	/// </summary>
	public ReactiveProperty<bool> isOnTheGround;

	/// <summary>
	/// The is not on the ground.
	/// </summary>
	public ReactiveProperty<bool> isNotOnTheGround;

	public CharacterModel()
	{
		speed = new ReactiveProperty<Vector3>(_initialSpeed);
		moveIntention = new ReactiveProperty<MovementIntention>(MovementIntention.Stop);
		isOnTheGround = new ReactiveProperty<bool>(true);
		isNotOnTheGround = new ReactiveProperty<bool>(true);

		this.isOnTheGround.Subscribe ((bool isGround) => {
			this.isNotOnTheGround.Value = !isGround;
		});
	}
}

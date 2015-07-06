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

	/// <summary>
	/// The move intention.
	/// </summary>
	public ReactiveProperty<MovementIntention> moveIntention;

	/// <summary>
	/// The move intention.
	/// </summary>
	public ReactiveProperty<JumpIntention> jumpIntention;

	/// <summary>
	/// The is on the ground.
	/// </summary>
	public ReactiveProperty<bool> isOnTheGround;

	/// <summary>
	/// The is not on the ground.
	/// </summary>
	public ReactiveProperty<bool> isNotOnTheGround;

	/// <summary>
	/// The should jump.
	/// </summary>
	public ReactiveProperty<bool> shouldJump;

	/// <summary>
	/// The is not on the ground.
	/// </summary>
	public ReactiveProperty<bool> jumpLocked;

	/// <summary>
	/// The jump performed.
	/// </summary>
	public ReactiveProperty<int> jumpPerformed;

	/// <summary>
	/// The jump state machine.
	/// </summary>
	public CharacterJumpStateMachine jumpStateMachine;

	public CharacterModel()
	{
		GameObject jumpStateMachineObje = new GameObject ();
		jumpStateMachine = jumpStateMachineObje.AddComponent<CharacterJumpStateMachine> ();
		jumpStateMachine.Init ();

		speed = new ReactiveProperty<Vector3>(_initialSpeed);
		moveIntention = new ReactiveProperty<MovementIntention>(MovementIntention.Stop);
		jumpIntention = new ReactiveProperty<JumpIntention>(JumpIntention.None);
		isOnTheGround = new ReactiveProperty<bool>(true);
		isNotOnTheGround = new ReactiveProperty<bool>(true);
		shouldJump = new ReactiveProperty<bool>(false);

		this.isOnTheGround.Subscribe ((bool isGround) => {
			this.isNotOnTheGround.Value = !isGround;
		});

		this.jumpIntention.Subscribe ((JumpIntention intention) => {
			this.shouldJump.Value = this.ComputeShouldJump();
		});

		this.shouldJump.Subscribe ((bool should) => {
			this.jumpStateMachine.JumpExpectedTransition();
		});

		this.isNotOnTheGround.Subscribe ((bool notGround) => {
			this.jumpStateMachine.LeftGroundTransition();
		});

		this.isNotOnTheGround.Subscribe ((bool notGround) => {
			this.jumpStateMachine.LandedTransition();
		});
	}

	protected bool ComputeShouldJump()
	{
		return isOnTheGround.Value && jumpIntention.Value == JumpIntention.Jump;
	}
}

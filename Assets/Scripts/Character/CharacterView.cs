using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class CharacterView : MonoBehaviour {

	protected Animator _animator;

	protected Transform _art;

	// Use this for initialization
	void Start () {
		_art = transform.FindChild ("CharacterArt");
		_animator = _art.GetComponent<Animator> ();
	}

	/// <summary>
	/// The movement intention observable.
	/// </summary>
	private IObservable<CharacterModel.MovementIntention> _MovementIntentionObservable;
	public IObservable<CharacterModel.MovementIntention> MovementIntentionObservable
	{
		get
		{
			if (_MovementIntentionObservable == null)
			{
				_MovementIntentionObservable = this.UpdateAsObservable ().Select ((_) => this.CalculateMovementIntention());
			}

			return _MovementIntentionObservable;
		}
	}

	/// <summary>
	/// Calculates the movement intention.
	/// </summary>
	/// <returns>The movement intention.</returns>
	protected CharacterModel.MovementIntention CalculateMovementIntention()
	{
		if (Input.GetKey(KeyCode.A))
		{
			return CharacterModel.MovementIntention.Left;
		}
		if (Input.GetKey(KeyCode.D))
		{
			return CharacterModel.MovementIntention.Right;
		}

		return CharacterModel.MovementIntention.Stop;
	}

	/// <summary>
	/// Jumps the intention as observable.
	/// </summary>
	/// <returns>The intention as observable.</returns>
	public IObservable<CharacterModel.JumpIntention> JumpIntentionAsObservable()
	{
		return this.UpdateAsObservable ().Select (_ => CalculateJumpIntention());
	}

	/// <summary>
	/// Calculates the jump intention.
	/// </summary>
	/// <returns>The jump intention.</returns>
	protected CharacterModel.JumpIntention CalculateJumpIntention()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			return CharacterModel.JumpIntention.Jump;
		}

		return CharacterModel.JumpIntention.None;
	}

	/// <summary>
	/// Raises the move left event.
	/// </summary>
	public void OnIdle(Vector3 speed)
	{
		_animator.SetInteger ("State", 0);
	}

	/// <summary>
	/// Raises the move left event.
	/// </summary>
	public void OnMoveLeft(Vector3 speed)
	{
		this.transform.Translate (speed * Time.deltaTime);

		_animator.SetInteger ("State", 1);
		_art.localEulerAngles = new Vector3 (0f, 180f, 0f);
	}

	/// <summary>
	/// Raises the move left event.
	/// </summary>
	public void OnMoveRight(Vector3 speed)
	{
		this.transform.Translate (speed * Time.deltaTime);

		_animator.SetInteger ("State", 1);
		_art.localEulerAngles = new Vector3 (0f, 0, 0f);
	}

	/// <summary>
	/// Raises the jump event.
	/// </summary>
	/// <param name="speed">Speed.</param>
	public void OnDoJump()
	{
		var rigid2D = this.GetComponent<Rigidbody2D> ();

		rigid2D.velocity = new Vector2 (rigid2D.velocity.x, 0f);
		rigid2D.AddForce (transform.up * 8, ForceMode2D.Impulse);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CharacterView"/> class.
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	public void IsNotOnTheGroundChanged(bool value)
	{
		_animator.SetBool("IsInTheAir", value);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CharacterView"/> class.
	/// </summary>
	public IObservable<bool> IsOnTheGroundAsObservalbe()
	{
		return this.UpdateAsObservable().Select(_ => CalculateIsOnTheGround());
	}

	/// <summary>
	/// Calculates the jump intention.
	/// </summary>
	/// <returns>The jump intention.</returns>
	protected bool CalculateIsOnTheGround()
	{
		return Physics2D.Raycast (transform.position, -transform.up.normalized, 0.05f, LayerMask.GetMask(new []{ "Floor" }));
	}
}

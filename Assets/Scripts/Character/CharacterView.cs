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
}

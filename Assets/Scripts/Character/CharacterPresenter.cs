using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class CharacterPresenter : MonoBehaviour
{
	/// <summary>
	/// The view.
	/// </summary>
	[SerializeField]
	private CharacterView view;

	/// <summary>
	/// The model.
	/// </summary>
	private CharacterModel model;

	void Awake()
	{
		this.model = new CharacterModel ();

		// view => model
		view.MovementIntentionObservable.Subscribe ((CharacterModel.MovementIntention intention) => {
			this.MovementIntentionHandler(intention);
			this.model.moveIntention.Value = intention;
		});

		view.JumpIntentionAsObservable ().Subscribe ((CharacterModel.JumpIntention intention) => {
			this.JumpIntentionHandler(intention);
		});

		view.IsOnTheGroundAsObservalbe ().Subscribe ((bool isOnTheGround) => {
			this.model.isOnTheGround.Value = isOnTheGround;
		});

		// model => view
		model.isNotOnTheGround.Subscribe ((bool isOnTheGround) => {
			this.view.IsNotOnTheGroundChanged(isOnTheGround);
		});
	}

	/// <summary>
	/// Updates the character position.
	/// </summary>
	/// <param name="intention">Intention.</param>
	public void MovementIntentionHandler(CharacterModel.MovementIntention intention)
	{
		if (intention == CharacterModel.MovementIntention.Right)
		{
			this.view.OnMoveLeft (- this.model.speed.Value);
			return;
		}

		if (intention == CharacterModel.MovementIntention.Left)
		{
			this.view.OnMoveRight (this.model.speed.Value);
			return;
		}

		this.view.OnIdle (this.model.speed.Value);
	}

	/// <summary>
	/// Jumps the intention handler.
	/// </summary>
	/// <param name="intention">Intention.</param>
	public void JumpIntentionHandler(CharacterModel.JumpIntention intention)
	{
		if (intention == CharacterModel.JumpIntention.Jump)
		{
			this.view.OnDoJump ();
		}
	}
}

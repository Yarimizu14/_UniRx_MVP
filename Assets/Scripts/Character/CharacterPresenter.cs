using UnityEngine;
using System;
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

		view.IsOnTheGroundAsObservalbe ().Subscribe ((bool isOnTheGround) => {
			this.model.isOnTheGround.Value = isOnTheGround;
			this.model.jumpStateMachine.LandedTransition();
		});

		view.JumpIntentionAsObservable ().Subscribe ((CharacterModel.JumpIntention intention) => {
			this.model.jumpIntention.Value = intention;
		});

		// model => view
		model.isNotOnTheGround.Subscribe ((bool isOnTheGround) => {
			this.view.IsNotOnTheGroundChanged(isOnTheGround);
		});

		model.jumpStateMachine.onDoJump.Subscribe ((Unit _) => {
			view.OnDoJump();
		});

		model.jumpStateMachine.jumpState.Subscribe (this.JumpStateChanged);
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

	private void JumpStateChanged(CharacterJumpStateMachine.States state)
	{
		if (state == CharacterJumpStateMachine.States.NoJump)
		{
			this.model.jumpPerformed.Value = 0;
		}
		else if (state == CharacterJumpStateMachine.States.DoJump)
		{
			this.model.jumpPerformed.Value = this.model.jumpPerformed.Value + 1;
			this.model.jumpLocked.Value = true;

			Observable.Timer (TimeSpan.FromMilliseconds (100))
				.Subscribe (l => {
					Debug.Log("this.model.jumpLocked.Value = false;");
					this.model.jumpLocked.Value = false;
				});
		}
		
	}
}

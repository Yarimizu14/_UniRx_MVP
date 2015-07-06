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

		view.MovementIntentionObservable.Subscribe ((CharacterModel.MovementIntention intention) => {
			this.UpdateCharacterPosition(intention);
			this.model.intention.Value = intention;
		});

		//this.model.intention.Subscribe ((_) => this.UpdateCharacterPosition(_));
	}

	/// <summary>
	/// Updates the character position.
	/// </summary>
	/// <param name="intention">Intention.</param>
	public void UpdateCharacterPosition(CharacterModel.MovementIntention intention)
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
}

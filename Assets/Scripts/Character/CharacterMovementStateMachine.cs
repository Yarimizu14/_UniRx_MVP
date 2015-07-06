using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class CharacterMovementStateMachine : StateBehaviour
{
	/// <summary>
	/// States.
	/// </summary>
	public enum States
	{
		Idle,
		MoveLeft,
		MoveRight
	}

	/// <summary>
	/// Init this instance.
	/// </summary>
	public void Init()
	{
		this.Initialize<States> ();
	}

	/// <summary>
	/// Gos the left.
	/// </summary>
	public void GoLeft()
	{
		this.ChangeState (States.MoveLeft);
	}

	/// <summary>
	/// Gos the right.
	/// </summary>
	public void GoRight()
	{
		this.ChangeState (States.MoveRight);
	}

	/// <summary>
	/// Gos the right.
	/// </summary>
	public void Stop()
	{
		this.ChangeState (States.MoveRight);
	}
}

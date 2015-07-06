using UnityEngine;
using System;
using System.Collections;
using MonsterLove.StateMachine;
using UniRx;
using UniRx.Triggers;

public class CharacterJumpStateMachine : StateBehaviour
{
	/// <summary>
	/// States.
	/// </summary>
	public enum States
	{
		NoJump,
		DoJump,
		InTheAir
	}

	/// <summary>
	/// The no jump subject.
	/// </summary>
	private Subject<Unit> _noJumpSubject;
	public IObservable<Unit> onNoJump
	{
		get
		{
			return _noJumpSubject ?? (_noJumpSubject = new Subject<Unit>());
		}
	}

	/// <summary>
	/// The do jump subject.
	/// </summary>
	private Subject<Unit> _doJumpSubject;
	public IObservable<Unit> onDoJump
	{
		get
		{
			return _doJumpSubject ?? (_doJumpSubject = new Subject<Unit>());
		}
	}

	private Subject<Unit> _inTheAirSubject;
	public IObservable<Unit> onInTheAir
	{
		get
		{
			return _inTheAirSubject ?? (_inTheAirSubject = new Subject<Unit>());
		}
	}

	/// <summary>
	/// Init this instance.
	/// </summary>
	public void Init()
	{
		this.Initialize<States> ();
		this.ChangeState (States.NoJump);
	}

	/// <summary>
	/// Nos the jump enter.
	/// </summary>
	void NoJump_Enter()
	{
		if (_noJumpSubject != null)
		{
			_noJumpSubject.OnNext (Unit.Default);
		}
	}

	/// <summary>
	/// Dos the jump enter.
	/// </summary>
	void DoJump_Enter()
	{
		Debug.Log ("DoJump_Enter");
		if (_doJumpSubject != null)
		{
			_doJumpSubject.OnNext (Unit.Default);
		}
	}

	/// <summary>
	/// Ins the air enter.
	/// </summary>
	void InTheAir_Enter()
	{
		if (_inTheAirSubject != null)
		{
			_inTheAirSubject.OnNext (Unit.Default);
		}
	}

	/*
	/// <summary>
	/// Changes to no jump.
	/// </summary>
	public void ChangeToNoJump()
	{
		this.ChangeState (States.NoJump);
	}

	/// <summary>
	/// Changes to do jump.
	/// </summary>
	public void ChangeToDoJump()
	{
		this.ChangeState (States.DoJump);
	}

	/// <summary>
	/// Changes to do jump.
	/// </summary>
	public void ChangeToInTheAir()
	{
		this.ChangeState (States.InTheAir);
	}
	*/

	public void JumpExpectedTransition()
	{
		Enum current = this.GetState ();

		if (current.Equals(States.NoJump))
		{
			this.ChangeState (States.DoJump);
		}

		if (current.Equals(States.DoJump))
		{
			return;
		}

		if (current.Equals(States.InTheAir))
		{
			this.ChangeState (States.DoJump);
		}
	}

	public void LeftGroundTransition()
	{
		Enum current = this.GetState ();

		if (current.Equals(States.NoJump))
		{
			this.ChangeState (States.InTheAir);
		}

		if (current.Equals(States.DoJump))
		{
			this.ChangeState (States.InTheAir);
		}

		if (current.Equals(States.InTheAir))
		{
			return;
		}
	}

	public void LandedTransition()
	{
		Enum current = this.GetState ();

		if (current.Equals(States.NoJump))
		{
			this.ChangeState (States.DoJump);
		}

		if (current.Equals(States.DoJump))
		{
			return;
		}

		if (current.Equals(States.InTheAir))
		{
			this.ChangeState (States.NoJump);
		}
	}
}

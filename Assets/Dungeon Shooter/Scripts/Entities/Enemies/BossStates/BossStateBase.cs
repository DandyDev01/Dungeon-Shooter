using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Enemies
{
	public abstract class BossStateBase : MonoBehaviour
	{
		protected Boss _boss;
		protected BossStateBase _attackState;
		protected BossStateBase _subState;
		protected bool _isRoot;

		public BossStateBase Parent { get; set; }
		public BossStateBase AttackState => _attackState;
		public bool IsRoot => _isRoot;	

		protected virtual void Awake()
		{
			_boss = GetComponentInParent<Boss>();

			_isRoot = false;
		}

		public abstract void Enter();

		public abstract void Exit();

		public abstract void Run();

		protected abstract void CheckForStateSwitch();

		public void SwitchState(BossStateBase newState)
		{
			if (newState.IsRoot)
			{
				Exit();
				newState.Enter();
				_boss.CurrentState = newState;
			}
			else
			{
				_boss.CurrentState.SwitchSubState(newState);
			}
		}

		public void SwitchSubState(BossStateBase subState)
		{
			if (subState == null)
				Debug.LogError("substate cannot be null.", gameObject);

			if (_subState != null)
			{
				_subState.Exit();
				_subState.Parent = null;
			}

			_subState = subState;
			_subState.Parent = this;
			_subState.Enter();
		}

		public void RemoveSubState()
		{
			if (_subState != null)
			{
				_subState.Exit();
				_subState = null;
			}
		}
	}
}
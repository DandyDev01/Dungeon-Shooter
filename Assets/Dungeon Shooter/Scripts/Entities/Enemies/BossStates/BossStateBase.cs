using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public abstract class BossStateBase : MonoBehaviour
	{
		protected Boss _boss;
		protected BossStateBase _attackState;
		protected BossStateBase _subState;
		protected bool _isRoot;

		public BossStateBase AttackState => _attackState;
		public bool IsRoot => _isRoot;	

		private void Awake()
		{
			_boss = GetComponentInParent<Boss>();

			_attackState = GetComponentInChildren<BossStageOneAttackState>();

			_isRoot = false;

			SwitchSubState(_boss.MoveState);
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
				_boss.CurrentRootState = newState;
			}
			else
				SwitchSubState(newState);
		}

		public void SwitchSubState(BossStateBase subState)
		{
			if (subState == null)
				Debug.LogError("substate cannot be null.", gameObject);

			if (_subState != null)
				_subState.Exit();

			_subState = subState;
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
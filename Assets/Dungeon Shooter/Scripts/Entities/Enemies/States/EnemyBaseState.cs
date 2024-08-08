using DungeonShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public abstract class EnemyBaseState : MonoBehaviour
	{
		protected Enemy _enemy;

		private void Awake()
		{
			_enemy = GetComponentInParent<Enemy>();
		}

		public abstract void Enter();

		public abstract void Exit();

		public abstract void Run();

		protected abstract void CheckForStateSwitch();

		public void SwitchState(EnemyBaseState newState)
		{
			Exit();

			newState.Enter();

			_enemy.CurrentState = newState;
		}
	}
}
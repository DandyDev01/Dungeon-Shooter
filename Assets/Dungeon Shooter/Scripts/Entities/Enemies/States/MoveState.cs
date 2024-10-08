using DungeonShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Enemies
{
	public class MoveState : EnemyBaseState
	{
		public override void Enter()
		{
			_enemy.PlayAnimation("Move");
		}

		public override void Exit()
		{
			
		}

		public override void Run()
		{
			_enemy.Move();
			CheckForStateSwitch();
		}

		protected override void CheckForStateSwitch()
		{
			float distanceFromTarget = Vector2.Distance(transform.position, _enemy.GetTargetPosition());

			if (distanceFromTarget < _enemy.AttackDistance)
				SwitchState(_enemy.AttackState);

			if (_enemy.Health.Current <= 0)
				SwitchState(_enemy.DeadState);
		}
	}
}
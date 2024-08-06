using DungeonShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public class MoveState : EnemyBaseState
	{
		
		public override void Enter()
		{
			
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
		}
	}
}
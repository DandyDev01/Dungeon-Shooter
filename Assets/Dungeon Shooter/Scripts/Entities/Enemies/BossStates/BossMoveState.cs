using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Enemies
{
	public class BossMoveState : BossStateBase
	{
		public override void Enter()
		{
			//_boss.PlayAnimation("Move");
		}

		public override void Exit()
		{

		}

		public override void Run()
		{
			_boss.Move();
			CheckForStateSwitch();
		}

		protected override void CheckForStateSwitch()
		{
			float distanceFromTarget = Vector2.Distance(transform.position, _boss.GetTargetPosition());

			if (distanceFromTarget < _boss.AttackDistance)
				SwitchState(_boss.CurrentState.AttackState);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Enemies
{
	public class BossStageTwoAttackState : BossStateBase
	{
		public override void Enter()
		{
		}

		public override void Exit()
		{
		}

		public override void Run()
		{
			CheckForStateSwitch();
		}

		protected override void CheckForStateSwitch()
		{
			if (Vector2.Distance(transform.position, _boss.GetTargetPosition()) > 2f)
				SwitchState(_boss.MoveState);
		}
	}
}
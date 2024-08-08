using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public class DeadState : EnemyBaseState
	{
		public override void Enter()
		{
			_enemy.SpeedModifier = 0f;
			_enemy.PlayAnimation("Dead");
			Debug.Log("Dead");
		}

		public override void Exit()
		{
			_enemy.SpeedModifier = 1f;
		}

		public override void Run()
		{
		}

		protected override void CheckForStateSwitch()
		{
		}
	}
}
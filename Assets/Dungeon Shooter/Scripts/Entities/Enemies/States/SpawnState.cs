using DungeonShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public class SpawnState : EnemyBaseState
	{
		public override void Enter()
		{
			SwitchState(_enemy.MoveState);
		}

		public override void Exit()
		{
		}

		public override void Run()
		{
		}

		protected override void CheckForStateSwitch()
		{
			throw new System.NotImplementedException();
		}
	}
}
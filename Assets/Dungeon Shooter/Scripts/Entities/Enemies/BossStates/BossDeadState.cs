using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Enemies
{
	public class BossDeadState : BossStateBase
	{
		private void Start()
		{
			_isRoot = true;
		}

		public override void Enter()
		{
			_boss.SpeedModifier = 0f;
			//_boss.PlayAnimation("Dead");
		}

		public override void Exit()
		{
			_boss.SpeedModifier = 1f;
		}

		public override void Run()
		{
		}

		protected override void CheckForStateSwitch()
		{
		}
	}
}
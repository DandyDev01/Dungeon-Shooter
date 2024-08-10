using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter
{
	public class BossStageOneState : BossStateBase
	{
		private void Start()
		{
			_isRoot = true;
			SwitchSubState(_boss.SpawnState);
		}

		public override void Enter()
		{
		}

		public override void Exit()
		{
		}

		public override void Run()
		{
			_subState.Run();
			CheckForStateSwitch();
		}

		protected override void CheckForStateSwitch()
		{
			if (_boss.Health.Current < _boss.Health.MaxHealth / 2)
				SwitchState(_boss.StageTwoState);
		}
	}
}
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
		}

		protected override void CheckForStateSwitch()
		{
		}
	}
}
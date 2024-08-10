using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Enemies
{
	public class BossStageTwoState : BossStateBase
	{
		private SpriteRenderer _spriteRenderer;

		private void Start()
		{
			_isRoot = true;
			_attackState = GetComponentInChildren<BossStageTwoAttackState>();
			_spriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
		}

		public override void Enter()
		{
			_spriteRenderer.color = Color.red;
			SwitchState(_boss.MoveState);
		}

		public override void Exit()
		{
			_spriteRenderer.color = Color.magenta;
		}

		public override void Run()
		{
			CheckForStateSwitch();
			_subState?.Run();
		}

		protected override void CheckForStateSwitch()
		{
			if (_boss.Health.Current <= 0)
				SwitchState(_boss.DeadState);
		}
	}
}
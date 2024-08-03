using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonShooter.Player
{
	public class PlayerIdleState : PlayerStateBase
	{
		public PlayerIdleState(Player player) : base(player)
		{
		}

		public override void Enter()
		{
			_player.PlayAnimation("Idle");
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
			if (_player.MoveVector != Vector2.zero)
				SwitchState(_player.StateHolder.MoveState);
		}
	}
}

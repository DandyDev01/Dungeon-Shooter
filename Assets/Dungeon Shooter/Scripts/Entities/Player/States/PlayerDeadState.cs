using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonShooter.Player
{
	public class PlayerDeadState : PlayerStateBase
	{
		public PlayerDeadState(PlayerCharacter player) : base(player)
		{
		}

		public override void Enter()
		{
			_player.PlayAnimation("Dead");
		}

		public override void Exit()
		{
		}

		public override void Run()
		{
		}

		protected override void CheckForStateSwitch()
		{
			throw new NotImplementedException();
		}

	}
}

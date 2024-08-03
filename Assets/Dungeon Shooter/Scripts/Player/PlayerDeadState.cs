using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonShooter.Player
{
	public class PlayerDeadState : PlayerStateBase
	{
		public PlayerDeadState(Player player) : base(player)
		{
		}

		public override void Enter()
		{
			_player.PlayAnimation("Dead");
		}

		public override void Exit()
		{
			throw new NotImplementedException();
		}

		public override void Run()
		{
			throw new NotImplementedException();
		}

		protected override void CheckForStateSwitch()
		{
			throw new NotImplementedException();
		}

	}
}

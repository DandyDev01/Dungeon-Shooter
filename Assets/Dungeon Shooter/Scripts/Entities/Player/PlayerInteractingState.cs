using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonShooter.Player
{
	public class PlayerInteractingState : PlayerStateBase
	{
		public PlayerInteractingState(PlayerCharacter player) : base(player)
		{
		}

		public override void Enter()
		{
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

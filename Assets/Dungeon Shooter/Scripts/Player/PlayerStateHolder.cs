using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Player
{
	public class PlayerStateHolder
	{
		public PlayerIdleState IdleState { get; private set; }
		public PlayerMoveState MoveState { get; private set; }
		public PlayerDodgeState DodgeState { get; private set; }
		public PlayerDeadState DeadState { get; private set; }
		public PlayerInteractingState InteractingState { get; private set; }

		public PlayerStateHolder(Player player)
		{
			IdleState = new PlayerIdleState(player);
			MoveState = new PlayerMoveState(player);
			DodgeState = new PlayerDodgeState(player);
			DeadState = new PlayerDeadState(player);
			InteractingState = new PlayerInteractingState(player);
		}
	}
}
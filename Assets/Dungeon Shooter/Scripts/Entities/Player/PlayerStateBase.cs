using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonShooter.Player
{
	[Serializable]
	public abstract class PlayerStateBase
	{
		protected readonly PlayerCharacter _player;


		public PlayerStateBase(PlayerCharacter player)
		{
			_player = player;
		}

		public abstract void Enter();

		public abstract void Exit();
		
		public abstract void Run();
		
		protected abstract void CheckForStateSwitch();

		protected void SwitchState(PlayerStateBase newState)
		{
			Exit();

			newState.Enter();

			_player.CurrentState = newState;
		}

	}

}
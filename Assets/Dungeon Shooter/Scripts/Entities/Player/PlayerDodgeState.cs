using DungeonShooter.Player.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonShooter.Player
{
	public class PlayerDodgeState : PlayerStateBase
	{
		private readonly Rigidbody2D _rigidbody;

		public PlayerDodgeState(Player player) : base(player)
		{
			_rigidbody = player.GetComponent<Rigidbody2D>();
		}

		public override void Enter()
		{
			_player.PlayAnimation("Dodge");
			_player.AddEffect(new InvincibleEffect(1f));

			_rigidbody.AddForce(_player.MoveVector * _player.DodgeForce, ForceMode2D.Impulse);
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
			SwitchState(_player.StateHolder.IdleState);
		}
	}
}
